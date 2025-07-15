import { Suspense } from 'react';
import { redirect } from 'next/navigation';
import { getCurrentUser } from '../../../lib/actions/auth-actions';
import RoleManagementHeader from '../../../components/roleManagement/RoleManagementHeader';
import RoleManagementStats from '../../../components/roleManagement/RoleManagementStats';
import RoleManagementTabs from '../../../components/roleManagement/RoleManagementTabs';
import RolesTable from '../../../components/roleManagement/RolesTable';
import UsersTable from '../../../components/roleManagement/UsersTable';
import CreateRoleForm from '../../../components/roleManagement/CreateRoleForm';
import BulkAssignmentForm from '../../../components/roleManagement/BulkAssignmentForm';
import { getRoleManagementData } from '../../../lib/actions/roleManagement';

interface RoleManagementPageProps {
  searchParams: Promise<{
    tab?: string;
    search?: string;
    page?: string;
    expanded?: string;
    edit?: string;
    expand?: string;
    user?: string;
    error?: string;
    success?: string;
    role?: string;
  }>;
}

async function checkSuperAdminAccess() {
  const user = await getCurrentUser();
  
  if (!user || !user.roles.includes('SuperAdmin')) {
    redirect('/auth/login?error=unauthorized&message=SuperAdmin access required');
  }
  
  return user;
}



function LoadingSpinner() {
  return (
    <div className="d-flex align-items-center justify-content-center" style={{ minHeight: '400px' }}>
      <div 
        className="spinner-border text-primary" 
        role="status" 
        style={{ width: '3rem', height: '3rem' }}
      >
        <span className="visually-hidden">Loading...</span>
      </div>
    </div>
  );
}

function ErrorDisplay({ error }: { error: string }) {
  return (
    <div className="min-vh-100 bg-light py-4">
      <div className="container-fluid" style={{ maxWidth: '1280px' }}>
        <div className="alert alert-danger border-danger" role="alert">
          <div className="d-flex">
            <div className="flex-shrink-0 me-3">
              <svg 
                className="text-danger" 
                width="20" 
                height="20" 
                viewBox="0 0 20 20" 
                fill="currentColor"
              >
                <path 
                  fillRule="evenodd" 
                  d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" 
                  clipRule="evenodd" 
                />
              </svg>
            </div>
            <div>
              <h6 className="alert-heading text-danger mb-2">Error Loading Data</h6>
              <p className="mb-0 text-danger">{error}</p>
              <div className="mt-3">
                <a href="/admin/role-management" className="btn btn-outline-danger btn-sm">
                  Try Again
                </a>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

function SuccessMessage({ message }: { message: string }) {
  return (
    <div className="alert alert-success alert-dismissible" role="alert">
      <div className="d-flex">
        <div className="flex-shrink-0 me-3">
          <svg className="text-success" width="20" height="20" viewBox="0 0 20 20" fill="currentColor">
            <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.293a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z" clipRule="evenodd" />
          </svg>
        </div>
        <div>
          <p className="mb-0 text-success">{message}</p>
        </div>
      </div>
      <form action="/admin/role-management" method="get" className="d-inline">
        <button type="submit" className="btn-close" aria-label="Close"></button>
      </form>
    </div>
  );
}

function ErrorMessage({ message }: { message: string }) {
  return (
    <div className="alert alert-danger alert-dismissible" role="alert">
      <div className="d-flex">
        <div className="flex-shrink-0 me-3">
          <svg className="text-danger" width="20" height="20" viewBox="0 0 20 20" fill="currentColor">
            <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
          </svg>
        </div>
        <div>
          <p className="mb-0 text-danger">{message}</p>
        </div>
      </div>
      <form action="/admin/role-management" method="get" className="d-inline">
        <button type="submit" className="btn-close" aria-label="Close"></button>
      </form>
    </div>
  );
}

export default async function RoleManagementPage({ searchParams }: RoleManagementPageProps) {
  const currentUser = await checkSuperAdminAccess();
  
  const params = await searchParams;
  const activeTab = params.tab || 'overview';
  const searchQuery = params.search || '';
  const currentPage = Math.max(1, parseInt(params.page || '1'));
  const expandedForm = params.expanded;
  const editingRoleId = params.edit;
  const expandedRoleId = params.expand;
  const selectedUserId = params.user ? parseInt(params.user) : undefined;
  const errorMessage = params.error;
  const successMessage = params.success;
  const selectedRole = params.role;

  const { roles, users, stats, error } = await getRoleManagementData();

  if (error) {
    return <ErrorDisplay error={error} />;
  }

  const filteredUsers = users.filter(user => {
    const matchesSearch = searchQuery === '' || [
      user.userName,
      user.email,
      user.firstName || '',
      user.lastName || '',
      ...user.roles
    ].some(field => field.toLowerCase().includes(searchQuery.toLowerCase()));

    const matchesRole = !selectedRole || user.roles.includes(selectedRole);

    return matchesSearch && matchesRole;
  });

  const filteredRoles = roles.filter(role =>
    searchQuery === '' || (role.name ?? '').toLowerCase().includes(searchQuery.toLowerCase())
  );

  const usersPerPage = 10;
  const totalPages = Math.ceil(filteredUsers.length / usersPerPage);
  const safePage = Math.min(currentPage, Math.max(1, totalPages));
  const startIndex = (safePage - 1) * usersPerPage;
  const paginatedUsers = filteredUsers.slice(startIndex, startIndex + usersPerPage);

  const roleDistribution = {
    superAdmins: users.filter(u => u.roles.includes('SuperAdmin')).length,
    admins: users.filter(u => u.roles.includes('Admin')).length,
    regularUsers: users.filter(u => u.roles.includes('User') && !u.roles.includes('Admin') && !u.roles.includes('SuperAdmin')).length,
    unassigned: users.filter(u => u.roles.length === 0).length,
    totalWithRoles: users.filter(u => u.roles.length > 0).length,
  };

  const additionalStats = {
    customRoles: roles.filter(r => !['SuperAdmin', 'Admin', 'User'].includes(r.name)).length,
    systemRoles: roles.filter(r => ['SuperAdmin', 'Admin', 'User'].includes(r.name)).length,
    usersPerRole: roles.map(role => ({
      roleName: role.name,
      userCount: users.filter(u => u.roles.includes(role.name)).length
    }))
  };

  return (
    <div className="min-vh-100 bg-light">
      <RoleManagementHeader currentUser={currentUser} />

      <div className="container-fluid py-4" style={{ maxWidth: '1280px' }}>
        {successMessage && <SuccessMessage message={decodeURIComponent(successMessage)} />}
        {errorMessage && <ErrorMessage message={decodeURIComponent(errorMessage)} />}

        {stats && (
          <Suspense fallback={<LoadingSpinner />}>
            <RoleManagementStats 
              stats={{
                ...stats,
                additionalStats
              }} 
            />
          </Suspense>
        )}

        <RoleManagementTabs 
          activeTab={activeTab} 
          searchParams={params}
          rolesCount={filteredRoles.length}
          usersCount={filteredUsers.length}
        />

        <div className="card shadow-sm">
          {activeTab === 'overview' && (
            <div className="card-body p-4">
              <div className="row g-4">
                <div className="col-12 col-lg-6">
                  <div className="border rounded p-4 h-100">
                    <h3 className="h5 text-dark mb-4">
                      <svg className="me-2" width="20" height="20" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M9.813 15.904L9 18.75l-.813-2.846a4.5 4.5 0 00-3.09-3.09L2.25 12l2.846-.813a4.5 4.5 0 003.09-3.09L9 5.25l.813 2.847a4.5 4.5 0 003.09 3.09L15.75 12l-2.847.813a4.5 4.5 0 00-3.09 3.09zM18.259 8.715L18 9.75l-.259-1.035a3.375 3.375 0 00-2.455-2.456L14.25 6l1.036-.259a3.375 3.375 0 002.455-2.456L18 2.25l.259 1.035a3.375 3.375 0 002.456 2.456L21.75 6l-1.035.259a3.375 3.375 0 00-2.456 2.456zM16.894 20.567L16.5 21.75l-.394-1.183a2.25 2.25 0 00-1.423-1.423L13.5 18.75l1.183-.394a2.25 2.25 0 001.423-1.423l.394-1.183.394 1.183a2.25 2.25 0 001.423 1.423l1.183.394-1.183.394a2.25 2.25 0 00-1.423 1.423z" />
                      </svg>
                      Quick Actions
                    </h3>
                    <div className="d-flex flex-column gap-3">
                      <CreateRoleForm 
                        isExpanded={expandedForm === 'create-role'}
                      />
                      <BulkAssignmentForm 
                        roles={roles} 
                        users={users}
                        isExpanded={expandedForm === 'bulk-assign'}
                      />
                    </div>
                  </div>
                </div>

                <div className="col-12 col-lg-6">
                  <div className="border rounded p-4 h-100">
                    <h3 className="h5 text-dark mb-4">
                      <svg className="me-2" width="20" height="20" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
                        <path strokeLinecap="round" strokeLinejoin="round" d="M3.75 3v11.25A2.25 2.25 0 006 16.5h2.25M3.75 3h-1.5m1.5 0h16.5m0 0h1.5m-1.5 0v11.25A2.25 2.25 0 0018 16.5h-2.25m-7.5 0h7.5m-7.5 0l-1 3m8.5-3l1 3m0 0l-1-3m1 3l-1-3m-16.5 0h2.25" />
                      </svg>
                      System Overview
                    </h3>
                    <div className="row g-3">
                      <div className="col-6">
                        <div className="d-flex justify-content-between align-items-center">
                          <span className="text-muted small">Total Roles:</span>
                          <span className="fw-medium text-primary">{roles.length}</span>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="d-flex justify-content-between align-items-center">
                          <span className="text-muted small">System Roles:</span>
                          <span className="fw-medium text-secondary">{additionalStats.systemRoles}</span>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="d-flex justify-content-between align-items-center">
                          <span className="text-muted small">Custom Roles:</span>
                          <span className="fw-medium text-info">{additionalStats.customRoles}</span>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="d-flex justify-content-between align-items-center">
                          <span className="text-muted small">Total Users:</span>
                          <span className="fw-medium text-primary">{users.length}</span>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="d-flex justify-content-between align-items-center">
                          <span className="text-muted small">Super Admins:</span>
                          <span className="fw-medium text-danger">{roleDistribution.superAdmins}</span>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="d-flex justify-content-between align-items-center">
                          <span className="text-muted small">Admins:</span>
                          <span className="fw-medium text-warning">{roleDistribution.admins}</span>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="d-flex justify-content-between align-items-center">
                          <span className="text-muted small">Regular Users:</span>
                          <span className="fw-medium text-success">{roleDistribution.regularUsers}</span>
                        </div>
                      </div>
                      <div className="col-6">
                        <div className="d-flex justify-content-between align-items-center">
                          <span className="text-muted small">Unassigned:</span>
                          <span className="fw-medium text-secondary">{roleDistribution.unassigned}</span>
                        </div>
                      </div>
                    </div>

                    <div className="mt-4">
                      <h6 className="text-muted small mb-3">Role Usage</h6>
                      {additionalStats.usersPerRole
                        .filter(r => r.userCount > 0)
                        .sort((a, b) => b.userCount - a.userCount)
                        .slice(0, 5)
                        .map(role => (
                          <div key={role.roleName} className="mb-2">
                            <div className="d-flex justify-content-between align-items-center small">
                              <span className="text-muted">{role.roleName}</span>
                              <span className="fw-medium">{role.userCount}</span>
                            </div>
                            <div className="progress" style={{ height: '4px' }}>
                              <div 
                                className="progress-bar bg-primary" 
                                style={{ width: `${(role.userCount / users.length) * 100}%` }}
                              />
                            </div>
                          </div>
                        ))
                      }
                    </div>
                  </div>
                </div>
              </div>
            </div>
          )}

          {activeTab === 'roles' && (
            <Suspense fallback={<LoadingSpinner />}>
              <RolesTable 
                roles={filteredRoles} 
                users={users}
                searchQuery={searchQuery}
                editingRoleId={editingRoleId}
                expandedRoleId={expandedRoleId}
                searchParams={params}
              />
            </Suspense>
          )}

          {activeTab === 'users' && (
            <Suspense fallback={<LoadingSpinner />}>
              <UsersTable 
                users={paginatedUsers} 
                roles={roles}
                searchQuery={searchQuery}
                currentPage={safePage}
                totalPages={totalPages}
                totalUsers={filteredUsers.length}
                selectedUserId={selectedUserId}
                selectedRole={selectedRole}
                searchParams={params}
              />
            </Suspense>
          )}
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Role Management - BlueBerry24 Admin',
  description: 'Manage user roles and permissions in the BlueBerry24 system',
};