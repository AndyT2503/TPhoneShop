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
  PermissionAPIService,
  RoleAPIService,
  RoleDto,
} from '@tphone-shop.web/data-access';
import { ToastService } from '@tphone-shop.web/ui-toast';
import { map, pipe, switchMap, tap } from 'rxjs';

type PermissionState = {
  selectedRole: RoleDto | null;
  isLoading: boolean;
};

const initialState: PermissionState = {
  selectedRole: null,
  isLoading: false,
};

export const PermissionStore = signalStore(
  withState(initialState),
  withProps(
    (
      store,
      roleAPIService = inject(RoleAPIService),
      permissionAPIService = inject(PermissionAPIService),
    ) => ({
      permissionsOfSelectedRole: rxResource({
        params: () => {
          const selectedRole = store.selectedRole();
          if (!selectedRole) {
            return undefined;
          }
          return selectedRole.id;
        },
        stream: (resourceParams) =>
          roleAPIService
            .getPermissionsByRole(resourceParams.params)
            .pipe(map((res) => res.permissions)),
        defaultValue: [],
      }),
      roles: rxResource({
        stream: () =>
          roleAPIService.getRoles().pipe(
            map((res) => res.roles),
            tap((roles) => {
              if (!store.selectedRole()) {
                patchState(store, {
                  selectedRole: roles[0],
                });
              }
            }),
          ),
        defaultValue: [],
      }),
      permissions: rxResource({
        stream: () =>
          permissionAPIService
            .getPermissions()
            .pipe(map((res) => res.permissions)),
        defaultValue: [],
      }),
    }),
  ),
  withMethods(
    (
      store,
      roleAPIService = inject(RoleAPIService),
      toast = inject(ToastService),
    ) => ({
      selectRole: (role: RoleDto) => {
        patchState(store, {
          selectedRole: role,
        });
      },
      savePermissions: rxMethod<{ roleId: string; permissions: string[] }>(
        pipe(
          switchMap((request) => {
            patchState(store, {
              isLoading: true,
            });
            return roleAPIService
              .assignPermissionToRole(request.roleId, request.permissions)
              .pipe(
                tapResponse({
                  next: () => {
                    toast.success('Cập nhật quyền thành công');
                    store.permissionsOfSelectedRole.reload();
                  },
                  error: (err) => {
                    if (err instanceof HttpErrorResponse) {
                      toast.error(err.error['message']);
                    } else {
                      toast.error('Cập nhật quyền thất bại');
                    }
                    console.error(err);
                  },
                  finalize: () => {
                    patchState(store, {
                      isLoading: false,
                    });
                  },
                }),
              );
          }),
        ),
      ),
      addRole: rxMethod<{ name: string; onSuccess: () => void }>(
        pipe(
          switchMap((request) => {
            patchState(store, {
              isLoading: true,
            });
            return roleAPIService.createRole(request.name).pipe(
              tapResponse({
                next: () => {
                  toast.success('Tạo vài trò mới thành công');
                  request.onSuccess();
                  store.roles.reload();
                },
                error: (err) => {
                  if (err instanceof HttpErrorResponse) {
                    toast.error(err.error['message']);
                  } else {
                    toast.error('Tạo vài trò mới thất bại');
                  }
                  console.error(err);
                },
                finalize: () => {
                  patchState(store, {
                    isLoading: false,
                  });
                },
              }),
            );
          }),
        ),
      ),
      updateRole: rxMethod<{
        roleId: string;
        name: string;
        onSuccess: () => void;
      }>(
        pipe(
          switchMap((request) => {
            patchState(store, {
              isLoading: true,
            });
            return roleAPIService.updateRole(request.roleId, request.name).pipe(
              tapResponse({
                next: () => {
                  toast.success('Cập nhật vai trò thành công');
                  request.onSuccess();
                  store.roles.reload();
                },
                error: (err) => {
                  if (err instanceof HttpErrorResponse) {
                    toast.error(err.error['message']);
                  } else {
                    toast.error('Cập nhật vai trò thất bại');
                  }
                  console.error(err);
                },
                finalize: () => {
                  patchState(store, {
                    isLoading: false,
                  });
                },
              }),
            );
          }),
        ),
      ),
      deleteRole: rxMethod<{
        roleId: string;
      }>(
        pipe(
          switchMap((request) => {
            patchState(store, {
              isLoading: true,
            });
            return roleAPIService.deleteRole(request.roleId).pipe(
              tapResponse({
                next: () => {
                  toast.success('Xóa vai trò thành công');
                  store.roles.reload();
                },
                error: (err) => {
                  if (err instanceof HttpErrorResponse) {
                    toast.error(err.error['message']);
                  } else {
                    toast.error('Xóa vai trò thất bại');
                  }
                  console.error(err);
                },
                finalize: () => {
                  patchState(store, {
                    isLoading: false,
                  });
                },
              }),
            );
          }),
        ),
      ),
    }),
  ),
);
