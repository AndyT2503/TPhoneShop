import {
  ChangeDetectionStrategy,
  Component,
  inject,
  input,
} from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import {
  LucideAward,
  LucideChevronRight,
  LucideDynamicIcon,
  LucideIconInput,
  LucideShieldCheck,
} from '@lucide/angular';
import { ADMIN_ROUTES, AdminRoutes } from '@tphone-shop.web/routing-config';

interface MenuItem {
  title: string;
  icon: LucideIconInput;
  link: AdminRoutes;
}

const MENU: MenuItem[] = [
  {
    title: 'Phân quyền',
    icon: LucideShieldCheck,
    link: ADMIN_ROUTES.permission,
  },
  {
    title: 'Nhãn hàng',
    icon: LucideAward,
    link: ADMIN_ROUTES.brandManagement,
  },
];

@Component({
  selector: 'app-admin-menu',
  imports: [
    RouterLink,
    RouterLinkActive,
    LucideDynamicIcon,
    LucideChevronRight,
  ],
  templateUrl: './admin-menu.component.html',
  styleUrl: './admin-menu.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminMenuComponent {
  private readonly router = inject(Router);

  readonly sidebarOpen = input.required<boolean>();
  readonly adminMenu = MENU;

  isRouteActive(path: string): boolean {
    return this.router.url.includes(path);
  }
}
