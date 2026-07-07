import {
  ChangeDetectionStrategy,
  Component,
  inject,
  input,
} from '@angular/core';
import { AuthStore } from '@tphone-shop.web/data-access';
import { ToastService } from '@tphone-shop.web/ui';

@Component({
  selector: 'app-external-login-btn',
  imports: [],
  templateUrl: './external-login-btn.component.html',
  styleUrl: './external-login-btn.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExternalLoginBtnComponent {
  private readonly toastService = inject(ToastService);
  readonly authStore = inject(AuthStore);
  readonly returnUrl = input<string>();

  login(): void {
    this.authStore.googleLogin({
      onError: (message) => {
        this.toastService.error(message);
      },
      returnUrl: this.returnUrl(),
    });
  }
}
