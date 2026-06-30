import { ChangeDetectionStrategy, Component } from '@angular/core';
import { PermissionStore } from './store/permission.store';
import { PermissionPanelComponent } from './ui/permission-panel/permission-panel.component';
import { RolePanelComponent } from './ui/role-panel/role-panel.component';

@Component({
  selector: 'app-permission',
  imports: [RolePanelComponent, PermissionPanelComponent],
  templateUrl: './permission.component.html',
  styleUrl: './permission.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [PermissionStore],
})
export class PermissionComponent {
}
