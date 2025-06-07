import { CategoryDto, CreateCategoryDto, UpdateCategoryDto } from "@/types/category";
import { ICategoryService } from "./interface";
import { ResponseDto } from "@/types/responseDto";
import { apiRequest } from "@/lib/utils/api";

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
const API_BASE = process.env.API_BASE_CATEGORY;

export class CategoryService implements ICategoryService {
  async getAll(): Promise<CategoryDto[]> {
    try {
      const json: ResponseDto<CategoryDto[]> = await apiRequest(`${API_BASE}`, {
        headers:{
        'Content-Type': 'application/json',
      },
        cache: 'no-store',
        isPublic: true,
      });
      
      if (!json.isSuccess || !json.data) {
        throw new Error(json.statusMessage || 'Failed to fetch categories');
      }
      return json.data;
    } catch (error) {
      console.error('Failed to fetch categories:', error);
      throw new Error('Failed to fetch categories');
    }
  }

  async getById(id: number): Promise<CategoryDto> {
    try {
      const json: ResponseDto<CategoryDto> = await apiRequest(`${API_BASE}/${id}`, {
        headers:{
        'Content-Type': 'application/json',
        },
        cache: 'no-store',
        isPublic: true
      });
      
      if (!json.isSuccess || !json.data) {
        throw new Error(json.statusMessage || 'Failed to fetch category');
      }
      return json.data;
    } catch (error) {
      console.error('Failed to fetch category by id:', error);
      throw new Error('Failed to fetch category');
    }
  }

  async getByName(name: string): Promise<CategoryDto>{
    try {
      const json: ResponseDto<CategoryDto> = await apiRequest(`${API_BASE}/name/${name}`, {
        headers:{
        'Content-Type': 'application/json',
        },
        cache: 'no-store',
        isPublic: true
      });
      
      if (!json.isSuccess || !json.data) {
        throw new Error(json.statusMessage || 'Failed to fetch category');
      }
      return json.data;
    } catch (error) {
      console.error('Failed to fetch category by id:', error);
      throw new Error('Failed to fetch category');
    }
  }

  async create(data: CreateCategoryDto): Promise<CategoryDto> {
    try {
      const json: ResponseDto<CategoryDto> = await apiRequest(`${API_BASE}`, {
        method: 'POST',
        body: JSON.stringify(data),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      
      if (!json.isSuccess || !json.data) {
        throw new Error(json.statusMessage || 'Failed to create category');
      }
      return json.data;
    } catch (error) {
      console.error('Failed to create category:', error);
      throw new Error('Failed to create category');
    }
  }

  async update(id: number, data: UpdateCategoryDto): Promise<CategoryDto> {
    try {
      const json: ResponseDto<CategoryDto> = await apiRequest(`${API_BASE}/${id}`, {
        method: 'PUT',
        body: JSON.stringify(data),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      
      if (!json.isSuccess || !json.data) {
        throw new Error(json.statusMessage || 'Failed to update category');
      }
      return json.data;
    } catch (error) {
      console.error('Failed to update category:', error);
      throw new Error('Failed to update category');
    }
  }

  async delete(id: number): Promise<boolean> {
    try {
      const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/${id}`, {
        method: 'DELETE',
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      
      if (!json.isSuccess) {
        throw new Error(json.statusMessage || 'Failed to delete category');
      }
      return json.data || true;
    } catch (error) {
      console.error('Failed to delete category:', error);
      throw new Error('Failed to delete category');
    }
  }

  async existsById(id: number): Promise<boolean> {
    try{
    const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/exists-by-id/${id}`, {
      headers:{
        'Content-Type': 'application/json',
      },
      requireAuth: true,
    });

    if (!json.isSuccess) {
        throw new Error(json.statusMessage);
      }
      return json.data || true;
    } catch (error) {
      console.error(`Failed to check existence of category by id: ${id}`, error);
      throw new Error(`Failed to check existence of category by id: ${id}`);
    }
  }

  async existsByName(name: string): Promise<boolean> {
    try{
    const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/exists-by-name/${name}`, {
      headers:{
        'Content-Type': 'application/json',
      },
      requireAuth: true,
    });

    if (!json.isSuccess) {
        throw new Error(json.statusMessage);
      }
      return json.data || true;
    } catch (error) {
      console.error(`Failed to check existence of category by name: ${name}`, error);
      throw new Error(`Failed to check existence of category by name: ${name}`);
    }
  }
}