export interface RoleDto {
  id: string;
  name: string;
}

export interface ListRoleResponse {
  roles: RoleDto[];
}
