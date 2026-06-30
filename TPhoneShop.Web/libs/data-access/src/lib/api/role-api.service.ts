import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { injectEnvironmentConfig } from '@tphone-shop.web/environment-config';
import { Observable, of } from 'rxjs';
import { ListPermissionResponse, ListRoleResponse } from '../models';

@Injectable({
  providedIn: 'root',
})
export class RoleAPIService {
  private readonly envConfig = injectEnvironmentConfig();
  private readonly httpClient = inject(HttpClient);

  getRoles(): Observable<ListRoleResponse> {
    return this.httpClient.get<ListRoleResponse>(
      `${this.envConfig.commerceService}/api/roles`,
    );
  }

  getPermissionsByRole(roleId: string): Observable<ListPermissionResponse> {
    return this.httpClient.get<ListPermissionResponse>(
      `${this.envConfig.commerceService}/api/roles/${roleId}/permissions`,
    );
  }

  createRole(name: string): Observable<object> {
    return this.httpClient.post<object>(
      `${this.envConfig.commerceService}/api/roles`,
      {
        name,
      },
    );
  }

  updateRole(id: string, name: string): Observable<object> {
    return this.httpClient.put<object>(
      `${this.envConfig.commerceService}/api/roles/${id}`,
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
      `${this.envConfig.commerceService}/api/roles/${roleId}/permissions`,
      {
        permissionIds,
      },
    );
  }

  deleteRole(roleId: string): Observable<object> {
    return this.httpClient.delete<object>(
      `${this.envConfig.commerceService}/api/roles/${roleId}`,
    );
  }
}
