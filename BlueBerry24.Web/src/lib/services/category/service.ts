import { ICategoryService } from "./interface";
import { CategoryDto, 
         CreateCategoryDto,
         UpdateCategoryDto,
 } from "@/types/category";
import { ResponseDto } from "@/types/responseDto";

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
const API_BASE = process.env.NEXT_PUBLIC_API_BASE_CATEGORY;

export class CategoryService implements ICategoryService{
    async getById(id: number): Promise<CategoryDto> {
        const res = await fetch(`${API_BASE}/${id}`);
        const json: ResponseDto<CategoryDto> = await res.json();
        if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
        return json.data;
    }
    async getByName(name: string): Promise<CategoryDto> {
        const res = await fetch(`${API_BASE}/${name}`);
        const json: ResponseDto<CategoryDto> = await res.json();
        if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
        return json.data;
    }
    async getAll(): Promise<CategoryDto[]> {
        const res = await fetch(`${API_BASE}`);
        const json:ResponseDto<CategoryDto[]> = await res.json();
        if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
        return json.data;
    }
    async create(data: CreateCategoryDto): Promise<CategoryDto> {
        const res = await fetch(`${API_BASE}`, {
            method: "POST",
            body: JSON.stringify(data),
            headers: {"Content-Type": "application/json"}
        });
        const json:ResponseDto<CategoryDto> = await res.json();
        if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
        return json.data;
    }
    async update(id: number, data: UpdateCategoryDto): Promise<CategoryDto> {
        var res = await fetch(`${API_BASE}/${id}`, {
            method: "PUT",
            body: JSON.stringify(data),
            headers: {"Content-Type": "application/json"}
        });

        var json:ResponseDto<CategoryDto> = await res.json();
        if(!json.isSuccess || !json.data) throw new Error(json.statusMessage);
        return json.data;
    }
    async delete(id: number): Promise<boolean> {
    const res = await fetch(`${API_BASE}/${id}`, { method: "DELETE" });
    const json: ResponseDto<boolean> = await res.json();
    if (!json.isSuccess) throw new Error(json.statusMessage);
    return json.data ?? false;
  }

  async exists(id: number): Promise<boolean> {
    const res = await fetch(`${API_BASE}/${id}/exists`);
    const json: ResponseDto<boolean> = await res.json();
    return json.isSuccess && json.data;
  }

  async existsByName(name: string): Promise<boolean> {
    const res = await fetch(`${API_BASE}/exists-by-name/${encodeURIComponent(name)}`);
    const json: ResponseDto<boolean> = await res.json();
    return json.isSuccess && json.data;
  }
}