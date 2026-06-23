import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  signal,
} from '@angular/core';
import {
  email,
  form,
  FormField,
  required,
  validate,
  validateTree,
} from '@angular/forms/signals';
import { RouterLink } from '@angular/router';
import {
  LucideDynamicIcon,
  LucideEye,
  LucideEyeOff,
  LucideLock,
  LucideMail,
  LucideUser,
} from '@lucide/angular';
import { AuthStore, RegisterRequest } from '@tphone-shop.web/data-access';
import { ToastService } from '@tphone-shop.web/ui-toast';
import { ExternalLoginBtnComponent } from '../shared/ui';
import { ShopLogoComponent } from '@tphone-shop.web/ui';
import { AUTH_ROUTES } from '@tphone-shop.web/routing-config';

@Component({
  selector: 'app-register',
  imports: [
    LucideLock,
    LucideDynamicIcon,
    RouterLink,
    LucideMail,
    ExternalLoginBtnComponent,
    LucideUser,
    FormField,
    ShopLogoComponent
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegisterComponent {
  private readonly toastService = inject(ToastService);
  private readonly authStore = inject(AuthStore);

  readonly isLoading = this.authStore.isLoading;
  readonly AUTH_ROUTES = AUTH_ROUTES;
  readonly isShowingPassword = signal(false);
  readonly passwordIcon = computed(() =>
    this.isShowingPassword() ? LucideEye : LucideEyeOff,
  );
  readonly registerModel = signal<
    RegisterRequest & { confirmPassword: string }
  >({
    email: '',
    password: '',
    fullName: '',
    confirmPassword: '',
  });

  readonly registerForm = form(this.registerModel, (schemaPath) => {
    required(schemaPath.email, { message: 'Vui lòng nhập email' });
    required(schemaPath.fullName, { message: 'Vui lòng nhập họ và tên' });
    email(schemaPath.email, { message: 'Địa chỉ email không hợp lệ' });
    required(schemaPath.password, { message: 'Vui lòng nhập mật khẩu' });
    required(schemaPath.confirmPassword, {
      message: 'Mật khẩu xác nhận không được để trống',
    });
    validateTree(schemaPath, (ctx) => {
      const confirmPassword = ctx.valueOf(schemaPath.confirmPassword);
      const password = ctx.valueOf(schemaPath.password);
      if (confirmPassword !== password) {
        return {
          kind: 'confirmPassword',
          message: 'Mật khẩu xác nhận phải giống mật khẩu đã nhập',
          fieldTree: ctx.fieldTree.confirmPassword,
        };
      }
      return null;
    });
  });

  onSubmit(e: Event): void {
    e.preventDefault();
    if (this.registerForm().invalid()) {
      const errors = [
        ...Object.values(this.registerForm.fullName().errors() ?? {}),
        ...Object.values(this.registerForm.email().errors() ?? {}),
        ...Object.values(this.registerForm.password().errors() ?? {}),
        ...Object.values(this.registerForm.confirmPassword().errors() ?? {}),
      ];
      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      this.toastService.error(errors[0].message!);
      return;
    }
    const registerData = this.registerForm().value();
    this.authStore.register(registerData);
  }
}
