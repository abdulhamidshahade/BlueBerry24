'use server'

import { revalidatePath } from 'next/cache';
import { redirect } from 'next/navigation';
import { ProductService } from '@/lib/services/product/service';
import { CreateProductDto, UpdateProductDto } from '@/types/product';
import { IProductService } from '../services/product/interface';

const productService: IProductService = new ProductService();

export async function createProduct(formData: FormData): Promise<void> {
  try {
    //get -> for one element/ getAll -> for multiple elements
    const categories = formData.getAll('categories');
    const categoryIds = categories ? categories.map(id => parseInt(id as string)).filter(id => !isNaN(id)) : [];

    const productData: CreateProductDto = {
      name: formData.get('name') as string,
      description: formData.get('description') as string,
      price: parseFloat(formData.get('price') as string),
      stockQuantity: parseInt(formData.get('stockQuantity') as string),
      imageUrl: formData.get('imageUrl') as string,
      reservedStock: parseInt(formData.get('reservedStock') as string) || 0,
      lowStockThreshold: parseInt(formData.get('lowStockThreshold') as string) || 10,
      isActive: formData.get('isActive') === 'on',
      sku: formData.get('sku') as string,
    };

    await productService.create(productData, categoryIds);

    revalidatePath('/admin/products');
    revalidatePath('/products');
    redirect('/admin/products?success=created');
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error creating product:', error);
    throw new Error('Failed to create product');
  }
}

export async function updateProduct(formData: FormData) {
  try {
    const id = parseInt(formData.get('id') as string);
    const categories = formData.getAll('categories');
    const categoryIds = categories ? categories.map(id => parseInt(id as string)).filter(id => !isNaN(id)) : [];

    const productData: UpdateProductDto = {
      id,
      name: formData.get('name') as string,
      description: formData.get('description') as string,
      price: parseFloat(formData.get('price') as string),
      stockQuantity: parseInt(formData.get('stockQuantity') as string),
      imageUrl: formData.get('imageUrl') as string,
      reservedStock: parseInt(formData.get('reservedStock') as string) || 0,
      lowStockThreshold: parseInt(formData.get('lowStockThreshold') as string) || 10,
      isActive: formData.get('isActive') === 'on',
      sku: formData.get('sku') as string,
    };

    await productService.update(id, productData, categoryIds);

    revalidatePath('/admin/products');
    revalidatePath('/products');
    redirect('/admin/products?success=updated');
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error updating product:', error);
    throw new Error('Failed to update product');
  }
}

export async function deleteProduct(formData: FormData) {
  try {
    const id = parseInt(formData.get('id') as string);

    await productService.delete(id);

    revalidatePath('/admin/products');
    revalidatePath('/products');
    redirect('/admin/products?success=deleted');
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error deleting product:', error);
    throw new Error('Failed to delete product');
  }
}

export async function getProducts() {
  try {
    return await productService.getAll();
  } catch (error) {
    console.error('Error fetching products:', error);
    return [];
  }
}

export async function getProduct(id: number) {
  try {
    return await productService.getById(id);
  } catch (error) {
    console.error('Error fetching product:', error);
    return null;
  }
}