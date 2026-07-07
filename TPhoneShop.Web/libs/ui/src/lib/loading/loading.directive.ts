import {
  ChangeDetectionStrategy,
  Component,
  ComponentRef,
  Directive,
  DestroyRef,
  ElementRef,
  EnvironmentInjector,
  Renderer2,
  createComponent,
  effect,
  inject,
  input,
} from '@angular/core';
import { LucideLoaderCircle } from '@lucide/angular';

@Component({
  selector: 'lib-loading-overlay',
  standalone: true,
  imports: [LucideLoaderCircle],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div
      class="absolute inset-0 z-50 flex items-center justify-center rounded-[inherit] bg-white/70 backdrop-blur-[2px]"
    >
      <svg
        lucideLoaderCircle
        [size]="36"
        class="animate-spin text-indigo-600"
      ></svg>
    </div>
  `,
})
class LoadingOverlayComponent {}

@Directive({
  selector: '[libLoading]',
  standalone: true,
})
export class LoadingDirective {
  readonly libLoading = input(false);

  private readonly host = inject<ElementRef<HTMLElement>>(ElementRef);
  private readonly renderer = inject(Renderer2);
  private readonly destroyRef = inject(DestroyRef);
  private readonly environmentInjector = inject(EnvironmentInjector);

  private overlayRef?: ComponentRef<LoadingOverlayComponent>;
  private originalPosition?: string;

  constructor() {
    effect(() => {
      if (this.libLoading()) {
        this.show();
      } else {
        this.hide();
      }
    });

    this.destroyRef.onDestroy(() => this.hide());
  }

  private show(): void {
    if (this.overlayRef) {
      return;
    }

    const host = this.host.nativeElement;

    const computedStyle = getComputedStyle(host);

    if (computedStyle.position === 'static') {
      this.originalPosition = '';
      this.renderer.setStyle(host, 'position', 'relative');
    } else {
      this.originalPosition = computedStyle.position;
    }

    this.overlayRef = createComponent(LoadingOverlayComponent, {
      environmentInjector: this.environmentInjector,
    });

    this.overlayRef.changeDetectorRef.detectChanges();

    host.appendChild(this.overlayRef.location.nativeElement);
  }

  private hide(): void {
    if (!this.overlayRef) {
      return;
    }

    this.overlayRef.destroy();
    this.overlayRef = undefined;

    if (this.originalPosition === '') {
      this.renderer.removeStyle(this.host.nativeElement, 'position');
    }
  }
}
