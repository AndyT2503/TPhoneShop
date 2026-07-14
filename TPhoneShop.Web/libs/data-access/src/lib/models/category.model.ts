export interface CategoryDto {
  id: string;
  parentId?: string;
  name: string;
  slug: string;
  description?: string;
  isActive: boolean;
  productCount: number;
}

export interface CreateCategoryRequest {
  parentId?: string;
  name: string;
  description?: string;
}

export interface UpdateCategoryRequest {
  id: string;
  parentId?: string;
  name: string;
  description?: string;
  isActive: boolean;
}
