export interface InventoryLogDto {
  id: number;
  productId: number;
  currentStockQuantity: number;
  quantityChanged: number;
  changeType: InventoryChangeType;
  referenceId: number;
  referenceType: string;
  performedByUserId?: number;
  notes: string;
  createdAt: string;
}

export interface CreateInventoryLogDto {
  productId: number;
  quantityChanged: number;
  changeType: InventoryChangeType;
  referenceId: number;
  referenceType: string;
  performedByUserId?: number;
  notes: string;
}

export interface StockUpdateDto {
  productId: number;
  newQuantity: number;
  changeType: InventoryChangeType;
  notes: string;
}

export enum InventoryChangeType {
  Purchase = 0,
  Return = 1,
  StockAdjustment = 2,
  InitialStock = 3,
  Reserved = 4,
  ReleaseReservation = 5,
  Damaged = 6,
  Restock = 7
} 