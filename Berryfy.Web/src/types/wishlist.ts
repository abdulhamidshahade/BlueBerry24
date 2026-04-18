import { ProductDto } from './product';

export interface WishlistDto {
  id: number;
  userId: number;
  name: string;
  isDefault: boolean;
  isPublic: boolean;
  createdDate: string;
  updatedDate: string;
  itemCount: number;
  totalValue: number;
  items: WishlistItemDto[];
}

export interface WishlistItemDto {
  id: number;
  wishlistId: number;
  productId: number;
  notes?: string;
  priority: number;
  addedDate: string;
  product: ProductDto;
}

export interface WishlistSummaryDto {
  totalWishlists: number;
  totalItems: number;
  totalValue: number;
  recentWishlists: WishlistDto[];
}

export interface GlobalWishlistStatsDto {
  totalUsers: number;
  totalWishlists: number;
  totalItems: number;
  totalValue: number;
  averageItemsPerWishlist: number;
  averageWishlistsPerUser: number;
  publicWishlists: number;
  privateWishlists: number;
  recentActivity: RecentActivityDto[];
}

export interface RecentActivityDto {
  date: string;
  newWishlists: number;
  newItems: number;
}

export interface CreateWishlistDto {
  name: string;
  isPublic: boolean;
}

export interface UpdateWishlistDto {
  name: string;
  isPublic: boolean;
}

export interface AddToWishlistDto {
  productId: number;
  wishlistId?: number;
  notes?: string;
  priority: number;
}

export interface UpdateWishlistItemDto {
  notes?: string;
  priority: number;
}

export interface WishlistResponse<T = any> {
  isSuccess: boolean;
  statusCode?: number;
  statusMessage?: string;
  data?: T;
}

export enum WishlistPriority {
  Low = 1,
  Medium = 2,
  High = 3
}

export const WishlistPriorityLabels = {
  [WishlistPriority.Low]: 'Low',
  [WishlistPriority.Medium]: 'Medium',
  [WishlistPriority.High]: 'High'
};