import { inject } from '@angular/core';
import { CanMatchFn, Router } from '@angular/router';
import { AuthStore } from '../store';
import { map } from 'rxjs';

export const nonAuthGuard: CanMatchFn = () => {
  const authStore = inject(AuthStore);
  const router = inject(Router);
  return authStore.isAuthenticated$.pipe(
    map((isAuth) => {
      if (isAuth) {
        return router.createUrlTree(['/']);
      }
      return true;
    }),
  );
};
