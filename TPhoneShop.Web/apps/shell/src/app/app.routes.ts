import { Route } from '@angular/router';
import { authMatcher } from '@tphone-shop.web/routing-config';

export const appRoutes: Route[] = [
  {
    matcher: authMatcher,
    loadChildren: () => import('auth/Routes').then((m) => m.remoteRoutes),
  },
  {
    path: '',
    loadChildren: () => import('commerce/Routes').then((m) => m.remoteRoutes),
  },
  // {
  //   path: 'admin',
  //   loadChildren: () => import('admin/Routes').then((m) => m.remoteRoutes),
  // },
];
