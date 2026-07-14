import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { injectEnvironmentConfig } from '@tphone-shop.web/environment-config';
import {
  BrandDto,
  CreateBrandRequest,
  GetBrandsQuery,
  PagingResponse,
} from '../models';
import { Observable } from 'rxjs';

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

  getBrands(query: GetBrandsQuery): Observable<PagingResponse<BrandDto>> {
    return this.httpClient.get<PagingResponse<BrandDto>>(
      `${this.envConfig.commerceService}/api/admin/brands`,
      {
        params: { ...query },
      },
    );
  }
}
