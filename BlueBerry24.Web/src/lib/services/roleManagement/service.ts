import { apiRequest } from "@/lib/utils/api";
import { ResponseDto } from "@/types/responseDto";
import {
    Role,
    UserWithRoles,
    RoleAssignmentRequest,
    CreateRoleRequest,
    RoleManagementResponse,
    RoleStats
} from "@/types/roleManagement"
import { User } from "../../../types/user";

const API_BASE_URL = process.env.API_BASE_ROLE_MANAGEMENT;

export class RoleManagementService{

    static async getAllRoles(): Promise<RoleManagementResponse<Role[]>> {

        const response: ResponseDto<Role[]>  = await apiRequest(`${API_BASE_URL}/roles`, {
            cache: 'no-store',
            requireAuth: true,
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to fetch roles: ${response.statusMessage}`);
        }

        return response;
    }

    static async createRole(request: CreateRoleRequest): Promise<RoleManagementResponse> {

        const response: ResponseDto<boolean> = await apiRequest(`${API_BASE_URL}/roles`, {
            method: 'POST',
            body: JSON.stringify(request.roleName),
            requireAuth: true,
        });

        if (!response.statusCode) {
            throw new Error(`Failed to create role: ${response.statusMessage}`);
        }

        return response;
    }

    static async updateRole(oldRoleName: string, newRoleName: string): Promise<RoleManagementResponse> {

        const response: ResponseDto<boolean> = await apiRequest(`${API_BASE_URL}/roles/${encodeURIComponent(oldRoleName)}`, {
            method: 'PUT',
            body: JSON.stringify(newRoleName),
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to update role: ${response.statusMessage}`);
        }

        return response;
    }

    static async deleteRole(roleName: string): Promise<RoleManagementResponse> {

        const response: ResponseDto<boolean> = await apiRequest(`${API_BASE_URL}/roles/${encodeURIComponent(roleName)}`, {
            method: 'DELETE',
            requireAuth: true,
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to delete role: ${response.statusMessage}`);
        }

        return response;
    }

    static async getAllUsers(): Promise<UserWithRoles[]> {


        const response: ResponseDto<UserWithRoles[]> = await apiRequest(`${API_BASE_URL}/users`, {
            requireAuth: true,
            cache: 'no-store',
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to fetch users: ${response.statusMessage}`);
        }

        return response.data;
    }

    static async getUserById(userId: number): Promise<RoleManagementResponse<UserWithRoles>> {

        const response: ResponseDto<UserWithRoles> = await apiRequest(`${API_BASE_URL}/users/${userId}`, {
            cache: 'no-store',
            requireAuth: true
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to fetch user: ${response.statusMessage}`);
        }

        return response;
    }

    static async getUserRoles(userId: number): Promise<RoleManagementResponse<string[]>> {


        const response: ResponseDto<string[]> = await apiRequest(`${API_BASE_URL}/users/${userId}/roles`, {

            requireAuth: true,
            cache: 'no-store',
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to fetch user roles: ${response.statusMessage}`);
        }

        return response;
    }

    static async getUsersInRole(roleName: string): Promise<RoleManagementResponse<User[]>> {


        const response: ResponseDto<User[]> = await apiRequest(`${API_BASE_URL}/roles/${encodeURIComponent(roleName)}/users`, {
            requireAuth: true,
            cache: 'no-store',
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to fetch users in role: ${response.statusMessage}`);
        }

        return response;
    }

    static async assignRoleToUser(request: RoleAssignmentRequest): Promise<RoleManagementResponse<boolean>> {


        const response: RoleManagementResponse<boolean> = await apiRequest(`${API_BASE_URL}/users/${request.userId}/roles/${encodeURIComponent(request.roleName)}`, {
            method: 'POST',
            requireAuth: true,
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to assign role: ${response.statusMessage}`);
        }

        return response;
    }

    static async removeRoleFromUser(request: RoleAssignmentRequest): Promise<RoleManagementResponse<boolean>> {


        const response: RoleManagementResponse<boolean> =
            await apiRequest(`${API_BASE_URL}/users/${request.userId}/roles/${encodeURIComponent(request.roleName)}`, {
            method: 'DELETE',
            requireAuth: true,
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to remove role: ${response.statusMessage}`);
        }

        return response;
    }

    static async isUserInRole(userId: number, roleName: string): Promise<RoleManagementResponse<{ userId: number; roleName: string; isInRole: boolean }>> {


        const response: RoleManagementResponse<{ userId: number; roleName: string; isInRole: boolean }> =
            await apiRequest(`${API_BASE_URL}/users/${userId}/roles/${encodeURIComponent(roleName)}/check`, {
            requireAuth: true,
            cache: 'no-store',
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to check user role: ${response.statusCode}`);
        }

        return response;
    }

    static async getRoleStats(): Promise<RoleManagementResponse<RoleStats>> {

        const response: RoleManagementResponse<RoleStats> = await apiRequest(`${API_BASE_URL}/stats`, {

            requireAuth: true,
            cache: 'no-store',
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to fetch role stats: ${response.statusMessage}`);
        }

        return response;
    }

    static async initializeDefaultRoles(): Promise<RoleManagementResponse> {


        const response: RoleManagementResponse = await apiRequest(`${API_BASE_URL}/rolemanagement/initialize-default-roles`, {
            method: 'POST',
            requireAuth: true,
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to initialize default roles: ${response.statusMessage}`);
        }

        return response;
    }

    static async bulkAssignRole(userIds: number[], roleName: string): Promise<RoleManagementResponse> {


        const response: RoleManagementResponse = await apiRequest(`${API_BASE_URL}/rolemanagement/bulk-assign`, {
            method: 'POST',
            requireAuth: true,
            body: JSON.stringify({ userIds, roleName }),
        });

        if (!response.isSuccess) {
            throw new Error(`Failed to bulk assign roles: ${response.statusMessage}`);
        }

        return response;
    }
} 