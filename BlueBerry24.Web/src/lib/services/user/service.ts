import { User } from "../../../types/user";
import { IUserService} from "./interface";
import { CreateUserData, UpdateUserData } from "../../../types/user";
import { ResponseDto } from "../../../types/responseDto";
import { apiRequest } from "../../utils/api";

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
const API_BASE = process.env.API_BASE_AUTH

export class UserService implements IUserService {
  async getAll(): Promise<User[]> {
    try {
      const json: ResponseDto<User[]> = await apiRequest(`${API_BASE}/users`, {
        requireAuth: true
      });
      if (!json.isSuccess || !json.data) {
        console.warn('Users API returned no data, returning empty array');
        return [];
      }
      return json.data;
    } catch (error) {
      console.error('Failed to fetch users:', error);
      return [];
    }
  }

  async getById(id: number): Promise<User> {
    try {
      const json: ResponseDto<User> = await apiRequest(`${API_BASE}/users/${id}`, {
        requireAuth: true
      });
      if (!json.isSuccess || !json.data) throw new Error(json.statusMessage);
      return json.data;
    } catch (error) {
      console.error('Failed to fetch user by id:', error);
      throw new Error('Failed to fetch user');
    }
  }

  async lockAccount(userId: number, lockoutEnd?: string): Promise<boolean> {
    try {
      const body = lockoutEnd ? { lockoutEnd } : {};
      const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/users/${userId}/lock`, {
        method: 'POST',
        body: JSON.stringify(body),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      return json.isSuccess;
    } catch (error) {
      console.error('Failed to lock user account:', error);
      return false;
    }
  }

  async unlockAccount(userId: number): Promise<boolean> {
    try {
      const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/users/${userId}/unlock`, {
        method: 'POST',
        requireAuth: true
      });
      return json.isSuccess;
    } catch (error) {
      console.error('Failed to unlock user account:', error);
      return false;
    }
  }

  async resetPassword(userId: number, newPassword: string): Promise<boolean> {
    try {
      const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/users/${userId}/reset-password`, {
        method: 'POST',
        body: JSON.stringify({ newPassword }),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      return json.isSuccess;
    } catch (error) {
      console.error('Failed to reset user password:', error);
      return false;
    }
  }

  async verifyEmail(userId: number): Promise<boolean> {
    try {
      const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/users/${userId}/verify-email`, {
        method: 'POST',
        requireAuth: true
      });
      return json.isSuccess;
    } catch (error) {
      console.error('Failed to verify user email:', error);
      return false;
    }
  }

  async updateUser(userId: number, userData: UpdateUserData): Promise<boolean> {
    try {
      const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/users/${userId}`, {
        method: 'PUT',
        body: JSON.stringify(userData),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      return json.isSuccess;
    } catch (error) {
      console.error('Failed to update user:', error);
      return false;
    }
  }

  async createUser(userData: CreateUserData): Promise<User> {
    try {
      const json: ResponseDto<User> = await apiRequest(`${API_BASE}/users`, {
        method: 'POST',
        body: JSON.stringify(userData),
        headers: {
          'Content-Type': 'application/json',
        },
        requireAuth: true
      });
      if (!json.isSuccess || !json.data) throw new Error(json.statusMessage);
      return json.data;
    } catch (error) {
      console.error('Failed to create user:', error);
      throw new Error('Failed to create user');
    }
  }

  async deleteUser(userId: number): Promise<boolean> {
    try {
      const json: ResponseDto<boolean> = await apiRequest(`${API_BASE}/users/${userId}`, {
        method: 'DELETE',
        requireAuth: true
      });
      return json.isSuccess;
    } catch (error) {
      console.error('Failed to delete user:', error);
      return false;
    }
  }
} 