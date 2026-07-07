import { CdkPortalOutlet, Portal } from '@angular/cdk/portal';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
} from '@angular/core';

import { DialogConfig } from './dialog-config';
import { DialogRef } from './dialog-ref';
import { DIALOG_CONFIG, DIALOG_PORTAL } from './dialog.tokens';
import { DialogComponent } from './dialog.component';

@Component({
  selector: 'lib-dialog-container',
  standalone: true,
  imports: [CdkPortalOutlet, DialogComponent],
  changeDetection: ChangeDetectionStrategy.OnPush,
  host: {
    class: 'w-full',
  },
  template: `
    <lib-dialog [config]="config" (close)="closeDialog()">
      <ng-template [cdkPortalOutlet]="portal"></ng-template>
    </lib-dialog>
  `,
})
export class DialogContainerComponent {
  protected readonly dialogRef = inject(DialogRef);
  protected readonly portal = inject(DIALOG_PORTAL) as Portal<unknown>;
  protected readonly config = inject(DIALOG_CONFIG, { optional: true }) as DialogConfig | undefined;

  protected closeDialog(): void {
    this.dialogRef.close();
  }
}
