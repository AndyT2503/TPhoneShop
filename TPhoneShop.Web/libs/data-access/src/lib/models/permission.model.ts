export interface PermissionDto {
  id: string;
  name: string;
}

export interface ListPermissionResponse {
  permissions: PermissionDto[]
}
