
import { User } from '../../types/user';
import Link from 'next/link';
import {addCouponToUserAction} from '../../lib/actions/coupon-actions';
import { getAllUsers } from '../../lib/actions/coupon-actions';


export async function AddUserForm({ couponId }: { couponId: number }) {
  let users: User[];
  try {
    users = await getAllUsers();
  } catch (error) {
    users = [];
  }

  if (users.length === 0) {
    return (
      <div className="text-center py-5">
        <div className="mb-4">
          <i className="bi bi-people display-1 text-muted"></i>
        </div>
        <h3 className="text-muted">No Users Found</h3>
        <p className="text-muted">No users available to assign this coupon to.</p>
      </div>
    );
  }

  return (
    <form action={addCouponToUserAction} className="needs-validation" noValidate>
      <input type="hidden" name="couponId" value={couponId} />

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
        <div className="form-text">
          <i className="bi bi-info-circle me-1"></i>
          The selected user will receive access to this coupon.
        </div>
      </div>

      <div className="d-flex gap-2">
        <button type="submit" className="btn btn-success">
          <i className="bi bi-plus-circle me-2"></i>
          Add Coupon to User
        </button>
        <Link
          href={`/admin/coupons/${couponId}/users`}
          className="btn btn-outline-secondary"
        >
          <i className="bi bi-arrow-left me-1"></i>
          Cancel
        </Link>
      </div>
    </form>
  );
}