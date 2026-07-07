export interface PagingQuery {
  /**
   * Min value of pageNumber is 1
   */
  pageNumber: number;
  pageSize: number;
}

export interface PagingResponse<T> {
  totalCount: number;
  items: T[];
}
