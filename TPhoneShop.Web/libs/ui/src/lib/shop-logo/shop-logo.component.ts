import { LucideSmartphone } from '@lucide/angular';
import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'lib-shop-logo',
  imports: [LucideSmartphone],
  templateUrl: './shop-logo.component.html',
  styleUrl: './shop-logo.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ShopLogoComponent {}
