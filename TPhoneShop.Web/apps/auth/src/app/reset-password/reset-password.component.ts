import { HttpErrorResponse } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  input,
  linkedSignal,
  signal,
} from '@angular/core';
import {
  form,
  FormField,
  required,
  validateTree,
} from '@angular/forms/signals';
import { RouterLink } from '@angular/router';
import {
  LucideCircleCheckBig,
  LucideDynamicIcon,
  LucideEye,
  LucideEyeOff,
  LucideLock,
} from '@lucide/angular';
import {
  AuthAPIService,
  ResetPasswordRequest,
} from '@tphone-shop.web/data-access';
import { AUTH_ROUTES } from '@tphone-shop.web/routing-config';
import { ToastService } from '@tphone-shop.web/ui';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-reset-password',
  imports: [
    LucideDynamicIcon,
    RouterLink,
    LucideLock,
    FormField,
    LucideCircleCheckBig,
  ],
  templateUrl: './reset-password.component.html',
  styleUrl: './reset-password.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResetPasswordComponent {
  private readonly toastService = inject(ToastService);
  private readonly authAPIService = inject(AuthAPIService);

  readonly isLoading = signal(false);
  readonly AUTH_ROUTES = AUTH_ROUTES;
  readonly isResetPasswordSuccess = signal(false);
  readonly isShowingPassword = signal(false);
  readonly passwordIcon = computed(() =>
    this.isShowingPassword() ? LucideEye : LucideEyeOff,
  );
  readonly token = input.required<string>();

  readonly resetPasswordModel = linkedSignal<
    ResetPasswordRequest & { confirmPassword: string }
  >(() => ({
    newPassword: '',
    confirmPassword: '',
    resetPasswordToken: this.token(),
  }));

  readonly resetPasswordForm = form(this.resetPasswordModel, (schemaPath) => {
    required(schemaPath.newPassword, { message: 'Vui lòng nhập mật khẩu' });
    required(schemaPath.confirmPassword, {
      message: 'Mật khẩu xác nhận không được để trống',
    });
    validateTree(schemaPath, (ctx) => {
      const confirmPassword = ctx.valueOf(schemaPath.confirmPassword);
      const password = ctx.valueOf(schemaPath.newPassword);
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
    if (this.resetPasswordForm().invalid()) {
      const errors = [
        ...Object.values(this.resetPasswordForm.newPassword().errors() ?? {}),
        ...Object.values(
          this.resetPasswordForm.confirmPassword().errors() ?? {},
        ),
      ];
      // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
      this.toastService.error(errors[0].message!);
      return;
    }
    const resetPasswordData = this.resetPasswordForm().value();
    this.isLoading.set(true);
    this.authAPIService
      .resetPassword(resetPasswordData)
      .pipe(
        finalize(() => {
          this.isLoading.set(false);
        }),
      )
      .subscribe({
        next: () => {
          this.isResetPasswordSuccess.set(true);
        },
        error: (err: HttpErrorResponse) => {
          this.toastService.error(err.error['message']);
        },
      });
  }
}
