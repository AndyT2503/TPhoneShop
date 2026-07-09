import { HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { tapResponse } from '@ngrx/operators';
import {
  patchState,
  signalStore,
  withMethods,
  withProps,
  withState,
} from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { AUTH_ROUTES } from '@tphone-shop.web/routing-config';
import {
  catchError,
  EMPTY,
  exhaustMap,
  filter,
  finalize,
  from,
  map,
  pipe,
  switchMap,
  tap,
} from 'rxjs';
import { AuthAPIService } from '../api';
import { FirebaseAuthService } from '../firebase/firebase-auth.service';
import { LoginRequest, RegisterRequest, UserProfileResponse } from '../models';
import { toSyncObservable } from '@tphone-shop.web/utils';
type AuthState = {
  authState: 'pending' | 'authenticated' | 'unauthenticated';
  accessToken: string;
  isLoading: boolean;
  currentUser: UserProfileResponse | null;
};

const initialState: AuthState = {
  accessToken: '',
  isLoading: false,
  authState: 'pending',
  currentUser: null,
};

function resolveErrorMessage(err: HttpErrorResponse): string {
  // A network/CORS failure (server down, blocked) has status 0 and no JSON
  // body, so err.error carries no `message` field.
  if (err.status === 0) {
    return 'Không thể kết nối tới máy chủ. Vui lòng thử lại sau.';
  }
  return err.error?.['message'] ?? 'Đã có lỗi xảy ra. Vui lòng thử lại.';
}

export const AuthStore = signalStore(
  { providedIn: 'root' },
  withState(initialState),
  withProps((store) => ({
    isAuthenticated$: toSyncObservable(store.authState).pipe(
      filter((state) => state !== 'pending'),
      map((state) => state === 'authenticated'),
    ),
  })),
  withMethods(
    (
      store,
      authAPIService = inject(AuthAPIService),
      router = inject(Router),

      firebaseAuthService = inject(FirebaseAuthService),
    ) => ({
      googleLogin: rxMethod<{
        onError: (message: string) => void;
        returnUrl?: string;
      }>(
        pipe(
          tap(() =>
            patchState(store, { isLoading: true, authState: 'pending' }),
          ),
          exhaustMap((req) =>
            from(firebaseAuthService.signInWithGoogle()).pipe(
              switchMap((idToken) => authAPIService.externalLogin({ idToken })),
              tapResponse({
                next: (res) => {
                  patchState(store, {
                    accessToken: res.accessToken,
                    authState: 'authenticated',
                  });
                  router.navigate([req.returnUrl ?? '/']);
                },
                error: (err) => {
                  patchState(store, {
                    authState: 'unauthenticated',
                  });
                  if (err instanceof HttpErrorResponse) {
                    req.onError(resolveErrorMessage(err));
                  }
                  console.error(err);
                },
                finalize: () => {
                  patchState(store, { isLoading: false });
                },
              }),
            ),
          ),
        ),
      ),
      login: rxMethod<{
        loginRequest: LoginRequest;
        onError: (message: string) => void;
        returnUrl?: string;
      }>(
        pipe(
          tap(() =>
            patchState(store, { isLoading: true, authState: 'pending' }),
          ),
          switchMap((req) =>
            authAPIService.login(req.loginRequest).pipe(
              tapResponse({
                next: (res) => {
                  patchState(store, {
                    accessToken: res.accessToken,
                    authState: 'authenticated',
                  });
                  router.navigate([req.returnUrl ?? '/']);
                },
                error: (err: HttpErrorResponse) => {
                  patchState(store, {
                    authState: 'unauthenticated',
                  });
                  req.onError(resolveErrorMessage(err));
                },
                finalize: () => {
                  patchState(store, { isLoading: false });
                },
              }),
            ),
          ),
        ),
      ),
      register: rxMethod<{
        request: RegisterRequest;
        onError: (message: string) => void;
        returnUrl?: string;
      }>(
        pipe(
          tap(() =>
            patchState(store, { isLoading: true, authState: 'pending' }),
          ),
          switchMap((req) =>
            authAPIService.register(req.request).pipe(
              tapResponse({
                next: (res) => {
                  patchState(store, {
                    accessToken: res.accessToken,
                    authState: 'authenticated',
                  });
                  router.navigate([req.returnUrl ?? '/']);
                },
                error: (err: HttpErrorResponse) => {
                  patchState(store, {
                    authState: 'unauthenticated',
                  });
                  req.onError(resolveErrorMessage(err));
                },
                finalize: () => {
                  patchState(store, { isLoading: false });
                },
              }),
            ),
          ),
        ),
      ),
      refreshToken: rxMethod<void>(
        exhaustMap(() => {
          patchState(store, {
            authState: 'pending',
          });
          return authAPIService.refreshToken().pipe(
            tapResponse({
              next: (res) =>
                patchState(store, {
                  accessToken: res.accessToken,
                  authState: 'authenticated',
                }),
              error: (err: HttpErrorResponse) => {
                patchState(store, {
                  authState: 'unauthenticated',
                });
                console.error(err);
              },
            }),
          );
        }),
      ),
      getCurrentUser: rxMethod<void>(
        switchMap(() =>
          authAPIService.getUserProfile().pipe(
            tapResponse({
              next: (res) => {
                patchState(store, {
                  currentUser: res,
                });
              },
              error: () => {
                patchState(store, {
                  currentUser: null,
                });
              },
            }),
          ),
        ),
      ),

      logout: rxMethod<void>(
        exhaustMap(() =>
          authAPIService.logout().pipe(
            finalize(() => {
              patchState(store, {
                accessToken: '',
                currentUser: null,
                authState: 'unauthenticated',
              });
              router.navigate([`/${AUTH_ROUTES.login}`], {
                queryParams: {
                  returnUrl: router.url,
                },
              });
            }),
            catchError((err) => {
              console.error(err);
              return EMPTY;
            }),
          ),
        ),
      ),
    }),
  ),
);
