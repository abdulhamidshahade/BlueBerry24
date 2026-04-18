'use client';

import { useActionState } from 'react';
import { CartCouponDto } from '../../types/cart';
import { applyCoupon, removeCoupon, CouponActionState } from '../../lib/actions/cart-actions';

interface CouponSectionProps {
  appliedCoupons: CartCouponDto[];
  couponError?: string;
}

export default function CouponSection({ appliedCoupons, couponError }: CouponSectionProps) {
  const [state, formAction, isPending] = useActionState<CouponActionState, FormData>(
    applyCoupon,
    null
  );

  return (
    <div className="card mb-3">
      <div className="card-header">
        <h6 className="mb-0">
          <i className="bi bi-tag me-2"></i>
          Coupons &amp; Discounts
        </h6>
      </div>
      <div className="card-body">

        {/* Apply coupon form */}
        <form action={formAction} className="mb-3">
          <div className="row g-2">
            <div className="col-md-8">
              <input
                type="text"
                name="couponCode"
                className={`form-control ${state?.error ? 'is-invalid' : state?.success ? 'is-valid' : ''}`}
                placeholder="Enter coupon code"
                required
                disabled={isPending}
              />
              {state?.error && (
                <div className="invalid-feedback d-block">
                  <i className="bi bi-exclamation-circle me-1"></i>
                  {state.error}
                </div>
              )}
              {state?.success && (
                <div className="valid-feedback d-block text-success">
                  <i className="bi bi-check-circle me-1"></i>
                  {state.success}
                </div>
              )}
            </div>
            <div className="col-md-4">
              <button
                type="submit"
                className="btn btn-outline-primary w-100"
                disabled={isPending}
              >
                {isPending ? (
                  <>
                    <span className="spinner-border spinner-border-sm me-1" role="status" aria-hidden="true" />
                    Applying…
                  </>
                ) : (
                  <>
                    <i className="bi bi-plus-circle me-1"></i>
                    Apply
                  </>
                )}
              </button>
            </div>
          </div>
        </form>

        {/* Error from remove coupon (passed via query param) */}
        {couponError && (
          <div className="alert alert-danger alert-sm py-2 mb-3">
            <i className="bi bi-exclamation-triangle me-2"></i>
            {couponError}
          </div>
        )}

        {/* Applied coupons list */}
        {appliedCoupons.length > 0 ? (
          <div>
            <h6 className="text-success mb-2">
              <i className="bi bi-check-circle me-1"></i>
              Applied Coupons:
            </h6>
            {appliedCoupons.map((coupon) => (
              <div
                key={coupon.id}
                className="d-flex justify-content-between align-items-center bg-success bg-opacity-10 border border-success border-opacity-25 rounded p-2 mb-2"
              >
                <div>
                  <strong className="text-success">{coupon.description}</strong>
                  <br />
                  <small className="text-muted">
                    Applied {new Date(coupon.appliedAt).toLocaleDateString()}
                  </small>
                </div>
                <div className="d-flex align-items-center gap-2">
                  <span className="text-success fw-bold">
                    −${coupon.discountAmount.toFixed(2)}
                  </span>
                  <form action={removeCoupon} className="d-inline">
                    <input type="hidden" name="couponId" value={coupon.couponId} />
                    <button
                      type="submit"
                      className="btn btn-outline-danger btn-sm"
                      title="Remove coupon"
                    >
                      <i className="bi bi-x-lg"></i>
                    </button>
                  </form>
                </div>
              </div>
            ))}
          </div>
        ) : (
          <div className="text-center text-muted py-3">
            <i className="bi bi-tag-fill fs-2 d-block mb-2 opacity-50"></i>
            <p className="mb-0">No coupons applied yet</p>
            <small>Enter a coupon code above to save on your order</small>
          </div>
        )}
      </div>
    </div>
  );
}
