import { ChangeDetectionStrategy, Component, inject, Injector } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  LucidePlus,
  LucideSearch,
  LucideSquarePen,
  LucideTrash
} from '@lucide/angular';
import { BrandForAdminDto } from '@tphone-shop.web/data-access';
import {
  ButtonDirective,
  ConfirmPopoverDirective,
  DialogRef,
  DialogService,
  TableModule,
} from '@tphone-shop.web/ui';
import { StatusFilterComponent } from '../shared/ui/status-filter/status-filter.component';
import { BrandManagementStore } from './store/brand-management.store';
import { AddBrandDialogComponent } from './ui/add-brand-dialog/add-brand-dialog.component';
import { UpdateBrandDialogComponent } from './ui/update-brand-dialog/update-brand-dialog.component';

@Component({
  selector: 'app-brand-management',
  imports: [
    ButtonDirective,
    LucidePlus,
    TableModule,
    LucideSquarePen,
    LucideTrash,
    LucideSearch,
    StatusFilterComponent,
    ConfirmPopoverDirective,
    FormsModule
  ],
  templateUrl: './brand-management.component.html',
  styleUrl: './brand-management.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [BrandManagementStore],
})
export class BrandManagementComponent {
  private readonly injector = inject(Injector);
  private readonly brandManagementStore = inject(BrandManagementStore);
  private readonly dialog = inject(DialogService);
  private addBrandDialogRef?: DialogRef;
  private updateBrandDialogRef?: DialogRef;

  readonly brandTable = this.brandManagementStore.brandTable;
  readonly query = this.brandManagementStore.query;
  readonly isLoading = this.brandManagementStore.isLoading;
  readonly searchValue = this.brandManagementStore.query().search;

  onPageNumberChange(pageNumber: number): void {
    this.brandManagementStore.changePageNumber(pageNumber);
  }

  onSearchChange(searchValue: string): void {
    this.brandManagementStore.changeSearch(searchValue);
  }

  onStatusFilterChange(isActive: boolean): void {
    this.brandManagementStore.changeStatusFilter(isActive);
  }

  openAddBrandDialog(): void {
    if (this.addBrandDialogRef) {
      return;
    }

    this.addBrandDialogRef = this.dialog.open(AddBrandDialogComponent, {
      title: 'Thêm nhãn hàng',
      injector: this.injector
    });

    this.addBrandDialogRef.afterClosed.subscribe(() => {
      this.addBrandDialogRef = undefined;
    });
  }

  openUpdateBrandDialog(brand: BrandForAdminDto): void {
    if (this.updateBrandDialogRef) {
      return;
    }

    this.updateBrandDialogRef = this.dialog.open(UpdateBrandDialogComponent, {
      title: 'Cập nhật nhãn hàng',
      data: brand,
      injector: this.injector
    });

    this.updateBrandDialogRef.afterClosed.subscribe(() => {
      this.updateBrandDialogRef = undefined;
    });
  }

  onDeleteBrand(brandId: string): void {
    this.brandManagementStore.deleteBrand({
      brandId,
    });
  }
}
