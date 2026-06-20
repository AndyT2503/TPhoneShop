import { ChangeDetectionStrategy, Component } from '@angular/core';
import { LucideSmartphone } from '@lucide/angular';

@Component({
  selector: 'app-auth-layout',
  imports: [LucideSmartphone],
  templateUrl: './auth-layout.component.html',
  styleUrl: './auth-layout.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthLayoutComponent {}
