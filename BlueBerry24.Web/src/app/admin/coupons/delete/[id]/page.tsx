import { getCoupon, deleteCoupon } from '@/lib/actions/coupon-actions';
import { 
  CouponTypeDisplay, 
  CouponValueDisplay, 
  CouponStatusDisplay, 
  CouponNewUserDisplay, 
  CouponMinimumAmountDisplay 
} from '@/components/coupon/CouponDisplayComponents';
import Link from 'next/link';
import { notFound } from 'next/navigation';

interface DeleteCouponPageProps {
  params: {
    id: string;
  };
  searchParams: {
    error?: string;
  };
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

export default async function DeleteCouponPage({ params, searchParams }: DeleteCouponPageProps) {
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
      <div className="row">
        <div className="col-md-8 mx-auto">
          <nav aria-label="breadcrumb" className="mb-4">
            <ol className="breadcrumb">
              <li className="breadcrumb-item">
                <Link href="/admin" className="text-decoration-none">
                  <i className="bi bi-house-door me-1"></i>Admin
                </Link>
              </li>
              <li className="breadcrumb-item">
                <Link href="/admin/coupons" className="text-decoration-none">
                  <i className="bi bi-tag me-1"></i>Coupons
                </Link>
              </li>
              <li className="breadcrumb-item active" aria-current="page">
                <i className="bi bi-trash me-1"></i>Delete {coupon.code}
              </li>
            </ol>
          </nav>

          {error && <ErrorAlert message={error} />}

          <div className="card shadow border-danger">
            <div className="card-header bg-danger text-white">
              <h3 className="card-title mb-0">
                <i className="bi bi-exclamation-triangle me-2"></i>
                Delete Coupon
              </h3>
            </div>
            <div className="card-body">
              <div className="alert alert-warning d-flex align-items-center" role="alert">
                <i className="bi bi-exclamation-triangle-fill me-2"></i>
                <div>
                  <strong>Warning!</strong> This action cannot be undone. Deleting this coupon will permanently remove it from the system.
                </div>
              </div>

              <div className="card border-left-primary mb-4">
                <div className="card-body">
                  <h5 className="card-title text-danger mb-3">
                    <i className="bi bi-tag me-1"></i>
                    Coupon Details
                  </h5>
                  
                  <div className="row">
                    <div className="col-md-6">
                      <div className="mb-3">
                        <label className="form-label fw-bold">
                          <i className="bi bi-hash me-1"></i>
                          Coupon Code
                        </label>
                        <p className="form-control-plaintext text-primary fs-5 fw-bold">
                          {coupon.code}
                        </p>
                      </div>
                    </div>
                    <div className="col-md-6">
                      <div className="mb-3">
                        <label className="form-label fw-bold">
                          <i className="bi bi-tags me-1"></i>
                          Type
                        </label>
                        <div className="form-control-plaintext">
                          <CouponTypeDisplay type={coupon.type} />
                        </div>
                      </div>
                    </div>
                  </div>

                  <div className="row">
                    <div className="col-md-6">
                      <div className="mb-3">
                        <label className="form-label fw-bold">
                          <i className="bi bi-currency-dollar me-1"></i>
                          Discount Value
                        </label>
                        <p className="form-control-plaintext fs-5 fw-bold text-success">
                          <CouponValueDisplay coupon={coupon} />
                        </p>
                      </div>
                    </div>
                    <div className="col-md-6">
                      <div className="mb-3">
                        <label className="form-label fw-bold">
                          <i className="bi bi-cart me-1"></i>
                          Minimum Amount
                        </label>
                        <p className="form-control-plaintext">
                          <CouponMinimumAmountDisplay minimumAmount={coupon.minimumOrderAmount} />
                        </p>
                      </div>
                    </div>
                  </div>

                  <div className="mb-3">
                    <label className="form-label fw-bold">
                      <i className="bi bi-file-text me-1"></i>
                      Description
                    </label>
                    <p className="form-control-plaintext">
                      {coupon.description}
                    </p>
                  </div>

                  <div className="row">
                    <div className="col-md-6">
                      <div className="mb-3">
                        <label className="form-label fw-bold">
                          <i className="bi bi-toggle-on me-1"></i>
                          Status
                        </label>
                        <div className="form-control-plaintext">
                          <CouponStatusDisplay isActive={coupon.isActive} />
                        </div>
                      </div>
                    </div>
                    <div className="col-md-6">
                      <div className="mb-3">
                        <label className="form-label fw-bold">
                          <i className="bi bi-person me-1"></i>
                          User Restriction
                        </label>
                        <div className="form-control-plaintext">
                          <CouponNewUserDisplay isForNewUsersOnly={coupon.isForNewUsersOnly} />
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>

              <div className="alert alert-info d-flex align-items-center" role="alert">
                <i className="bi bi-info-circle-fill me-2"></i>
                <div>
                  Before deleting this coupon, make sure no active orders or shopping carts are using it. 
                  Consider deactivating the coupon instead if you want to prevent future use while keeping historical data.
                </div>
              </div>

              <hr />

              <div className="d-flex justify-content-between">
                <Link href="/admin/coupons" className="btn btn-outline-secondary">
                  <i className="bi bi-arrow-left me-1"></i>
                  Cancel
                </Link>
                <form action={deleteCoupon} className="d-inline">
                  <input type="hidden" name="id" value={coupon.id} />
                  <button 
                    type="submit" 
                    className="btn btn-danger"
                  >
                    <i className="bi bi-trash me-1"></i>
                    Delete Coupon
                  </button>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Delete Coupon - Admin Dashboard',
  description: 'Delete coupon from the system',
};