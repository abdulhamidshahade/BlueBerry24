import { CreateProductDto, ProductDto, UpdateProductDto } from "../../../types/product";

export interface IProductService{
    getById(id: number): Promise<ProductDto>;
    getByName(name: string): Promise<ProductDto>;
    getAll(): Promise<ProductDto[]>;
    create(data: CreateProductDto, categories: number[]): Promise<ProductDto>;
    update(id: number, data: UpdateProductDto, categories: number[]): Promise<ProductDto>;
    delete(id: number): Promise<boolean>;
    existsById(id: number): Promise<boolean>;
    existsByName(name: string): Promise<boolean>;
}