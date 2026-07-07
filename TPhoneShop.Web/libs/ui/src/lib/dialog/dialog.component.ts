import {
  ChangeDetectionStrategy,
  Component,
  input,
  output,
} from '@angular/core';

import { DialogConfig } from './dialog-config';

@Component({
  selector: 'lib-dialog',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `
    <div class="overflow-hidden rounded-2xl bg-white shadow-2xl">
      @if (config()?.title || config()?.showCloseButton !== false) {
        <div class="flex items-center justify-between border-b border-slate-200 px-6 py-4">
          <h3 class="text-lg font-semibold text-slate-800">
            {{ config()?.title ?? 'Thông báo' }}
          </h3>

          @if (config()?.showCloseButton !== false) {
            <button
              type="button"
              class="rounded-full p-2 text-slate-500 transition hover:bg-slate-100 hover:text-slate-700"
              (click)="close.emit()"
            >
              ×
            </button>
          }
        </div>
      }

      <div class="px-6 py-5">
        <ng-content></ng-content>
      </div>
    </div>
  `,
})
export class DialogComponent {
  readonly config = input<DialogConfig | undefined>(undefined);
  // eslint-disable-next-line @angular-eslint/no-output-native
  readonly close = output<void>();
}
