import { User } from "@/types/user";
import { CouponDto, CreateCouponDto, UpdateCouponDto, UserCouponDto } from "@/types/coupon";

export interface ICouponService {
  getAll(): Promise<CouponDto[]>;
  getById(id: number): Promise<CouponDto>;
  getByCode(code: string): Promise<CouponDto>;
  getUserCoupons(userId: number): Promise<CouponDto[]>;
  getUsersByCouponId(couponId: number): Promise<User[]>;
  create(data: CreateCouponDto): Promise<CouponDto>;
  update(id: number, data: UpdateCouponDto): Promise<CouponDto>;
  delete(id: number): Promise<boolean>;
  existsById(id: number): Promise<boolean>;
  existsByCode(code: string): Promise<boolean>;
  addCouponToUser(userId: number, couponId: number): Promise<UserCouponDto>;
  disableUserCoupon(userId: number, couponId: number): Promise<boolean>;
  hasUserUsedCoupon(userId: number, couponCode: string): Promise<boolean>;
  addCouponToSpecificUsers(couponId: number, userIds: number[]): Promise<boolean>;
  addCouponToAllUsers(couponId: number): Promise<boolean>;
  addCouponToNewUsers(couponId: number): Promise<boolean>;
}