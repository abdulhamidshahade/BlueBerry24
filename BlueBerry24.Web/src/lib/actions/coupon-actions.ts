'use server'

import { revalidatePath } from 'next/cache';
import { redirect } from 'next/navigation';
import { CouponService } from '../services/coupon/service';
import { ICouponService } from '../services/coupon/interface';
import { CreateCouponDto, UpdateCouponDto, CouponType, CouponDto } from '../../types/coupon';
import { getCurrentUser } from './auth-actions';
import { User } from '../../types/user';
import { IUserService } from '../services/user/interface';
import { UserService } from '../services/user/service';

const couponService: ICouponService = new CouponService();
const userService:IUserService = new UserService();

export async function getCoupons(): Promise<CouponDto[]> {
  try {
    return await couponService.getAll();
  } catch (error) {
    console.error('Error fetching coupons:', error);
    return [];
  }
}

export async function getAllUsers(): Promise<User[]>{
  try{
    const users = await userService.getAll();
    if(!users){
      throw new Error('Error getting users!');
    }
    return users;
  }catch(error){
    console.error('Error fetching coupon:', error);
    return [];
  }
}

export async function getCoupon(id: number): Promise<CouponDto | null> {
  try {
    return await couponService.getById(id);
  } catch (error) {
    console.error('Error fetching coupon:', error);
    return null;
  }
}

export async function getCouponByCode(code: string): Promise<CouponDto | null> {
  try {
    return await couponService.getByCode(code);
  } catch (error) {
    console.error('Error fetching coupon by code:', error);
    return null;
  }
}

export async function getUserCoupons(userId?: number): Promise<CouponDto[]> {
  try {
    let targetUserId = userId;
    
    if (!targetUserId) {
      const currentUser = await getCurrentUser();
      if (!currentUser) {
        throw new Error('User not authenticated');
      }
      targetUserId = currentUser.id;
    }

    return await couponService.getUserCoupons(targetUserId);
  } catch (error) {
    console.error('Error fetching user coupons:', error);
    return [];
  }
}


export async function getCouponUsers(couponId: number): Promise<User[]> {
  try {
    let targetCouponId = couponId;
    
    if (!targetCouponId) {
      const currentCoupon = await couponService.getById(couponId);
      if (!currentCoupon) {
        throw new Error('User not authenticated');
      }
      targetCouponId = currentCoupon.id;
    }

    return await couponService.getUsersByCouponId(targetCouponId);
  } catch (error) {
    console.error('Error fetching user coupons:', error);
    return [];
  }
} 

export async function checkCouponExists(id: number): Promise<boolean> {
  try {
    return await couponService.existsById(id);
  } catch (error) {
    console.error('Error checking coupon existence:', error);
    return false;
  }
}

export async function checkCouponCodeExists(code: string): Promise<boolean> {
  try {
    return await couponService.existsByCode(code);
  } catch (error) {
    console.error('Error checking coupon code existence:', error);
    return false;
  }
}

export async function addCouponToUser(userId: number, couponId: number) {
  try {
    return await couponService.addCouponToUser(userId, couponId);
  } catch (error) {
    console.error('Error adding coupon to user:', error);
    throw new Error('Failed to add coupon to user');
  }
}

export async function disableUserCoupon(userId: number, couponId: number): Promise<boolean> {
  try {
    return await couponService.disableUserCoupon(userId, couponId);
  } catch (error) {
    console.error('Error disabling user coupon:', error);
    return false;
  }
}

export async function hasUserUsedCoupon(userId: number, couponCode: string): Promise<boolean> {
  try {
    return await couponService.hasUserUsedCoupon(userId, couponCode);
  } catch (error) {
    console.error('Error checking if user used coupon:', error);
    return false;
  }
}

export async function createCoupon(formData: FormData) {
  try {
    const couponData: CreateCouponDto = {
      code: formData.get('code') as string,
      description: formData.get('description') as string,
      type: parseInt(formData.get('type') as string) as CouponType,
      value: parseFloat(formData.get('value') as string),
      minimumAmount: parseFloat(formData.get('minimumAmount') as string) || 0,
      discountAmount: parseFloat(formData.get('value') as string), // Same as value for now
      isActive: formData.get('isActive') === 'on',
      isForNewUsersOnly: formData.get('isForNewUsersOnly') === 'on',
    };

    if (!couponData.code || couponData.code.trim() === '') {
      throw new Error('Coupon code is required');
    }

    if (!couponData.description || couponData.description.trim() === '') {
      throw new Error('Coupon description is required');
    }

    if (couponData.value <= 0) {
      throw new Error('Coupon value must be greater than 0');
    }

    const codeExists = await checkCouponCodeExists(couponData.code);
    if (codeExists) {
      throw new Error('A coupon with this code already exists');
    }

    await couponService.create(couponData);
    
    revalidatePath('/admin/coupons');
    redirect('/admin/coupons?success=created');
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error creating coupon:', error);
    if (error instanceof Error) {
      redirect(`/admin/coupons/create?error=${encodeURIComponent(error.message)}`);
    } else {
      redirect('/admin/coupons/create?error=Failed%20to%20create%20coupon');
    }
  }
}

export async function updateCoupon(formData: FormData) {
  try {
    const id = parseInt(formData.get('id') as string);
    
    const couponExists = await checkCouponExists(id);
    if (!couponExists) {
      throw new Error('Coupon not found');
    }

    const couponData: UpdateCouponDto = {
      id,
      code: formData.get('code') as string,
      description: formData.get('description') as string,
      type: parseInt(formData.get('type') as string) as CouponType,
      value: parseFloat(formData.get('value') as string),
      minimumOrderAmount: parseFloat(formData.get('minimumAmount') as string) || 0,
      discountAmount: parseFloat(formData.get('value') as string),
      isActive: formData.get('isActive') === 'on',
      isForNewUsersOnly: formData.get('isForNewUsersOnly') === 'on',
    };

    if (!couponData.code || couponData.code.trim() === '') {
      throw new Error('Coupon code is required');
    }

    if (!couponData.description || couponData.description.trim() === '') {
      throw new Error('Coupon description is required');
    }

    if (couponData.value <= 0) {
      throw new Error('Coupon value must be greater than 0');
    }

    await couponService.update(id, couponData);
    
    revalidatePath('/admin/coupons');
    redirect('/admin/coupons?success=updated');
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error updating coupon:', error);
    const id = formData.get('id') as string;
    if (error instanceof Error) {
      redirect(`/admin/coupons/update/${id}?error=${encodeURIComponent(error.message)}`);
    } else {
      redirect(`/admin/coupons/update/${id}?error=Failed%20to%20update%20coupon`);
    }
  }
}

export async function deleteCoupon(formData: FormData) {
  try {
    const id = parseInt(formData.get('id') as string);
    
    const couponExists = await checkCouponExists(id);
    if (!couponExists) {
      throw new Error('Coupon not found');
    }

    await couponService.delete(id);
    
    revalidatePath('/admin/coupons');
    redirect('/admin/coupons?success=deleted');
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error deleting coupon:', error);
    const id = formData.get('id') as string;
    if (error instanceof Error) {
      redirect(`/admin/coupons/delete/${id}?error=${encodeURIComponent(error.message)}`);
    } else {
      redirect(`/admin/coupons/delete/${id}?error=Failed%20to%20delete%20coupon`);
    }
  }
}

export async function toggleCouponStatus(formData: FormData) {
  try {
    const id = parseInt(formData.get('id') as string);
    
    const currentCoupon = await getCoupon(id);
    if (!currentCoupon) {
      throw new Error('Coupon not found');
    }

    const updateData: UpdateCouponDto = {
      id: currentCoupon.id,
      code: currentCoupon.code,
      description: currentCoupon.description,
      type: currentCoupon.type,
      value: currentCoupon.value,
      minimumOrderAmount: currentCoupon.minimumOrderAmount,
      discountAmount: currentCoupon.discountAmount,
      isActive: !currentCoupon.isActive,
      isForNewUsersOnly: currentCoupon.isForNewUsersOnly,
    };

    await couponService.update(id, updateData);
    
    revalidatePath('/admin/coupons');
    redirect('/admin/coupons?success=status_toggled');
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error toggling coupon status:', error);
    if (error instanceof Error) {
      redirect(`/admin/coupons?error=${encodeURIComponent(error.message)}`);
    } else {
      redirect('/admin/coupons?error=Failed%20to%20update%20coupon%20status');
    }
  }
}

export async function addCouponToUserAction(formData: FormData) {
  try {
    const userId = parseInt(formData.get('userId') as string);
    const couponId = parseInt(formData.get('couponId') as string);

    if (!userId || !couponId) {
      throw new Error('User and coupon are required');
    }

    const couponExists = await checkCouponExists(couponId);
    if (!couponExists) {
      throw new Error('Coupon not found');
    }

    await addCouponToUser(userId, couponId);
    
    revalidatePath('/admin/coupons');
    redirect(`/admin/coupons/${couponId}/users?success=coupon_added`);
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error adding coupon to user:', error);
    if (error instanceof Error) {
      redirect(`/admin/coupons/add-to-user?error=${encodeURIComponent(error.message)}`);
    } else {
      redirect('/admin/coupons/add-to-user?error=Failed%20to%20add%20coupon%20to%20user');
    }
  }
}

export async function addCouponToSpecificUsersAction(formData: FormData) {
  try {
    const couponId = parseInt(formData.get('couponId') as string);
    const userIdsString = formData.getAll('userIds') as string[];

    if (!couponId || !userIdsString) {
      throw new Error('Coupon and users are required');
    }

    const userIds = userIdsString.map(id => parseInt(id.trim())).filter(id => !isNaN(id));
    
    if (userIds.length === 0) {
      throw new Error('At least one user must be selected');
    }

    const couponExists = await checkCouponExists(couponId);
    if (!couponExists) {
      throw new Error('Coupon not found');
    }

    const success = await couponService.addCouponToSpecificUsers(couponId, userIds);
    if (!success) {
      throw new Error('Failed to add coupon to specific users');
    }
    
    revalidatePath('/admin/coupons');
    redirect('/admin/coupons?success=coupon_added_to_specific_users');
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error adding coupon to specific users:', error);
    if (error instanceof Error) {
      redirect(`/admin/coupons/add-to-specific-users?error=${encodeURIComponent(error.message)}`);
    } else {
      redirect('/admin/coupons/add-to-specific-users?error=Failed%20to%20add%20coupon%20to%20specific%20users');
    }
  }
}

export async function addCouponToAllUsersAction(formData: FormData) {
  try {
    const couponId = parseInt(formData.get('couponId') as string);

    if (!couponId) {
      throw new Error('Coupon is required');
    }

    const couponExists = await checkCouponExists(couponId);
    if (!couponExists) {
      throw new Error('Coupon not found');
    }

    const success = await couponService.addCouponToAllUsers(couponId);
    if (!success) {
      throw new Error('Failed to add coupon to all users');
    }
    
    revalidatePath('/admin/coupons');
    redirect('/admin/coupons?success=coupon_added_to_all_users');
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error adding coupon to all users:', error);
    if (error instanceof Error) {
      redirect(`/admin/coupons/add-to-all-users?error=${encodeURIComponent(error.message)}`);
    } else {
      redirect('/admin/coupons/add-to-all-users?error=Failed%20to%20add%20coupon%20to%20all%20users');
    }
  }
}

export async function addCouponToNewUsersAction(formData: FormData) {
  try {
    const couponId = parseInt(formData.get('couponId') as string);

    if (!couponId) {
      throw new Error('Coupon is required');
    }

    const couponExists = await checkCouponExists(couponId);
    if (!couponExists) {
      throw new Error('Coupon not found');
    }

    const success = await couponService.addCouponToNewUsers(couponId);
    if (!success) {
      throw new Error('Failed to add coupon to new users');
    }
    
    revalidatePath('/admin/coupons');
    redirect('/admin/coupons?success=coupon_added_to_new_users');
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error adding coupon to new users:', error);
    if (error instanceof Error) {
      redirect(`/admin/coupons/add-to-new-users?error=${encodeURIComponent(error.message)}`);
    } else {
      redirect('/admin/coupons/add-to-new-users?error=Failed%20to%20add%20coupon%20to%20new%20users');
    }
  }
}

export async function disableUserCouponAction(formData: FormData) {
  try {
    const userId = parseInt(formData.get('userId') as string);
    const couponId = parseInt(formData.get('couponId') as string);

    if (!userId || !couponId) {
      throw new Error('User and coupon are required');
    }

    const success = await disableUserCoupon(userId, couponId);
    if (!success) {
      throw new Error('Failed to disable user coupon');
    }
    
    revalidatePath('/admin/coupons');
    revalidatePath(`/admin/coupons/${couponId}/users`);
    redirect(`/admin/coupons/${couponId}/users?success=coupon_disabled`);
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error disabling user coupon:', error);
    const couponId = formData.get('couponId') as string;
    if (error instanceof Error) {
      redirect(`/admin/coupons/${couponId}/users?error=${encodeURIComponent(error.message)}`);
    } else {
      redirect(`/admin/coupons/${couponId}/users?error=Failed%20to%20disable%20user%20coupon`);
    }
  }
} 