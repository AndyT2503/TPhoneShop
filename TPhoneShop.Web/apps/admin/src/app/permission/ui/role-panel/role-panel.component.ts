import {
  ChangeDetectionStrategy,
  Component,
  inject,
  model,
  signal
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  LucideCheck,
  LucidePen,
  LucidePlus,
  LucideShieldCheck,
  LucideTrash2,
  LucideX,
} from '@lucide/angular';
import { RoleDto } from '@tphone-shop.web/data-access';
import { PermissionStore } from '../../store/permission.store';
import { ConfirmPopoverDirective } from '@tphone-shop.web/ui';

@Component({
  selector: 'app-role-panel',
  imports: [
    LucideCheck,
    LucidePen,
    LucidePlus,
    LucideShieldCheck,
    LucideTrash2,
    LucideX,
    FormsModule,
    ConfirmPopoverDirective
  ],
  templateUrl: './role-panel.component.html',
  styleUrl: './role-panel.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RolePanelComponent {
  private readonly permissionStore = inject(PermissionStore);

  readonly roles = this.permissionStore.roles;
  readonly selectedRole = this.permissionStore.selectedRole;
  readonly editingRoleId = signal<string | null>(null);
  readonly editName = model('');
  readonly newRoleName = model('');
  readonly showNewRole = signal(false);

  toggleNewRole(): void {
    if (this.showNewRole()) {
      this.newRoleName.set('');
    }
    this.showNewRole.update((isShow) => !isShow);
  }

  addRole(): void {
    this.permissionStore.addRole({
      name: this.newRoleName(),
      onSuccess: () => {
        this.newRoleName.set('');
      },
    });
    this.cancelNewRole();
  }

  cancelNewRole(): void {
    this.newRoleName.set('');
    this.showNewRole.set(false);
  }

  selectRole(role: RoleDto): void {
    this.permissionStore.selectRole(role);
  }

  startEdit(role: RoleDto): void {
    this.editingRoleId.set(role.id);
    this.editName.set(role.name);
  }

  saveEdit(): void {
    const editingRoleId = this.editingRoleId();
    if (!editingRoleId) {
      return;
    }
    this.permissionStore.updateRole({
      roleId: editingRoleId,
      name: this.editName(),
      onSuccess: () => {
        this.cancelEdit();
      },
    });
  }

  cancelEdit(): void {
    this.editingRoleId.set(null);
    this.editName.set('');
  }

  deleteRole(roleId: string): void {
    this.permissionStore.deleteRole({ roleId });
  }
}
