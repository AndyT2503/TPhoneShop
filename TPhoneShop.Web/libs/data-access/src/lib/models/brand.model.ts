import { PagingQuery } from './paging.model';

export interface BrandDto {
  id: string;
  name: string;
  slug: string;
  description: string;
  logoUrl: string;
  isActive: boolean;
}

export interface CreateBrandRequest {
  name: string;
  description: string;
  logoId: string;
}

export interface GetBrandsQuery extends PagingQuery {
  search?: string;
  isActive?: boolean;
}
