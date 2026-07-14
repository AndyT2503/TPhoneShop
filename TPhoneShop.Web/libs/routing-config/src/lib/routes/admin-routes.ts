import { ValueOf } from '@tphone-shop.web/utils';
export const ADMIN_ROUTES = {
  permission: 'permission',
  brandManagement: 'brand-management',
  category: 'category',
} as const;

export type AdminRoutes = ValueOf<typeof ADMIN_ROUTES>;
