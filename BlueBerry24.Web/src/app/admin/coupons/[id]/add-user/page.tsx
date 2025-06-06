import { Suspense } from 'react';
import Link from 'next/link';
import { notFound } from 'next/navigation';
import { getCoupon} from '@/lib/actions/coupon-actions';
import {
  CouponTypeDisplay,
  CouponValueDisplay,
  CouponMinimumAmountDisplay,
  CouponStatusDisplay,
  CouponNewUserDisplay
} from '@/components/coupon/CouponDisplayComponents';
import { AddUserForm } from '@/components/coupon/AddUserForm';

interface SearchParams {
  error?: string;
}

interface PageProps {
  params: {
    id: string;
  };
  searchParams: SearchParams;
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

export default async function AddUserToCouponPage({ params, searchParams }: PageProps) {
  const couponId = parseInt(params.id);
  const { error } = searchParams;

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
          <li className="breadcrumb-item">
            <Link href={`/admin/coupons/${couponId}/users`} className="text-decoration-none">
              <i className="bi bi-people me-1"></i>Users for {coupon.code}
            </Link>
          </li>
          <li className="breadcrumb-item active" aria-current="page">
            <i className="bi bi-person-plus me-1"></i>Add User
          </li>
        </ol>
      </nav>

      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h1 className="h2 mb-1">
            <i className="bi bi-person-plus me-2"></i>Add User to Coupon: {coupon.code}
          </h1>
          <p className="text-muted mb-0">Grant a user access to this coupon</p>
        </div>
        <Link
          href={`/admin/coupons/${couponId}/users`}
          className="btn btn-outline-secondary"
        >
          <i className="bi bi-arrow-left me-1"></i>Back to Users
        </Link>
      </div>

      {error && <ErrorAlert message={error} />}

      <div className="row">
        <div className="col-md-8">
          <div className="card shadow-sm">
            <div className="card-header bg-light">
              <h5 className="card-title mb-0">
                <i className="bi bi-person-plus me-2"></i>Select User
              </h5>
            </div>
            <div className="card-body">
              <Suspense fallback={
                <div className="d-flex justify-content-center py-4">
                  <div className="spinner-border text-primary" role="status">
                    <span className="visually-hidden">Loading...</span>
                  </div>
                </div>
              }>
                <AddUserForm couponId={couponId} />
              </Suspense>
            </div>
          </div>
        </div>

        <div className="col-md-4">
          <div className="card shadow-sm">
            <div className="card-header bg-light">
              <h5 className="card-title mb-0">
                <i className="bi bi-tag me-2"></i>Coupon Details
              </h5>
            </div>
            <div className="card-body">
              <div className="mb-3">
                <strong>Code:</strong><br />
                <span className="text-primary fs-5">{coupon.code}</span>
              </div>

              <div className="mb-3">
                <strong>Type:</strong><br />
                <CouponTypeDisplay type={coupon.type} />
              </div>

              <div className="mb-3">
                <strong>Value:</strong><br />
                <span className="text-success fs-5"><CouponValueDisplay coupon={coupon} /></span>
              </div>

              {coupon.minimumOrderAmount > 0 && (
                <div className="mb-3">
                  <strong>Minimum Amount:</strong><br />
                  <CouponMinimumAmountDisplay minimumAmount={coupon.minimumOrderAmount} />
                </div>
              )}

              <div className="mb-3">
                <strong>Status:</strong><br />
                <CouponStatusDisplay isActive={coupon.isActive} />
              </div>

              {coupon.isForNewUsersOnly && (
                <div className="mb-3">
                  <CouponNewUserDisplay isForNewUsersOnly={coupon.isForNewUsersOnly} />
                </div>
              )}

              {coupon.description && (
                <div className="mb-0">
                  <strong>Description:</strong><br />
                  <span className="text-muted">{coupon.description}</span>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Add User to Coupon - Admin Dashboard',
  description: 'Grant a user access to a coupon',
}; 