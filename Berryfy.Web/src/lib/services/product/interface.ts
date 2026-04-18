import { CreateProductDto, ProductDto, UpdateProductDto } from "../../../types/product";
import { PaginationDto, ProductFilterDto } from "../../../types/pagination";

export interface IProductService{
    getById(id: number): Promise<ProductDto>;
    getByName(name: string): Promise<ProductDto>;
    getPaginated(filter: ProductFilterDto): Promise<PaginationDto<ProductDto>>;
    create(data: CreateProductDto, categories: number[]): Promise<ProductDto>;
    update(id: number, data: UpdateProductDto, categories: number[]): Promise<ProductDto>;
    delete(id: number): Promise<boolean>;
    existsById(id: number): Promise<boolean>;
    existsByName(name: string): Promise<boolean>;
    getAll(): Promise<ProductDto[]>;
}