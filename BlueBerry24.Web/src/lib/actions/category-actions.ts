'use server';

import { CreateCategoryDto, UpdateCategoryDto } from "@/types/category";
import { CategoryService } from "../services/category/service";
import { ICategoryService } from "../services/category/interface";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";

const categoryService: ICategoryService = new CategoryService();

export async function createCategory(formData: FormData) {
  const name = formData.get('name') as string;
  const description = formData.get('description') as string;
  const imageUrl = formData.get('imageUrl') as string;
  
  const errors: Record<string, string> = {};
  
  if (!name?.trim()) {
    errors.name = 'Category name is required';
  } else if (name.length < 2) {
    errors.name = 'Category name must be at least 2 characters';
  }
  
  if (!description?.trim()) {
    errors.description = 'Description is required';
  } else if (description.length < 10) {
    errors.description = 'Description must be at least 10 characters';
  }
  
  if (!imageUrl?.trim()) {
    errors.imageUrl = 'Image URL is required';
  } else {
    try {
      new URL(imageUrl);
    } catch {
      errors.imageUrl = 'Please enter a valid URL';
    }
  }
  
  if (Object.keys(errors).length > 0) {
    const errorParams = new URLSearchParams();
    Object.entries(errors).forEach(([key, value]) => {
      errorParams.set(`error_${key}`, value);
    });
    errorParams.set('name', name || '');
    errorParams.set('description', description || '');
    errorParams.set('imageUrl', imageUrl || '');
    
    redirect(`/admin/categories/create?${errorParams.toString()}`);
  }
  
  try {
    const createData: CreateCategoryDto = {
      name: name.trim(),
      description: description.trim(),
      imageUrl: imageUrl.trim()
    };
    
    await categoryService.create(createData);
    revalidatePath('/admin/categories');
    redirect('/admin/categories?success=Category created successfully!');
  } catch (error) {
    const errorMessage = error instanceof Error ? error.message : 'An error occurred';
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    redirect(`/admin/categories/create?error=${encodeURIComponent(errorMessage)}&name=${encodeURIComponent(name || '')}&description=${encodeURIComponent(description || '')}&imageUrl=${encodeURIComponent(imageUrl || '')}`);
  }
}

export async function updateCategory(formData: FormData) {
  const id = formData.get('id') as string;
  const name = formData.get('name') as string;
  const description = formData.get('description') as string;
  const imageUrl = formData.get('imageUrl') as string;
  
  const errors: Record<string, string> = {};
  
  if (!id) {
    redirect('/admin/categories?error=Invalid category ID');
  }
  
  if (!name?.trim()) {
    errors.name = 'Category name is required';
  } else if (name.length < 2) {
    errors.name = 'Category name must be at least 2 characters';
  }
  
  if (!description?.trim()) {
    errors.description = 'Description is required';
  } else if (description.length < 10) {
    errors.description = 'Description must be at least 10 characters';
  }
  
  if (!imageUrl?.trim()) {
    errors.imageUrl = 'Image URL is required';
  } else {
    try {
      new URL(imageUrl);
    } catch {
      errors.imageUrl = 'Please enter a valid URL';
    }
  }
  
  if (Object.keys(errors).length > 0) {
    const errorParams = new URLSearchParams();
    Object.entries(errors).forEach(([key, value]) => {
      errorParams.set(`error_${key}`, value);
    });
    errorParams.set('name', name || '');
    errorParams.set('description', description || '');
    errorParams.set('imageUrl', imageUrl || '');
    
    redirect(`/admin/categories/update/${id}?${errorParams.toString()}`);
  }
  
  try {
    const updateData: UpdateCategoryDto = {
      id: parseInt(id),
      name: name.trim(),
      description: description.trim(),
      imageUrl: imageUrl.trim()
    };
    
    await categoryService.update(parseInt(id), updateData);
    revalidatePath('/admin/categories');
    redirect('/admin/categories?success=Category updated successfully!');
  } catch (error) {
    const errorMessage = error instanceof Error ? error.message : 'An error occurred';
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    redirect(`/admin/categories/update/${id}?error=${encodeURIComponent(errorMessage)}&name=${encodeURIComponent(name || '')}&description=${encodeURIComponent(description || '')}&imageUrl=${encodeURIComponent(imageUrl || '')}`);
  }
}

export async function validateConfirmation(formData: FormData) {
  const confirmationText = formData.get('confirmationText') as string;
  const categoryId = formData.get('categoryId') as string;
  const expectedConfirmation = 'DELETE';
  
  console.log('Validation attempt:', { confirmationText, categoryId, expectedConfirmation });
  
  if (!categoryId) {
    redirect('/admin/categories?error=Invalid category ID');
  }
  
  if (!confirmationText || confirmationText.trim() !== expectedConfirmation) {
    const errorParam = confirmationText 
      ? 'Please type "DELETE" exactly as shown (case sensitive)'
      : 'Please type "DELETE" to confirm';
    
    const params = new URLSearchParams({
      error: errorParam,
      attempted: 'true'
    });
    
    redirect(`/admin/categories/delete/${categoryId}?${params.toString()}`);
  }
  
  const params = new URLSearchParams({
    confirmed: 'true'
  });
  
  redirect(`/admin/categories/delete/${categoryId}?${params.toString()}`);
}

export async function deleteCategory(formData: FormData) {
  const categoryId = parseInt(formData.get('id') as string);
  
  console.log('Delete attempt for category ID:', categoryId);
  
  if (isNaN(categoryId)) {
    redirect('/admin/categories?error=Invalid category ID');
  }
  
  try {
    const categoryExists = await categoryService.exists(categoryId);
    console.log('Category exists:', categoryExists);
    
    if (!categoryExists) {
      redirect(`/admin/categories/delete/${categoryId}?error=Category not found&confirmed=true`);
    }
    
    const success = await categoryService.delete(categoryId);
    console.log('Delete result:', success);
    
    if (!success) {
      redirect(`/admin/categories/delete/${categoryId}?error=Failed to delete category - operation returned false&confirmed=true`);
    }
    
    revalidatePath('/admin/categories');
    revalidatePath(`/admin/categories/delete/${categoryId}`);
    
    redirect('/admin/categories?success=Category deleted successfully&deleted=true');
    
  } catch (error) {
    console.error('Error deleting category:', error);
    
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    
    const errorMessage = error instanceof Error ? error.message : 'Failed to delete category';
    console.error('Redirecting with error:', errorMessage);
    
    redirect(`/admin/categories/delete/${categoryId}?error=${encodeURIComponent(errorMessage)}&confirmed=true`);
  }
}