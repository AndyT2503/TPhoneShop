import { HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { tapResponse } from '@ngrx/operators';
import {
  patchState,
  signalStore,
  withMethods,
  withState
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import {
  AdminCategoryAPIService,
  CategoryDto,
  CreateCategoryRequest,
  UpdateCategoryRequest,
} from '@tphone-shop.web/data-access';
import { ToastService } from '@tphone-shop.web/ui';
import { pipe, switchMap, tap } from 'rxjs';

type CategoryManagementState = {
  categories: CategoryDto[];
  isLoading: boolean;
};

const initialState: CategoryManagementState = {
  categories: [],
  isLoading: false,
};

export const CategoryManagementStore = signalStore(
  { providedIn: 'root' },
  withState(initialState),
  withMethods(
    (
      store,
      categoryAPIService = inject(AdminCategoryAPIService),
      toast = inject(ToastService),
    ) => ({
      loadCategories: rxMethod<string | undefined>(
        pipe(
          tap(() => {
            patchState(store, { isLoading: true });
          }),
          switchMap((search) =>
            categoryAPIService.getCategories(search).pipe(
              tapResponse({
                next: (categories) => {
                  patchState(store, { categories });
                },
                error: (err: HttpErrorResponse) => {
                  toast.error(
                    err.error?.message || 'Không thể tải danh sách danh mục',
                  );
                },
                finalize: () => {
                  patchState(store, { isLoading: false });
                },
              }),
            ),
          ),
        ),
      ),
      createCategory: rxMethod<{
        request: CreateCategoryRequest;
        onSuccess: () => void;
      }>(
        pipe(
          tap(() => {
            patchState(store, { isLoading: true });
          }),
          switchMap(({ request, onSuccess }) =>
            categoryAPIService.createCategory(request).pipe(
              tapResponse({
                next: () => {
                  toast.success('Tạo danh mục thành công');
                  onSuccess();
                },
                error: (err: HttpErrorResponse) => {
                  toast.error(
                    err.error?.message || 'Tạo danh mục thất bại',
                  );
                },
                finalize: () => {
                  patchState(store, { isLoading: false });
                },
              }),
            ),
          ),
        ),
      ),
      updateCategory: rxMethod<{
        id: string;
        request: UpdateCategoryRequest;
        onSuccess: () => void;
      }>(
        pipe(
          tap(() => {
            patchState(store, { isLoading: true });
          }),
          switchMap(({ id, request, onSuccess }) =>
            categoryAPIService.updateCategory(id, request).pipe(
              tapResponse({
                next: () => {
                  toast.success('Cập nhật danh mục thành công');
                  onSuccess();
                },
                error: (err: HttpErrorResponse) => {
                  toast.error(
                    err.error?.message || 'Cập nhật danh mục thất bại',
                  );
                },
                finalize: () => {
                  patchState(store, { isLoading: false });
                },
              }),
            ),
          ),
        ),
      ),
      deleteCategory: rxMethod<{
        id: string;
        onSuccess: () => void;
      }>(
        pipe(
          tap(() => {
            patchState(store, { isLoading: true });
          }),
          switchMap(({ id, onSuccess }) =>
            categoryAPIService.deleteCategory(id).pipe(
              tapResponse({
                next: () => {
                  toast.success('Xóa danh mục thành công');
                  onSuccess();
                },
                error: (err: HttpErrorResponse) => {
                  toast.error(
                    err.error?.message || 'Xóa danh mục thất bại',
                  );
                },
                finalize: () => {
                  patchState(store, { isLoading: false });
                },
              }),
            ),
          ),
        ),
      ),
    }),
  ),
);
