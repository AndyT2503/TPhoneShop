import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { injectEnvironmentConfig } from '@tphone-shop.web/environment-config';
import { Observable } from 'rxjs';
import { ListPermissionResponse } from '../models';

@Injectable({
  providedIn: 'root',
})
export class PermissionAPIService {
  private readonly envConfig = injectEnvironmentConfig();
  private readonly httpClient = inject(HttpClient);

  getPermissions(): Observable<ListPermissionResponse> {
    return this.httpClient.get<ListPermissionResponse>(
      `${this.envConfig.commerceService}/api/permissions`,
    );
  }
}
