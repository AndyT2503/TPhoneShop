import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { AuthStore } from '../store';
import { map } from 'rxjs';
import { AUTH_ROUTES } from '@tphone-shop.web/routing-config';

export const authGuard: CanMatchFn = (_, segments) => {
  const authStore = inject(AuthStore);
  const router = inject(Router);
  const returnUrl = '/' + segments.map((s) => s.path).join('/');
  return authStore.isAuthenticated$.pipe(
    map((isAuth) => {
      if (!isAuth) {
        return router.createUrlTree([`/${AUTH_ROUTES.login}`], {
          queryParams: {
            returnUrl,
          },
        });
      }
      return true;
    }),
  );
};
