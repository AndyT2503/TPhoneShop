import { Route } from '@angular/router';

export const remoteRoutes: Route[] = [
  {
    path: '',
    loadComponent: () =>
      import('../auth-layout/auth-layout.component').then(
        (c) => c.AuthLayoutComponent,
      ),
    loadChildren: () =>
      import('../auth-layout/auth-layout.routes').then((m) => m.authRoutes),
  },
];
