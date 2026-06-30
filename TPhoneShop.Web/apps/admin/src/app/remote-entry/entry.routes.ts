import { Route } from '@angular/router';

export const remoteRoutes: Route[] = [
  {
    path: '',
    loadComponent: () =>
      import('../admin-layout/admin-layout.component').then(
        (c) => c.AdminLayoutComponent,
      ),
    loadChildren: () =>
      import('../admin-layout/admin-layout.routes').then((m) => m.adminRoutes),
  },
];
