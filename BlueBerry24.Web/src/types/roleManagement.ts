export interface Role {
  id: string;
  name: string;
  normalizedName: string;
  concurrencyStamp?: string;
}

export interface UserWithRoles {
  id: number;
  email: string;
  userName: string;
  firstName?: string;
  lastName?: string;
  roles: string[];
  emailConfirmed: boolean;
  lockoutEnd?: string;
  accessFailedCount: number;
}

export interface RoleAssignmentRequest {
  userId: number;
  roleName: string;
}

export interface CreateRoleRequest {
  roleName: string;
}

export interface RoleManagementResponse<T = any> {
  isSuccess: boolean;
  statusCode: number;
  statusMessage: string;
  data?: T;
  errors?: string[];
}

export interface RoleStats {
  totalRoles: number;
  totalUsers: number;
  usersWithRoles: number;
  usersWithoutRoles: number;
  additionalStats?: {
    customRoles: number;
    systemRoles: number;
    usersPerRole: Array<{
      roleName: string;
      userCount: number;
    }>;
  };
}

export interface UserRoleHistory {
  userId: number;
  userName: string;
  roleName: string;
  action: 'assigned' | 'removed';
  timestamp: string;
  performedBy: string;
} 