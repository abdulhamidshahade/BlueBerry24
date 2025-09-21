import { ProductDto, CreateProductDto, UpdateProductDto } from "../../../types/product";
import { IProductService } from "./interface";
import { ResponseDto } from "../../../types/responseDto";
import { apiRequest } from "../../utils/api";
import { cookies } from 'next/headers';
import { PaginationDto, ProductFilterDto } from "../../../types/pagination";
process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
const API_BASE = process.env.API_BASE_PRODUCT;

export class ProductService implements IProductService{
    async getById(id: number): Promise<ProductDto> {
        try {
            const json: ResponseDto<ProductDto> = await apiRequest(`${API_BASE}/${id}`, {
                isPublic: true,
            });
            if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
            return json.data;
        } catch (error) {
            console.error('Failed to fetch product by id:', error);
            throw new Error('Failed to fetch product');
        }
    }


    async getAll(): Promise<ProductDto[]> {
        const cookieStore = await cookies();
        const token = cookieStore.get('cart_session')?.value;
        try {
            const json: ResponseDto<ProductDto[]> = await apiRequest(`${API_BASE}/all`, {
                isPublic: true,
                headers: {
                    'X-Session-Id': token ?? ''
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
    
    async getByName(name: string): Promise<ProductDto> {
        try {
            const json: ResponseDto<ProductDto> = await apiRequest(`${API_BASE}/name/${name}`, {
                isPublic: true,
            });
            if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
            return json.data;
        } catch (error) {
            console.error('Failed to fetch product by name:', error);
            throw new Error('Failed to fetch product');
        }
    }
    
    async getPaginated(filter: ProductFilterDto): Promise<PaginationDto<ProductDto>> {
        const cookieStore = await cookies();
        const token = cookieStore.get('cart_session')?.value;
        
        try {
            const queryParams = new URLSearchParams();
            
            if (filter.pageNumber) queryParams.append('pageNumber', filter.pageNumber.toString());
            if (filter.pageSize) queryParams.append('pageSize', filter.pageSize.toString());
            if (filter.searchTerm) queryParams.append('searchTerm', filter.searchTerm);
            if (filter.category) queryParams.append('category', filter.category);
            if (filter.sortBy) queryParams.append('sortBy', filter.sortBy);
            if (filter.minPrice !== undefined) queryParams.append('minPrice', filter.minPrice.toString());
            if (filter.maxPrice !== undefined) queryParams.append('maxPrice', filter.maxPrice.toString());
            if (filter.isActive !== undefined) queryParams.append('isActive', filter.isActive.toString());

            const url = `${API_BASE}?${queryParams.toString()}`;
            
            const json: ResponseDto<PaginationDto<ProductDto>> = await apiRequest(url, {
                isPublic: true,
                headers: {
                    'X-Session-Id': token ?? ''
                }
            });
            
            if(!json.isSuccess || !json.data) {
                console.warn('Products pagination API returned no data, returning empty pagination');
                return {
                    data: [],
                    pageNumber: 1,
                    pageSize: filter.pageSize || 12,
                    totalCount: 0,
                    totalPages: 0,
                    hasPreviousPage: false,
                    hasNextPage: false,
                    firstItemOnPage: 0,
                    lastItemOnPage: 0
                };
            }
            return json.data;
        } catch (error) {
            console.error('Failed to fetch paginated products:', error);
            return {
                data: [],
                pageNumber: 1,
                pageSize: filter.pageSize || 12,
                totalCount: 0,
                totalPages: 0,
                hasPreviousPage: false,
                hasNextPage: false,
                firstItemOnPage: 0,
                lastItemOnPage: 0
            };
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

            const categoriesQuery = categories.map(c => `categories=${c}`).join('&');
            const url = `${API_BASE}/${id}?${categoriesQuery}`;

            const json: ResponseDto<ProductDto> = await apiRequest(url, {
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
            const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/exists-by-id/${id}`, {
                requireAuth: true,
            });
            return json.isSuccess && json.data;
        } catch (error) {
            console.error('Failed to check product existence by id:', error);
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
            console.error('Failed to check product existence by name:', error);
            return false;
        }
    }
}