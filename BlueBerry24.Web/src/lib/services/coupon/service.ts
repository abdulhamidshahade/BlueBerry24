import { CouponDto, CreateCouponDto, UpdateCouponDto, UserCouponDto } from "@/types/coupon";
import { ICouponService } from "./interface";
import { ResponseDto } from "@/types/responseDto";
import { apiRequest } from "@/lib/utils/api";
import { User } from "@/types/auth";

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
const API_BASE = process.env.API_BASE_COUPON;

export class CouponService implements ICouponService {
  async getAll(): Promise<CouponDto[]> {
    try {
      const json: ResponseDto<CouponDto[]> = await apiRequest(`${API_BASE}`, {
        requireAuth: true,
        cache: 'no-store'
      });
      if (!json.isSuccess || !json.data) {
        console.warn('Coupons API returned no data, returning empty array');
        return [];
      }
      return json.data;
    } catch (error) {
      console.error('Failed to fetch coupons:', error);
      return [];
    }
  }

  async getById(id: number): Promise<CouponDto> {
    try {
      const json: ResponseDto<CouponDto> = await apiRequest(`${API_BASE}/${id}`, {
        requireAuth: true,
        cache: 'no-store'
      });
      if (!json.isSuccess || !json.data) throw new Error(json.statusMessage);
      return json.data;
    } catch (error) {
      console.error('Failed to fetch coupon by id:', error);
      throw new Error('Failed to fetch coupon');
    }
  }

  async getByCode(code: string): Promise<CouponDto> {
    try {
      const json: ResponseDto<CouponDto> = await apiRequest(`${API_BASE}/code/${code}`, {
        requireAuth: true,
        cache: 'no-store'
      });
      if (!json.isSuccess || !json.data) throw new Error(json.statusMessage);
      return json.data;
    } catch (error) {
      console.error('Failed to fetch coupon by code:', error);
      throw new Error('Failed to fetch coupon');
    }
  }

  async getUserCoupons(userId: number): Promise<CouponDto[]> {
    try {
      const res: ResponseDto<CouponDto[]> = await apiRequest(`${API_BASE}/users/${userId}/coupons`, {
        cache: 'no-store',
        requireAuth: true
      });

      if (!res.isSuccess || !res.data) {
        console.warn('No coupons found for user, returning empty array');
        return [];
      }
      return res.data;
    } catch (error) {
      console.error('Failed to fetch user coupons:', error);
      return [];
    }
  }

  async getUsersByCouponId(couponId: number): Promise<User[]> {
    try {
      const res: ResponseDto<User[]> = await apiRequest(`${API_BASE}/${couponId}/users`, {
        requireAuth: true,
        cache: 'no-store'
      });

      if (!res.isSuccess || !res.data) {
        console.warn('No users found for coupon, returning empty array');
        return [];
      }
      return res.data;
    } catch (error) {
      console.error('Failed to fetch users by coupon:', error);
      return [];
    }
  }

  async create(data: CreateCouponDto): Promise<CouponDto> {
    try {
      const json: ResponseDto<CouponDto> = await apiRequest(`${API_BASE}`, {
        method: "POST",
        body: JSON.stringify(data),
        requireAuth: true
      });
      if (!json.isSuccess || !json.data) throw new Error(json.statusMessage);
      return json.data;
    } catch (error) {
      console.error('Failed to create coupon:', error);
      throw new Error('Failed to create coupon');
    }
  }

  async update(id: number, data: UpdateCouponDto): Promise<CouponDto> {
    try {
      const json: ResponseDto<CouponDto> = await apiRequest(`${API_BASE}/${id}`, {
        method: "PUT",
        body: JSON.stringify(data),
        requireAuth: true
      });
      if (!json.isSuccess || !json.data) throw new Error(json.statusMessage);
      return json.data;
    } catch (error) {
      console.error('Failed to update coupon:', error);
      throw new Error('Failed to update coupon');
    }
  }

  async delete(id: number): Promise<boolean> {
    try {
      const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/${id}`, {
        method: "DELETE",
        requireAuth: true
      });
      return json.isSuccess;
    } catch (error) {
      console.error('Failed to delete coupon:', error);
      throw new Error('Failed to delete coupon');
    }
  }

  async existsById(id: number): Promise<boolean> {
    try {
      const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/exists-by-id/${id}`, {
        requireAuth: true
      });
      return json.isSuccess;
    } catch (error) {
      console.error('Failed to check coupon existence by id:', error);
      return false;
    }
  }

  async existsByCode(code: string): Promise<boolean> {
    try {
      const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/exists-by-code/${code}`, {
        requireAuth: true
      });
      return json.isSuccess;
    } catch (error) {
      console.error('Failed to check coupon existence by code:', error);
      return false;
    }
  }

  async addCouponToUser(userId: number, couponId: number): Promise<any> {
    try {
      const json: ResponseDto<any> = await apiRequest(`${API_BASE}/users/${userId}/coupons`, {
        method: "POST",
        body: JSON.stringify({ couponId }),
        requireAuth: true
      });
      if (!json.isSuccess || !json.data) throw new Error(json.statusMessage);
      return json.data;
    } catch (error) {
      console.error('Failed to add coupon to user:', error);
      throw new Error('Failed to add coupon to user');
    }
  }

  async disableUserCoupon(userId: number, couponId: number): Promise<boolean> {
    try {
      const res: ResponseDto<boolean> = await apiRequest(`${API_BASE}/users/${userId}/coupons/${couponId}/disable`, {
        method: 'PUT',
        requireAuth: true
      });

      return res.isSuccess;
    } catch (error) {
      console.error('Failed to disable user coupon:', error);
      throw new Error('Failed to disable user coupon');
    }
  }

  async hasUserUsedCoupon(userId: number, couponCode: string): Promise<boolean> {
    try {
      const res: ResponseDto<any> = await apiRequest(`${API_BASE}/users/${userId}/coupons/${couponCode}/used`, {
        requireAuth: true
      });

      if (res.isSuccess && res.statusMessage === "The coupon has used") {
        return true;
      }
      return false;
    } catch (error) {
      console.error('Failed to check coupon usage:', error);
      return false;
    }
  }

  async addCouponToSpecificUsers(couponId: number, userIds: number[]): Promise<boolean> {
    try {
      const queryParams = userIds.map(id => `UserIds=${id}`).join('&');
      const res: ResponseDto<any> = await apiRequest(`${API_BASE}/add-coupon-to-specific-users/${couponId}?${queryParams}`, {
        method: 'POST',
        requireAuth: true
      });

      return res.isSuccess;
    } catch (error) {
      console.error('Failed to add coupon to specific users:', error);
      throw new Error('Failed to add coupon to specific users');
    }
  }

  async addCouponToAllUsers(couponId: number): Promise<boolean> {
    try {
      const res: ResponseDto<any> = await apiRequest(`${API_BASE}/add-coupon-to-all-users/${couponId}`, {
        method: 'POST',
        requireAuth: true
      });

      return res.isSuccess;
    } catch (error) {
      console.error('Failed to add coupon to all users:', error);
      throw new Error('Failed to add coupon to all users');
    }
  }

  async addCouponToNewUsers(couponId: number): Promise<boolean> {
    try {
      const res: ResponseDto<any> = await apiRequest(`${API_BASE}/add-coupon-to-new-users/${couponId}`, {
        method: 'POST',
        requireAuth: true
      });

      return res.isSuccess;
    } catch (error) {
      console.error('Failed to add coupon to new users:', error);
      throw new Error('Failed to add coupon to new users');
    }
  }
}