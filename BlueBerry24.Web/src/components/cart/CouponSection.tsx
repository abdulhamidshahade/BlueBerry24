import { CartCouponDto } from "../../types/cart";
import { applyCoupon, removeCoupon } from "../../lib/actions/cart-actions";

interface CouponSectionProps {
  appliedCoupons: CartCouponDto[];
  code: string;
}

export default function CouponSection({ appliedCoupons, code }: CouponSectionProps) {
  return (
    <div className="card mb-3">
      <div className="card-header">
        <h6 className="mb-0">
          <i className="bi bi-tag me-2"></i>
          Coupons & Discounts
        </h6>
      </div>
      <div className="card-body">
        <form action={applyCoupon} className="mb-3">
          <div className="row">
            <div className="col-md-8">
              <input
                type="text"
                name="couponCode"
                className="form-control"
                placeholder="Enter coupon code"
                defaultValue={code}
                required
              />
            </div>
            <div className="col-md-4">
              <button type="submit" className="btn btn-outline-primary w-100">
                <i className="bi bi-plus-circle me-1"></i>
                Apply Coupon
              </button>
            </div>
          </div>
        </form>

        {appliedCoupons.length > 0 && (
          <div>
            <h6 className="text-success mb-2">Applied Coupons:</h6>
            {appliedCoupons.map((coupon) => (
              <div
                key={coupon.id}
                className="d-flex justify-content-between align-items-center bg-light rounded p-2 mb-2"
              >
                <div>
                  <strong>{coupon.description}</strong>
                  <br />
                  <small className="text-muted">
                    Applied on {new Date(coupon.appliedAt).toLocaleDateString()}
                  </small>
                </div>
                <div className="d-flex align-items-center">
                  <span className="text-success fw-bold me-2">
                    -${coupon.discountAmount.toFixed(2)}
                  </span>
                  <form action={removeCoupon} className="d-inline">
                    <input type="hidden" name="couponId" value={coupon.couponId} />
                    <button
                      type="submit"
                      className="btn btn-outline-danger btn-sm"
                      title="Remove coupon"
                    >
                      <i className="bi bi-x"></i>
                    </button>
                  </form>
                </div>
              </div>
            ))}
          </div>
        )}

        {appliedCoupons.length === 0 && (
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