import { ValueOf } from '@tphone-shop.web/utils';

export const AUTH_ROUTES = {
  login: 'login',
  register: 'register',
  forgotPassword: 'forgot-password',
  resetPassword: 'reset-password',
} as const;

export type AuthRoutes = ValueOf<typeof AUTH_ROUTES>;
