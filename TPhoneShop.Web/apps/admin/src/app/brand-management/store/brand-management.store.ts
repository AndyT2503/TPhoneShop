import { HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { rxResource } from '@angular/core/rxjs-interop';
import { tapResponse } from '@ngrx/operators';
import {
  patchState,
  signalStore,
  withMethods,
  withProps,
  withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import {
  AdminBrandAPIService,
  BrandDto,
  CreateBrandRequest,
  DEFAULT_PAGE_SIZE,
  GetBrandsQuery,
  PagingResponse,
} from '@tphone-shop.web/data-access';
import { ToastService } from '@tphone-shop.web/ui';
import { pipe, switchMap, tap } from 'rxjs';

type BrandManagementState = {
  isLoading: boolean;
  query: GetBrandsQuery;
};

const initialState: BrandManagementState = {
  isLoading: false,
  query: {
    pageNumber: 1,
    pageSize: DEFAULT_PAGE_SIZE,
  },
};

export const BrandManagementStore = signalStore(
  withState(initialState),
  withProps((store, brandAPIService = inject(AdminBrandAPIService)) => ({
    brandTable: rxResource({
      params: () => store.query(),
      stream: (resourceParams) => {
        return brandAPIService.getBrands(resourceParams.params);
      },
      defaultValue: {
        items: [],
        totalCount: 0
      }
    }),
  })),
  withMethods(
    (
      store,
      brandAPIService = inject(AdminBrandAPIService),
      toast = inject(ToastService),
    ) => ({
      createBrand: rxMethod<{
        createBrandRequest: CreateBrandRequest;
        onSuccess: () => void;
      }>(
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
                  store.brandTable.reload();
                },
                error: (err: HttpErrorResponse) => {
                  toast.error(err.error?.message || 'Tạo nhãn hàng thất bại');
                },
                finalize: () => {
                  patchState(store, { isLoading: false });
                },
              }),
            ),
          ),
        ),
      ),
      changePageNumber(pageNumber: number) {
        patchState(store, {
          query: {
            ...store.query(),
            pageNumber,
          },
        });

        store.brandTable.reload();
      },
    }),
  ),
);
