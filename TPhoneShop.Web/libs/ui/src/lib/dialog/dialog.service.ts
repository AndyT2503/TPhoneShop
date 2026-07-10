import { Overlay, OverlayConfig, OverlayRef } from '@angular/cdk/overlay';
import { ComponentPortal, TemplatePortal } from '@angular/cdk/portal';
import {
  EnvironmentInjector,
  Injectable,
  Injector,
  inject,
} from '@angular/core';
import { takeUntil } from 'rxjs';

import { DEFAULT_DIALOG_CONFIG, DialogConfig } from './dialog-config';
import { DialogContainerComponent } from './dialog-container.component';
import { DialogRef } from './dialog-ref';
import { DIALOG_CONFIG, DIALOG_DATA, DIALOG_PORTAL } from './dialog.tokens';

@Injectable({
  providedIn: 'root',
})
export class DialogService {
  private readonly overlay = inject(Overlay);
  private readonly injector = inject(EnvironmentInjector);

  open<TData = unknown, TResult = unknown>(
    componentOrTemplate: unknown,
    config?: DialogConfig<TData>
  ): DialogRef<TResult> {
    const mergedConfig = { ...DEFAULT_DIALOG_CONFIG, ...config };
    const overlayRef = this.createOverlay(mergedConfig);
    const dialogRef = new DialogRef<TResult>(overlayRef);

    const injector = Injector.create({
      providers: [
        { provide: DialogRef, useValue: dialogRef },
        { provide: DIALOG_CONFIG, useValue: mergedConfig },
        { provide: DIALOG_DATA, useValue: mergedConfig.data },
      ],
       parent: mergedConfig.injector ?? this.injector,
    });

    const portal = this.createPortal(componentOrTemplate, injector);
    const portalInjector = Injector.create({
      providers: [{ provide: DIALOG_PORTAL, useValue: portal }],
      parent: injector,
    });

    overlayRef.attach(
      new ComponentPortal(DialogContainerComponent, null, portalInjector)
    );

    overlayRef.backdropClick()
      .pipe(takeUntil(dialogRef.afterClosed))
      .subscribe(() => {
        if (!mergedConfig.disableClose) {
          dialogRef.close();
        }
      });

    return dialogRef;
  }

  private createOverlay(config: DialogConfig): OverlayRef {
    const overlayConfig = new OverlayConfig({
      hasBackdrop: config.hasBackdrop,
      backdropClass: config.backdropClass,
      panelClass: config.panelClass,
      width: config.width,
      maxWidth: config.maxWidth,
      minHeight: config.minHeight,
      positionStrategy: this.overlay
        .position()
        .global()
        .centerHorizontally()
        .centerVertically(),
      scrollStrategy: this.overlay.scrollStrategies.block(),
    });

    return this.overlay.create(overlayConfig);
  }

  private createPortal(componentOrTemplate: unknown, injector: Injector) {
    if (componentOrTemplate instanceof TemplatePortal) {
      return componentOrTemplate;
    }

    if (typeof componentOrTemplate === 'function') {
      return new ComponentPortal(componentOrTemplate as never, null, injector);
    }

    if (componentOrTemplate && typeof componentOrTemplate === 'object') {
      return new ComponentPortal(componentOrTemplate as never, null, injector);
    }

    throw new Error('Dialog content must be a component or template portal');
  }
}
