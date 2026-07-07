import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal } from '@angular/cdk/portal';
import {
  ChangeDetectionStrategy,
  Component,
  Directive,
  ElementRef,
  HostListener,
  InjectionToken,
  Injector,
  OnDestroy,
  inject,
  input,
  output,
} from '@angular/core';
import { Subscription } from 'rxjs';

interface ConfirmPopoverData {
  message: string;
  confirm: () => void;
  cancel: () => void;
}

const CONFIRM_POPOVER_DATA = new InjectionToken<ConfirmPopoverData>(
  'CONFIRM_POPOVER_DATA'
);

@Component({
  selector: 'lib-confirm-popover-panel',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="min-w-60 max-w-[calc(100vw-16px)] overflow-hidden rounded-xl border border-indigo-600 bg-white shadow-lg">
      <div class="p-3">
        <p class="mb-2.5 break-words text-sm leading-5 text-slate-800">
          {{ data.message }}
        </p>

        <div class="flex flex-wrap justify-end gap-2">
          <button
            type="button"
            class="w-[70px] cursor-pointer rounded-md border border-slate-300 px-2 py-1 text-sm text-slate-700 transition hover:bg-slate-50"
            (click)="data.cancel()"
          >
            Hủy
          </button>
          <button
            type="button"
            class="w-[70px] cursor-pointer rounded-md bg-indigo-600 px-2 py-1 text-sm text-white transition hover:bg-indigo-700"
            (click)="data.confirm()"
          >
            OK
          </button>
        </div>
      </div>
    </div>
  `,
})
class ConfirmPopoverPanelComponent {
  protected readonly data = inject(CONFIRM_POPOVER_DATA);
}

@Directive({
  selector: '[libConfirmPopover]',
  standalone: true,
})
export class ConfirmPopoverDirective implements OnDestroy {
  private readonly el = inject<ElementRef<HTMLElement>>(ElementRef);
  private readonly overlay = inject(Overlay);
  private readonly injector = inject(Injector);
  private overlayRef: OverlayRef | null = null;
  private outsidePointerSubscription?: Subscription;

  readonly message = input('Bạn có chắc chắn?');
  readonly confirm = output<void>();
  // eslint-disable-next-line @angular-eslint/no-output-native
  readonly cancel = output<void>();

  @HostListener('click', ['$event'])
  handleClick(event: MouseEvent): void {
    event.stopPropagation();

    if (this.overlayRef) {
      this.destroyPopover();
      return;
    }

    this.createPopover();
  }

  ngOnDestroy(): void {
    this.destroyPopover();
  }

  private createPopover(): void {
    const positionStrategy = this.overlay
      .position()
      .flexibleConnectedTo(this.el)
      .withFlexibleDimensions(false)
      .withPush(true)
      .withPositions([
        {
          originX: 'end',
          originY: 'top',
          overlayX: 'end',
          overlayY: 'bottom',
          offsetY: -8,
        },
        {
          originX: 'end',
          originY: 'bottom',
          overlayX: 'end',
          overlayY: 'top',
          offsetY: 8,
        },
        {
          originX: 'start',
          originY: 'top',
          overlayX: 'start',
          overlayY: 'bottom',
          offsetY: -8,
        },
        {
          originX: 'start',
          originY: 'bottom',
          overlayX: 'start',
          overlayY: 'top',
          offsetY: 8,
        },
      ]);

    this.overlayRef = this.overlay.create({
      positionStrategy,
      scrollStrategy: this.overlay.scrollStrategies.reposition(),
    });

    const panelInjector = Injector.create({
      providers: [
        {
          provide: CONFIRM_POPOVER_DATA,
          useValue: {
            message: this.message(),
            confirm: () => {
              this.confirm.emit();
              this.destroyPopover();
            },
            cancel: () => {
              this.cancel.emit();
              this.destroyPopover();
            },
          } satisfies ConfirmPopoverData,
        },
      ],
      parent: this.injector,
    });

    this.overlayRef.attach(
      new ComponentPortal(ConfirmPopoverPanelComponent, null, panelInjector)
    );

    this.outsidePointerSubscription = this.overlayRef
      .outsidePointerEvents()
      .subscribe((event) => {
        if (this.el.nativeElement.contains(event.target as Node)) {
          return;
        }

        this.destroyPopover();
      });
  }

  private destroyPopover(): void {
    this.outsidePointerSubscription?.unsubscribe();
    this.outsidePointerSubscription = undefined;
    this.overlayRef?.dispose();
    this.overlayRef = null;
  }
}
