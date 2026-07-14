import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { injectEnvironmentConfig } from '@tphone-shop.web/environment-config';
import {
  CategoryDto,
  CreateCategoryRequest,
  UpdateCategoryRequest,
} from '../models';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AdminCategoryAPIService {
  private readonly httpClient = inject(HttpClient);
  private readonly envConfig = injectEnvironmentConfig();

  getCategories(search?: string): Observable<CategoryDto[]> {
    const params: Record<string, string> = {};
    if (search) {
      params['search'] = search;
    }
    return this.httpClient.get<CategoryDto[]>(
      `${this.envConfig.commerceService}/api/admin/categories`,
      { params }
    );
  }

  createCategory(request: CreateCategoryRequest): Observable<object> {
    return this.httpClient.post(
      `${this.envConfig.commerceService}/api/admin/categories`,
      request
    );
  }

  updateCategory(id: string, request: UpdateCategoryRequest): Observable<object> {
    return this.httpClient.put(
      `${this.envConfig.commerceService}/api/admin/categories/${id}`,
      request
    );
  }

  deleteCategory(id: string): Observable<object> {
    return this.httpClient.delete(
      `${this.envConfig.commerceService}/api/admin/categories/${id}`
    );
  }
}
