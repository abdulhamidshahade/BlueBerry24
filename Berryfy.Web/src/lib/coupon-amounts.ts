import { CouponDto, CouponType } from '../types/coupon';

export function discountFieldValueForForm(coupon: CouponDto): number {
  if (coupon.type === CouponType.Percentage) {
    if (coupon.value > 0 && coupon.value <= 1) {
      return coupon.value * 100;
    }
    if (coupon.value > 1) {
      return coupon.value;
    }
    return coupon.discountAmount > 0 ? coupon.discountAmount : 0;
  }
  return coupon.value > 0 ? coupon.value : coupon.discountAmount;
}

export function parseDiscountFormValues(
  type: CouponType,
  inputAmount: number
): { value: number; discountAmount: number } {
  if (!Number.isFinite(inputAmount) || inputAmount <= 0) {
    throw new Error('Discount amount must be greater than 0');
  }
  if (type === CouponType.Percentage) {
    if (inputAmount > 100) {
      throw new Error('Percentage cannot exceed 100');
    }
    if (inputAmount < 1) {
      return {
        value: inputAmount,
        discountAmount: Math.round(inputAmount * 10000) / 100,
      };
    }
    return { value: inputAmount / 100, discountAmount: inputAmount };
  }
  return { value: inputAmount, discountAmount: inputAmount };
}
