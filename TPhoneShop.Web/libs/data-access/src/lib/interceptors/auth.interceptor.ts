import {
  HttpErrorResponse,
  HttpInterceptorFn,
  HttpStatusCode,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthStore } from '../store';

const AUTHORIZATION_SKIP_URLS = [] as const;

const REFRESH_TOKEN_SKIP_URLS = [
  'login',
  'refresh-token',
  'register',
] as const;

const shouldSkip = (url: string, patterns: readonly string[]) =>
  patterns.some(pattern => url.includes(pattern));

const withAuthorization = (
  req: Parameters<HttpInterceptorFn>[0],
  token: string | null,
) =>
  req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`,
    },
  });

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authStore = inject(AuthStore);

  if (shouldSkip(req.url, AUTHORIZATION_SKIP_URLS)) {
    return next(req);
  }

  return next(withAuthorization(req, authStore.accessToken())).pipe(
    catchError(err => {
      if (
        !(err instanceof HttpErrorResponse) ||
        err.status !== HttpStatusCode.Unauthorized ||
        shouldSkip(req.url, REFRESH_TOKEN_SKIP_URLS)
      ) {
        return throwError(() => err);
      }

      authStore.refreshToken();

      return authStore.isAuthenticated$.pipe(
        switchMap(isAuthenticated => {
          if (!isAuthenticated) {
            return throwError(() => err);
          }

          return next(withAuthorization(req, authStore.accessToken()));
        }),
      );
    }),
  );
};
