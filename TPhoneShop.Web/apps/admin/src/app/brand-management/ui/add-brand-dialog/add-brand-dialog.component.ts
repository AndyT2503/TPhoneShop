import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
} from '@angular/core';
import { form, FormField, required } from '@angular/forms/signals';
import { CreateBrandRequest } from '@tphone-shop.web/data-access';
import {
  ButtonDirective,
  DialogRef,
  ToastService,
  UploadInputComponent,
} from '@tphone-shop.web/ui';
import { BrandManagementStore } from '../../store/brand-management.store';

@Component({
  selector: 'app-add-brand-dialog',
  imports: [FormField, ButtonDirective, UploadInputComponent],
  templateUrl: './add-brand-dialog.component.html',
  styleUrl: './add-brand-dialog.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddBrandDialogComponent {
  private readonly dialogRef = inject(DialogRef);
  private readonly store = inject(BrandManagementStore);
  private readonly toastService = inject(ToastService);

  readonly brandModel = signal<CreateBrandRequest>({
    description: '',
    logoId: '',
    name: '',
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
  } | null>(null);

  readonly isUploadingFile = signal(false);
  readonly isLoading = this.store.isLoading;

  onImageChange(logoId: string | undefined): void {
    if (!logoId) {
      return;
    }
    this.brandModel.update((v) => ({ ...v, logoId }));
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
    const createBrandRequest = this.brandForm().value();
    this.store.createBrand({
      createBrandRequest,
      onSuccess: () => {
        this.dialogRef.close();
      },
    });
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
