'use server';

import { revalidatePath } from 'next/cache';
import { UserService } from '@/lib/services/user/service';
import { IUserService } from '@/lib/services/user/interface';
import { CreateUserData, UpdateUserData } from '@/types/user';

const userService: IUserService = new UserService();

export async function lockCustomerAccount(userId: number, lockoutEnd?: string) {
  try {
    const result = await userService.lockAccount(userId, lockoutEnd);
    if (result) {
      revalidatePath('/admin/customers');
      return { success: true };
    }
    return { success: false, error: 'Failed to lock customer account' };
  } catch (error) {
    console.error('Error locking customer account:', error);
    return { success: false, error: 'An error occurred while locking the account' };
  }
}

export async function unlockCustomerAccount(userId: number) {
  try {
    const result = await userService.unlockAccount(userId);
    if (result) {
      revalidatePath('/admin/customers');
      return { success: true };
    }
    return { success: false, error: 'Failed to unlock customer account' };
  } catch (error) {
    console.error('Error unlocking customer account:', error);
    return { success: false, error: 'An error occurred while unlocking the account' };
  }
}

export async function resetCustomerPassword(userId: number, newPassword: string) {
  try {
    const result = await userService.resetPassword(userId, newPassword);
    if (result) {
      revalidatePath('/admin/customers');
      return { success: true };
    }
    return { success: false, error: 'Failed to reset customer password' };
  } catch (error) {
    console.error('Error resetting customer password:', error);
    return { success: false, error: 'An error occurred while resetting the password' };
  }
}

export async function verifyCustomerEmail(userId: number) {
  try {
    const result = await userService.verifyEmail(userId);
    if (result) {
      revalidatePath('/admin/customers');
      return { success: true };
    }
    return { success: false, error: 'Failed to verify customer email' };
  } catch (error) {
    console.error('Error verifying customer email:', error);
    return { success: false, error: 'An error occurred while verifying the email' };
  }
}

export async function updateCustomer(userId: number, userData: UpdateUserData) {
  try {
    const result = await userService.updateUser(userId, userData);
    if (result) {
      revalidatePath('/admin/customers');
      return { success: true };
    }
    return { success: false, error: 'Failed to update customer' };
  } catch (error) {
    console.error('Error updating customer:', error);
    return { success: false, error: 'An error occurred while updating the customer' };
  }
}

export async function createCustomer(userData: CreateUserData) {
  try {
    const result = await userService.createUser(userData);
    if (result) {
      revalidatePath('/admin/customers');
      return { success: true, data: result };
    }
    return { success: false, error: 'Failed to create customer' };
  } catch (error) {
    console.error('Error creating customer:', error);
    return { success: false, error: 'An error occurred while creating the customer' };
  }
}

export async function deleteCustomer(userId: number) {
  try {
    const result = await userService.deleteUser(userId);
    if (result) {
      revalidatePath('/admin/customers');
      return { success: true };
    }
    return { success: false, error: 'Failed to delete customer' };
  } catch (error) {
    console.error('Error deleting customer:', error);
    return { success: false, error: 'An error occurred while deleting the customer' };
  }
} 