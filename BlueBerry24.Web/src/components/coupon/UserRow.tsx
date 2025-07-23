import {hasUserUsedCoupon, disableUserCouponAction } from '../../lib/actions/coupon-actions';

export async function UserRow({ user, couponId, couponCode }: { user: any; couponId: number; couponCode: string }) {
  const hasUsed = await hasUserUsedCoupon(user.id, couponCode);

  return (
    <tr>
      <td>
        <div className="d-flex align-items-center">
          <div className="avatar-sm me-3">
            <div className="bg-primary rounded-circle d-flex align-items-center justify-content-center text-white" style={{ width: '32px', height: '32px' }}>
              <i className="bi bi-person"></i>
            </div>
          </div>
          <div>
            <strong>{user.firstName} {user.lastName}</strong>
            <small className="d-block text-muted">@{user.userName}</small>
          </div>
        </div>
      </td>
      <td>
        <span className="text-muted">{user.email}</span>
      </td>
      <td>
        {hasUsed ? (
          <span className="badge bg-success">
            <i className="bi bi-check-circle me-1"></i>
            Used
          </span>
        ) : (
          <span className="badge bg-warning text-dark">
            <i className="bi bi-clock me-1"></i>
            Available
          </span>
        )}
      </td>
      <td>
        <div className="btn-group btn-group-sm" role="group">
          {!hasUsed && (
            <form action={disableUserCouponAction} className="d-inline">
              <input type="hidden" name="userId" value={user.id} />
              <input type="hidden" name="couponId" value={couponId} />
              <button
                type="submit"
                className="btn btn-outline-danger"
                title="Disable Coupon for User"
              >
                <i className="bi bi-x-circle"></i>
                <span className="d-none d-lg-inline ms-1">Disable</span>
              </button>
            </form>
          )}
        </div>
      </td>
    </tr>
  );
}