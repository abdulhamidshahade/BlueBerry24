import { Suspense } from 'react';
import Link from 'next/link';
import { notFound } from 'next/navigation';
import { getCoupon} from '../../../../../lib/actions/coupon-actions';
import { 
  CouponTypeDisplay, 
  CouponValueDisplay, 
  CouponMinimumAmountDisplay,
  CouponStatusDisplay,
  CouponNewUserDisplay
} from '../../../../../components/coupon/CouponDisplayComponents';
import { UsersList } from '../../../../../components/coupon/UserList';

interface SearchParams {
  success?: string;
  error?: string;
}

interface PageProps {
  params: Promise<{
    id: string;
  }>;
  searchParams: Promise<SearchParams>;
}

function SuccessAlert({ message }: { message: string }) {
  const messages = {
    coupon_disabled: 'User coupon disabled successfully!',
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

function UsersLoadingSkeleton() {
  return (
    <div className="table-responsive">
      <table className="table table-hover">
        <thead className="table-light">
          <tr>
            <th>User</th>
            <th>Email</th>
            <th>Usage Status</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {Array.from({ length: 5 }, (_, i) => (
            <tr key={i}>
              <td><div className="placeholder-glow"><span className="placeholder col-6"></span></div></td>
              <td><div className="placeholder-glow"><span className="placeholder col-8"></span></div></td>
              <td><div className="placeholder-glow"><span className="placeholder col-4"></span></div></td>
              <td><div className="placeholder-glow"><span className="placeholder col-6"></span></div></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}


export default async function CouponUsersPage({ params, searchParams }: PageProps) {

  var resolvedSearchParams = await searchParams;
  const {id} = await params;
  const couponId = (id as string) ? parseInt(id) : NaN;
  const { success, error } = resolvedSearchParams;

  if (isNaN(couponId)) {
    notFound();
  }

  const coupon = await getCoupon(couponId);

  if (!coupon) {
    notFound();
  }

  return (
    <div className="container-fluid">
      <nav aria-label="breadcrumb" className="mb-4">
        <ol className="breadcrumb">
          <li className="breadcrumb-item">
            <Link href="/admin" className="text-decoration-none">
              <i className="bi bi-speedometer2 me-1"></i>Admin
            </Link>
          </li>
          <li className="breadcrumb-item">
            <Link href="/admin/coupons" className="text-decoration-none">
              <i className="bi bi-tag me-1"></i>Coupons
            </Link>
          </li>
          <li className="breadcrumb-item active" aria-current="page">
            <i className="bi bi-people me-1"></i>Users for {coupon.code}
          </li>
        </ol>
      </nav>

      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h1 className="h2 mb-1">
            <i className="bi bi-people me-2"></i>Users for Coupon: {coupon.code}
          </h1>
          <p className="text-muted mb-0">Manage users who have access to this coupon</p>
        </div>
        <div className="d-flex gap-2">
          <Link 
            href={`/admin/coupons/${couponId}/add-user`} 
            className="btn btn-success"
          >
            <i className="bi bi-person-plus me-2"></i>Add User
          </Link>
          <Link 
            href="/admin/coupons" 
            className="btn btn-outline-secondary"
          >
            <i className="bi bi-arrow-left me-1"></i>Back to Coupons
          </Link>
        </div>
      </div>

      {success && <SuccessAlert message={success} />}
      {error && <ErrorAlert message={error} />}

      <div className="card shadow-sm mb-4">
        <div className="card-header bg-light">
          <h5 className="card-title mb-0">
            <i className="bi bi-tag me-2"></i>Coupon Details
          </h5>
        </div>
        <div className="card-body">
          <div className="row">
            <div className="col-md-2">
              <strong>Code:</strong><br />
              <span className="text-primary">{coupon.code}</span>
            </div>
            <div className="col-md-2">
              <strong>Type:</strong><br />
              <CouponTypeDisplay type={coupon.type} />
            </div>
            <div className="col-md-2">
              <strong>Value:</strong><br />
              <span className="text-success"><CouponValueDisplay coupon={coupon} /></span>
            </div>
            <div className="col-md-2">
              <strong>Min Amount:</strong><br />
              <CouponMinimumAmountDisplay minimumAmount={coupon.minimumOrderAmount} />
            </div>
            <div className="col-md-2">
              <strong>Status:</strong><br />
              <CouponStatusDisplay isActive={coupon.isActive} />
            </div>
            <div className="col-md-2">
              <strong>New Users Only:</strong><br />
              <CouponNewUserDisplay isForNewUsersOnly={coupon.isForNewUsersOnly} />
            </div>
          </div>
          {coupon.description && (
            <div className="mt-3">
              <strong>Description:</strong><br />
              <span className="text-muted">{coupon.description}</span>
            </div>
          )}
        </div>
      </div>

      <div className="card shadow-sm">
        <div className="card-header bg-light">
          <div className="row align-items-center">
            <div className="col">
              <h5 className="card-title mb-0">
                <i className="bi bi-people me-2"></i>Users with Access
              </h5>
            </div>
          </div>
        </div>
        <div className="card-body p-0">
          <Suspense fallback={<UsersLoadingSkeleton />}>
            <UsersList couponId={couponId} couponCode={coupon.code} />
          </Suspense>
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Coupon Users - Admin Dashboard',
  description: 'Manage users who have access to coupons',
};