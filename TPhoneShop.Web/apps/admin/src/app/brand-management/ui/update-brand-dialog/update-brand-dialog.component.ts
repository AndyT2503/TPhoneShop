import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
} from '@angular/core';
import { form, FormField, required } from '@angular/forms/signals';
import { BrandForAdminDto, UpdateBrandRequest } from '@tphone-shop.web/data-access';
import {
  ButtonDirective,
  DialogRef,
  ToastService,
  UploadInputComponent,
  DIALOG_DATA,
} from '@tphone-shop.web/ui';
import { BrandManagementStore } from '../../store/brand-management.store';

@Component({
  selector: 'app-update-brand-dialog',
  imports: [FormField, ButtonDirective, UploadInputComponent],
  templateUrl: './update-brand-dialog.component.html',
  styleUrl: './update-brand-dialog.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateBrandDialogComponent {
  private readonly dialogRef = inject(DialogRef);
  private readonly store = inject(BrandManagementStore);
  private readonly toastService = inject(ToastService);
  readonly brand = inject(DIALOG_DATA) as BrandForAdminDto;

  readonly brandModel = signal<UpdateBrandRequest>({
    description: this.brand.description,
    logoId: this.brand.logoId,
    name: this.brand.name,
    isActive: this.brand.isActive,
  });

  readonly brandForm = form(this.brandModel, (schemaPath) => {
    required(schemaPath.description, { message: 'Vui lòng nhập mô tả.' });
    required(schemaPath.logoId, {
      message: 'Vui lòng upload logo thương hiệu.',
    });
    required(schemaPath.name, { message: 'Vui lòng nhập tên.' });
  });

  readonly imagePreview = signal<{
    presignedUrl: string;
    mediaId: string;
  } | null>({
    presignedUrl: this.brand.logoUrl,
    mediaId: this.brand.logoId,
  });

  readonly isUploadingFile = signal(false);
  readonly isLoading = this.store.isLoading;

  onImageChange(logoId: string | undefined): void {
    if (!logoId) {
      return;
    }
    this.brandModel.update((v) => ({ ...v, logoId }));
  }

  onActiveChange(isActive: boolean): void {
    this.brandModel.update((v) => ({ ...v, isActive }));
  }

  submit(e: Event): void {
    e.preventDefault();
    if (this.brandForm().invalid()) {
      const errors = [
        ...Object.values(this.brandForm.name().errors() ?? {}),
        ...Object.values(this.brandForm.description().errors() ?? {}),
        ...Object.values(this.brandForm.logoId().errors() ?? {}),
      ];
      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      this.toastService.error(errors[0].message!);
      return;
    }
    const updateBrandRequest = this.brandForm().value();
    this.store.updateBrand({
      brandId: this.brand.id,
      updateBrandRequest,
      onSuccess: () => {
        this.dialogRef.close();
      },
    });
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
