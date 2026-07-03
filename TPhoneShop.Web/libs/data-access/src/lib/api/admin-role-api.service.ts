import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { injectEnvironmentConfig } from '@tphone-shop.web/environment-config';
import { Observable } from 'rxjs';
import { ListPermissionResponse, ListRoleResponse } from '../models';

@Injectable({
  providedIn: 'root',
})
export class AdminRoleAPIService {
  private readonly envConfig = injectEnvironmentConfig();
  private readonly httpClient = inject(HttpClient);

  getRoles(): Observable<ListRoleResponse> {
    return this.httpClient.get<ListRoleResponse>(
      `${this.envConfig.commerceService}/api/admin/roles`,
    );
  }

  getPermissionsByRole(roleId: string): Observable<ListPermissionResponse> {
    return this.httpClient.get<ListPermissionResponse>(
      `${this.envConfig.commerceService}/api/admin/roles/${roleId}/permissions`,
    );
  }

  createRole(name: string): Observable<object> {
    return this.httpClient.post<object>(
      `${this.envConfig.commerceService}/api/admin/roles`,
      {
        name,
      },
    );
  }

  updateRole(id: string, name: string): Observable<object> {
    return this.httpClient.put<object>(
      `${this.envConfig.commerceService}/api/admin/roles/${id}`,
      {
        name,
      },
    );
  }

  assignPermissionToRole(
    roleId: string,
    permissionIds: string[],
  ): Observable<object> {
    return this.httpClient.post<object>(
      `${this.envConfig.commerceService}/api/admin/roles/${roleId}/permissions`,
      {
        permissionIds,
      },
    );
  }

  deleteRole(roleId: string): Observable<object> {
    return this.httpClient.delete<object>(
      `${this.envConfig.commerceService}/api/admin/roles/${roleId}`,
    );
  }
}
