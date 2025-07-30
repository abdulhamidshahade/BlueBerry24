'use server';

import { revalidatePath } from 'next/cache';
import { redirect } from 'next/navigation';
import { RoleManagementService } from '../services/roleManagement/service';
import { getCurrentUser } from './auth-actions';
import { Role, UserWithRoles, RoleStats } from '../../types/roleManagement';

async function checkSuperAdminAccess() {
  const user = await getCurrentUser();
  
  if (!user || !user.roles.includes('SuperAdmin')) {
    redirect('/auth/login?error=unauthorized');
  }
  
  return user;
}

export async function createRoleAction(formData: FormData) {
  await checkSuperAdminAccess();

  const roleName = formData.get('roleName') as string;
  const redirectUrl = formData.get('redirectUrl') as string;

  if (!roleName || roleName.trim() === '') {
    redirect(`/admin/role-management?error=${encodeURIComponent('Role name is required')}`);
  }

  const systemRoles = ['SuperAdmin', 'Admin', 'User'];
  if (systemRoles.includes(roleName.trim())) {
    redirect(`/admin/role-management?error=${encodeURIComponent(`Cannot create system role "${roleName.trim()}". Please choose a different name.`)}`);
  }

  try {
    const response = await RoleManagementService.createRole({ roleName: roleName.trim() });

    if (response.isSuccess) {
      revalidatePath('/admin/role-management');
      redirect(redirectUrl || `/admin/role-management?success=${encodeURIComponent('Role created successfully')}`);
    } else {
      redirect(`/admin/role-management?error=${encodeURIComponent(response.statusMessage || 'Failed to create role')}`);
    }
  } catch (error) {
    console.error('Create role error:', error);
    redirect(`/admin/role-management?error=${encodeURIComponent('An unexpected error occurred while creating the role')}`);
  }
}

export async function updateRoleAction(formData: FormData) {
  await checkSuperAdminAccess();

  const oldRoleName = formData.get('oldRoleName') as string;
  const newRoleName = formData.get('newRoleName') as string;
  const redirectUrl = formData.get('redirectUrl') as string;

  if (!oldRoleName || !newRoleName || newRoleName.trim() === '') {
    redirect(`/admin/role-management?error=${encodeURIComponent('Both old and new role names are required')}`);
  }

  try {
    const response = await RoleManagementService.updateRole(oldRoleName, newRoleName.trim());

    if (response.isSuccess) {
      revalidatePath('/admin/role-management');
      redirect(redirectUrl || `/admin/role-management?success=${encodeURIComponent(response.statusMessage || 'Role updated successfully')}`);
    } else {
      redirect(`/admin/role-management?error=${encodeURIComponent(response.statusMessage || 'Failed to update role')}`);
    }
  } catch (error) {
    console.error('Update role error:', error);
    redirect(`/admin/role-management?error=${encodeURIComponent('An unexpected error occurred while updating the role')}`);
  }
}

export async function deleteRoleAction(formData: FormData) {
  await checkSuperAdminAccess();

  const roleName = formData.get('roleName') as string;
  const redirectUrl = formData.get('redirectUrl') as string;

  if (!roleName) {
    redirect(`/admin/role-management?error=${encodeURIComponent('Role name is required')}`);
  }

  const protectedRoles = ['SuperAdmin', 'Admin'];
  if (protectedRoles.includes(roleName)) {
    redirect(`/admin/role-management?error=${encodeURIComponent('Cannot delete system roles (SuperAdmin, Admin)')}`);
  }

  try {
    const response = await RoleManagementService.deleteRole(roleName);

    if (response.isSuccess) {
      revalidatePath('/admin/role-management');
      redirect(redirectUrl || `/admin/role-management?success=${encodeURIComponent(response.statusMessage || 'Role deleted successfully')}`);
    } else {
      redirect(`/admin/role-management?error=${encodeURIComponent(response.statusMessage || 'Failed to delete role')}`);
    }
  } catch (error) {
    console.error('Delete role error:', error);
    redirect(`/admin/role-management?error=${encodeURIComponent('An unexpected error occurred while deleting the role')}`);
  }
}

export async function assignRoleToUserAction(formData: FormData) {
  await checkSuperAdminAccess();

  const userId = parseInt(formData.get('userId') as string);
  const roleName = formData.get('roleName') as string;
  const redirectUrl = formData.get('redirectUrl') as string;

  if (!userId || !roleName) {
    redirect(`/admin/role-management?error=${encodeURIComponent('User ID and role name are required')}`);
  }

  try {
    const response = await RoleManagementService.assignRoleToUser({ userId, roleName });

    if (response.isSuccess) {
      revalidatePath('/admin/role-management');
      redirect(redirectUrl || `/admin/role-management?success=${encodeURIComponent(`Successfully assigned ${roleName} role to user`)}`);
    } else {
      redirect(`/admin/role-management?error=${encodeURIComponent(response.statusMessage || 'Failed to assign role to user')}`);
    }
  } catch (error) {
    console.error('Assign role error:', error);
    redirect(`/admin/role-management?error=${encodeURIComponent('An unexpected error occurred while assigning the role')}`);
  }
}

export async function removeRoleFromUserAction(formData: FormData) {
  await checkSuperAdminAccess();

  const userId = parseInt(formData.get('userId') as string);
  const roleName = formData.get('roleName') as string;
  const redirectUrl = formData.get('redirectUrl') as string;

  if (!userId || !roleName) {
    redirect(`/admin/role-management?error=${encodeURIComponent('User ID and role name are required')}`);
  }

  if (roleName === 'SuperAdmin') {
    try {
      const superAdmins = await RoleManagementService.getUsersInRole('SuperAdmin');
      if (superAdmins.isSuccess && superAdmins.data && superAdmins.data.length <= 1) {
        redirect(`/admin/role-management?error=${encodeURIComponent('Cannot remove the last SuperAdmin role')}`);
      }
    } catch (error) {
      console.error('Error checking SuperAdmin count:', error);
      redirect(`/admin/role-management?error=${encodeURIComponent('Error validating SuperAdmin removal')}`);
    }
  }

  try {
    const response = await RoleManagementService.removeRoleFromUser({ userId, roleName });

    if (response.isSuccess) {
      revalidatePath('/admin/role-management');
      redirect(redirectUrl || `/admin/role-management?success=${encodeURIComponent(`Successfully removed ${roleName} role from user`)}`);
    } else {
      redirect(`/admin/role-management?error=${encodeURIComponent(response.statusMessage || 'Failed to remove role from user')}`);
    }
  } catch (error) {
    console.error('Remove role error:', error);
    redirect(`/admin/role-management?error=${encodeURIComponent('An unexpected error occurred while removing the role')}`);
  }
}

export async function bulkAssignRolesAction(formData: FormData) {
  await checkSuperAdminAccess();

  const userIds = formData.getAll('userIds') as string[];
  const roleName = formData.get('roleName') as string;
  const redirectUrl = formData.get('redirectUrl') as string;

  if (!userIds.length || !roleName) {
    redirect(`/admin/role-management?error=${encodeURIComponent('Please select users and a role for bulk assignment')}`);
  }

  try {
    const userIdArray = userIds.map(id => parseInt(id)).filter(id => !isNaN(id));
    
    if (userIdArray.length === 0) {
      redirect(`/admin/role-management?error=${encodeURIComponent('No valid users selected')}`);
    }

    const response = await RoleManagementService.bulkAssignRole(userIdArray, roleName);

    if (response.isSuccess) {
      revalidatePath('/admin/role-management');
      redirect(redirectUrl || `/admin/role-management?success=${encodeURIComponent(`Successfully assigned ${roleName} role to ${userIdArray.length} user(s)`)}`);
    } else {
      redirect(`/admin/role-management?error=${encodeURIComponent(response.statusMessage || 'Failed to bulk assign roles')}`);
    }
  } catch (error) {
    console.error('Bulk assign roles error:', error);
    redirect(`/admin/role-management?error=${encodeURIComponent('An unexpected error occurred during bulk role assignment')}`);
  }
}

export async function initializeDefaultRolesAction(formData: FormData) {
  await checkSuperAdminAccess();

  const redirectUrl = formData.get('redirectUrl') as string;

  try {
    const response = await RoleManagementService.initializeDefaultRoles();

    if (response.isSuccess) {
      revalidatePath('/admin/role-management');
      redirect(redirectUrl || `/admin/role-management?success=${encodeURIComponent(response.statusMessage || 'Default roles initialized successfully')}`);
    } else {
      redirect(`/admin/role-management?error=${encodeURIComponent(response.statusMessage || 'Failed to initialize default roles')}`);
    }
  } catch (error) {
    console.error('Initialize default roles error:', error);
    redirect(`/admin/role-management?error=${encodeURIComponent('An unexpected error occurred while initializing default roles')}`);
  }
} 

export async function getRoleManagementData() {
  try {
    const [rolesResponse, usersResponse, statsResponse] = await Promise.all([
      RoleManagementService.getAllRoles(),
      RoleManagementService.getAllUsers(),
      RoleManagementService.getRoleStats(),
    ]);

    const roles = rolesResponse.isSuccess ? rolesResponse.data || [] : [];
    const users = usersResponse ? usersResponse || [] : [];
    const stats = statsResponse.isSuccess ? statsResponse.data : null;

    const errors = [];
    if (!rolesResponse.isSuccess) errors.push(`Roles: ${rolesResponse.statusMessage}`);
    if (!usersResponse) errors.push(`Users: ${usersResponse}`);
    if (!statsResponse.isSuccess) errors.push(`Stats: ${statsResponse.statusMessage}`);

    return {
      roles,
      users,
      stats,
      error: errors.length > 0 ? errors.join('; ') : null,
    };
  } catch (error) {
    console.error('Error fetching role management data:', error);
    return {
      roles: [] as Role[],
      users: [] as UserWithRoles[],
      stats: null as RoleStats | null,
      error: 'Failed to load role management data. Please check your connection and try again.',
    };
  }
}