import { ProductDto } from "./product";

export interface CategoryDto {
  id: number;
  name: string;
  description: string;
  imageUrl: string;
  products: ProductDto[]
}

export interface CreateCategoryDto {
  name: string;
  description: string;
  imageUrl: string;
}

export interface UpdateCategoryDto {
  id: number;
  name?: string;
  description?: string;
  imageUrl?: string;
}