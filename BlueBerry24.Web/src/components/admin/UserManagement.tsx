import { UserWithRoles, Role } from '@/types/roleManagement';
import {
  promoteUserToAdmin,
  demoteAdminUser,
  updateUserRoles,
  lockUserAccount,
  unlockUserAccount,
  resetUserPassword,
  verifyUserEmail,
  updateUser,
  createAdminUser,
  deleteUser
} from '@/lib/actions/user-actions';

interface UserManagementProps {
  users: UserWithRoles[];
  roles: Role[];
  currentUser: any;
  searchQuery?: string;
  statusFilter?: string;
  showModal?: string;
  selectedUserId?: string;
  showAll?: string
}

export default function UserManagement({ 
  users, 
  roles,
  currentUser,
  searchQuery = '', 
  statusFilter = '',
  showModal,
  selectedUserId ,
  showAll
}: UserManagementProps) {
  
  const formatDate = (dateString?: string) => {
    if (!dateString) return 'Never';
    return new Date(dateString).toLocaleDateString();
  };

  const getUserStatus = (user: UserWithRoles) => {
    if (user.lockoutEnd && new Date(user.lockoutEnd) > new Date()) {
      return { status: 'Locked', className: 'bg-danger' };
    }
    if (!user.emailConfirmed) {
      return { status: 'Unverified', className: 'bg-warning' };
    }
    return { status: 'Active', className: 'bg-success' };
  };

  const getHighestRole = (userRoles: string[]) => {
    if (userRoles.some(role => role.toLowerCase().includes('superadmin'))) {
      return { role: 'Super Admin', className: 'bg-danger' };
    }
    if (userRoles.some(role => role.toLowerCase().includes('admin'))) {
      return { role: 'Admin', className: 'bg-primary' };
    }
    return { role: 'User', className: 'bg-secondary' };
  };

  const adminUsers = users.filter((user: UserWithRoles) => {
    const userRoles = user.roles || [];
    return userRoles.some((role: string) => 
      role.toLowerCase().includes('admin') || 
      role.toLowerCase().includes('superadmin')
    );
  });

  const regularUsers = users.filter((user: UserWithRoles) => {
    const userRoles = user.roles || [];
    return !userRoles.some((role: string) => 
      role.toLowerCase().includes('admin') || 
      role.toLowerCase().includes('superadmin')
    );
  });

  const adminRoles = roles.filter((role: Role) => 
    role.name.toLowerCase().includes('admin') || 
    role.name.toLowerCase().includes('superadmin')
  );

  const selectedUser = selectedUserId ? 
    users.find(u => u.id.toString() === selectedUserId) : null;

  const filteredAdminUsers = adminUsers.filter(user => {
    if (searchQuery === '') return true;
    return user.firstName?.toLowerCase().includes(searchQuery.toLowerCase()) ||
           user.lastName?.toLowerCase().includes(searchQuery.toLowerCase()) ||
           user.email.toLowerCase().includes(searchQuery.toLowerCase()) ||
           user.userName.toLowerCase().includes(searchQuery.toLowerCase());
  });

  const filteredRegularUsers = regularUsers.filter(user => {
    if (searchQuery === '') return true;
    return user.firstName?.toLowerCase().includes(searchQuery.toLowerCase()) ||
           user.lastName?.toLowerCase().includes(searchQuery.toLowerCase()) ||
           user.email.toLowerCase().includes(searchQuery.toLowerCase()) ||
           user.userName.toLowerCase().includes(searchQuery.toLowerCase());
  });

  return (
    <>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h1 className="h3 mb-1">
            <i className="bi bi-person-gear me-2"></i>
            Admin Users Management
          </h1>
          <p className="text-muted mb-0">
            Manage super admin and admin users with elevated privileges
          </p>
        </div>
        <div className="btn-group" role="group">
          <a href="/admin/users/export" className="btn btn-outline-primary">
            <i className="bi bi-download me-1"></i>
            Export
          </a>
          <a href="/admin/users?modal=promote" className="btn btn-outline-primary">
            <i className="bi bi-person-plus me-1"></i>
            Promote User
          </a>
          <a href="/admin/users?modal=create" className="btn btn-primary">
            <i className="bi bi-shield-plus me-1"></i>
            Create Admin
          </a>
        </div>
      </div>

      <div className="row mb-4">
        <div className="col-xl-3 col-md-6 mb-4">
          <div className="card border-left-danger shadow h-100 py-2">
            <div className="card-body">
              <div className="row no-gutters align-items-center">
                <div className="col mr-2">
                  <div className="text-xs font-weight-bold text-danger text-uppercase mb-1">
                    Super Admins
                  </div>
                  <div className="h5 mb-0 font-weight-bold text-gray-800">
                    {adminUsers.filter(u => u.roles.some(r => r.toLowerCase().includes('superadmin'))).length}
                  </div>
                </div>
                <div className="col-auto">
                  <i className="bi bi-shield-fill-exclamation fa-2x text-gray-300"></i>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="col-xl-3 col-md-6 mb-4">
          <div className="card border-left-primary shadow h-100 py-2">
            <div className="card-body">
              <div className="row no-gutters align-items-center">
                <div className="col mr-2">
                  <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                    Admins
                  </div>
                  <div className="h5 mb-0 font-weight-bold text-gray-800">
                    {adminUsers.filter(u => u.roles.some(r => r.toLowerCase().includes('admin') && !r.toLowerCase().includes('superadmin'))).length}
                  </div>
                </div>
                <div className="col-auto">
                  <i className="bi bi-shield-fill-check fa-2x text-gray-300"></i>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="col-xl-3 col-md-6 mb-4">
          <div className="card border-left-success shadow h-100 py-2">
            <div className="card-body">
              <div className="row no-gutters align-items-center">
                <div className="col mr-2">
                  <div className="text-xs font-weight-bold text-success text-uppercase mb-1">
                    Active Admin Users
                  </div>
                  <div className="h5 mb-0 font-weight-bold text-gray-800">
                    {adminUsers.filter(u => getUserStatus(u).status === 'Active').length}
                  </div>
                </div>
                <div className="col-auto">
                  <i className="bi bi-check-circle fa-2x text-gray-300"></i>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="col-xl-3 col-md-6 mb-4">
          <div className="card border-left-info shadow h-100 py-2">
            <div className="card-body">
              <div className="row no-gutters align-items-center">
                <div className="col mr-2">
                  <div className="text-xs font-weight-bold text-info text-uppercase mb-1">
                    Regular Users
                  </div>
                  <div className="h5 mb-0 font-weight-bold text-gray-800">
                    {regularUsers.length}
                  </div>
                </div>
                <div className="col-auto">
                  <i className="bi bi-people fa-2x text-gray-300"></i>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="card shadow mb-4">
        <div className="card-header py-3">
          <form method="GET" action="/admin/users">
            <div className="row align-items-center">
              <div className="col-md-10">
                <div className="input-group">
                  <span className="input-group-text">
                    <i className="bi bi-search"></i>
                  </span>
                  <input 
                    type="text" 
                    className="form-control" 
                    name="search"
                    placeholder="Search users by name, email, or username..."
                    defaultValue={searchQuery}
                  />
                </div>
              </div>
              <div className="col-md-2">
                <button type="submit" className="btn btn-primary w-100">
                  <i className="bi bi-search"></i>
                  Search
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>

      <div className="card shadow mb-4">
        <div className="card-header py-3 d-flex justify-content-between align-items-center">
          <h6 className="m-0 font-weight-bold text-primary">Current Admin Users</h6>
          <span className="badge bg-primary">{filteredAdminUsers.length} Users</span>
        </div>
        <div className="card-body">
          {filteredAdminUsers.length > 0 ? (
            <div className="table-responsive">
              <table className="table table-bordered table-hover">
                <thead className="table-light">
                  <tr>
                    <th>ID</th>
                    <th>User</th>
                    <th>Email</th>
                    <th>Role Level</th>
                    <th>Status</th>
                    <th>All Roles</th>
                    <th>Last Activity</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredAdminUsers.map((adminUser) => {
                    const status = getUserStatus(adminUser);
                    const highestRole = getHighestRole(adminUser.roles);
                    return (
                      <tr key={adminUser.id}>
                        <td className="text-center font-weight-bold">
                          #{adminUser.id}
                        </td>
                        <td>
                          <div className="d-flex align-items-center">
                            <div className="me-3">
                              <div className="rounded-circle bg-gradient-primary text-white d-flex align-items-center justify-content-center" 
                                   style={{ width: '40px', height: '40px' }}>
                                <i className="bi bi-shield-fill"></i>
                              </div>
                            </div>
                            <div>
                              <div className="font-weight-bold">
                                {adminUser.firstName && adminUser.lastName 
                                  ? `${adminUser.firstName} ${adminUser.lastName}`
                                  : adminUser.userName
                                }
                              </div>
                              <small className="text-muted">@{adminUser.userName}</small>
                            </div>
                          </div>
                        </td>
                        <td>
                          <a href={`mailto:${adminUser.email}`} className="text-decoration-none">
                            {adminUser.email}
                          </a>
                        </td>
                        <td>
                          <span className={`badge ${highestRole.className} px-2 py-1`}>
                            <i className="bi bi-shield-fill me-1"></i>
                            {highestRole.role}
                          </span>
                        </td>
                        <td>
                          <span className={`badge ${status.className} px-2 py-1`}>
                            {status.status}
                          </span>
                        </td>
                        <td>
                          <div className="d-flex flex-wrap gap-1">
                            {adminUser.roles.map((role, index) => (
                              <span key={index} className="badge bg-secondary text-white">
                                {role}
                              </span>
                            ))}
                          </div>
                        </td>
                        <td>
                          <small className="text-muted">
                            {formatDate(adminUser.lockoutEnd)}
                          </small>
                        </td>
                        <td>
                          <div className="btn-group" role="group">
                            <a 
                              href={`/admin/users?modal=details&id=${adminUser.id}`}
                              className="btn btn-sm btn-outline-primary" 
                              title="View Details"
                            >
                              <i className="bi bi-eye"></i>
                            </a>
                            <a 
                              href={`/admin/users?modal=editRoles&id=${adminUser.id}`}
                              className="btn btn-sm btn-outline-secondary" 
                              title="Edit Roles"
                            >
                              <i className="bi bi-shield-check"></i>
                            </a>
                            <a 
                              href={`/admin/users?modal=resetPassword&id=${adminUser.id}`}
                              className="btn btn-sm btn-outline-warning" 
                              title="Reset Password"
                            >
                              <i className="bi bi-key"></i>
                            </a>
                            {adminUser.id !== currentUser.id && (
                              <a 
                                href={`/admin/users?modal=demote&id=${adminUser.id}`}
                                className="btn btn-sm btn-outline-danger" 
                                title="Remove Admin"
                              >
                                <i className="bi bi-shield-x"></i>
                              </a>
                            )}
                          </div>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          ) : (
            <div className="text-center py-4">
              <i className="bi bi-shield-exclamation display-4 text-muted mb-3"></i>
              <h4 className="text-muted">No admin users found</h4>
              <p className="text-muted">
                {searchQuery ? 'No admin users match your search criteria.' : 'There are no users with admin privileges.'}
              </p>
            </div>
          )}
        </div>
      </div>

      <div className="card shadow">
        <div className="card-header py-3 d-flex justify-content-between align-items-center">
          <h6 className="m-0 font-weight-bold text-success">Users Available for Promotion</h6>
          <span className="badge bg-success">{filteredRegularUsers.length} Users</span>
        </div>
        <div className="card-body">
          {filteredRegularUsers.length > 0 ? (
            <div className="table-responsive">
              <table className="table table-bordered table-hover">
                <thead className="table-light">
                  <tr>
                    <th>ID</th>
                    <th>User</th>
                    <th>Email</th>
                    <th>Current Status</th>
                    <th>Email Verified</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredRegularUsers.slice(0, showAll==='true' ? regularUsers.length : 10).map((regularUser) => {
                    const status = getUserStatus(regularUser);
                    return (
                      <tr key={regularUser.id}>
                        <td className="text-center font-weight-bold">
                          #{regularUser.id}
                        </td>
                        <td>
                          <div className="d-flex align-items-center">
                            <div className="me-3">
                              <div className="rounded-circle bg-secondary text-white d-flex align-items-center justify-content-center" 
                                   style={{ width: '35px', height: '35px' }}>
                                <i className="bi bi-person"></i>
                              </div>
                            </div>
                            <div>
                              <div className="font-weight-bold">
                                {regularUser.firstName && regularUser.lastName 
                                  ? `${regularUser.firstName} ${regularUser.lastName}`
                                  : regularUser.userName
                                }
                              </div>
                              <small className="text-muted">@{regularUser.userName}</small>
                            </div>
                          </div>
                        </td>
                        <td>
                          <a href={`mailto:${regularUser.email}`} className="text-decoration-none">
                            {regularUser.email}
                          </a>
                        </td>
                        <td>
                          <span className={`badge ${status.className} px-2 py-1`}>
                            {status.status}
                          </span>
                        </td>
                        <td className="text-center">
                          {regularUser.emailConfirmed ? (
                            <i className="bi bi-check-circle text-success" title="Verified"></i>
                          ) : (
                            <i className="bi bi-x-circle text-danger" title="Not Verified"></i>
                          )}
                        </td>
                        <td>
                          <div className="btn-group" role="group">
                            <form action={promoteUserToAdmin} method="POST" style={{ display: 'inline' }}>
                              <input type="hidden" name="userId" value={regularUser.id} />
                              <input type="hidden" name="roleName" value="Admin" />
                              <button 
                                type="submit" 
                                className="btn btn-sm btn-outline-primary"
                                title="Promote to Admin"
                                disabled={!regularUser.emailConfirmed}
                              >
                                <i className="bi bi-shield-plus me-1"></i>
                                Make Admin
                              </button>
                            </form>
                            <form action={promoteUserToAdmin} method="POST" style={{ display: 'inline' }}>
                              <input type="hidden" name="userId" value={regularUser.id} />
                              <input type="hidden" name="roleName" value="SuperAdmin" />
                              <button 
                                type="submit" 
                                className="btn btn-sm btn-outline-danger"
                                title="Promote to Super Admin"
                                disabled={!regularUser.emailConfirmed}
                              >
                                <i className="bi bi-shield-fill-exclamation me-1"></i>
                                Super Admin
                              </button>
                            </form>
                          </div>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
              {filteredRegularUsers.length > 10 && (
                <div className="text-center mt-3">
                  <a href="/admin/users?showAll=true" className="btn btn-outline-primary">
                    View All {filteredRegularUsers.length} Users
                  </a>
                </div>
              )}
            </div>
          ) : (
            <div className="text-center py-4">
              <i className="bi bi-people display-4 text-muted mb-3"></i>
              <h4 className="text-muted">No regular users found</h4>
              <p className="text-muted">
                {searchQuery ? 'No regular users match your search criteria.' : 'All users in the system already have admin privileges.'}
              </p>
            </div>
          )}
        </div>
      </div>

      {showModal === 'details' && selectedUser && (
        <UserDetailsOverlay user={selectedUser} />
      )}

      {showModal === 'editRoles' && selectedUser && (
        <EditUserRolesOverlay user={selectedUser} roles={roles} />
      )}

      {showModal === 'resetPassword' && selectedUser && (
        <ResetPasswordOverlay user={selectedUser} />
      )}

      {showModal === 'demote' && selectedUser && (
        <DemoteUserOverlay user={selectedUser} />
      )}

      {showModal === 'promote' && (
        <PromoteUserOverlay regularUsers={regularUsers.filter(u => !u.emailConfirmed)} adminRoles={adminRoles} />
      )}

      {showModal === 'create' && (
        <CreateAdminOverlay adminRoles={adminRoles} />
      )}
    </>
  );
}

function UserDetailsOverlay({ user }: { user: UserWithRoles }) {
  const formatDate = (dateString?: string) => {
    if (!dateString) return 'Never';
    return new Date(dateString).toLocaleDateString();
  };

  const getUserStatus = (user: UserWithRoles) => {
    if (user.lockoutEnd && new Date(user.lockoutEnd) > new Date()) {
      return { status: 'Locked', className: 'bg-danger text-white' };
    }
    if (!user.emailConfirmed) {
      return { status: 'Unverified', className: 'bg-warning text-dark' };
    }
    return { status: 'Active', className: 'bg-success text-white' };
  };

  const status = getUserStatus(user);

  return (
    <div className="modal fade show d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog modal-lg">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">
              <i className="bi bi-person-circle me-2"></i>
              User Details
            </h5>
            <a href="/admin/users" className="btn-close"></a>
          </div>
          <div className="modal-body">
            <div className="row">
              <div className="col-md-6">
                <div className="card h-100">
                  <div className="card-header">
                    <h6 className="mb-0">Basic Information</h6>
                  </div>
                  <div className="card-body">
                    <div className="mb-3">
                      <label className="form-label fw-bold">Full Name:</label>
                      <p className="mb-0">
                        {user.firstName && user.lastName 
                          ? `${user.firstName} ${user.lastName}`
                          : 'No name provided'
                        }
                      </p>
                    </div>
                    <div className="mb-3">
                      <label className="form-label fw-bold">Email:</label>
                      <p className="mb-0">{user.email}</p>
                    </div>
                    <div className="mb-3">
                      <label className="form-label fw-bold">Username:</label>
                      <p className="mb-0">{user.userName}</p>
                    </div>
                    <div className="mb-3">
                      <label className="form-label fw-bold">User ID:</label>
                      <p className="mb-0">#{user.id}</p>
                    </div>
                  </div>
                </div>
              </div>
              <div className="col-md-6">
                <div className="card h-100">
                  <div className="card-header">
                    <h6 className="mb-0">Account Status & Roles</h6>
                  </div>
                  <div className="card-body">
                    <div className="mb-3">
                      <label className="form-label fw-bold">Status:</label>
                      <br />
                      <span className={`badge ${status.className} px-2 py-1`}>
                        {status.status}
                      </span>
                    </div>
                    <div className="mb-3">
                      <label className="form-label fw-bold">Roles:</label>
                      <div className="d-flex flex-wrap gap-1">
                        {user.roles.map((role, index) => (
                          <span key={index} className="badge bg-primary">
                            {role}
                          </span>
                        ))}
                      </div>
                    </div>
                    <div className="mb-3">
                      <label className="form-label fw-bold">Email Verified:</label>
                      <p className="mb-0">
                        {user.emailConfirmed ? (
                          <span className="text-success">
                            <i className="bi bi-check-circle me-1"></i>
                            Verified
                          </span>
                        ) : (
                          <span className="text-danger">
                            <i className="bi bi-x-circle me-1"></i>
                            Not Verified
                          </span>
                        )}
                      </p>
                    </div>
                    <div className="mb-3">
                      <label className="form-label fw-bold">Failed Login Attempts:</label>
                      <p className="mb-0">
                        <span className={`badge ${user.accessFailedCount > 0 ? 'bg-warning text-dark' : 'bg-light text-dark'}`}>
                          {user.accessFailedCount}
                        </span>
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="modal-footer">
            <a href="/admin/users" className="btn btn-secondary">
              Close
            </a>
          </div>
        </div>
      </div>
    </div>
  );
}

function EditUserRolesOverlay({ user, roles }: { user: UserWithRoles; roles: Role[] }) {
  return (
    <div className="modal fade show d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">
              <i className="bi bi-shield-check me-2"></i>
              Edit User Roles
            </h5>
            <a href="/admin/users" className="btn-close"></a>
          </div>
          <form action={updateUserRoles}>
            <div className="modal-body">
              <input type="hidden" name="userId" value={user.id} />
              
              <div className="alert alert-info">
                <i className="bi bi-info-circle me-2"></i>
                Managing roles for: <strong>{user.firstName && user.lastName 
                  ? `${user.firstName} ${user.lastName}`
                  : user.userName
                } ({user.email})</strong>
              </div>

              <div className="mb-3">
                <label className="form-label fw-bold">Assign Roles:</label>
                {roles.map((role) => (
                  <div key={role.id} className="form-check">
                    <input
                      className="form-check-input"
                      type="checkbox"
                      id={`role-${role.id}`}
                      name="roles"
                      value={role.name}
                      defaultChecked={user.roles.includes(role.name)}
                    />
                    <label className="form-check-label" htmlFor={`role-${role.id}`}>
                      {role.name}
                    </label>
                  </div>
                ))}
              </div>
            </div>
            <div className="modal-footer">
              <a href="/admin/users" className="btn btn-secondary">
                Cancel
              </a>
              <button type="submit" className="btn btn-primary">
                <i className="bi bi-save me-1"></i>
                Update Roles
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}

function ResetPasswordOverlay({ user }: { user: UserWithRoles }) {
  return (
    <div className="modal fade show d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">
              <i className="bi bi-key me-2"></i>
              Reset Password
            </h5>
            <a href="/admin/users" className="btn-close"></a>
          </div>
          <form action={resetUserPassword}>
            <div className="modal-body">
              <input type="hidden" name="userId" value={user.id} />
              
              <div className="alert alert-warning">
                <i className="bi bi-exclamation-triangle me-2"></i>
                You are about to reset the password for <strong>{user.email}</strong>
              </div>

              <div className="mb-3">
                <label htmlFor="newPassword" className="form-label">New Password *</label>
                <input
                  type="password"
                  className="form-control"
                  id="newPassword"
                  name="newPassword"
                  placeholder="Enter new password"
                  minLength={6}
                  required
                />
                <div className="form-text">Password must be at least 6 characters long.</div>
              </div>
            </div>
            <div className="modal-footer">
              <a href="/admin/users" className="btn btn-secondary">
                Cancel
              </a>
              <button type="submit" className="btn btn-warning">
                <i className="bi bi-key me-1"></i>
                Reset Password
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}

function DemoteUserOverlay({ user }: { user: UserWithRoles }) {
  const adminRoles = user.roles.filter(role => 
    role.toLowerCase().includes('admin') || 
    role.toLowerCase().includes('superadmin')
  );

  return (
    <div className="modal fade show d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">
              <i className="bi bi-shield-x text-danger me-2"></i>
              Remove Admin Privileges
            </h5>
            <a href="/admin/users" className="btn-close"></a>
          </div>
          <div className="modal-body">
            <div className="alert alert-danger">
              <i className="bi bi-exclamation-triangle-fill me-2"></i>
              <strong>Warning!</strong> This will remove admin privileges.
            </div>
            
            <p>Are you sure you want to remove admin privileges from:</p>
            <div className="card">
              <div className="card-body">
                <h6 className="card-title">{user.firstName && user.lastName 
                  ? `${user.firstName} ${user.lastName}`
                  : user.userName
                }</h6>
                <p className="card-text">
                  <strong>Email:</strong> {user.email}<br />
                  <strong>Current Admin Roles:</strong> {adminRoles.join(', ')}<br />
                  <strong>ID:</strong> #{user.id}
                </p>
              </div>
            </div>
            
            <div className="mt-3">
              <p className="text-muted">Choose which admin role to remove:</p>
              {adminRoles.map((role) => (
                <form key={role} action={demoteAdminUser} method="POST" className="d-inline me-2">
                  <input type="hidden" name="userId" value={user.id} />
                  <input type="hidden" name="roleName" value={role} />
                  <button type="submit" className="btn btn-outline-danger btn-sm">
                    Remove {role}
                  </button>
                </form>
              ))}
            </div>
          </div>
          <div className="modal-footer">
            <a href="/admin/users" className="btn btn-secondary">
              Cancel
            </a>
          </div>
        </div>
      </div>
    </div>
  );
}

function PromoteUserOverlay({ regularUsers, adminRoles }: { regularUsers: UserWithRoles[]; adminRoles: Role[] }) {
  return (
    <div className="modal fade show d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog modal-lg">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">
              <i className="bi bi-person-plus me-2"></i>
              Promote User to Admin
            </h5>
            <a href="/admin/users" className="btn-close"></a>
          </div>
          <form action={promoteUserToAdmin}>
            <div className="modal-body">
              <div className="mb-3">
                <label htmlFor="userId" className="form-label">Select User</label>
                <select className="form-select" name="userId" required>
                  <option value="">Choose a user to promote...</option>
                  {regularUsers.map((user) => (
                    <option key={user.id} value={user.id}>
                      {user.firstName && user.lastName 
                        ? `${user.firstName} ${user.lastName} (${user.email})`
                        : `${user.userName} (${user.email})`
                      }
                    </option>
                  ))}
                </select>
              </div>
              <div className="mb-3">
                <label htmlFor="roleName" className="form-label">Admin Role</label>
                <select className="form-select" name="roleName" required>
                  <option value="">Select admin role...</option>
                  {adminRoles.map((role) => (
                    <option key={role.id} value={role.name}>
                      {role.name}
                    </option>
                  ))}
                </select>
              </div>
              <div className="alert alert-info">
                <i className="bi bi-info-circle me-2"></i>
                <strong>Note:</strong> Promoting a user to admin will grant them access to the admin dashboard and management features.
              </div>
            </div>
            <div className="modal-footer">
              <a href="/admin/users" className="btn btn-secondary">
                Cancel
              </a>
              <button type="submit" className="btn btn-primary">
                <i className="bi bi-shield-plus me-1"></i>
                Promote User
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}

function CreateAdminOverlay({ adminRoles }: { adminRoles: Role[] }) {
  return (
    <div className="modal fade show d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog modal-lg">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">
              <i className="bi bi-shield-plus me-2"></i>
              Create New Admin User
            </h5>
            <a href="/admin/users" className="btn-close"></a>
          </div>
          <form action={createAdminUser}>
            <div className="modal-body">
              <div className="row">
                <div className="col-md-6 mb-3">
                  <label htmlFor="firstName" className="form-label">First Name</label>
                  <input type="text" className="form-control" name="firstName" />
                </div>
                <div className="col-md-6 mb-3">
                  <label htmlFor="lastName" className="form-label">Last Name</label>
                  <input type="text" className="form-control" name="lastName" />
                </div>
              </div>
              <div className="mb-3">
                <label htmlFor="email" className="form-label">Email Address *</label>
                <input type="email" className="form-control" name="email" required />
              </div>
              <div className="mb-3">
                <label htmlFor="userName" className="form-label">Username *</label>
                <input type="text" className="form-control" name="userName" required />
              </div>
              <div className="mb-3">
                <label htmlFor="password" className="form-label">Password *</label>
                <input type="password" className="form-control" name="password" minLength={6} required />
                <div className="form-text">Password must be at least 6 characters long.</div>
              </div>
              <div className="mb-3">
                <label className="form-label">Admin Roles</label>
                {adminRoles.map((role) => (
                  <div key={role.id} className="form-check">
                    <input
                      className="form-check-input"
                      type="checkbox"
                      name="roles"
                      value={role.name}
                      id={`admin-role-${role.id}`}
                    />
                    <label className="form-check-label" htmlFor={`admin-role-${role.id}`}>
                      {role.name}
                    </label>
                  </div>
                ))}
              </div>
              <div className="mb-3">
                <div className="form-check">
                  <input
                    className="form-check-input"
                    type="checkbox"
                    name="emailConfirmed"
                    id="emailConfirmed"
                  />
                  <label className="form-check-label" htmlFor="emailConfirmed">
                    Mark email as verified
                  </label>
                </div>
              </div>
              <div className="alert alert-warning">
                <i className="bi bi-exclamation-triangle me-2"></i>
                <strong>Security Note:</strong> The user will be created with the provided password. They should change it on first login.
              </div>
            </div>
            <div className="modal-footer">
              <a href="/admin/users" className="btn btn-secondary">
                Cancel
              </a>
              <button type="submit" className="btn btn-primary">
                <i className="bi bi-person-plus me-1"></i>
                Create Admin User
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
} 