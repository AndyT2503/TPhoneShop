import { ChangeDetectionStrategy, Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { LucideHouse, LucideSmartphone } from '@lucide/angular';
@Component({
  selector: 'app-auth-layout',
  imports: [RouterOutlet, RouterLink, LucideSmartphone, LucideHouse],
  templateUrl: './auth-layout.component.html',
  styleUrl: './auth-layout.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AuthLayoutComponent {}
