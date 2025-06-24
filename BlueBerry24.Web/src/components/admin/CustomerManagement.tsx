import { UserWithRoles } from '@/types/roleManagement';
import {
  lockCustomerAccount,
  unlockCustomerAccount,
  resetCustomerPassword,
  verifyCustomerEmail,
  updateCustomer,
  createCustomer,
  deleteCustomer
} from '@/lib/actions/customer-actions';

interface CustomerManagementProps {
  customers: UserWithRoles[];
  searchQuery?: string;
  statusFilter?: string;
  showModal?: string;
  selectedCustomerId?: string;
}

export default function CustomerManagement({ 
  customers, 
  searchQuery = '', 
  statusFilter = '',
  showModal,
  selectedCustomerId 
}: CustomerManagementProps) {
  
  const formatDate = (dateString?: string) => {
    if (!dateString) return 'Never';
    return new Date(dateString).toLocaleDateString();
  };

  const getUserStatus = (customer: UserWithRoles) => {
    if (customer.lockoutEnd && new Date(customer.lockoutEnd) > new Date()) {
      return { status: 'Locked', className: 'bg-danger' };
    }
    if (!customer.emailConfirmed) {
      return { status: 'Unverified', className: 'bg-warning' };
    }
    return { status: 'Active', className: 'bg-success' };
  };

  const filteredCustomers = customers.filter(customer => {
    const matchesSearch = searchQuery === '' || 
      customer.firstName?.toLowerCase().includes(searchQuery.toLowerCase()) ||
      customer.lastName?.toLowerCase().includes(searchQuery.toLowerCase()) ||
      customer.email.toLowerCase().includes(searchQuery.toLowerCase()) ||
      customer.userName.toLowerCase().includes(searchQuery.toLowerCase());

    const status = getUserStatus(customer).status.toLowerCase();
    const matchesStatus = statusFilter === '' || status === statusFilter;

    return matchesSearch && matchesStatus;
  });

  const selectedCustomer = selectedCustomerId ? 
    customers.find(c => c.id.toString() === selectedCustomerId) : null;

  const exportCustomersAction = async () => {
    'use server';
    console.log('Export functionality would be implemented as a route handler');
  };

  const quickActionLock = async (formData: FormData) => {
    'use server';
    const userId = parseInt(formData.get('userId') as string);
    await lockCustomerAccount(userId);
  };

  const quickActionUnlock = async (formData: FormData) => {
    'use server';
    const userId = parseInt(formData.get('userId') as string);
    await unlockCustomerAccount(userId);
  };

  const quickActionVerify = async (formData: FormData) => {
    'use server';
    const userId = parseInt(formData.get('userId') as string);
    await verifyCustomerEmail(userId);
  };

  const handleEditSubmit = async (formData: FormData) => {
    'use server';
    const userId = parseInt(formData.get('userId') as string);
    const userData = {
      email: formData.get('email') as string,
      userName: formData.get('userName') as string,
      firstName: formData.get('firstName') as string || undefined,
      lastName: formData.get('lastName') as string || undefined,
      emailConfirmed: formData.get('emailConfirmed') === 'on'
    };
    await updateCustomer(userId, userData);
  };

  const handleCreateSubmit = async (formData: FormData) => {
    'use server';
    const userData = {
      email: formData.get('email') as string,
      userName: formData.get('userName') as string,
      firstName: formData.get('firstName') as string || undefined,
      lastName: formData.get('lastName') as string || undefined,
      password: formData.get('password') as string,
      emailConfirmed: formData.get('emailConfirmed') === 'on',
      roles: ['User']
    };
    await createCustomer(userData);
  };

  const handleResetPasswordSubmit = async (formData: FormData) => {
    'use server';
    const userId = parseInt(formData.get('userId') as string);
    const newPassword = formData.get('newPassword') as string;
    await resetCustomerPassword(userId, newPassword);
  };

  const handleDeleteConfirm = async (formData: FormData) => {
    'use server';
    const userId = parseInt(formData.get('userId') as string);
    await deleteCustomer(userId);
  };

  return (
    <>
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h1 className="h3 mb-1">
            <i className="bi bi-people me-2"></i>
            Customers Management
          </h1>
          <p className="text-muted mb-0">
            Manage all customer accounts and their information
          </p>
        </div>
        <div className="btn-group" role="group">
          <a href="/admin/customers/export" className="btn btn-outline-primary">
            <i className="bi bi-download me-1"></i>
            Export
          </a>
          <a href="/admin/customers?modal=create" className="btn btn-primary">
            <i className="bi bi-person-plus me-1"></i>
            Add Customer
          </a>
        </div>
      </div>

      <div className="row mb-4">
        <div className="col-xl-3 col-md-6 mb-4">
          <div className="card border-left-primary shadow h-100 py-2">
            <div className="card-body">
              <div className="row no-gutters align-items-center">
                <div className="col mr-2">
                  <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
                    Total Customers
                  </div>
                  <div className="h5 mb-0 font-weight-bold text-gray-800">
                    {customers.length}
                  </div>
                </div>
                <div className="col-auto">
                  <i className="bi bi-people fa-2x text-gray-300"></i>
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
                    Active Customers
                  </div>
                  <div className="h5 mb-0 font-weight-bold text-gray-800">
                    {customers.filter(c => getUserStatus(c).status === 'Active').length}
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
          <div className="card border-left-warning shadow h-100 py-2">
            <div className="card-body">
              <div className="row no-gutters align-items-center">
                <div className="col mr-2">
                  <div className="text-xs font-weight-bold text-warning text-uppercase mb-1">
                    Unverified
                  </div>
                  <div className="h5 mb-0 font-weight-bold text-gray-800">
                    {customers.filter(c => !c.emailConfirmed).length}
                  </div>
                </div>
                <div className="col-auto">
                  <i className="bi bi-exclamation-triangle fa-2x text-gray-300"></i>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="col-xl-3 col-md-6 mb-4">
          <div className="card border-left-danger shadow h-100 py-2">
            <div className="card-body">
              <div className="row no-gutters align-items-center">
                <div className="col mr-2">
                  <div className="text-xs font-weight-bold text-danger text-uppercase mb-1">
                    Locked Accounts
                  </div>
                  <div className="h5 mb-0 font-weight-bold text-gray-800">
                    {customers.filter(c => getUserStatus(c).status === 'Locked').length}
                  </div>
                </div>
                <div className="col-auto">
                  <i className="bi bi-lock fa-2x text-gray-300"></i>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="card shadow mb-4">
        <div className="card-header py-3">
          <form method="GET" action="/admin/customers">
            <div className="row align-items-center">
              <div className="col-md-8">
                <div className="input-group">
                  <span className="input-group-text">
                    <i className="bi bi-search"></i>
                  </span>
                  <input 
                    type="text" 
                    className="form-control" 
                    name="search"
                    placeholder="Search customers by name, email, or username..."
                    defaultValue={searchQuery}
                  />
                </div>
              </div>
              <div className="col-md-3">
                <select 
                  className="form-select"
                  name="status"
                  defaultValue={statusFilter}
                >
                  <option value="">All Status</option>
                  <option value="active">Active</option>
                  <option value="unverified">Unverified</option>
                  <option value="locked">Locked</option>
                </select>
              </div>
              <div className="col-md-1">
                <button type="submit" className="btn btn-primary w-100">
                  <i className="bi bi-search"></i>
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>

      <div className="card shadow">
        <div className="card-header py-3">
          <h6 className="m-0 font-weight-bold text-primary">
            Customers List 
            {searchQuery || statusFilter ? (
              <span className="badge bg-secondary ms-2">{filteredCustomers.length} filtered</span>
            ) : null}
          </h6>
        </div>
        <div className="card-body">
          {filteredCustomers.length > 0 ? (
            <div className="table-responsive">
              <table className="table table-bordered table-hover">
                <thead className="table-light">
                  <tr>
                    <th>ID</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Username</th>
                    <th>Status</th>
                    <th>Email Verified</th>
                    <th>Failed Attempts</th>
                    <th>Lockout End</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredCustomers.map((customer) => {
                    const status = getUserStatus(customer);
                    return (
                      <tr key={customer.id}>
                        <td className="text-center font-weight-bold">
                          #{customer.id}
                        </td>
                        <td>
                          <div className="d-flex align-items-center">
                            <div className="me-3">
                              <div className="rounded-circle bg-primary text-white d-flex align-items-center justify-content-center" 
                                   style={{ width: '40px', height: '40px' }}>
                                <i className="bi bi-person"></i>
                              </div>
                            </div>
                            <div>
                              <div className="font-weight-bold">
                                {customer.firstName && customer.lastName 
                                  ? `${customer.firstName} ${customer.lastName}`
                                  : 'No name provided'
                                }
                              </div>
                              <small className="text-muted">Customer ID: {customer.id}</small>
                            </div>
                          </div>
                        </td>
                        <td>
                          <a href={`mailto:${customer.email}`} className="text-decoration-none">
                            {customer.email}
                          </a>
                        </td>
                        <td className="font-weight-bold">
                          {customer.userName}
                        </td>
                        <td>
                          <span className={`badge ${status.className} px-2 py-1`}>
                            {status.status}
                          </span>
                        </td>
                        <td className="text-center">
                          {customer.emailConfirmed ? (
                            <i className="bi bi-check-circle text-success"></i>
                          ) : (
                            <i className="bi bi-x-circle text-danger"></i>
                          )}
                        </td>
                        <td className="text-center">
                          <span className={`badge ${customer.accessFailedCount > 0 ? 'bg-warning' : 'bg-light text-dark'}`}>
                            {customer.accessFailedCount}
                          </span>
                        </td>
                        <td>
                          {formatDate(customer.lockoutEnd)}
                        </td>
                        <td>
                          <div className="btn-group" role="group">
                            <a 
                              href={`/admin/customers?modal=details&id=${customer.id}`}
                              className="btn btn-sm btn-outline-primary" 
                              title="View Details"
                            >
                              <i className="bi bi-eye"></i>
                            </a>
                            <a 
                              href={`/admin/customers?modal=edit&id=${customer.id}`}
                              className="btn btn-sm btn-outline-secondary" 
                              title="Edit"
                            >
                              <i className="bi bi-pencil"></i>
                            </a>
                            <a 
                              href={`/admin/customers?modal=resetPassword&id=${customer.id}`}
                              className="btn btn-sm btn-outline-warning" 
                              title="Reset Password"
                            >
                              <i className="bi bi-key"></i>
                            </a>
                            {status.status === 'Locked' ? (
                              <form action={quickActionUnlock} method="POST" style={{ display: 'inline' }}>
                                <input type="hidden" name="userId" value={customer.id} />
                                <button 
                                  type="submit" 
                                  className="btn btn-sm btn-outline-success" 
                                  title="Unlock Account"
                                >
                                  <i className="bi bi-unlock"></i>
                                </button>
                              </form>
                            ) : (
                              <form action={quickActionLock} method="POST" style={{ display: 'inline' }}>
                                <input type="hidden" name="userId" value={customer.id} />
                                <button 
                                  type="submit" 
                                  className="btn btn-sm btn-outline-danger" 
                                  title="Lock Account"
                                >
                                  <i className="bi bi-lock"></i>
                                </button>
                              </form>
                            )}
                            {!customer.emailConfirmed && (
                              <form action={quickActionVerify} method="POST" style={{ display: 'inline' }}>
                                <input type="hidden" name="userId" value={customer.id} />
                                <button 
                                  type="submit" 
                                  className="btn btn-sm btn-outline-info" 
                                  title="Verify Email"
                                >
                                  <i className="bi bi-check-lg"></i>
                                </button>
                              </form>
                            )}
                            <a 
                              href={`/admin/customers?modal=delete&id=${customer.id}`}
                              className="btn btn-sm btn-outline-danger" 
                              title="Delete Customer"
                            >
                              <i className="bi bi-trash"></i>
                            </a>
                          </div>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          ) : (
            <div className="text-center py-5">
              <i className="bi bi-people display-1 text-muted mb-3"></i>
              <h4 className="text-muted">
                {searchQuery || statusFilter ? 'No customers found' : 'No customers found'}
              </h4>
              <p className="text-muted">
                {searchQuery || statusFilter 
                  ? 'Try adjusting your search criteria or filters.'
                  : 'There are no customer accounts in the system yet.'
                }
              </p>
              {!searchQuery && !statusFilter && (
                <a 
                  href="/admin/customers?modal=create"
                  className="btn btn-primary"
                >
                  <i className="bi bi-person-plus me-2"></i>
                  Add First Customer
                </a>
              )}
            </div>
          )}
        </div>
      </div>

      {filteredCustomers.length > 0 && (
        <div className="d-flex justify-content-between align-items-center mt-4">
          <div className="text-muted">
            Showing 1 to {filteredCustomers.length} of {filteredCustomers.length} customers
          </div>
          <nav>
            <ul className="pagination mb-0">
              <li className="page-item disabled">
                <span className="page-link">Previous</span>
              </li>
              <li className="page-item active">
                <span className="page-link">1</span>
              </li>
              <li className="page-item disabled">
                <span className="page-link">Next</span>
              </li>
            </ul>
          </nav>
        </div>
      )}

      {showModal === 'details' && selectedCustomer && (
        <CustomerDetailsOverlay customer={selectedCustomer} />
      )}

      {showModal === 'edit' && selectedCustomer && (
        <EditCustomerOverlay customer={selectedCustomer} onSubmit={handleEditSubmit} />
      )}

      {showModal === 'create' && (
        <CreateCustomerOverlay onSubmit={handleCreateSubmit} />
      )}

      {showModal === 'resetPassword' && selectedCustomer && (
        <ResetPasswordOverlay customer={selectedCustomer} onSubmit={handleResetPasswordSubmit} />
      )}

      {showModal === 'delete' && selectedCustomer && (
        <DeleteCustomerOverlay customer={selectedCustomer} onSubmit={handleDeleteConfirm} />
      )}
    </>
  );
}

function CustomerDetailsOverlay({ customer }: { customer: UserWithRoles }) {
  const formatDate = (dateString?: string) => {
    if (!dateString) return 'Never';
    return new Date(dateString).toLocaleDateString();
  };

  const getUserStatus = (customer: UserWithRoles) => {
    if (customer.lockoutEnd && new Date(customer.lockoutEnd) > new Date()) {
      return { status: 'Locked', className: 'bg-danger text-white' };
    }
    if (!customer.emailConfirmed) {
      return { status: 'Unverified', className: 'bg-warning text-dark' };
    }
    return { status: 'Active', className: 'bg-success text-white' };
  };

  const status = getUserStatus(customer);

  return (
    <div className="modal fade show d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog modal-lg">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">
              <i className="bi bi-person-circle me-2"></i>
              Customer Details
            </h5>
            <a href="/admin/customers" className="btn-close"></a>
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
                        {customer.firstName && customer.lastName 
                          ? `${customer.firstName} ${customer.lastName}`
                          : 'No name provided'
                        }
                      </p>
                    </div>
                    <div className="mb-3">
                      <label className="form-label fw-bold">Email:</label>
                      <p className="mb-0">{customer.email}</p>
                    </div>
                    <div className="mb-3">
                      <label className="form-label fw-bold">Username:</label>
                      <p className="mb-0">{customer.userName}</p>
                    </div>
                    <div className="mb-3">
                      <label className="form-label fw-bold">Customer ID:</label>
                      <p className="mb-0">#{customer.id}</p>
                    </div>
                  </div>
                </div>
              </div>
              <div className="col-md-6">
                <div className="card h-100">
                  <div className="card-header">
                    <h6 className="mb-0">Account Status</h6>
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
                      <label className="form-label fw-bold">Email Verified:</label>
                      <p className="mb-0">
                        {customer.emailConfirmed ? (
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
                        <span className={`badge ${customer.accessFailedCount > 0 ? 'bg-warning text-dark' : 'bg-light text-dark'}`}>
                          {customer.accessFailedCount}
                        </span>
                      </p>
                    </div>
                    <div className="mb-3">
                      <label className="form-label fw-bold">Lockout End:</label>
                      <p className="mb-0">{formatDate(customer.lockoutEnd)}</p>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div className="modal-footer">
            <a href="/admin/customers" className="btn btn-secondary">
              Close
            </a>
          </div>
        </div>
      </div>
    </div>
  );
}

function EditCustomerOverlay({ customer, onSubmit }: { customer: UserWithRoles; onSubmit: any }) {
  return (
    <div className="modal fade show d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">
              <i className="bi bi-pencil me-2"></i>
              Edit Customer
            </h5>
            <a href="/admin/customers" className="btn-close"></a>
          </div>
          <form action={onSubmit}>
            <div className="modal-body">
              <input type="hidden" name="userId" value={customer.id} />
              
              <div className="mb-3">
                <label htmlFor="firstName" className="form-label">First Name</label>
                <input
                  type="text"
                  className="form-control"
                  id="firstName"
                  name="firstName"
                  defaultValue={customer.firstName || ''}
                />
              </div>

              <div className="mb-3">
                <label htmlFor="lastName" className="form-label">Last Name</label>
                <input
                  type="text"
                  className="form-control"
                  id="lastName"
                  name="lastName"
                  defaultValue={customer.lastName || ''}
                />
              </div>

              <div className="mb-3">
                <label htmlFor="email" className="form-label">Email Address</label>
                <input
                  type="email"
                  className="form-control"
                  id="email"
                  name="email"
                  defaultValue={customer.email}
                  required
                />
              </div>

              <div className="mb-3">
                <label htmlFor="userName" className="form-label">Username</label>
                <input
                  type="text"
                  className="form-control"
                  id="userName"
                  name="userName"
                  defaultValue={customer.userName}
                  required
                />
              </div>

              <div className="mb-3">
                <div className="form-check">
                  <input
                    className="form-check-input"
                    type="checkbox"
                    id="emailConfirmed"
                    name="emailConfirmed"
                    defaultChecked={customer.emailConfirmed}
                  />
                  <label className="form-check-label" htmlFor="emailConfirmed">
                    Email Verified
                  </label>
                </div>
              </div>
            </div>
            <div className="modal-footer">
              <a href="/admin/customers" className="btn btn-secondary">
                Cancel
              </a>
              <button type="submit" className="btn btn-primary">
                <i className="bi bi-save me-1"></i>
                Save Changes
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}

function CreateCustomerOverlay({ onSubmit }: { onSubmit: any }) {
  return (
    <div className="modal fade show d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">
              <i className="bi bi-person-plus me-2"></i>
              Add New Customer
            </h5>
            <a href="/admin/customers" className="btn-close"></a>
          </div>
          <form action={onSubmit}>
            <div className="modal-body">
              <div className="mb-3">
                <label htmlFor="firstName" className="form-label">First Name</label>
                <input
                  type="text"
                  className="form-control"
                  id="firstName"
                  name="firstName"
                  placeholder="Enter first name"
                />
              </div>

              <div className="mb-3">
                <label htmlFor="lastName" className="form-label">Last Name</label>
                <input
                  type="text"
                  className="form-control"
                  id="lastName"
                  name="lastName"
                  placeholder="Enter last name"
                />
              </div>

              <div className="mb-3">
                <label htmlFor="email" className="form-label">Email Address *</label>
                <input
                  type="email"
                  className="form-control"
                  id="email"
                  name="email"
                  placeholder="Enter email address"
                  required
                />
              </div>

              <div className="mb-3">
                <label htmlFor="userName" className="form-label">Username *</label>
                <input
                  type="text"
                  className="form-control"
                  id="userName"
                  name="userName"
                  placeholder="Enter username"
                  required
                />
              </div>

              <div className="mb-3">
                <label htmlFor="password" className="form-label">Password *</label>
                <input
                  type="password"
                  className="form-control"
                  id="password"
                  name="password"
                  placeholder="Enter password"
                  minLength={6}
                  required
                />
                <div className="form-text">Password must be at least 6 characters long.</div>
              </div>

              <div className="mb-3">
                <div className="form-check">
                  <input
                    className="form-check-input"
                    type="checkbox"
                    id="emailConfirmed"
                    name="emailConfirmed"
                  />
                  <label className="form-check-label" htmlFor="emailConfirmed">
                    Mark email as verified
                  </label>
                </div>
              </div>
            </div>
            <div className="modal-footer">
              <a href="/admin/customers" className="btn btn-secondary">
                Cancel
              </a>
              <button type="submit" className="btn btn-primary">
                <i className="bi bi-person-plus me-1"></i>
                Create Customer
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}

function ResetPasswordOverlay({ customer, onSubmit }: { customer: UserWithRoles; onSubmit: any }) {
  return (
    <div className="modal fade show d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">
              <i className="bi bi-key me-2"></i>
              Reset Password
            </h5>
            <a href="/admin/customers" className="btn-close"></a>
          </div>
          <form action={onSubmit}>
            <div className="modal-body">
              <input type="hidden" name="userId" value={customer.id} />
              
              <div className="alert alert-info">
                <i className="bi bi-info-circle me-2"></i>
                You are about to reset the password for <strong>{customer.email}</strong>
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

              <div className="mb-3">
                <label htmlFor="confirmPassword" className="form-label">Confirm New Password *</label>
                <input
                  type="password"
                  className="form-control"
                  id="confirmPassword"
                  name="confirmPassword"
                  placeholder="Confirm new password"
                  minLength={6}
                  required
                />
              </div>
            </div>
            <div className="modal-footer">
              <a href="/admin/customers" className="btn btn-secondary">
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

function DeleteCustomerOverlay({ customer, onSubmit }: { customer: UserWithRoles; onSubmit: any }) {
  return (
    <div className="modal fade show d-block" style={{ backgroundColor: 'rgba(0,0,0,0.5)' }}>
      <div className="modal-dialog">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">
              <i className="bi bi-exclamation-triangle text-danger me-2"></i>
              Delete Customer
            </h5>
            <a href="/admin/customers" className="btn-close"></a>
          </div>
          <form action={onSubmit}>
            <input type="hidden" name="userId" value={customer.id} />
            <div className="modal-body">
              <div className="alert alert-danger">
                <i className="bi bi-exclamation-triangle-fill me-2"></i>
                <strong>Warning!</strong> This action cannot be undone.
              </div>
              
              <p>Are you sure you want to delete the customer account for:</p>
              <div className="card">
                <div className="card-body">
                  <h6 className="card-title">{customer.firstName && customer.lastName 
                    ? `${customer.firstName} ${customer.lastName}`
                    : 'No name provided'
                  }</h6>
                  <p className="card-text">
                    <strong>Email:</strong> {customer.email}<br />
                    <strong>Username:</strong> {customer.userName}<br />
                    <strong>ID:</strong> #{customer.id}
                  </p>
                </div>
              </div>
              
              <p className="mt-3 text-muted">
                This will permanently delete all customer data including orders, shopping cart, and account history.
              </p>
            </div>
            <div className="modal-footer">
              <a href="/admin/customers" className="btn btn-secondary">
                Cancel
              </a>
              <button type="submit" className="btn btn-danger">
                <i className="bi bi-trash me-1"></i>
                Delete Customer
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
} 