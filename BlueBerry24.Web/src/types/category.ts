export interface CategoryDto{
    id: number;
    name: string;
    description: string;
    imageUrl: string;
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