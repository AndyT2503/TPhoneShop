import { HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { tapResponse } from '@ngrx/operators';
import { patchState, signalStore, withMethods, withState } from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { ToastService } from '@tphone-shop.web/ui-toast';
import { exhaustMap, from, pipe, switchMap, tap } from 'rxjs';
import { AuthAPIService } from '../api';
import { FirebaseAuthService } from '../firebase/firebase-auth.service';
import { LoginRequest, RegisterRequest } from '../models';

type AuthState = {
  accessToken: string;
  isLoading: boolean;
};

const initialState: AuthState = {
  accessToken: '',
  isLoading: false,
};

export const AuthStore = signalStore(
  { providedIn: 'root' },
  withState(initialState),
  withMethods(
    (
      store,
      authAPIService = inject(AuthAPIService),
      router = inject(Router),
      toastService = inject(ToastService),
      firebaseAuthService = inject(FirebaseAuthService),
    ) => ({
      googleLogin: rxMethod<void>(
        pipe(
          tap(() => patchState(store, { isLoading: true })),
          exhaustMap(() =>
            from(firebaseAuthService.signInWithGoogle()).pipe(
              switchMap((idToken) => authAPIService.externalLogin({ idToken })),
              tapResponse({
                next: (res) => {
                  patchState(store, {
                    accessToken: res.accessToken,
                  });
                  router.navigate(['/']);
                },
                error: (err) => {
                  if (err instanceof HttpErrorResponse) {
                    toastService.error(err.error['message']);
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
      login: rxMethod<LoginRequest>(
        pipe(
          tap(() => patchState(store, { isLoading: true })),
          switchMap((req) =>
            authAPIService.login(req).pipe(
              tapResponse({
                next: (res) => {
                  patchState(store, {
                    accessToken: res.accessToken,
                  });
                  router.navigate(['/']);
                },
                error: (err: HttpErrorResponse) => {
                  toastService.error(err.error['message']);
                },
                finalize: () => {
                  patchState(store, { isLoading: false });
                },
              }),
            ),
          ),
        ),
      ),
      register: rxMethod<RegisterRequest>(
        pipe(
          tap(() => patchState(store, { isLoading: true })),
          switchMap((req) =>
            authAPIService.register(req).pipe(
              tapResponse({
                next: (res) => {
                  patchState(store, {
                    accessToken: res.accessToken,
                  });
                  router.navigate(['/']);
                },
                error: (err: HttpErrorResponse) => {
                  toastService.error(err.error['message']);
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
        switchMap(() =>
          authAPIService.refreshToken().pipe(
            tapResponse({
              next: (res) =>
                patchState(store, {
                  accessToken: res.accessToken,
                }),
              error: (err: HttpErrorResponse) => {
                console.error(err);
              },
            }),
          ),
        ),
      ),
    }),
  ),
);
