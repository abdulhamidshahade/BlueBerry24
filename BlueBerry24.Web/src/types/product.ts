export interface ProductDto{
    id: number;
    name: string;
    description: string;
    price: number;
    stockQuantity: number;
    imageUrl: string;
    reservedStock: number;
    lowStockThreshold: number;
    isActive: boolean;
    sku: string;
    productCategoryNames: string[]
}

export interface CreateProductDto{
    name: string;
    description: string;
    price: number;
    stockQuantity: number;
    imageUrl: string;
    reservedStock: number;
    lowStockThreshold: number;
    isActive: boolean;
    sku: string;
}

export interface UpdateProductDto{
    id: number;
    name: string;
    description: string;
    price: number;
    stockQuantity: number;
    imageUrl: string;
    reservedStock: number;
    lowStockThreshold: number;
    isActive: boolean;
    sku: string;
}

export interface DeleteProductDto{
    id: number;
    name: string;
    description: string;
    price: number;
    stockQuantity: number;
    imageUrl: string;
    reservedStock: number;
    lowStockThreshold: number;
    isActive: boolean;
    sku: string;
}