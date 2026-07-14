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
  AdminBrandAPIService,
  CreateBrandRequest,
} from '@tphone-shop.web/data-access';
import { ToastService } from '@tphone-shop.web/ui';
import { pipe, switchMap, tap } from 'rxjs';

type BrandManagementState = {
  isLoading: boolean;
};

const initialState: BrandManagementState = {
  isLoading: false,
};

export const BrandManagementStore = signalStore(
  { providedIn: 'root' },
  withState(initialState),
  withMethods(
    (
      store,
      brandAPIService = inject(AdminBrandAPIService),
      toast = inject(ToastService),
    ) => ({
      createBrand: rxMethod<{createBrandRequest: CreateBrandRequest, onSuccess: () => void}>(
        pipe(
          tap(() => {
            patchState(store, { isLoading: true });
          }),
          switchMap((req) =>
            brandAPIService.createBrand(req.createBrandRequest).pipe(
              tapResponse({
                next: () => {
                  toast.success('Tạo nhãn hàng thành công');
                  req.onSuccess();
                },
                error: (err: HttpErrorResponse) => {
                  toast.error(
                    err.error?.message || 'Tạo nhãn hàng thất bại',
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
