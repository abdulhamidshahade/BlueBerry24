import { User } from "../../../types/user";
import { Role, RoleManagementResponse, RoleStats, UserWithRoles } from "../../../types/roleManagement";

export interface IRoleManagementService {
    getAllRoles(): Promise<RoleManagementResponse<Role[]>>;
    createRole(roleName: string): Promise<RoleManagementResponse>;
    updateRole(oldRoleName: string, newRoleName: string): Promise<RoleManagementResponse>;
    deleteRole(roleName: string): Promise<RoleManagementResponse>;
    getAllUsers(): Promise<RoleManagementResponse<UserWithRoles[]>>;
    getUserById(userId: number): Promise<RoleManagementResponse<UserWithRoles>>;
    getUserRoles(userId: numberBu): Promise<RoleManagementResponse<string[]>>;
    getUsersInRole(roleName: string): Promise<RoleManagementResponse<User[]>>;
    assignRoleToUser(userId: number, roleName: string): Promise<RoleManagementResponse<boolean>>;
    removeRoleFromUser(userId: number, roleName: string): Promise<RoleManagementResponse<boolean>>;
    isUserInRole(userId: number, roleName: string): Promise<RoleManagementResponse<boolean>>;
    getRoleStats(): Promise<RoleManagementResponse<RoleStats>>;
    initializeDefaultRoles(): Promise<RoleManagementResponse>;
    bulkAssignRole(userIds: number[], roleName: string): Promise<RoleManagementResponse>;
}