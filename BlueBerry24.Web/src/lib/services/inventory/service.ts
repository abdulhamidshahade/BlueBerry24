import { apiRequest } from "../../utils/api";
import {
  InventoryLogDto,
} from "../../../types/inventory";
import { ProductDto } from "../../../types/product";
import { ResponseDto } from "../../../types/responseDto";

export interface AddStockRequest {
  productId: number;
  quantity: number;
  notes: string;
  performedByUserId?: number;
}

export interface AdjustStockRequest {
  productId: number;
  newQuantity: number;
  notes: string;
  performedByUserId?: number;
}

export interface IInventoryService {
  addStock(request: AddStockRequest): Promise<boolean>;
  adjustStock(request: AdjustStockRequest): Promise<boolean>;
  getProductWithStockInfo(productId: number): Promise<ProductDto>;
  getLowStockProducts(limit?: number): Promise<ProductDto[]>;
  getInventoryHistory(
    productId: number,
    limit?: number
  ): Promise<InventoryLogDto[]>;
  isInStock(productId: number, quantity: number): Promise<boolean>;
  reserveStock(
    productId: number,
    quantity: number,
    referenceId: number,
    referenceType: string
  ): Promise<boolean>;
  releaseReservedStock(
    productId: number,
    quantity: number,
    referenceId: number,
    referenceType: string
  ): Promise<boolean>;
}

export class InventoryService implements IInventoryService {
  private readonly baseUrl: string;
  
  //TODO: remove the constructor and replace it with .env.local
  constructor() {
    this.baseUrl = "https://localhost:7105/api";
  }

  async addStock(request: AddStockRequest): Promise<boolean> {
    try {
      const response: ResponseDto<boolean> = await apiRequest(
        `${this.baseUrl}/Inventories/add-stock`,
        {
          method: "POST",
          body: JSON.stringify(request),
          requireAuth: true,
        }
      );

      if (!response.isSuccess) {
        throw new Error(`HTTP error! status: ${response.statusMessage}`);
      }

      const data = await response.isSuccess;
      return data;
    } catch (error) {
      console.error("Error adding stock:", error);
      throw error;
    }
  }

  async adjustStock(request: AdjustStockRequest): Promise<boolean> {
    try {
      const response: ResponseDto<boolean> = await apiRequest(
        `${this.baseUrl}/Inventories/adjust-stock`,
        {
          method: "PUT",
          body: JSON.stringify(request),
          requireAuth: true,
        }
      );

      if (!response.isSuccess) {
        throw new Error(`HTTP error! status: ${response.statusMessage}`);
      }

      const data = await response.isSuccess;
      return data;
    } catch (error) {
      console.error("Error adjusting stock:", error);
      throw error;
    }
  }

  async getProductWithStockInfo(productId: number): Promise<ProductDto> {
    try {
      const response: ResponseDto<ProductDto> = await apiRequest(
        `${this.baseUrl}/Inventories/product/${productId}`,
        {
          requireAuth: true,
        }
      );

      if (!response.isSuccess) {
        throw new Error(`HTTP error! status: ${response.statusMessage}`);
      }

      const data = await response.data;
      return data;
    } catch (error) {
      console.error("Error getting product with stock info:", error);
      throw error;
    }
  }

  async getLowStockProducts(limit: number = 50): Promise<ProductDto[]> {
    try {
      const response: ResponseDto<ProductDto[]> = await apiRequest(
        `${this.baseUrl}/Inventories/low-stock?limit=${limit}`,
        {
          requireAuth: true,
        }
      );

      if (!response.isSuccess) {
        throw new Error(`HTTP error! status: ${response.statusMessage}`);
      }

      const data = await response.data;
      return data;
    } catch (error) {
      console.error("Error getting low stock products:", error);
      throw error;
    }
  }

  async getInventoryHistory(
    productId: number,
    limit: number = 50
  ): Promise<InventoryLogDto[]> {
    try {
      const response: ResponseDto<InventoryLogDto[]> = await apiRequest(
        `${this.baseUrl}/Inventories/history/${productId}?limit=${limit}`,
        {
          requireAuth: true,
        }
      );

      if (!response.statusCode) {
        throw new Error(`HTTP error! status: ${response.statusMessage}`);
      }

      const data = await response.data;
      return data;
    } catch (error) {
      console.error("Error getting inventory history:", error);
      throw error;
    }
  }

  async isInStock(productId: number, quantity: number): Promise<boolean> {
    try {
      const response: ResponseDto<boolean> = await apiRequest(
        `${this.baseUrl}/Inventories/check-stock/${productId}/${quantity}`,
        {
          requireAuth: true,
        }
      );

      if (!response.isSuccess) {
        throw new Error(`HTTP error! status: ${response.statusMessage}`);
      }

      const data = await response.isSuccess;
      return data;
    } catch (error) {
      console.error("Error checking stock:", error);
      throw error;
    }
  }

  async reserveStock(
    productId: number,
    quantity: number,
    referenceId: number,
    referenceType: string
  ): Promise<boolean> {
    try {
      const response: ResponseDto<boolean> = await apiRequest(
        `${this.baseUrl}/Inventories/reserve-stock`,
        {
          method: "POST",
          requireAuth: true,
          body: JSON.stringify({
            productId,
            quantity,
            referenceId,
            referenceType,
          }),
        }
      );

      if (!response.isSuccess) {
        throw new Error(`HTTP error! status: ${response.statusMessage}`);
      }

      const data = await response.isSuccess;
      return data;
    } catch (error) {
      console.error("Error reserving stock:", error);
      throw error;
    }
  }

  async releaseReservedStock(
    productId: number,
    quantity: number,
    referenceId: number,
    referenceType: string
  ): Promise<boolean> {
    try {
      const response: ResponseDto<boolean> = await apiRequest(
        `${this.baseUrl}/Inventories/release-reserved-stock`,
        {
          method: "POST",
          requireAuth: true,
          body: JSON.stringify({
            productId,
            quantity,
            referenceId,
            referenceType,
          }),
        }
      );

      if (!response.isSuccess) {
        throw new Error(`HTTP error! status: ${response.statusMessage}`);
      }

      const data = await response.isSuccess;
      return data;
    } catch (error) {
      console.error("Error releasing reserved stock:", error);
      throw error;
    }
  }
}