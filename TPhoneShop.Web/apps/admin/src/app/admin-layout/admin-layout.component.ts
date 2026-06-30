import {
  ChangeDetectionStrategy,
  Component,
  inject,
  OnInit,
  signal,
} from '@angular/core';
import {
  ActivatedRoute,
  NavigationEnd,
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet,
} from '@angular/router';
import {
  LucideBell,
  LucideChevronRight,
  LucideClipboardList,
  LucideLayoutDashboard,
  LucideLogOut,
  LucideMenu,
  LucidePackage,
  LucideSettings,
  LucideShoppingCart,
  LucideSmartphone,
  LucideUsers,
  LucideX,
} from '@lucide/angular';
import { AuthStore } from '@tphone-shop.web/data-access';
import { UserAvatarComponent } from '@tphone-shop.web/ui';
import { AdminMenuComponent } from './ui/admin-menu/admin-menu.component';
import { toSignal } from '@angular/core/rxjs-interop';
import { filter, map, tap } from 'rxjs';

@Component({
  selector: 'app-admin-layout',
  imports: [
    RouterLink,
    RouterOutlet,
    LucideBell,
    LucideLogOut,
    LucideMenu,
    LucideSmartphone,
    LucideX,
    UserAvatarComponent,
    AdminMenuComponent,
  ],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminLayoutComponent implements OnInit {
  private readonly authStore = inject(AuthStore);
  private readonly router = inject(Router);
  private readonly activatedRoute = inject(ActivatedRoute);

  readonly currentUser = this.authStore.currentUser;
  readonly sidebarOpen = signal(true);
  readonly title = toSignal(
    this.router.events.pipe(
      filter((event) => event instanceof NavigationEnd),
      map(() => {
        let current = this.activatedRoute;

        while (current.firstChild) {
          current = current.firstChild;
        }

        return current.snapshot.data;
      }),
      map((data) => data['title'] || 'Quản trị'),
    ),
  );

  ngOnInit(): void {
    this.authStore.getCurrentUser();
  }

  toggleSidebar(): void {
    this.sidebarOpen.update((isOpen) => !isOpen);
  }

  logout(): void {
    this.authStore.logout();
  }
}
