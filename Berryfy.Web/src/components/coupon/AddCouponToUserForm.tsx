import { getCoupons, addCouponToUserAction } from '../../lib/actions/coupon-actions';
import { User } from '../../types/user';
import { CouponDto } from '../../types/coupon';
import Link from 'next/link';
import { getAllUsers } from '../../lib/actions/coupon-actions';

export async function AddCouponToUserForm() {
  let users: User[] = [];
  let coupons: CouponDto[] = [];
  
  try {
    [users, coupons] = await Promise.all([
      getAllUsers(),
      getCoupons()
    ]);
  } catch (error) {
    console.error('Error loading data:', error);
  }

  if (users.length === 0 || coupons.length === 0) {
    return (
      <div className="text-center py-5">
        <div className="mb-4">
          <i className="bi bi-exclamation-triangle display-1 text-warning"></i>
        </div>
        <h3 className="text-muted">Unable to Load Data</h3>
        <p className="text-muted">
          {users.length === 0 && coupons.length === 0 && 'No users or coupons available.'}
          {users.length === 0 && coupons.length > 0 && 'No users available.'}
          {users.length > 0 && coupons.length === 0 && 'No coupons available.'}
        </p>
      </div>
    );
  }

  return (
    <form action={addCouponToUserAction} className="needs-validation" noValidate>
      <div className="row">
        <div className="col-md-6">
          <div className="mb-4">
            <label htmlFor="couponId" className="form-label">
              <i className="bi bi-tag me-1"></i>
              Select Coupon <span className="text-danger">*</span>
            </label>
            <select 
              className="form-select form-select-lg" 
              id="couponId" 
              name="couponId" 
              required
            >
              <option value="">Choose a coupon...</option>
              {coupons.map((coupon: CouponDto) => (
                <option key={coupon.id} value={coupon.id}>
                  {coupon.code} - {coupon.description}
                </option>
              ))}
            </select>
            <div className="invalid-feedback">
              Please select a coupon.
            </div>
          </div>
        </div>

        <div className="col-md-6">
          <div className="mb-4">
            <label htmlFor="userId" className="form-label">
              <i className="bi bi-person me-1"></i>
              Select User <span className="text-danger">*</span>
            </label>
            <select 
              className="form-select form-select-lg" 
              id="userId" 
              name="userId" 
              required
            >
              <option value="">Choose a user...</option>
              {users.map((user: User) => (
                <option key={user.id} value={user.id}>
                  {user.firstName} {user.lastName} ({user.email})
                </option>
              ))}
            </select>
            <div className="invalid-feedback">
              Please select a user.
            </div>
          </div>
        </div>
      </div>

      <div className="form-text mb-4">
        <i className="bi bi-info-circle me-1"></i>
        The selected user will receive access to the selected coupon.
      </div>

      <div className="d-flex gap-2">
        <button type="submit" className="btn btn-success btn-lg">
          <i className="bi bi-plus-circle me-2"></i>
          Add Coupon to User
        </button>
        <Link 
          href="/admin/coupons" 
          className="btn btn-outline-secondary btn-lg"
        >
          <i className="bi bi-arrow-left me-1"></i>
          Cancel
        </Link>
      </div>
    </form>
  );
}
