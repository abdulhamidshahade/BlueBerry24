import { ProductDto } from "../../../types/product";
import { AddStockRequest, AdjustStockRequest } from "./service";
import { InventoryLogDto } from "../../../types/inventory";

export interface IInventoryService{

    addStock(request: AddStockRequest) : Promise<boolean>;
    adjustStock(request: AdjustStockRequest) : Promise<boolean>;
    getProductWithStockInfo(productId: number) : Promise<ProductDto>;
    getLowStockProducts(limit: number) : Promise<ProductDto[]>;
    getInventoryHistory(productId: number, limit: number) : Promise<InventoryLogDto[]>;
    isInStock(productId: number, quality: number) : Promise<boolean>;
    reserveStock(productId: number, quantity: number, referenceId: number, referenceType: string) : Promise<string>;
    releaseReservedStock(productId: number, quantity: number, referenceId: number, referenceType: string) : Promise<boolean>;
}