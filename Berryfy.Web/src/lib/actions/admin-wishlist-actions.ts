'use server'

import { revalidatePath } from 'next/cache';
import { redirect } from 'next/navigation';
import { getCurrentUser } from './auth-actions';
import { WishlistService } from '../services/wishlist/service';
import { WishlistDto, WishlistSummaryDto } from '../../types/wishlist';

const wishlistService = new WishlistService();

async function checkAdminAuth() {
  const user = await getCurrentUser();
  
  if (!user) {
    redirect('/auth/login?redirectTo=/admin/wishlists');
  }
  
  // You might want to add role checking here
  // if (!user.roles?.includes('Admin')) {
  //   redirect('/dashboard?error=Unauthorized');
  // }
  
  return user;
}

export async function getAllWishlists() {
  await checkAdminAuth();
  
  try {
    return await wishlistService.getAllWishlists();
  } catch (error) {
    console.error('Error fetching all wishlists:', error);
    throw error;
  }
}

export async function getGlobalWishlistStats() {
  await checkAdminAuth();
  
  try {
    return await wishlistService.getGlobalStats();
  } catch (error) {
    console.error('Error fetching global wishlist stats:', error);
    throw error;
  }
}

export async function getUserWishlistData(userId: string) {
  await checkAdminAuth();
  
  try {
    const summary = await wishlistService.getUserSummary();
    return summary;
  } catch (error) {
    console.error('Error fetching user wishlist data:', error);
    throw error;
  }
}

export async function adminDeleteWishlist(formData: FormData) {
  await checkAdminAuth();
  
  try {
    const wishlistId = parseInt(formData.get('wishlistId') as string);
    const confirmDelete = formData.get('confirmDelete') as string;

    if (confirmDelete !== 'DELETE') {
      redirect('/admin/wishlists?error=' + encodeURIComponent('Please type DELETE to confirm'));
    }

    const success = await wishlistService.adminDeleteWishlist(wishlistId);
    
    if (!success) {
      redirect('/admin/wishlists?error=' + encodeURIComponent('Failed to delete wishlist'));
    }

    revalidatePath('/admin/wishlists');
    redirect('/admin/wishlists?success=' + encodeURIComponent('Wishlist deleted successfully'));
  } catch (error) {
    console.error('Error deleting wishlist:', error);
    redirect('/admin/wishlists?error=' + encodeURIComponent('Failed to delete wishlist'));
  }
}

export async function adminClearWishlist(formData: FormData) {
  await checkAdminAuth();
  
  try {
    const wishlistId = parseInt(formData.get('wishlistId') as string);

    const success = await wishlistService.adminClearWishlist(wishlistId);
    
    if (!success) {
      redirect('/admin/wishlists?error=' + encodeURIComponent('Failed to clear wishlist'));
    }

    revalidatePath('/admin/wishlists');
    redirect('/admin/wishlists?success=' + encodeURIComponent('Wishlist cleared successfully'));
  } catch (error) {
    console.error('Error clearing wishlist:', error);
    redirect('/admin/wishlists?error=' + encodeURIComponent('Failed to clear wishlist'));
  }
}

export async function adminToggleWishlistVisibility(formData: FormData) {
  await checkAdminAuth();
  
  try {
    const wishlistId = parseInt(formData.get('wishlistId') as string);
    const isPublic = formData.get('isPublic') === 'true';

    const success = await wishlistService.adminToggleWishlistVisibility(wishlistId, isPublic);
    
    if (!success) {
      redirect('/admin/wishlists?error=' + encodeURIComponent('Failed to update wishlist visibility'));
    }

    revalidatePath('/admin/wishlists');
    redirect('/admin/wishlists?success=' + encodeURIComponent(`Wishlist ${isPublic ? 'made public' : 'made private'} successfully`));
  } catch (error) {
    console.error('Error updating wishlist visibility:', error);
    redirect('/admin/wishlists?error=' + encodeURIComponent('Failed to update wishlist visibility'));
  }
}

export async function exportWishlistData(formData: FormData) {
  await checkAdminAuth();
  
  try {
    const format = formData.get('format') as string || 'json';
    
    redirect(`/admin/wishlists/export?format=${format}`);
  } catch (error) {
    console.error('Error exporting wishlist data:', error);
    redirect('/admin/wishlists?error=' + encodeURIComponent('Failed to export wishlist data'));
  }
} 