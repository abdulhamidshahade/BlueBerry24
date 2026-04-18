import { User } from "../../../types/user";
import { CreateUserData, UpdateUserData } from "../../../types/user";

export interface IUserService {
  getAll(): Promise<User[]>;
  getById(id: number): Promise<User>;
  
  lockAccount(userId: number, lockoutEnd?: string): Promise<boolean>;
  unlockAccount(userId: number): Promise<boolean>;
  resetPassword(userId: number, newPassword: string): Promise<boolean>;
  verifyEmail(userId: number): Promise<boolean>;
  updateUser(userId: number, userData: UpdateUserData): Promise<boolean>;
  createUser(userData: CreateUserData): Promise<User>;
  deleteUser(userId: number): Promise<boolean>;
}

