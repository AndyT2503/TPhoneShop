import { HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { tapResponse } from '@ngrx/operators';
import { patchState, signalStore, withMethods, withState } from '@ngrx/signals';
import { rxMethod } from '@ngrx/signals/rxjs-interop';
import { ToastService } from '@tphone-shop.web/ui-toast';
import { switchMap } from 'rxjs';
import {
  ForgotPasswordRequest,
  LoginRequest,
  RegisterRequest,
} from '../models';
import { AuthAPIService } from '../services';

type AuthState = {
  accessToken: string;
};

const initialState: AuthState = {
  accessToken: '',
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
    ) => ({
      login: rxMethod<LoginRequest>(
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
            }),
          ),
        ),
      ),
      register: rxMethod<RegisterRequest>(
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
            }),
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
