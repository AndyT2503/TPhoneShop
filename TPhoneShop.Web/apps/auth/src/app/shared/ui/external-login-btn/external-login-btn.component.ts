import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { AuthStore } from '@tphone-shop.web/data-access';

@Component({
  selector: 'app-external-login-btn',
  imports: [],
  templateUrl: './external-login-btn.component.html',
  styleUrl: './external-login-btn.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExternalLoginBtnComponent {
  readonly authStore = inject(AuthStore);
}
