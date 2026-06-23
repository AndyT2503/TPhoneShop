import { HttpErrorResponse } from '@angular/common/http';
import {
  ChangeDetectionStrategy,
  Component,
  inject,
  model,
  signal,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { LucideArrowLeft, LucideMail } from '@lucide/angular';
import { AuthAPIService } from '@tphone-shop.web/data-access';
import { AUTH_ROUTES } from '@tphone-shop.web/routing-config';
import { ShopLogoComponent } from '@tphone-shop.web/ui';
import { ToastService } from '@tphone-shop.web/ui-toast';

@Component({
  selector: 'app-forgot-password',
  imports: [
    LucideMail,
    LucideArrowLeft,
    FormsModule,
    RouterLink,
    ShopLogoComponent,
  ],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ForgotPasswordComponent {
  private readonly toastService = inject(ToastService);
  private readonly authAPIService = inject(AuthAPIService);
  readonly AUTH_ROUTES = AUTH_ROUTES;
  readonly emailSent = signal(false);
  readonly email = model('');

  onSubmit() {
    const email = this.email();
    if (!email) {
      this.toastService.error('Vui lòng nhập email của bạn');
      return;
    }
    this.authAPIService.forgotPassword({ email }).subscribe({
      next: () => {
        this.emailSent.set(true);
      },
      error: (err: HttpErrorResponse) => {
        this.toastService.error(err.error['message']);
      },
    });
  }
}
