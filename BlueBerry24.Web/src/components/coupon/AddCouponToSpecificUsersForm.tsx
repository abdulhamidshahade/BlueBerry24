import Link from "next/link";
import { getCoupons, addCouponToSpecificUsersAction } from '../../lib/actions/coupon-actions';
import { User } from "../../types/user";
import { CouponDto } from '../../types/coupon';
import { getAllUsers } from "../../lib/actions/coupon-actions";

export async function AddCouponToSpecificUsersForm() {
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
    <>
      <form action={addCouponToSpecificUsersAction} className="needs-validation" noValidate>
        <div className="row">
          <div className="col-12">
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
        </div>

        <div className="row">
          <div className="col-12">
            <div className="mb-4">
              <label className="form-label">
                <i className="bi bi-people me-1"></i>
                Select Users <span className="text-danger">*</span>
              </label>
              <div className="form-text mb-3">
                <i className="bi bi-info-circle me-1"></i>
                Select multiple users by checking their boxes.
              </div>
              <div style={{ maxHeight: '300px', overflowY: 'auto' }} className="border rounded p-3">
                {/* <input type="hidden" name="userIds" id="userIds" value="" /> */}
                <div className="row g-2">
                  {users.map((user: User) => (
                    <div key={user.id} className="col-md-6 col-lg-4">
                      <div className="form-check">
                        <input 
                          className="form-check-input user-checkbox" 
                          type="checkbox" 
                          value={user.id} 
                          id={`user-${user.id}`}
                          data-user-id={user.id}
                          name='userIds'
                        />
                        <label className="form-check-label" htmlFor={`user-${user.id}`}>
                          <div>
                            <strong>{user.firstName} {user.lastName}</strong>
                            <small className="d-block text-muted">{user.email}</small>
                          </div>
                        </label>
                      </div>
                    </div>
                  ))}
                </div>
              </div>
            </div>
          </div>
        </div>

        <div className="alert alert-info">
          <i className="bi bi-info-circle me-2"></i>
          <strong>Note:</strong> The selected coupon will be added to all selected users. 
          Users who already have this coupon will be skipped.
        </div>

        <div className="d-flex gap-2">
          <button type="submit" className="btn btn-success btn-lg">
            <i className="bi bi-people-fill me-2"></i>
            Add Coupon to Selected Users
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
    </>
  );
}
