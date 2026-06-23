import {
  ApplicationRef,
  DOCUMENT,
  EnvironmentInjector,
  Injectable,
  createComponent,
  inject,
  signal,
} from '@angular/core';

import { ToastOverlayComponent } from '../components/toast-overlay.component';
import { Toast, ToastType } from '../models/toast.model';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private readonly document = inject(DOCUMENT);
  private readonly appRef = inject(ApplicationRef);
  private readonly injector = inject(EnvironmentInjector);
  private initialized = false;

  readonly toasts = signal<Toast[]>([]);

  success(message: string) {
    this.show(message, 'success');
  }

  error(message: string) {
    this.show(message, 'error');
  }

  warning(message: string) {
    this.show(message, 'warning');
  }

  info(message: string) {
    this.show(message, 'info');
  }

  show(message: string, type: ToastType = 'info', duration = 3000) {
    this.ensureOverlay();
    const toast: Toast = {
      id: crypto.randomUUID(),
      message,
      type,
    };
    this.toasts.update((v) => [...v, toast]);
    setTimeout(() => {
      this.dismiss(toast.id);
    }, duration);
  }

  dismiss(id: string) {
    this.toasts.update((v) => v.filter((x) => x.id !== id));
  }

  private ensureOverlay() {
    if (this.initialized) {
      return;
    }

    const componentRef = createComponent(ToastOverlayComponent, {
      environmentInjector: this.injector,
    });
    this.appRef.attachView(componentRef.hostView);
    this.document.body.appendChild(componentRef.location.nativeElement);
    this.initialized = true;
  }
}
