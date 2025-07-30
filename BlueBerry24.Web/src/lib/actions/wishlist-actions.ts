'use server'

import { revalidatePath } from 'next/cache';
import { redirect } from 'next/navigation';
import { getCurrentUser } from './auth-actions';
import { WishlistService } from '../services/wishlist/service';
import { 
  CreateWishlistDto, 
  UpdateWishlistDto, 
  AddToWishlistDto, 
  UpdateWishlistItemDto,
  WishlistPriority 
} from '../../types/wishlist';

const wishlistService = new WishlistService();

async function checkAuth() {
  const user = await getCurrentUser();
  
  if (!user) {
    redirect('/auth/login?redirectTo=/profile/wishlist');
  }
  
  return user;
}

export async function getUserWishlists() {
  await checkAuth();
  
  try {
    return await wishlistService.getUserWishlists();
  } catch (error) {
    console.error('Error fetching user wishlists:', error);
    throw error;
  }
}

export async function getWishlistById(id: number) {
  await checkAuth();
  
  try {
    return await wishlistService.getById(id);
  } catch (error) {
    console.error('Error fetching wishlist:', error);
    throw error;
  }
}

export async function getDefaultWishlist() {
  await checkAuth();
  
  try {
    return await wishlistService.getDefaultWishlist();
  } catch (error) {
    console.error('Error fetching default wishlist:', error);
    throw error;
  }
}

export async function getUserWishlistSummary() {
  await checkAuth();
  
  try {
    return await wishlistService.getUserSummary();
  } catch (error) {
    console.error('Error fetching wishlist summary:', error);
    throw error;
  }
}

export async function createWishlist(formData: FormData) {
  await checkAuth();
  
  try {
    const name = formData.get('name') as string;
    const isPublic = formData.get('isPublic') === 'on';

    if (!name || name.trim().length === 0) {
      redirect('/profile/wishlist?error=' + encodeURIComponent('Wishlist name is required'));
    }

    const createData: CreateWishlistDto = {
      name: name.trim(),
      isPublic
    };

    const wishlist = await wishlistService.create(createData);

    revalidatePath('/profile/wishlist');
    redirect('/profile/wishlist?success=' + encodeURIComponent('Wishlist created successfully'));
  } catch (error) {
    console.error('Error creating wishlist:', error);
    redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to create wishlist'));
  }
}

export async function updateWishlist(formData: FormData) {
  await checkAuth();
  
  try {
    const id = parseInt(formData.get('id') as string);
    const name = formData.get('name') as string;
    const isPublic = formData.get('isPublic') === 'on';

    if (!name || name.trim().length === 0) {
      redirect('/profile/wishlist?error=' + encodeURIComponent('Wishlist name is required'));
    }

    const updateData: UpdateWishlistDto = {
      name: name.trim(),
      isPublic
    };

    await wishlistService.update(id, updateData);

    revalidatePath('/profile/wishlist');
    redirect('/profile/wishlist?success=' + encodeURIComponent('Wishlist updated successfully'));
  } catch (error) {
    console.error('Error updating wishlist:', error);
    redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to update wishlist'));
  }
}


export async function deleteWishlist(formData: FormData) {
  await checkAuth();
  
  try {
    const id = parseInt(formData.get('id') as string);

    const success = await wishlistService.delete(id);
    
    if (!success) {
      redirect('/profile/wishlist?error=' + encodeURIComponent('Cannot delete the default wishlist'));
    }

    revalidatePath('/profile/wishlist');
    redirect('/profile/wishlist?success=' + encodeURIComponent('Wishlist deleted successfully'));
  } catch (error) {
    console.error('Error deleting wishlist:', error);
    redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to delete wishlist'));
  }
}

export async function addToWishlist(formData: FormData) {
  await checkAuth();
  
  try {
    const productId = parseInt(formData.get('productId') as string);
    const wishlistId = formData.get('wishlistId') ? parseInt(formData.get('wishlistId') as string) : undefined;
    const notes = formData.get('notes') as string;
    const priority = parseInt(formData.get('priority') as string) || WishlistPriority.Low;

    const addData: AddToWishlistDto = {
      productId,
      wishlistId,
      notes: notes || undefined,
      priority
    };

    await wishlistService.addItem(addData);

    revalidatePath('/profile/wishlist');
    revalidatePath('/products');
    redirect(`/products/${productId}?success=` + encodeURIComponent('Product added to wishlist successfully'));
  } catch (error) {
    console.error('Error adding item to wishlist:', error);
    const productId = formData.get('productId') as string;
    redirect(`/products/${productId}?error=` + encodeURIComponent('Failed to add product to wishlist'));
  }
}

export async function quickAddToWishlist(formData: FormData) {
  await checkAuth();
  
  try {
    const productId = parseInt(formData.get('productId') as string);

    const addData: AddToWishlistDto = {
      productId,
      priority: WishlistPriority.Low
    };

    await wishlistService.addItem(addData);

    revalidatePath('/profile/wishlist');
    revalidatePath('/products');
    
    const returnUrl = formData.get('returnUrl') as string || `/products/${productId}`;
    redirect(returnUrl + '?success=' + encodeURIComponent('Product added to wishlist'));
  } catch (error) {
    console.error('Error adding item to wishlist:', error);
    const productId = formData.get('productId') as string;
    const returnUrl = formData.get('returnUrl') as string || `/products/${productId}`;
    redirect(returnUrl + '?error=' + encodeURIComponent('Failed to add product to wishlist'));
  }
}

export async function updateWishlistItem(formData: FormData) {
  await checkAuth();
  
  try {
    const wishlistId = parseInt(formData.get('wishlistId') as string);
    const productId = parseInt(formData.get('productId') as string);
    const notes = formData.get('notes') as string;
    const priority = parseInt(formData.get('priority') as string) || WishlistPriority.Low;

    const updateData: UpdateWishlistItemDto = {
      notes: notes || undefined,
      priority
    };

    await wishlistService.updateItem(wishlistId, productId, updateData);

    revalidatePath('/profile/wishlist');
    redirect('/profile/wishlist?success=' + encodeURIComponent('Wishlist item updated successfully'));
  } catch (error) {
    console.error('Error updating wishlist item:', error);
    redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to update wishlist item'));
  }
}

export async function removeFromWishlist(formData: FormData) {
  await checkAuth();
  
  try {
    const wishlistId = parseInt(formData.get('wishlistId') as string);
    const productId = parseInt(formData.get('productId') as string);

    const success = await wishlistService.removeItem(wishlistId, productId);
    
    if (!success) {
      redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to remove item from wishlist'));
    }

    revalidatePath('/profile/wishlist');
    redirect('/profile/wishlist?success=' + encodeURIComponent('Item removed from wishlist successfully'));
  } catch (error) {
    console.error('Error removing item from wishlist:', error);
    redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to remove item from wishlist'));
  }
}

export async function bulkRemoveItems(formData: FormData) {
  await checkAuth();
  
  try {
    const wishlistId = parseInt(formData.get('wishlistId') as string);
    const selectedItems = formData.getAll('selectedItems').map(id => parseInt(id as string));

    if (selectedItems.length === 0) {
      redirect('/profile/wishlist?error=' + encodeURIComponent('No items selected'));
    }

    const success = await wishlistService.removeMultipleItems(wishlistId, selectedItems);
    
    if (!success) {
      redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to remove items from wishlist'));
    }

    revalidatePath('/profile/wishlist');
    redirect('/profile/wishlist?success=' + encodeURIComponent(`${selectedItems.length} items removed from wishlist successfully`));
  } catch (error) {
    console.error('Error removing items from wishlist:', error);
    redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to remove items from wishlist'));
  }
}

export async function clearWishlist(formData: FormData) {
  await checkAuth();
  
  try {
    const wishlistId = parseInt(formData.get('wishlistId') as string);

    const success = await wishlistService.clearWishlist(wishlistId);
    
    if (!success) {
      redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to clear wishlist'));
    }

    revalidatePath('/profile/wishlist');
    redirect('/profile/wishlist?success=' + encodeURIComponent('Wishlist cleared successfully'));
  } catch (error) {
    console.error('Error clearing wishlist:', error);
    redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to clear wishlist'));
  }
}

export async function shareWishlist(formData: FormData) {
  await checkAuth();
  
  try {
    const wishlistId = parseInt(formData.get('wishlistId') as string);
    const isPublic = formData.get('isPublic') === 'true';

    const success = await wishlistService.shareWishlist(wishlistId, isPublic);
    
    if (!success) {
      redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to update wishlist sharing'));
    }

    revalidatePath('/profile/wishlist');
    redirect('/profile/wishlist?success=' + encodeURIComponent(`Wishlist ${isPublic ? 'shared' : 'made private'} successfully`));
  } catch (error) {
    console.error('Error sharing wishlist:', error);
    redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to update wishlist sharing'));
  }
}

export async function duplicateWishlist(formData: FormData) {
  await checkAuth();
  
  try {
    const wishlistId = parseInt(formData.get('wishlistId') as string);
    const newName = formData.get('newName') as string;

    if (!newName || newName.trim().length === 0) {
      redirect('/profile/wishlist?error=' + encodeURIComponent('New wishlist name is required'));
    }

    await wishlistService.duplicateWishlist(wishlistId, newName.trim());

    revalidatePath('/profile/wishlist');
    redirect('/profile/wishlist?success=' + encodeURIComponent('Wishlist duplicated successfully'));
  } catch (error) {
    console.error('Error duplicating wishlist:', error);
    redirect('/profile/wishlist?error=' + encodeURIComponent('Failed to duplicate wishlist'));
  }
}

export async function isProductInWishlist(productId: number) {
  try {
    const user = await getCurrentUser();
    if (!user) return false;
    
    return await wishlistService.isProductInWishlist(productId);
  } catch (error) {
    console.error('Error checking if product is in wishlist:', error);
    return false;
  }
}

export async function quickRemoveFromWishlist(formData: FormData) {
  await checkAuth();
  
  try {
    const productId = parseInt(formData.get('productId') as string);
    
    const wishlists = await wishlistService.getUserWishlists();
    const wishlistWithProduct = wishlists.find(w => 
      w.items.some(item => item.productId === productId)
    );

    if (!wishlistWithProduct) {
      const returnUrl = formData.get('returnUrl') as string || `/products/${productId}`;
      redirect(returnUrl + '?error=' + encodeURIComponent('Product not found in any wishlist'));
    }

    const success = await wishlistService.removeItem(wishlistWithProduct.id, productId);
    
    if (!success) {
      const returnUrl = formData.get('returnUrl') as string || `/products/${productId}`;
      redirect(returnUrl + '?error=' + encodeURIComponent('Failed to remove from wishlist'));
    }

    revalidatePath('/profile/wishlist');
    revalidatePath('/products');
    
    const returnUrl = formData.get('returnUrl') as string || `/products/${productId}`;
    redirect(returnUrl + '?success=' + encodeURIComponent('Product removed from wishlist'));
  } catch (error) {
    console.error('Error removing item from wishlist:', error);
    const productId = formData.get('productId') as string;
    const returnUrl = formData.get('returnUrl') as string || `/products/${productId}`;
    redirect(returnUrl + '?error=' + encodeURIComponent('Failed to remove product from wishlist'));
  }
} 