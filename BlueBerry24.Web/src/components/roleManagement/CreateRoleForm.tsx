import { createRoleAction } from '@/lib/actions/roleManagement';

interface CreateRoleFormProps {
  initialExpanded?: boolean;
  isExpanded?: boolean;
}

export default function CreateRoleForm({ 
  initialExpanded = false,
  isExpanded = false 
}: CreateRoleFormProps) {
  const showForm = initialExpanded || isExpanded;

  return (
    <div className="mb-3">
      {!showForm ? (
        <div>
          <form action="/admin/role-management" method="get">
            <input type="hidden" name="expanded" value="create-role" />
            <button
              type="submit"
              className="btn btn-primary w-100 d-flex align-items-center justify-content-center"
            >
              <svg className="me-2" width="16" height="16" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
              </svg>
              Create New Role
            </button>
          </form>
        </div>
      ) : null}
      
      {showForm && (
        <div className="card border-success">
          <div className="card-header bg-success bg-opacity-10 d-flex align-items-center">
            <svg className="me-2 text-success" width="20" height="20" fill="none" viewBox="0 0 24 24" strokeWidth="1.5" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" d="M9 12.75L11.25 15 15 9.75m-3-7.036A11.959 11.959 0 013.598 6 11.99 11.99 0 003 9.749c0 5.592 3.824 10.29 9 11.623 5.176-1.332 9-6.03 9-11.623 0-1.31-.21-2.571-.598-3.751h-.152c-3.196 0-6.1-1.248-8.25-3.285z" />
            </svg>
            <h5 className="card-title mb-0 text-success">Create New Role</h5>
          </div>
          <div className="card-body">
            <form action={createRoleAction}>
              <input type="hidden" name="redirectUrl" value="/admin/role-management?success=Role created successfully" />
              
              <div className="mb-3">
                <label htmlFor="roleName" className="form-label fw-medium">
                  Role Name
                  <span className="text-danger">*</span>
                </label>
                <input
                  type="text"
                  name="roleName"
                  id="roleName"
                  required
                  maxLength={50}
                  pattern="^[a-zA-Z][a-zA-Z0-9\s]*$"
                  title="Role name must start with a letter and contain only letters, numbers, and spaces"
                  className="form-control"
                  placeholder="Enter role name (e.g., Manager, Moderator)"
                  autoComplete="off"
                />
                <div className="form-text">
                  <svg className="me-1" width="14" height="14" fill="currentColor" viewBox="0 0 16 16">
                    <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"/>
                    <path d="m8.93 6.588-2.29.287-.082.38.45.083c.294.07.352.176.288.469l-.738 3.468c-.194.897.105 1.319.808 1.319.545 0 1.178-.252 1.465-.598l.088-.416c-.2.176-.492.246-.686.246-.275 0-.375-.193-.304-.533L8.93 6.588zM9 4.5a1 1 0 1 1-2 0 1 1 0 0 1 2 0z"/>
                  </svg>
                  Role names must start with a letter and can contain letters, numbers, and spaces (max 50 characters).
                </div>
              </div>

              <div className="alert alert-info d-flex" role="alert">
                <div className="flex-shrink-0 me-3">
                  <svg width="20" height="20" viewBox="0 0 20 20" fill="currentColor">
                    <path fillRule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clipRule="evenodd" />
                  </svg>
                </div>
                <div>
                  <strong>Note:</strong> Custom roles can be assigned to users and later modified or deleted. 
                  System roles (SuperAdmin, Admin, User) cannot be created through this form.
                </div>
              </div>

              <div className="alert alert-warning d-flex small" role="alert">
                <div className="flex-shrink-0 me-3">
                  <svg width="16" height="16" viewBox="0 0 20 20" fill="currentColor">
                    <path fillRule="evenodd" d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z" clipRule="evenodd" />
                  </svg>
                </div>
                <div>
                  Reserved names: <strong>SuperAdmin</strong>, <strong>Admin</strong>, <strong>User</strong>
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
                  className="btn btn-success order-1 order-sm-2"
                  formNoValidate={false}
                >
                  <svg className="me-2" width="14" height="14" fill="currentColor" viewBox="0 0 16 16">
                    <path d="M13.854 3.646a.5.5 0 0 1 0 .708l-7 7a.5.5 0 0 1-.708 0l-3.5-3.5a.5.5 0 1 1 .708-.708L6.5 10.293l6.646-6.647a.5.5 0 0 1 .708 0z"/>
                  </svg>
                  Create Role
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}