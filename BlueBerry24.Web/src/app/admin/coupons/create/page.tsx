import { createCoupon } from '@/lib/actions/coupon-actions';
import { CouponTypeSelect } from '@/components/coupon/CouponDisplayComponents';
import Link from 'next/link';

interface SearchParams {
  error?: string;
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

export default async function CreateCouponPage({
  searchParams,
}: {
  searchParams: SearchParams
}) {
  const { error } = searchParams;

  return (
    <div className="container-fluid">
      <div className="row">
        <div className="col-12">
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
                <i className="bi bi-plus-circle me-1"></i>Create Coupon
              </li>
            </ol>
          </nav>

          {error && <ErrorAlert message={error} />}

          <div className="card shadow">
            <div className="card-header bg-primary text-white">
              <h3 className="card-title mb-0">
                <i className="bi bi-plus-circle me-2"></i>
                Create New Coupon
              </h3>
            </div>
            <div className="card-body">
              <form action={createCoupon}>
                <div className="row">
                  <div className="col-md-6">
                    <div className="mb-3">
                      <label htmlFor="code" className="form-label">
                        <i className="bi bi-hash me-1"></i>
                        Coupon Code <span className="text-danger">*</span>
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        id="code"
                        name="code"
                        placeholder="e.g., SAVE20, WELCOME10"
                        required
                        pattern="[A-Za-z0-9]+"
                        title="Only letters and numbers allowed"
                        maxLength={20}
                      />
                      <div className="form-text">
                        Use letters and numbers only. This will be what customers enter at checkout.
                      </div>
                    </div>
                  </div>

                  <div className="col-md-6">
                    <div className="mb-3">
                      <label htmlFor="type" className="form-label">
                        <i className="bi bi-tags me-1"></i>
                        Coupon Type <span className="text-danger">*</span>
                      </label>
                      <CouponTypeSelect name="type" required />
                    </div>
                  </div>
                </div>

                <div className="row">
                  <div className="col-md-6">
                    <div className="mb-3">
                      <label htmlFor="value" className="form-label">
                        <i className="bi bi-currency-dollar me-1"></i>
                        Discount Value <span className="text-danger">*</span>
                      </label>
                      <div className="input-group">
                        <input
                          type="number"
                          className="form-control"
                          id="value"
                          name="value"
                          step="0.01"
                          min="0.01"
                          placeholder="10"
                          required
                        />
                        <span className="input-group-text">
                          <span id="valueUnit">%</span>
                        </span>
                      </div>
                      <div className="form-text">
                        Enter the discount value (e.g., 10 for 10% or $10)
                      </div>
                    </div>
                  </div>

                  <div className="col-md-6">
                    <div className="mb-3">
                      <label htmlFor="minimumAmount" className="form-label">
                        <i className="bi bi-cart me-1"></i>
                        Minimum Order Amount
                      </label>
                      <div className="input-group">
                        <span className="input-group-text">$</span>
                        <input
                          type="number"
                          className="form-control"
                          id="minimumAmount"
                          name="minimumAmount"
                          step="0.01"
                          min="0"
                          placeholder="0.00"
                        />
                      </div>
                      <div className="form-text">
                        Leave blank or 0 for no minimum requirement
                      </div>
                    </div>
                  </div>
                </div>

                <div className="mb-4">
                  <label htmlFor="description" className="form-label">
                    <i className="bi bi-file-text me-1"></i>
                    Description <span className="text-danger">*</span>
                  </label>
                  <textarea
                    className="form-control"
                    id="description"
                    name="description"
                    rows={3}
                    placeholder="Describe what this coupon offers..."
                    required
                    maxLength={500}
                  ></textarea>
                  <div className="form-text">
                    Provide a clear description that will be shown to customers (max 500 characters)
                  </div>
                </div>

                <div className="row">
                  <div className="col-md-6">
                    <div className="form-check form-switch mb-3">
                      <input
                        className="form-check-input"
                        type="checkbox"
                        id="isActive"
                        name="isActive"
                        defaultChecked
                      />
                      <label className="form-check-label" htmlFor="isActive">
                        <i className="bi bi-toggle-on me-1"></i>
                        Active Coupon
                      </label>
                      <div className="form-text">
                        Only active coupons can be used by customers
                      </div>
                    </div>
                  </div>

                  <div className="col-md-6">
                    <div className="form-check form-switch mb-3">
                      <input
                        className="form-check-input"
                        type="checkbox"
                        id="isForNewUsersOnly"
                        name="isForNewUsersOnly"
                      />
                      <label className="form-check-label" htmlFor="isForNewUsersOnly">
                        <i className="bi bi-person-plus me-1"></i>
                        New Users Only
                      </label>
                      <div className="form-text">
                        Restrict this coupon to new customers only
                      </div>
                    </div>
                  </div>
                </div>

                <hr />

                <div className="d-flex justify-content-between">
                  <Link href="/admin/coupons" className="btn btn-outline-secondary">
                    <i className="bi bi-arrow-left me-1"></i>
                    Cancel
                  </Link>
                  <button type="submit" className="btn btn-primary">
                    <i className="bi bi-check me-1"></i>
                    Create Coupon
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Create Coupon - Admin Dashboard',
  description: 'Create a new discount coupon for your store',
}; 