import { Role, UserWithRoles } from '../../types/roleManagement';
import { bulkAssignRolesAction } from '../../lib/actions/roleManagement';

interface BulkAssignmentFormProps {
  roles: Role[];
  users: UserWithRoles[];
  isExpanded?: boolean;
}

export default function BulkAssignmentForm({ 
  roles, 
  users,
  isExpanded = false 
}: BulkAssignmentFormProps) {

  const availableRoles = roles.filter(role => role.name !== 'SuperAdmin');
  const availableUsers = users.filter(user => !user.roles.includes('SuperAdmin'));

  return (
    <div className="mb-3">
      {!isExpanded ? (
        <div>
          <form action="/admin/role-management" method="get">
            <input type="hidden" name="expanded" value="bulk-assign" />
            <button
              type="submit"
              className="btn btn-outline-primary w-100 d-flex align-items-center justify-content-center"
            >
              <svg className="me-2" width="16" height="16" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" d="M15 19.128a9.38 9.38 0 002.625.372 9.337 9.337 0 004.121-.952 4.125 4.125 0 00-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 018.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0111.964-3.07M12 6.375a3.375 3.375 0 11-6.75 0 3.375 3.375 0 016.75 0zm8.25 2.25a2.625 2.625 0 11-5.25 0 2.625 2.625 0 015.25 0z" />
              </svg>
              Bulk Assign Roles
            </button>
          </form>
        </div>
      ) : null}
      
      {isExpanded && (
        <div className="card border-primary">
          <div className="card-header bg-primary bg-opacity-10 d-flex align-items-center">
            <svg className="me-2 text-primary" width="20" height="20" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" d="M15 19.128a9.38 9.38 0 002.625.372 9.337 9.337 0 004.121-.952 4.125 4.125 0 00-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 018.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0111.964-3.07M12 6.375a3.375 3.375 0 11-6.75 0 3.375 3.375 0 016.75 0zm8.25 2.25a2.625 2.625 0 11-5.25 0 2.625 2.625 0 015.25 0z" />
            </svg>
            <h5 className="card-title mb-0 text-primary">Bulk Role Assignment</h5>
          </div>
          <div className="card-body">
            {availableRoles.length === 0 || availableUsers.length === 0 ? (
              <div className="alert alert-warning d-flex" role="alert">
                <div className="flex-shrink-0 me-3">
                  <svg width="20" height="20" viewBox="0 0 20 20" fill="currentColor">
                    <path fillRule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clipRule="evenodd" />
                  </svg>
                </div>
                <div>
                  {availableRoles.length === 0 ? (
                    <>No roles available for bulk assignment. SuperAdmin role cannot be bulk assigned.</>
                  ) : (
                    <>No users available for role assignment. SuperAdmin users cannot be modified via bulk assignment.</>
                  )}
                </div>
              </div>
            ) : (
              <form action={bulkAssignRolesAction}>
                <input type="hidden" name="redirectUrl" value="/admin/role-management?success=Bulk role assignment completed successfully" />
                
                <div className="row g-3">
                  <div className="col-md-6">
                    <label htmlFor="roleName" className="form-label fw-medium">
                      Select Role
                      <span className="text-danger">*</span>
                    </label>
                    <select
                      name="roleName"
                      id="roleName"
                      required
                      className="form-select"
                    >
                      <option value="">Choose a role...</option>
                      {availableRoles.map((role) => (
                        <option key={role.id} value={role.id}>
                          {role.name}
                          {['Admin', 'User'].includes(role.name) && ' (System Role)'}
                        </option>
                      ))}
                    </select>
                  </div>

                  <div className="col-md-6">
                    <label className="form-label fw-medium">
                      Select Users
                      <span className="text-danger">*</span>
                    </label>
                    <div className="border rounded p-3" style={{ maxHeight: '200px', overflowY: 'auto' }}>
                      {availableUsers.length === 0 ? (
                        <p className="text-muted small mb-0">No users available</p>
                      ) : (
                        <>
                          {availableUsers.map((user) => (
                            <div key={user.id} className="form-check mb-1">
                              <input
                                type="checkbox"
                                name="userIds"
                                value={user.id}
                                className="form-check-input"
                                id={`user-${user.id}`}
                              />
                              <label className="form-check-label small" htmlFor={`user-${user.id}`}>
                                <div className="d-flex justify-content-between align-items-center">
                                  <div>
                                    <span className="fw-medium">
                                      {user.firstName && user.lastName 
                                        ? `${user.firstName} ${user.lastName}` 
                                        : user.userName
                                      }
                                    </span>
                                    <br />
                                    <span className="text-muted">{user.email}</span>
                                  </div>
                                  <div>
                                    {user.roles.length > 0 && (
                                      <div className="d-flex flex-wrap gap-1">
                                        {user.roles.slice(0, 2).map(role => (
                                          <span key={role} className="badge bg-secondary small">
                                            {role}
                                          </span>
                                        ))}
                                        {user.roles.length > 2 && (
                                          <span className="badge bg-light text-dark small">
                                            +{user.roles.length - 2}
                                          </span>
                                        )}
                                      </div>
                                    )}
                                  </div>
                                </div>
                              </label>
                            </div>
                          ))}
                        </>
                      )}
                    </div>
                    <div className="form-text">
                      Select one or more users to assign the chosen role to.
                    </div>
                  </div>
                </div>

                <div className="mt-4">
                  <div className="alert alert-info d-flex small" role="alert">
                    <div className="flex-shrink-0 me-3">
                      <svg width="16" height="16" viewBox="0 0 20 20" fill="currentColor">
                        <path fillRule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clipRule="evenodd" />
                      </svg>
                    </div>
                    <div>
                      <strong>Note:</strong> This will assign the selected role to all chosen users. 
                      Users who already have this role will be skipped. SuperAdmin users and the SuperAdmin role are excluded for security.
                    </div>
                  </div>
                </div>

                <div className="d-flex flex-column flex-sm-row justify-content-end gap-2">
                  <form action="/admin/role-management" method="get" className="order-2 order-sm-1">
                    <button type="submit" className="btn btn-outline-secondary w-100">
                      <svg className="me-2" width="14" height="14" fill="currentColor" viewBox="0 0 16 16">
                        <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                        <path d="M4.646 4.646a.5.5 0 0 1 .708 0L8 7.293l2.646-2.647a.5.5 0 0 1 .708.708L8.707 8l2.647 2.646a.5.5 0 0 1-.708.708L8 8.707l-2.646 2.647a.5.5 0 0 1-.708-.708L7.293 8 4.646 5.354a.5.5 0 0 1 0-.708z"/>
                      </svg>
                      Cancel
                    </button>
                  </form>
                  
                  <button
                    type="submit"
                    className="btn btn-primary order-1 order-sm-2"
                  >
                    <svg className="me-2" width="14" height="14" fill="currentColor" viewBox="0 0 16 16">
                      <path d="M15 19.128a9.38 9.38 0 002.625.372 9.337 9.337 0 004.121-.952 4.125 4.125 0 00-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 018.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0111.964-3.07M12 6.375a3.375 3.375 0 11-6.75 0 3.375 3.375 0 016.75 0zm8.25 2.25a2.625 2.625 0 11-5.25 0 2.625 2.625 0 015.25 0z"/>
                    </svg>
                    Assign Roles
                  </button>
                </div>
              </form>
            )}
          </div>
        </div>
      )}
    </div>
  );
}