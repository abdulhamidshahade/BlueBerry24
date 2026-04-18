import { getCoupons, addCouponToAllUsersAction } from '../../lib/actions/coupon-actions';

import { User } from '../../types/user';
import { CouponDto } from '../../types/coupon';
import Link from 'next/link';
import { getAllUsers } from '../../lib/actions/coupon-actions';

export async function AddCouponToAllUsersForm() {
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
    <form action={addCouponToAllUsersAction} className="needs-validation" noValidate>
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

      <div className="card mb-4">
        <div className="card-header bg-info text-white">
          <h5 className="card-title mb-0">
            <i className="bi bi-info-circle me-2"></i>User Overview
          </h5>
        </div>
        <div className="card-body">
          <div className="row">
            <div className="col-md-4">
              <div className="text-center">
                <h3 className="text-primary">{users.length}</h3>
                <p className="text-muted mb-0">Total Users</p>
              </div>
            </div>
            <div className="col-md-8">
              <h6>Users that will receive this coupon:</h6>
              <div style={{ maxHeight: '200px', overflowY: 'auto' }} className="border rounded p-2">
                <div className="row g-2">
                  {users.slice(0, 20).map((user: User) => (
                    <div key={user.id} className="col-md-6">
                      <div className="d-flex align-items-center">
                        <i className="bi bi-person-check text-success me-2"></i>
                        <div>
                          <small className="fw-semibold">{user.firstName} {user.lastName}</small>
                          <br />
                          <small className="text-muted">{user.email}</small>
                        </div>
                      </div>
                    </div>
                  ))}
                  {users.length > 20 && (
                    <div className="col-12">
                      <small className="text-muted">... and {users.length - 20} more users</small>
                    </div>
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="alert alert-warning">
        <i className="bi bi-exclamation-triangle me-2"></i>
        <strong>Warning:</strong> This action will add the selected coupon to ALL {users.length} users in the system. 
        Users who already have this coupon will be skipped. This action cannot be undone easily.
      </div>

      <div className="d-flex gap-2">
        <button type="submit" className="btn btn-warning btn-lg">
          <i className="bi bi-globe me-2"></i>
          Add Coupon to All {users.length} Users
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