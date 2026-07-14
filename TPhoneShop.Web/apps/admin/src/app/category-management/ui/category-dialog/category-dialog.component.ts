import {
  ChangeDetectionStrategy,
  Component,
  inject,
  signal,
} from '@angular/core';
import { form, FormField, required } from '@angular/forms/signals';
import { CategoryDto, CreateCategoryRequest } from '@tphone-shop.web/data-access';
import {
  ButtonDirective,
  DialogRef,
  ToastService,
  DIALOG_DATA,
} from '@tphone-shop.web/ui';
import { CategoryManagementStore } from '../../store/category-management.store';

@Component({
  selector: 'app-category-dialog',
  imports: [FormField, ButtonDirective],
  templateUrl: './category-dialog.component.html',
  styleUrl: './category-dialog.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CategoryDialogComponent {
  private readonly dialogRef = inject(DialogRef);
  private readonly store = inject(CategoryManagementStore);
  private readonly toastService = inject(ToastService);
  private readonly data = inject<{ category?: CategoryDto; categories: CategoryDto[] }>(DIALOG_DATA);

  readonly isEdit = !!this.data.category;
  readonly categories = signal<CategoryDto[]>(
    this.data.categories.filter((c) => !this.data.category || c.id !== this.data.category.id)
  );

  readonly categoryModel = signal({
    name: this.data.category?.name ?? '',
    description: this.data.category?.description ?? '',
    parentId: this.data.category?.parentId ?? '',
    isActive: this.data.category?.isActive ?? true,
  });

  readonly categoryForm = form(this.categoryModel, (schemaPath) => {
    required(schemaPath.name, { message: 'Vui lòng nhập tên danh mục.' });
  });

  readonly isLoading = this.store.isLoading;

  submit(e: Event): void {
    e.preventDefault();
    if (this.categoryForm().invalid()) {
      const errors = [
        ...Object.values(this.categoryForm.name().errors() ?? {}),
      ];
      this.toastService.error(errors[0].message!);
      return;
    }

    const formValue = this.categoryForm().value();
    
    if (this.isEdit) {
      this.store.updateCategory({
        id: this.data.category!.id,
        request: {
          id: this.data.category!.id,
          name: formValue.name,
          description: formValue.description || undefined,
          parentId: formValue.parentId || undefined,
          isActive: formValue.isActive,
        },
        onSuccess: () => {
          this.store.loadCategories(undefined);
          this.dialogRef.close();
        },
      });
    } else {
      this.store.createCategory({
        request: {
          name: formValue.name,
          description: formValue.description || undefined,
          parentId: formValue.parentId || undefined,
        },
        onSuccess: () => {
          this.store.loadCategories(undefined);
          this.dialogRef.close();
        },
      });
    }
  }

  closeDialog(): void {
    this.dialogRef.close();
  }
}
