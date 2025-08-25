export interface PaginationDto<T> {
  data: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  firstItemOnPage: number;
  lastItemOnPage: number;
}

export interface ProductFilterDto {
  pageNumber: number;
  pageSize: number;
  searchTerm?: string;
  category?: string;
  sortBy?: string;
  minPrice?: number;
  maxPrice?: number;
  isActive?: boolean;
  includeInactive?: boolean;
} 