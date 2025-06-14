import { CartDto, 
         AddToCartDto, 
         UpdateCartItemDto, 
         ApplyCouponDto, 
         CheckoutRequest, 
         CheckoutResponse } from "@/types/cart";

export interface ICartService {
  create(): Promise<CartDto>;
  addItem(data: AddToCartDto): Promise<CartDto>;
  updateItemQuantity(cartId: number, data: UpdateCartItemDto): Promise<CartDto>;
  removeItem(cartId: number, productId: number): Promise<boolean>;
  clearCart(cartId: number): Promise<boolean>;
  completeCart(cartId: number): Promise<boolean>;
  applyCoupon(cartId: number, data: ApplyCouponDto): Promise<CartDto>;
  removeCoupon(cartId: number, couponId: number): Promise<CartDto>;
  getByUserId(): Promise<CartDto | null>;
  getBySessionId(sessionId: string): Promise<CartDto | null>;
  getById(id: number): Promise<CartDto>;
  checkout(cartId: number, data: CheckoutRequest): Promise<CheckoutResponse>;
} 