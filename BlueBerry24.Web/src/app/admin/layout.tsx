import Link from 'next/link';
import { Suspense } from 'react';
import { redirect } from 'next/navigation';
import { getCurrentUser } from '../../lib/actions/auth-actions';
import MiniCart from '../../components/cart/MiniCart';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap-icons/font/bootstrap-icons.css';


interface AdminLayoutProps {
  children: React.ReactNode;
}

export default async function AdminLayout({ children }: AdminLayoutProps) {

  const user = await getCurrentUser();
  
  if (!user) {
    redirect('/auth/login?redirectTo=/admin');
  }

  const userRoles = user.roles || [];
  const hasAdminRole = userRoles.some(role => 
    role.toLowerCase().includes('admin') || 
    role.toLowerCase().includes('superadmin')
  );

  if (!hasAdminRole) {
    redirect('/?error=' + encodeURIComponent('You do not have permission to access the admin area'));
  }

  return (
    <div className="d-flex vh-100">
      <nav className="bg-dark text-white" style={{ width: '280px', minHeight: '100vh' }}>
        <div className="d-flex flex-column h-100">
          <div className="p-3 border-bottom border-secondary">
            <Link href="/admin" className="text-white text-decoration-none">
              <h4 className="mb-0">
                <i className="bi bi-speedometer2 me-2"></i>
                Admin Dashboard
              </h4>
            </Link>
          </div>

          <div className="flex-grow-1 overflow-auto">
            <ul className="nav nav-pills flex-column p-3">
              <li className="nav-item mb-1">
                <Link href="/admin" className="nav-link text-white">
                  <i className="bi bi-house me-2"></i>
                  Dashboard
                </Link>
              </li>
              
              <li className="nav-item mb-1">
                <h6 className="text-white small text-uppercase mt-3 mb-2 opacity-50">
  Catalog Management
</h6>
              </li>
              
              <li className="nav-item mb-1">
                <Link href="/admin/products" className="nav-link text-white">
                  <i className="bi bi-box me-2"></i>
                  Products
                </Link>
              </li>
              
              <li className="nav-item mb-1">
                <Link href="/admin/categories" className="nav-link text-white">
                  <i className="bi bi-tags me-2"></i>
                  Categories
                </Link>
              </li>
              
              <li className="nav-item mb-1">
                <Link href="/admin/inventory" className="nav-link text-white">
                  <i className="bi bi-boxes me-2"></i>
                  Inventory
                </Link>
              </li>

              <li className="nav-item mb-1">
                <h6 className="text-white small text-uppercase mt-3 mb-2 opacity-50">
                  Sales & Marketing
                </h6>
              </li>
              
              <li className="nav-item mb-1">
                <Link href="/admin/orders" className="nav-link text-white">
                  <i className="bi bi-receipt me-2"></i>
                  Orders
                </Link>
              </li>
              
              <li className="nav-item mb-1">
                <Link href="/admin/payments" className="nav-link text-white">
                  <i className="bi bi-credit-card me-2"></i>
                  Payments
                </Link>
              </li>
              
              <li className="nav-item mb-1">
                <Link href="/admin/coupons" className="nav-link text-white">
                  <i className="bi bi-tag me-2"></i>
                  Coupons
                </Link>
              </li>
              
              <li className="nav-item mb-1">
                <Link href="/admin/customers" className="nav-link text-white">
                  <i className="bi bi-people me-2"></i>
                  Customers
                </Link>
              </li>

              <li className="nav-item mb-1">
                <h6 className="text-white small text-uppercase mt-3 mb-2 opacity-50">
                  Analytics & Reports
                </h6>
              </li>
              
              <li className="nav-item mb-1">
                <Link href="/admin/analytics" className="nav-link text-white">
                  <i className="bi bi-graph-up me-2"></i>
                  Analytics
                </Link>
              </li>
              
              <li className="nav-item mb-1">
                <Link href="/admin/reports" className="nav-link text-white">
                  <i className="bi bi-file-earmark-text me-2"></i>
                  Reports
                </Link>
              </li>

              <li className="nav-item mb-1">
                <Link href="/admin/Traffic" className="nav-link text-white">
                  <i className="bi bi-hdd-network-fill me-2"></i>
                  Traffic
                </Link>
              </li>

              <li className="nav-item mb-1">
                <h6 className="text-white small text-uppercase mt-3 mb-2 opacity-50">
                  System
                </h6>
              </li>
              
              <li className="nav-item mb-1">
                <Link href="/admin/users" className="nav-link text-white">
                  <i className="bi bi-person-gear me-2"></i>
                  Users
                </Link>
              </li>
              
              <li className="nav-item mb-1">
                <Link href="/admin/role-management" className="nav-link text-white">
                  <i className="bi bi-shield-lock me-2"></i>
                  Role Management
                </Link>
              </li>
              
              <li className="nav-item mb-1">
                <Link href="/admin/settings" className="nav-link text-white">
                  <i className="bi bi-gear me-2"></i>
                  Settings
                </Link>
              </li>
            </ul>
          </div>

          <div className="p-3 border-top border-secondary">
            <div className="d-flex align-items-center justify-content-between">
              <Link href="/" className="btn btn-outline-light btn-sm">
                <i className="bi bi-arrow-left me-1"></i>
                Back to Site
              </Link>
              <Suspense fallback={<div className="btn btn-outline-light btn-sm">Cart</div>}>
                <MiniCart />
              </Suspense>
            </div>
          </div>
        </div>
      </nav>

      <div className="flex-grow-1 d-flex flex-column">
        <header className="bg-white border-bottom px-4 py-3">
          <div className="d-flex justify-content-between align-items-center">
            <h5 className="mb-0 text-muted">Admin Panel</h5>
            <div className="d-flex align-items-center gap-3">
              <span className="badge bg-primary">Online</span>
              <div className="dropdown">
                <button className="btn btn-outline-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown">
                  <i className="bi bi-person-circle me-1"></i>
                  {user.firstName ? `${user.firstName} ${user.lastName}` : user.userName}
                </button>
                <ul className="dropdown-menu">
                  <li><Link className="dropdown-item" href="/profile">Profile</Link></li>
                  <li><Link className="dropdown-item" href="/admin/settings">Settings</Link></li>
                  <li><hr className="dropdown-divider" /></li>
                  <li>
                    <form action="/api/auth/logout" method="post">
                      <button type="submit" className="dropdown-item">Logout</button>
                    </form>
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </header>

        <main className="flex-grow-1 p-4 bg-light overflow-auto">
          {children}
        </main>
      </div>
    </div>
  );
} 