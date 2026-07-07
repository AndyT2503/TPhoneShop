import { ChangeDetectionStrategy, Component, inject } from '@angular/core';

import { ToastItemComponent } from './toast-item.component';
import { ToastService } from '../services/toast.service';

@Component({
  selector: 'lib-toast-overlay',
  standalone: true,
  imports: [ToastItemComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div
      class="pointer-events-none fixed top-4 right-4 z-[9999] flex flex-col gap-2"
    >
      @for (toast of toastService.toasts(); track toast.id) {
        <lib-toast-item [toast]="toast" />
      }
    </div>
  `,
})
export class ToastOverlayComponent {
  protected readonly toastService = inject(ToastService);
}
