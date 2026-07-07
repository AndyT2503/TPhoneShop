import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { LucidePlus } from '@lucide/angular';
import { ButtonDirective, DialogRef, DialogService } from '@tphone-shop.web/ui';

import { AddBrandDialogComponent } from './ui/add-brand-dialog/add-brand-dialog.component';

@Component({
  selector: 'app-brand-management',
  imports: [ButtonDirective, LucidePlus],
  templateUrl: './brand-management.component.html',
  styleUrl: './brand-management.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BrandManagementComponent {
  private readonly dialog = inject(DialogService);
  private addBrandDialogRef?: DialogRef;

  openAddBrandDialog(): void {
    if (this.addBrandDialogRef) {
      return;
    }

    this.addBrandDialogRef = this.dialog.open(AddBrandDialogComponent, {
      title: 'Thêm nhãn hàng',
    });

    this.addBrandDialogRef.afterClosed.subscribe(() => {
      this.addBrandDialogRef = undefined;
    });
  }
}
