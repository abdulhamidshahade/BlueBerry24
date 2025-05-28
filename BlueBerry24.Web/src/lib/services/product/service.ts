import { ProductDto, CreateProductDto, UpdateProductDto } from "@/types/product";
import { IProductService } from "./interface";
import { ResponseDto } from "@/types/responseDto";
import { apiRequest } from "@/lib/utils/api";

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
const API_BASE = process.env.API_BASE_PRODUCT;

export class ProductService implements IProductService{
    async getById(id: number): Promise<ProductDto> {
        try {
            const json: ResponseDto<ProductDto> = await apiRequest(`${API_BASE}/${id}`, {
                isPublic: true,
                next:{
                    revalidate: 60 * 5 // 5 minutes
                }
            });
            if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
            return json.data;
        } catch (error) {
            console.error('Failed to fetch product by id:', error);
            throw new Error('Failed to fetch product');
        }
    }
    
    async getByName(name: string): Promise<ProductDto> {
        try {
            const json: ResponseDto<ProductDto> = await apiRequest(`${API_BASE}/${name}`, {
                isPublic: true,
                next: {
                    revalidate: 60 * 5 // 5 minutes
                }
            });
            if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
            return json.data;
        } catch (error) {
            console.error('Failed to fetch product by name:', error);
            throw new Error('Failed to fetch product');
        }
    }
    
    async getAll(): Promise<ProductDto[]> {
        try {
            const json: ResponseDto<ProductDto[]> = await apiRequest(`${API_BASE}`, {
                isPublic: true,
                next: {
                    revalidate: 60 * 5   //5 minutes
                }
            });
            if(!json.isSuccess || !json.data) {
                console.warn('Products API returned no data, returning empty array');
                return [];
            }
            return json.data;
        } catch (error) {
            console.error('Failed to fetch products:', error);
            return [];
        }
    }
    
    async create(data: CreateProductDto, categories: number[]): Promise<ProductDto> {
        try {

            const queryString = categories.map(c => `categories=${c}`).join('&');
            const url = `${API_BASE}?${queryString}`;

            const json: ResponseDto<ProductDto> = await apiRequest(url, {
                method: 'POST',
                body: JSON.stringify(data),
                requireAuth: true
            });
            if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
            return json.data;
        } catch (error) {
            console.error('Failed to create product:', error);
            throw new Error('Failed to create product');
        }
    }
    
    async update(id: number, data: UpdateProductDto, categories: number[]): Promise<ProductDto> {
        try {
            const json: ResponseDto<ProductDto> = await apiRequest(`${API_BASE}/${id}`, {
                method: 'PUT',
                body: JSON.stringify(data),
                requireAuth: true
            });
            if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
            return json.data;
        } catch (error) {
            console.error('Failed to update product:', error);
            throw new Error('Failed to update product');
        }
    }
    
    async delete(id: number): Promise<boolean> {
        try {
            const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/${id}`, {
                method: 'DELETE',
                requireAuth: true
            });
            if(!json.isSuccess) throw new Error(json.statusMessage);
            return json.data ?? true;
        } catch (error) {
            console.error('Failed to delete product:', error);
            throw new Error('Failed to delete product');
        }
    }
    
    async existsById(id: number): Promise<boolean> {
        try {
            const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/exists/${id}`, {
                requireAuth: true
            });
            return json.isSuccess && json.data;
        } catch (error) {
            console.error('Failed to check product existence by id:', error);
            return false;
        }
    }
    
    async existsByName(name: string): Promise<boolean> {
        try {
            const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/exists/name/${encodeURIComponent(name)}`, {
                requireAuth: true
            });
            return json.isSuccess && json.data;
        } catch (error) {
            console.error('Failed to check product existence by name:', error);
            return false;
        }
    }
}