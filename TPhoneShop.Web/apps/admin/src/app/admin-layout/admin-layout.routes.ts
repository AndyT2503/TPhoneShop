import { Routes } from '@angular/router';
import { ADMIN_ROUTES } from '@tphone-shop.web/routing-config';

export const adminRoutes: Routes = [
  {
    path: ADMIN_ROUTES.permission,
    loadComponent: () =>
      import('../permission/permission.component').then(
        (c) => c.PermissionComponent,
      ),
    data: {
      title: 'Phân quyền',
    },
  },
];
