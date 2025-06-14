import { 
  WishlistDto, 
  WishlistItemDto, 
  CreateWishlistDto, 
  UpdateWishlistDto, 
  AddToWishlistDto, 
  UpdateWishlistItemDto,
  WishlistSummaryDto,
  GlobalWishlistStatsDto 
} from '@/types/wishlist';

export interface IWishlistService {
  getUserWishlists(): Promise<WishlistDto[]>;
  getById(id: number): Promise<WishlistDto>;
  getDefaultWishlist(): Promise<WishlistDto>;
  getUserSummary(): Promise<WishlistSummaryDto>;
  create(data: CreateWishlistDto): Promise<WishlistDto>;
  update(id: number, data: UpdateWishlistDto): Promise<WishlistDto>;
  delete(id: number): Promise<boolean>;

  addItem(data: AddToWishlistDto): Promise<WishlistItemDto>;
  updateItem(wishlistId: number, productId: number, data: UpdateWishlistItemDto): Promise<WishlistItemDto>;
  removeItem(wishlistId: number, productId: number): Promise<boolean>;
  isProductInWishlist(productId: number): Promise<boolean>;

  addMultipleItems(wishlistId: number, productIds: number[]): Promise<boolean>;
  removeMultipleItems(wishlistId: number, productIds: number[]): Promise<boolean>;
  clearWishlist(wishlistId: number): Promise<boolean>;

  shareWishlist(wishlistId: number, isPublic: boolean): Promise<boolean>;
  duplicateWishlist(wishlistId: number, newName: string): Promise<WishlistDto>;

  getAllWishlists(): Promise<WishlistDto[]>;
  getGlobalStats(): Promise<GlobalWishlistStatsDto>;
  adminDeleteWishlist(wishlistId: number): Promise<boolean>;
  adminClearWishlist(wishlistId: number): Promise<boolean>;
  adminToggleWishlistVisibility(wishlistId: number, isPublic: boolean): Promise<boolean>;
} 