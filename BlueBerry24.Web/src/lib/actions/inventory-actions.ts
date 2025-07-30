'use server';

import { revalidatePath } from 'next/cache';
import { redirect } from 'next/navigation';
import { InventoryService, AddStockRequest, AdjustStockRequest } from '../services/inventory/service'

const inventoryService = new InventoryService();

export async function addStock(formData: FormData) {
  try {
    const productId = parseInt(formData.get('productId') as string);
    const quantity = parseInt(formData.get('quantity') as string);
    const notes = formData.get('notes') as string || '';
    const changeType = formData.get('adjustmentType') as string;

    if (isNaN(productId) || isNaN(quantity) || quantity <= 0) {
      throw new Error('Invalid product ID or quantity');
    }

    const request: AddStockRequest = {
      productId,
      quantity,
      notes: `${changeType}: ${notes}`,
      performedByUserId: undefined
    };

    const success = await inventoryService.addStock(request);

    if (!success) {
      throw new Error('Failed to add stock');
    }

    revalidatePath('/admin/inventory');
    revalidatePath('/admin/products');
    revalidatePath('/products');
  } catch (error) {
    console.error('Error adding stock:', error);
    throw new Error('Failed to add stock');
  }
}

export async function adjustStock(formData: FormData) {
  try {
    const productId = parseInt(formData.get('productId') as string);
    const newQuantity = parseInt(formData.get('newQuantity') as string);
    const notes = formData.get('notes') as string || '';
    const changeType = formData.get('adjustmentType') as string || 'Manual Adjustment';

    if (isNaN(productId) || isNaN(newQuantity) || newQuantity < 0) {
      throw new Error('Invalid product ID or quantity');
    }

    const request: AdjustStockRequest = {
      productId,
      newQuantity,
      notes: `${changeType}: ${notes}`,
      performedByUserId: undefined
    };

    const success = await inventoryService.adjustStock(request);

    if (!success) {
      throw new Error('Failed to adjust stock');
    }

    revalidatePath('/admin/inventory');
    revalidatePath('/admin/products');
    revalidatePath('/products');
  } catch (error) {
    console.error('Error adjusting stock:', error);
    throw new Error('Failed to adjust stock');
  }
}

export async function quickRestock(formData: FormData) {
  try {
    const productId = parseInt(formData.get('productId') as string);
    const quantity = parseInt(formData.get('quantity') as string);

    if (isNaN(productId) || isNaN(quantity) || quantity <= 0) {
      throw new Error('Invalid product ID or quantity');
    }

    const request: AddStockRequest = {
      productId,
      quantity,
      notes: 'Quick restock action',
      performedByUserId: undefined
    };

    const success = await inventoryService.addStock(request);

    if (!success) {
      throw new Error('Failed to restock');
    }

    revalidatePath('/admin/inventory');
    revalidatePath('/admin/products');
  } catch (error) {
    console.error('Error during quick restock:', error);
    throw new Error('Failed to restock product');
  }
}

export async function updateLowStockThreshold(formData: FormData) {
  try {
    const productId = parseInt(formData.get('productId') as string);
    const lowStockThreshold = parseInt(formData.get('lowStockThreshold') as string);

    if (isNaN(productId) || isNaN(lowStockThreshold) || lowStockThreshold < 0) {
      throw new Error('Invalid product ID or threshold');
    }

    redirect(`/admin/products/${productId}/edit?focus=lowStockThreshold`);
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error updating low stock threshold:', error);
    throw new Error('Failed to update low stock threshold');
  }
}

export async function releaseReservedStock(formData: FormData) {
  try {
    const productId = parseInt(formData.get('productId') as string);
    const quantity = parseInt(formData.get('quantity') as string);
    const referenceId = parseInt(formData.get('referenceId') as string) || 0;
    const referenceType = formData.get('referenceType') as string || 'manual';

    if (isNaN(productId) || isNaN(quantity) || quantity <= 0) {
      throw new Error('Invalid product ID or quantity');
    }

    const success = await inventoryService.releaseReservedStock(productId, quantity, referenceId, referenceType);

    if (!success) {
      throw new Error('Failed to release reserved stock');
    }

    revalidatePath('/admin/inventory');
    revalidatePath('/admin/products');
  } catch (error) {
    console.error('Error releasing reserved stock:', error);
    throw new Error('Failed to release reserved stock');
  }
}

export async function getInventoryHistory(productId: number, limit: number = 50) {
  try {
    const history = await inventoryService.getInventoryHistory(productId, limit);
    return history;
  } catch (error) {
    console.error('Error getting inventory history:', error);
    throw new Error('Failed to get inventory history');
  }
}

export async function getLowStockProducts(limit: number = 50) {
  try {
    const products = await inventoryService.getLowStockProducts(limit);
    return products;
  } catch (error) {
    console.error('Error getting low stock products:', error);
    throw new Error('Failed to get low stock products');
  }
} 