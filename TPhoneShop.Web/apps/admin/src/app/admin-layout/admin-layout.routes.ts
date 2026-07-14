import { Routes } from '@angular/router';
import { ADMIN_ROUTES } from '@tphone-shop.web/routing-config';

export const adminRoutes: Routes = [
  {
    path: '',
    redirectTo: ADMIN_ROUTES.permission,
    pathMatch: 'full',
  },
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
  {
    path: ADMIN_ROUTES.brandManagement,
    loadComponent: () =>
      import('../brand-management/brand-management.component').then(
        (c) => c.BrandManagementComponent,
      ),
    data: {
      title: 'Nhãn hàng',
    },
  },
  {
    path: ADMIN_ROUTES.category,
    loadComponent: () =>
      import('../category-management/category-management.component').then(
        (c) => c.CategoryManagementComponent,
      ),
    data: {
      title: 'Danh mục',
    },
  },
];
