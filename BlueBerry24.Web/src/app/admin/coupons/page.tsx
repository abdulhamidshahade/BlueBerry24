import { Suspense } from 'react';
import Link from 'next/link';
import { getCoupons, toggleCouponStatus } from '@/lib/actions/coupon-actions';
import { 
  CouponTypeDisplay, 
  CouponValueDisplay, 
  CouponStatusDisplay, 
  CouponNewUserDisplay, 
  CouponMinimumAmountDisplay 
} from '@/components/coupon/CouponDisplayComponents';

interface SearchParams {
  success?: string;
  error?: string;
}

function SuccessAlert({ message }: { message: string }) {
  const messages = {
    created: 'Coupon created successfully!',
    updated: 'Coupon updated successfully!',
    deleted: 'Coupon deleted successfully!',
    status_toggled: 'Coupon status updated successfully!',
    coupon_added: 'Coupon added to user successfully!',
    coupon_added_to_specific_users: 'Coupon added to specific users successfully!',
    coupon_added_to_all_users: 'Coupon added to all users successfully!',
    coupon_added_to_new_users: 'Coupon added to new users successfully!'
  };

  return (
    <div className="alert alert-success alert-dismissible fade show" role="alert">
      <i className="bi bi-check-circle-fill me-2"></i>
      {messages[message as keyof typeof messages] || message}
      <button type="button" className="btn-close" data-bs-dismiss="alert"></button>
    </div>
  );
}

function ErrorAlert({ message }: { message: string }) {
  return (
    <div className="alert alert-danger alert-dismissible fade show" role="alert">
      <i className="bi bi-exclamation-circle-fill me-2"></i>
      {decodeURIComponent(message)}
      <button type="button" className="btn-close" data-bs-dismiss="alert"></button>
    </div>
  );
}

function CouponsLoadingSkeleton() {
  return (
    <div className="table-responsive">
      <table className="table table-hover">
        <thead className="table-light">
          <tr>
            <th>Code</th>
            <th>Type</th>
            <th>Value</th>
            <th>Min Amount</th>
            <th>Status</th>
            <th>For New Users</th>
            <th>Users</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {Array.from({ length: 5 }, (_, i) => (
            <tr key={i}>
              <td><div className="placeholder-glow"><span className="placeholder col-6"></span></div></td>
              <td><div className="placeholder-glow"><span className="placeholder col-4"></span></div></td>
              <td><div className="placeholder-glow"><span className="placeholder col-3"></span></div></td>
              <td><div className="placeholder-glow"><span className="placeholder col-3"></span></div></td>
              <td><div className="placeholder-glow"><span className="placeholder col-4"></span></div></td>
              <td><div className="placeholder-glow"><span className="placeholder col-4"></span></div></td>
              <td><div className="placeholder-glow"><span className="placeholder col-3"></span></div></td>
              <td><div className="placeholder-glow"><span className="placeholder col-8"></span></div></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

async function CouponsList() {
  const coupons = await getCoupons();

  if (coupons.length === 0) {
    return (
      <div className="text-center py-5">
        <div className="mb-4">
          <i className="bi bi-tag display-1 text-muted"></i>
        </div>
        <h3 className="text-muted">No coupons found</h3>
        <p className="text-muted mb-4">Start by creating your first coupon.</p>
        <Link href="/admin/coupons/create" className="btn btn-primary">
          <i className="bi bi-plus-circle me-2"></i>Create First Coupon
        </Link>
      </div>
    );
  }

  return (
    <div className="table-responsive">
      <table className="table table-hover">
        <thead className="table-light">
          <tr>
            <th scope="col">
              <i className="bi bi-hash me-1"></i>
              Code
            </th>
            <th scope="col">
              <i className="bi bi-tags me-1"></i>
              Type
            </th>
            <th scope="col">
              <i className="bi bi-currency-dollar me-1"></i>
              Value
            </th>
            <th scope="col">
              <i className="bi bi-cart me-1"></i>
              Min Amount
            </th>
            <th scope="col">
              <i className="bi bi-toggle-on me-1"></i>
              Status
            </th>
            <th scope="col">
              <i className="bi bi-person me-1"></i>
              For New Users
            </th>
            <th scope="col">
              <i className="bi bi-people me-1"></i>
              Users
            </th>
            <th scope="col">
              <i className="bi bi-gear me-1"></i>
              Actions
            </th>
          </tr>
        </thead>
        <tbody>
          {coupons.map((coupon) => (
            <tr key={coupon.id}>
              <td>
                <div>
                  <strong className="text-primary">{coupon.code}</strong>
                  <small className="d-block text-muted">{coupon.description}</small>
                </div>
              </td>
              <td>
                <CouponTypeDisplay type={coupon.type} />
              </td>
              <td>
                <strong><CouponValueDisplay coupon={coupon} /></strong>
              </td>
              <td>
                <CouponMinimumAmountDisplay minimumAmount={coupon.minimumOrderAmount} />
              </td>
              <td>
                <form action={toggleCouponStatus} className="d-inline">
                  <input type="hidden" name="id" value={coupon.id} />
                  <button
                    type="submit"
                    className={`btn btn-sm ${
                      coupon.isActive 
                        ? 'btn-success' 
                        : 'btn-outline-secondary'
                    }`}
                    title={`Click to ${coupon.isActive ? 'disable' : 'enable'} coupon`}
                  >
                    {coupon.isActive ? (
                      <>
                        <i className="bi bi-toggle-on me-1"></i>
                        Active
                      </>
                    ) : (
                      <>
                        <i className="bi bi-toggle-off me-1"></i>
                        Inactive
                      </>
                    )}
                  </button>
                </form>
              </td>
              <td>
                <CouponNewUserDisplay isForNewUsersOnly={coupon.isForNewUsersOnly} />
              </td>
              <td>
                <Link 
                  href={`/admin/coupons/${coupon.id}/users`}
                  className="btn btn-outline-info btn-sm"
                  title="View Users"
                >
                  <i className="bi bi-people"></i>
                  <span className="d-none d-lg-inline ms-1">View Users</span>
                </Link>
              </td>
              <td>
                <div className="btn-group btn-group-sm" role="group">
                  <Link 
                    href={`/admin/coupons/${coupon.id}/add-user`}
                    className="btn btn-outline-success"
                    title="Add to User"
                  >
                    <i className="bi bi-person-plus"></i>
                  </Link>
                  <Link 
                    href={`/admin/coupons/update/${coupon.id}`}
                    className="btn btn-outline-primary"
                    title="Edit Coupon"
                  >
                    <i className="bi bi-pencil"></i>
                  </Link>
                  <Link 
                    href={`/admin/coupons/delete/${coupon.id}`}
                    className="btn btn-outline-danger"
                    title="Delete Coupon"
                  >
                    <i className="bi bi-trash"></i>
                  </Link>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default async function AdminCouponsPage({
  searchParams,
}: {
  searchParams: SearchParams
}) {
  const { success, error } = searchParams;

  return (
    <div className="container-fluid">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h1 className="h2 mb-1">
            <i className="bi bi-tag me-2"></i>Coupon Management
          </h1>
          <p className="text-muted mb-0">Manage discount coupons and promotions</p>
        </div>
        <div className="d-flex gap-2">
          <div className="btn-group" role="group">
            <button 
              type="button" 
              className="btn btn-outline-info dropdown-toggle" 
              data-bs-toggle="dropdown" 
              aria-expanded="false"
            >
              <i className="bi bi-people me-2"></i>Distribute Coupons
            </button>
            <ul className="dropdown-menu">
              <li>
                <Link 
                  href="/admin/coupons/add-to-user" 
                  className="dropdown-item"
                >
                  <i className="bi bi-person-plus me-2"></i>Add to Single User
                </Link>
              </li>
              <li>
                <Link 
                  href="/admin/coupons/add-to-specific-users" 
                  className="dropdown-item"
                >
                  <i className="bi bi-people-fill me-2"></i>Add to Specific Users
                </Link>
              </li>
              <li>
                <Link 
                  href="/admin/coupons/add-to-all-users" 
                  className="dropdown-item"
                >
                  <i className="bi bi-globe me-2"></i>Add to All Users
                </Link>
              </li>
              <li>
                <Link 
                  href="/admin/coupons/add-to-new-users" 
                  className="dropdown-item"
                >
                  <i className="bi bi-person-plus me-2"></i>Add to New Users
                </Link>
              </li>
            </ul>
          </div>
          <Link 
            href="/admin/coupons/create" 
            className="btn btn-primary"
          >
            <i className="bi bi-plus-circle me-2"></i>Add New Coupon
          </Link>
        </div>
      </div>

      {success && <SuccessAlert message={success} />}
      {error && <ErrorAlert message={error} />}

      <div className="card shadow-sm">
        <div className="card-header bg-light">
          <div className="row align-items-center">
            <div className="col">
              <h5 className="card-title mb-0">
                <i className="bi bi-list-ul me-2"></i>Coupons Overview
              </h5>
            </div>
            <div className="col-auto">
              <div className="btn-group btn-group-sm" role="group">
                <button 
                  type="button" 
                  className="btn btn-outline-secondary active"
                  title="Table View"
                >
                  <i className="bi bi-table"></i>
                </button>
                <button 
                  type="button" 
                  className="btn btn-outline-secondary"
                  title="Card View"
                >
                  <i className="bi bi-grid-3x3-gap"></i>
                </button>
              </div>
            </div>
          </div>
        </div>
        <div className="card-body p-0">
          <Suspense fallback={<CouponsLoadingSkeleton />}>
            <CouponsList />
          </Suspense>
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Coupon Management - Admin Dashboard',
  description: 'Manage discount coupons and promotions',
}; 