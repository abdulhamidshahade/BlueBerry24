import { ProductDto } from "./product"; 

export interface CartDto {
  id: number;
  isActive: boolean;
  userId?: number;
  sessionId?: string;
  status: CartStatus;
  expiresAt?: string;
  cartItems: CartItemDto[];
  cartCoupons: CartCouponDto[];
  note?: string;
  subTotal: number;
  discountTotal: number;
  taxAmount: number;
  shippingAmount: number;
  total: number;
  createdAt: string;
  updatedAt: string;
  version: number;
}

export interface CartItemDto {
  id: number;
  shoppingCartId: number;
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  product?: ProductDto;
  createdAt: string;
  updatedAt: string;
}

export interface CartCouponDto {
  id: number;
  couponId: number;
  userId?: number;
  sessionId?: string;
  cartId: number;
  description: string;
  discountAmount: number;
  appliedAt: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateCartDto {
  userId?: number;
  sessionId?: string;
}

export interface AddToCartDto {
  productId: number;
  quantity: number;
  userId?: number;
  sessionId?: string;
  cartId?: number;
}

export interface UpdateCartItemDto {
  productId: number;
  quantity: number;
  userId?: number;
  sessionId?: string;
}

export interface ApplyCouponDto {
  couponCode: string;
}

export interface CheckoutRequest {
  cartId: number;
  customerEmail: string;
  customerPhone?: string;
  shippingName: string;
  shippingAddressLine1: string;
  shippingAddressLine2?: string;
  shippingCity: string;
  shippingState: string;
  shippingPostalCode: string;
  shippingCountry: string;
  paymentProvider: string;
  paymentTransactionId: number;
  isPaid: boolean;
}

export interface CheckoutResponse {
  id: number;
  orderNumber: string;
  total: number;
  status: string;
}

export enum CartStatus {
  Active = 0,
  Abandoned = 1,
  Converted = 2,
  Expired = 3
}