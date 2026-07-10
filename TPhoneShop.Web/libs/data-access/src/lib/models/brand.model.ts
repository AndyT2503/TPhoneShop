import { PagingQuery } from './paging.model';

export interface BrandDto {
  id: string;
  name: string;
  slug: string;
  description: string;
  logoUrl: string;
}

export interface BrandForAdminDto extends BrandDto {
  isActive: boolean;
  logoId: string;
}

export interface CreateBrandRequest {
  name: string;
  description: string;
  logoId: string;
}

export interface GetBrandsQuery extends PagingQuery {
  search?: string;
  isActive: boolean;
}

export interface UpdateBrandRequest {
  name: string;
  description: string;
  logoId: string;
  isActive: boolean;
}
