import { CategoryDto, CreateCategoryDto, UpdateCategoryDto } from "@/types/category";

export interface ICategoryService {
  getAll(): Promise<CategoryDto[]>;
  getById(id: number): Promise<CategoryDto>;
  getByName(name: string): Promise<CategoryDto>;
  create(data: CreateCategoryDto): Promise<CategoryDto>;
  update(id: number, data: UpdateCategoryDto): Promise<CategoryDto>;
  delete(id: number): Promise<boolean>;
  existsById(id: number): Promise<boolean>;
  existsByName(name: string): Promise<boolean>;
} 