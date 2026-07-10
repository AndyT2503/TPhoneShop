import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { injectEnvironmentConfig } from '@tphone-shop.web/environment-config';
import { Observable } from 'rxjs';
import {
  BrandForAdminDto,
  CreateBrandRequest,
  GetBrandsQuery,
  PagingResponse,
  UpdateBrandRequest
} from '../models';

@Injectable({
  providedIn: 'root',
})
export class AdminBrandAPIService {
  private readonly httpClient = inject(HttpClient);
  private readonly envConfig = injectEnvironmentConfig();

  createBrand(request: CreateBrandRequest): Observable<object> {
    return this.httpClient.post(
      `${this.envConfig.commerceService}/api/admin/brands`,
      request,
    );
  }

  getBrands(
    query: GetBrandsQuery,
  ): Observable<PagingResponse<BrandForAdminDto>> {
    return this.httpClient.get<PagingResponse<BrandForAdminDto>>(
      `${this.envConfig.commerceService}/api/admin/brands`,
      {
        params: { ...query },
      },
    );
  }

  updateBrand(id: string, request: UpdateBrandRequest): Observable<object> {
    return this.httpClient.put(
      `${this.envConfig.commerceService}/api/admin/brands/${id}`,
      request,
    );
  }

  deleteBrand(id: string): Observable<object> {
    return this.httpClient.delete(
      `${this.envConfig.commerceService}/api/admin/brands/${id}`,
    );
  }
}
