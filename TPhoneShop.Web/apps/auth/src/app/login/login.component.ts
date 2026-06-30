import { AUTH_ROUTES } from '@tphone-shop.web/routing-config';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  input,
  signal,
} from '@angular/core';
import { email, form, FormField, required } from '@angular/forms/signals';
import { RouterLink } from '@angular/router';
import {
  LucideDynamicIcon,
  LucideEye,
  LucideEyeOff,
  LucideLock,
  LucideMail,
} from '@lucide/angular';
import { AuthStore, LoginRequest } from '@tphone-shop.web/data-access';
import { ToastService } from '@tphone-shop.web/ui-toast';
import { ExternalLoginBtnComponent } from '../shared/ui';
import { ShopLogoComponent } from '@tphone-shop.web/ui';

@Component({
  selector: 'app-login',
  imports: [
    LucideLock,
    LucideDynamicIcon,
    RouterLink,
    LucideMail,
    ExternalLoginBtnComponent,
    FormField,
    ShopLogoComponent,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
  private readonly toastService = inject(ToastService);
  private readonly authStore = inject(AuthStore);

  readonly returnUrl = input<string>();
  readonly isLoading = this.authStore.isLoading;
  readonly AUTH_ROUTES = AUTH_ROUTES;
  readonly isShowingPassword = signal(false);
  readonly passwordIcon = computed(() =>
    this.isShowingPassword() ? LucideEye : LucideEyeOff,
  );
  readonly loginModel = signal<LoginRequest>({
    email: '',
    password: '',
  });

  readonly loginForm = form(this.loginModel, (schemaPath) => {
    required(schemaPath.email, { message: 'Vui lòng nhập email' });
    email(schemaPath.email, { message: 'Địa chỉ email không hợp lệ' });
    required(schemaPath.password, { message: 'Vui lòng nhập mật khẩu' });
  });

  onSubmit(e: Event): void {
    e.preventDefault();
    if (this.loginForm().invalid()) {
      const errors = [
        ...Object.values(this.loginForm.email().errors() ?? {}),
        ...Object.values(this.loginForm.password().errors() ?? {}),
      ];
      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      this.toastService.error(errors[0].message!);
      return;
    }
    const loginData = this.loginForm().value();
    this.authStore.login({
      loginRequest: loginData,
      returnUrl: this.returnUrl(),
    });
  }
}
