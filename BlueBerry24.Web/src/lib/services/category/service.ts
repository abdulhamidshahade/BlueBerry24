import { ICategoryService } from "./interface";
import { CategoryDto, 
         CreateCategoryDto,
         UpdateCategoryDto,
 } from "@/types/category";
import { ResponseDto } from "@/types/responseDto";
import { apiRequest } from "@/lib/utils/api";

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
const API_BASE = process.env.API_BASE_CATEGORY;

export class CategoryService implements ICategoryService{
    async getById(id: number): Promise<CategoryDto> {
        try {
            const json: ResponseDto<CategoryDto> = await apiRequest(`${API_BASE}/${id}`, {
                isPublic: true
            });
            if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
            return json.data;
        } catch (error) {
            console.error('Failed to fetch category by id:', error);
            throw new Error('Failed to fetch category');
        }
    }
    
    async getByName(name: string): Promise<CategoryDto> {
        try {
            const json: ResponseDto<CategoryDto> = await apiRequest(`${API_BASE}/name/${name}`, {
                isPublic: true
            });
            if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
            return json.data;
        } catch (error) {
            console.error('Failed to fetch category by name:', error);
            throw new Error('Failed to fetch category');
        }
    }
    
    async getAll(): Promise<CategoryDto[]> {
        try {
            const json: ResponseDto<CategoryDto[]> = await apiRequest(`${API_BASE}`, {
                isPublic: true
            });
            if(!json.isSuccess || !json.data) {
                console.warn('Categories API returned no data, returning empty array');
                return [];
            }
            return json.data;
        } catch (error) {
            console.error('Failed to fetch categories:', error);
            return [];
        }
    }
    
    async create(data: CreateCategoryDto): Promise<CategoryDto> {
        try {
            const json: ResponseDto<CategoryDto> = await apiRequest(`${API_BASE}`, {
                method: "POST",
                body: JSON.stringify(data),
                requireAuth: true
            });
            if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
            return json.data;
        } catch (error) {
            console.error('Failed to create category:', error);
            throw new Error('Failed to create category');
        }
    }
    
    async update(id: number, data: UpdateCategoryDto): Promise<CategoryDto> {
        try {
            const json: ResponseDto<CategoryDto> = await apiRequest(`${API_BASE}/${id}`, {
                method: "PUT",
                body: JSON.stringify(data),
                requireAuth: true
            });
            if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
            return json.data;
        } catch (error) {
            console.error('Failed to update category:', error);
            throw new Error('Failed to update category');
        }
    }
    
    async delete(id: number): Promise<boolean> {
        try {
            const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/${id}`, { 
                method: "DELETE",
                requireAuth: true
            });
            if (!json.isSuccess) throw new Error(json.statusMessage);
            return json.data ?? false;
        } catch (error) {
            console.error('Failed to delete category:', error);
            throw new Error('Failed to delete category');
        }
    }

    async exists(id: number): Promise<boolean> {
        try {
            const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/exists-by-id/${id}`, {
                requireAuth: true
            });
            return json.isSuccess && json.data;
        } catch (error) {
            console.error('Failed to check category existence:', error);
            return false;
        }
    }

    async existsByName(name: string): Promise<boolean> {
        try {
            const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/exists-by-name/${encodeURIComponent(name)}`, {
                requireAuth: true
            });
            return json.isSuccess && json.data;
        } catch (error) {
            console.error('Failed to check category existence by name:', error);
            return false;
        }
    }
}