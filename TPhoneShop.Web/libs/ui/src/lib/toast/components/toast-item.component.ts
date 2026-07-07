import { ChangeDetectionStrategy, Component, input } from '@angular/core';

import { Toast } from '../models/toast.model';

@Component({
  selector: 'lib-toast-item',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div
      class="pointer-events-auto flex min-w-[340px] items-center gap-3 rounded-xl px-4 py-3 shadow-xl"
      [class.bg-emerald-500]="toast().type === 'success'"
      [class.bg-red-500]="toast().type === 'error'"
      [class.bg-amber-500]="toast().type === 'warning'"
      [class.bg-sky-500]="toast().type === 'info'"
    >
      <div
        class="flex h-8 w-8 items-center justify-center rounded-full bg-white/20 font-semibold text-white"
      >
        @switch (toast().type) {
          @case ('success') {
            ✓
          }
          @case ('error') {
            ✕
          }
          @case ('warning') {
            !
          }
          @default {
            i
          }
        }
      </div>

      <p class="flex-1 text-sm font-medium text-white">
        {{ toast().message }}
      </p>
    </div>
  `,
})
export class ToastItemComponent {
  readonly toast = input.required<Toast>();
}
