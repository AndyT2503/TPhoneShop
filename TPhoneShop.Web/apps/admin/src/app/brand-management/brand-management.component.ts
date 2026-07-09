import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import {
  LucideSquarePen,
  LucideEye,
  LucidePlus,
  LucideTrash,
} from '@lucide/angular';
import {
  ButtonDirective,
  DialogRef,
  DialogService,
  TableModule,
} from '@tphone-shop.web/ui';
import { BrandManagementStore } from './store/brand-management.store';
import { AddBrandDialogComponent } from './ui/add-brand-dialog/add-brand-dialog.component';

@Component({
  selector: 'app-brand-management',
  imports: [
    ButtonDirective,
    LucidePlus,
    TableModule,
    LucideEye,
    LucideSquarePen,
    LucideTrash,
  ],
  templateUrl: './brand-management.component.html',
  styleUrl: './brand-management.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [BrandManagementStore],
})
export class BrandManagementComponent {
  private readonly brandManagementStore = inject(BrandManagementStore);
  private readonly dialog = inject(DialogService);
  private addBrandDialogRef?: DialogRef;

  readonly brandTable = this.brandManagementStore.brandTable;
  readonly query = this.brandManagementStore.query;

  onPageNumberChange(pageNumber: number): void {
    this.brandManagementStore.changePageNumber(pageNumber);
  }

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
