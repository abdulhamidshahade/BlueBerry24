export interface CouponDto {
  id: number;
  code: string;
  discountAmount: number;
  minimumOrderAmount: number;
  description: string;
  isActive: boolean;
  type: CouponType;
  value: number;
  isForNewUsersOnly: boolean;
}

export interface CreateCouponDto {
  code: string;
  discountAmount: number;
  minimumAmount: number;
  description: string;
  isActive: boolean;
  type: CouponType;
  value: number;
  isForNewUsersOnly: boolean;
}

export interface UpdateCouponDto {
  id: number;
  code: string;
  discountAmount: number;
  minimumOrderAmount: number;
  description: string;
  isActive: boolean;
  type: CouponType;
  value: number;
  isForNewUsersOnly: boolean;
}

export interface UserCouponDto {
  userId: number;
  couponId: number;
  isUsed: boolean;
}

export interface AddCouponToUserDto{
  userId: number;
  couponId: number;
}

export enum CouponType {
  Percentage = 0,
  FixedAmount = 1,
  FreeShipping = 2,
  BuyXGetY = 3
}