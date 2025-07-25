import { 
  WishlistDto, 
  WishlistItemDto, 
  CreateWishlistDto, 
  UpdateWishlistDto, 
  AddToWishlistDto, 
  UpdateWishlistItemDto,
  WishlistSummaryDto,
  WishlistResponse 
} from '../../../types/wishlist';
import { IWishlistService } from './interface';
import { apiRequest } from '../../utils/api';

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
const API_BASE = 'https://localhost:7105/api';

export class WishlistService implements IWishlistService {
  
  async getUserWishlists(): Promise<WishlistDto[]> {
    try {
      const response: WishlistResponse<WishlistDto[]> = await apiRequest(`${API_BASE}/Wishlists`, {
        requireAuth: true,
      });
      
      if (!response.isSuccess || !response.data) {
        throw new Error(response.statusMessage || 'Failed to fetch wishlists');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to fetch user wishlists:', error);
      throw new Error('Failed to fetch wishlists');
    }
  }

  async getById(id: number): Promise<WishlistDto> {
    try {
      const response: WishlistResponse<WishlistDto> = await apiRequest(`${API_BASE}/Wishlists/${id}`, {
        requireAuth: true,
      });
      
      if (!response.isSuccess || !response.data) {
        throw new Error(response.statusMessage || 'Wishlist not found');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to fetch wishlist:', error);
      throw new Error('Failed to fetch wishlist');
    }
  }

  async getDefaultWishlist(): Promise<WishlistDto> {
    try {
      const response: WishlistResponse<WishlistDto> = await apiRequest(`${API_BASE}/Wishlists/default`, {
        requireAuth: true,
      });
      
      if (!response.isSuccess || !response.data) {
        throw new Error(response.statusMessage || 'Failed to fetch default wishlist');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to fetch default wishlist:', error);
      throw new Error('Failed to fetch default wishlist');
    }
  }

  async getUserSummary(): Promise<WishlistSummaryDto> {
    try {
      const response: WishlistResponse<WishlistSummaryDto> = await apiRequest(`${API_BASE}/Wishlists/summary`, {
        requireAuth: true,
      });
      
      if (!response.isSuccess || !response.data) {
        throw new Error(response.statusMessage || 'Failed to fetch wishlist summary');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to fetch wishlist summary:', error);
      throw new Error('Failed to fetch wishlist summary');
    }
  }

  async create(data: CreateWishlistDto): Promise<WishlistDto> {
    try {
      const response: WishlistResponse<WishlistDto> = await apiRequest(`${API_BASE}/Wishlists`, {
        method: 'POST',
        body: JSON.stringify(data),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      
      if (!response.isSuccess || !response.data) {
        throw new Error(response.statusMessage || 'Failed to create wishlist');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to create wishlist:', error);
      throw new Error('Failed to create wishlist');
    }
  }

  async update(id: number, data: UpdateWishlistDto): Promise<WishlistDto> {
    try {
      const response: WishlistResponse<WishlistDto> = await apiRequest(`${API_BASE}/Wishlists/${id}`, {
        method: 'PUT',
        body: JSON.stringify(data),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      
      if (!response.isSuccess || !response.data) {
        throw new Error(response.statusMessage || 'Failed to update wishlist');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to update wishlist:', error);
      throw new Error('Failed to update wishlist');
    }
  }

  async delete(id: number): Promise<boolean> {
    try {
      const response: WishlistResponse = await apiRequest(`${API_BASE}/Wishlists/${id}`, {
        method: 'DELETE',
        requireAuth: true
      });
      
      return response.isSuccess;
    } catch (error) {
      console.error('Failed to delete wishlist:', error);
      return false;
    }
  }


  async addItem(data: AddToWishlistDto): Promise<WishlistItemDto> {
    try {
      const response: WishlistResponse<WishlistItemDto> = await apiRequest(`${API_BASE}/Wishlists/items/add`, {
        method: 'POST',
        body: JSON.stringify(data),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      
      if (!response.isSuccess || !response.data) {
        throw new Error(response.statusMessage || 'Failed to add item to wishlist');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to add item to wishlist:', error);
      throw new Error('Failed to add item to wishlist');
    }
  }

  async updateItem(wishlistId: number, productId: number, data: UpdateWishlistItemDto): Promise<WishlistItemDto> {
    try {
      const response: WishlistResponse<WishlistItemDto> = await apiRequest(`${API_BASE}/Wishlists/${wishlistId}/items/${productId}`, {
        method: 'PUT',
        body: JSON.stringify(data),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      
      if (!response.isSuccess || !response.data) {
        throw new Error(response.statusMessage || 'Failed to update wishlist item');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to update wishlist item:', error);
      throw new Error('Failed to update wishlist item');
    }
  }

  async removeItem(wishlistId: number, productId: number): Promise<boolean> {
    try {
      const response: WishlistResponse = await apiRequest(`${API_BASE}/Wishlists/${wishlistId}/items/${productId}`, {
        method: 'DELETE',
        requireAuth: true
      });
      
      return response.isSuccess;
    } catch (error) {
      console.error('Failed to remove item from wishlist:', error);
      return false;
    }
  }

  async isProductInWishlist(productId: number): Promise<boolean> {
    try {
      const response: WishlistResponse<{ isInWishlist: boolean }> = await apiRequest(`${API_BASE}/Wishlists/check-product/${productId}`, {
        requireAuth: true,
      });
      
      if (!response.isSuccess || !response.data) {
        return false;
      }
      return response.data.isInWishlist;
    } catch (error) {
      console.error('Failed to check if product is in wishlist:', error);
      return false;
    }
  }

  async addMultipleItems(wishlistId: number, productIds: number[]): Promise<boolean> {
    try {
      const response: WishlistResponse = await apiRequest(`${API_BASE}/Wishlists/${wishlistId}/items/bulk-add`, {
        method: 'POST',
        body: JSON.stringify(productIds),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      
      return response.isSuccess;
    } catch (error) {
      console.error('Failed to add multiple items to wishlist:', error);
      return false;
    }
  }

  async removeMultipleItems(wishlistId: number, productIds: number[]): Promise<boolean> {
    try {
      const response: WishlistResponse = await apiRequest(`${API_BASE}/Wishlists/${wishlistId}/items/bulk-remove`, {
        method: 'DELETE',
        body: JSON.stringify(productIds),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      
      return response.isSuccess;
    } catch (error) {
      console.error('Failed to remove multiple items from wishlist:', error);
      return false;
    }
  }

  async clearWishlist(wishlistId: number): Promise<boolean> {
    try {
      const response: WishlistResponse = await apiRequest(`${API_BASE}/Wishlists/${wishlistId}/clear`, {
        method: 'DELETE',
        requireAuth: true
      });
      
      return response.isSuccess;
    } catch (error) {
      console.error('Failed to clear wishlist:', error);
      return false;
    }
  }

  async shareWishlist(wishlistId: number, isPublic: boolean): Promise<boolean> {
    try {
      const response: WishlistResponse = await apiRequest(`${API_BASE}/Wishlists/${wishlistId}/share`, {
        method: 'PUT',
        body: JSON.stringify(isPublic),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      
      return response.isSuccess;
    } catch (error) {
      console.error('Failed to share wishlist:', error);
      return false;
    }
  }

  async duplicateWishlist(wishlistId: number, newName: string): Promise<WishlistDto> {
    try {
      const response: WishlistResponse<WishlistDto> = await apiRequest(`${API_BASE}/Wishlists/${wishlistId}/duplicate`, {
        method: 'POST',
        body: JSON.stringify(newName),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      
      if (!response.isSuccess || !response.data) {
        throw new Error(response.statusMessage || 'Failed to duplicate wishlist');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to duplicate wishlist:', error);
      throw new Error('Failed to duplicate wishlist');
    }
  }

  async getAllWishlists(): Promise<WishlistDto[]> {
    try {
      const response: WishlistResponse<WishlistDto[]> = await apiRequest(`${API_BASE}/Wishlists/admin/all`, {
        requireAuth: true,
      });
      
      if (!response.isSuccess || !response.data) {
        throw new Error(response.statusMessage || 'Failed to fetch all wishlists');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to fetch all wishlists:', error);
      throw new Error('Failed to fetch all wishlists');
    }
  }

  async getGlobalStats(): Promise<any> {
    try {
      const response: WishlistResponse<any> = await apiRequest(`${API_BASE}/Wishlists/admin/stats`, {
        requireAuth: true,
      });
      
      if (!response.isSuccess || !response.data) {
        throw new Error(response.statusMessage || 'Failed to fetch global stats');
      }
      return response.data;
    } catch (error) {
      console.error('Failed to fetch global stats:', error);
      throw new Error('Failed to fetch global stats');
    }
  }

  async adminDeleteWishlist(wishlistId: number): Promise<boolean> {
    try {
      const response: WishlistResponse = await apiRequest(`${API_BASE}/Wishlists/admin/${wishlistId}`, {
        method: 'DELETE',
        requireAuth: true
      });
      
      return response.isSuccess;
    } catch (error) {
      console.error('Failed to delete wishlist:', error);
      return false;
    }
  }

  async adminClearWishlist(wishlistId: number): Promise<boolean> {
    try {
      const response: WishlistResponse = await apiRequest(`${API_BASE}/Wishlists/admin/${wishlistId}/clear`, {
        method: 'DELETE',
        requireAuth: true
      });
      
      return response.isSuccess;
    } catch (error) {
      console.error('Failed to clear wishlist:', error);
      return false;
    }
  }

  async adminToggleWishlistVisibility(wishlistId: number, isPublic: boolean): Promise<boolean> {
    try {
      const response: WishlistResponse = await apiRequest(`${API_BASE}/Wishlists/admin/${wishlistId}/visibility`, {
        method: 'PUT',
        body: JSON.stringify(isPublic),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      
      return response.isSuccess;
    } catch (error) {
      console.error('Failed to toggle wishlist visibility:', error);
      return false;
    }
  }
} 