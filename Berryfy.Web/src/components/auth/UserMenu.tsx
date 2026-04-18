import { User } from '../../types/user';
import { logoutAction } from '../../lib/actions/auth-actions';

interface UserMenuProps {
  user: User;
}

export default function UserMenu({ user }: UserMenuProps) {
  const userRoles = user.roles || [];
  const isAdmin = userRoles.includes('Admin') || userRoles.includes('SuperAdmin');

  return (
    <div className="dropdown">
      <button
        className="btn btn-outline-light dropdown-toggle"
        type="button"
        data-bs-toggle="dropdown"
        aria-expanded="false"
      >
        <i className="bi bi-person-circle me-1"></i>
        <span className="d-none d-md-inline">{user.userName}</span>
      </button>
      <ul className="dropdown-menu dropdown-menu-end dropdown-menu-user">
        <li>
          <div className="dropdown-header">
            <div className="fw-bold">{user.userName}</div>
            <small className="text-muted">{user.email}</small>
          </div>
        </li>
        <li><hr className="dropdown-divider" /></li>
        
        <li>
          <a className="dropdown-item" href="/profile">
            <i className="bi bi-person me-2"></i>
            My Profile
          </a>
        </li>
        
        <li>
          <a className="dropdown-item" href="/orders">
            <i className="bi bi-box-seam me-2"></i>
            My Orders
          </a>
        </li>
        
        <li>
          <a className="dropdown-item" href="/wishlist">
            <i className="bi bi-heart me-2"></i>
            Wishlist
          </a>
        </li>

        {isAdmin && (
          <>
            <li><hr className="dropdown-divider" /></li>
            <li>
              <a className="dropdown-item" href="/admin">
                <i className="bi bi-gear me-2"></i>
                Admin Panel
              </a>
            </li>
          </>
        )}

        <li><hr className="dropdown-divider" /></li>
        
        <li>
          <form action={logoutAction}>
            <button type="submit" className="dropdown-item text-danger">
              <i className="bi bi-box-arrow-right me-2"></i>
              Sign Out
            </button>
          </form>
        </li>
      </ul>
    </div>
  );
} 