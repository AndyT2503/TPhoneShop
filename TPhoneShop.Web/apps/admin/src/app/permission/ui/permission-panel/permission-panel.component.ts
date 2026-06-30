import {
  ChangeDetectionStrategy,
  Component,
  computed,
  inject,
  linkedSignal,
} from '@angular/core';
import {
  LucideCheck,
  LucideChevronDown,
  LucideChevronRight,
  LucideSave,
} from '@lucide/angular';
import { PermissionDto } from '@tphone-shop.web/data-access';
import { PermissionStore } from '../../store/permission.store';

interface PermissionGroup {
  label: string;
  permissions: PermissionDto[];
}

interface ActionMeta {
  label: string;
  modifier: string;
}


const ACTION_META: Record<string, ActionMeta> = {
  read: {
    label: 'Xem',
    modifier: 'read',
  },
  create: {
    label: 'Tạo mới',
    modifier: 'create',
  },
  update: {
    label: 'Cập nhật',
    modifier: 'update',
  },
  delete: {
    label: 'Xóa',
    modifier: 'delete',
  },
  ['assign-permission']: {
    label: 'Phân quyền',
    modifier: 'default'
  }
};

@Component({
  selector: 'app-permission-panel',
  imports: [LucideCheck, LucideChevronDown, LucideChevronRight, LucideSave],
  templateUrl: './permission-panel.component.html',
  styleUrl: './permission-panel.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PermissionPanelComponent {
  private readonly permissionStore = inject(PermissionStore);

  readonly allPermissions = this.permissionStore.permissions;

  readonly groups = computed(() => {
    const allPermissions = this.allPermissions.value();
    let permissionGroups: PermissionGroup[] = [];
    allPermissions.forEach((p) => {
      const group = p.name.split('.')[0];
      const permissionGroup = permissionGroups.find(
        (g) => g.label === group.toUpperCase(),
      );
      if (permissionGroup) {
        permissionGroup.permissions = [...permissionGroup.permissions, p];
      } else {
        permissionGroups = [
          ...permissionGroups,
          {
            label: group.toUpperCase(),
            permissions: [p],
          },
        ];
      }
    });
    return permissionGroups;
  });
  readonly permissionsOfSelectedRole =
    this.permissionStore.permissionsOfSelectedRole;
  readonly activePerms = linkedSignal(() =>
    this.permissionsOfSelectedRole.value(),
  );
  readonly dirty = computed(
    () =>
      this.permissionsOfSelectedRole.value().length !==
      this.activePerms().length,
  );
  readonly selectedRole = this.permissionStore.selectedRole;
  readonly expandedGroups = computed(() => {
    return new Set<string>(this.groups().map((g) => g.label));
  });

  selectedInGroup(group: PermissionGroup): PermissionDto[] {
    return group.permissions.filter((permission) =>
      this.activePerms().some(
        (activePermission) => activePermission.id === permission.id,
      ),
    );
  }

  isExpanded(group: PermissionGroup): boolean {
    return this.expandedGroups().has(group.label);
  }

  isAllSelected(group: PermissionGroup): boolean {
    return this.selectedInGroup(group).length === group.permissions.length;
  }

  isSomeSelected(group: PermissionGroup): boolean {
    const selectedCount = this.selectedInGroup(group).length;

    return selectedCount > 0 && selectedCount < group.permissions.length;
  }

  isChecked(permission: PermissionDto): boolean {
    return this.activePerms().some(
      (activePermission) => activePermission.id === permission.id,
    );
  }

  actionOf(permission: PermissionDto): string {
    return permission.name.split('.')[1] ?? permission.name;
  }

  actionMeta(permission: PermissionDto): ActionMeta {
    return (
      ACTION_META[this.actionOf(permission)] ?? {
        label: this.actionOf(permission).split('-').join(' '),
        modifier: 'default',
      }
    );
  }

  toggleExpandGroup(group: PermissionGroup): void {
    if (this.expandedGroups().has(group.label)) {
      this.expandedGroups().delete(group.label);
    } else {
      this.expandedGroups().add(group.label);
    }
  }

  toggleGroupPerms(group: PermissionGroup): void {
    const selectedPermissions = this.activePerms();

    if (this.isAllSelected(group)) {
      const groupPermissionIds = new Set(group.permissions.map((p) => p.id));
      this.activePerms.set(
        selectedPermissions.filter((p) => !groupPermissionIds.has(p.id)),
      );
      return;
    }

    const selectedPermissionIds = new Set(selectedPermissions.map((p) => p.id));

    this.activePerms.set([
      ...selectedPermissions,
      ...group.permissions.filter((p) => !selectedPermissionIds.has(p.id)),
    ]);
  }

  togglePerm(permission: PermissionDto): void {
    const currentActivePermissions = this.activePerms();
    const currentActivePermissionIds = new Set(
      currentActivePermissions.map((p) => p.id),
    );
    const isExist = currentActivePermissionIds.has(permission.id);
    this.activePerms.set(
      isExist
        ? currentActivePermissions.filter((p) => p.id !== permission.id)
        : [...currentActivePermissions, permission],
    );
  }

  savePerms(): void {
    const roleId = this.selectedRole()?.id;
    const activePermissions = this.activePerms().map((p) => p.id);

    if (!roleId) {
      return;
    }
    this.permissionStore.savePermissions({
      roleId,
      permissions: activePermissions,
    });
  }

  discardChanges(): void {
    this.activePerms.set(this.permissionsOfSelectedRole.value());
  }
}
