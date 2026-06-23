import { AUTH_ROUTES } from '@tphone-shop.web/routing-config';
import { Routes } from '@angular/router';

export const authRoutes: Routes = [
  {
    path: AUTH_ROUTES.login,
    loadComponent: () => import('../login/login.component').then((c) => c.LoginComponent),
  },
  {
    path: AUTH_ROUTES.register,
    loadComponent: () =>
      import('../register/register.component').then((c) => c.RegisterComponent),
  },
  {
    path: AUTH_ROUTES.forgotPassword,
    loadComponent: () =>
      import('../forgot-password/forgot-password.component').then(
        (c) => c.ForgotPasswordComponent,
      ),
  },
  {
    path: AUTH_ROUTES.resetPassword,
    loadComponent: () =>
      import('../reset-password/reset-password.component').then(
        (c) => c.ResetPasswordComponent,
      ),
  },
];
