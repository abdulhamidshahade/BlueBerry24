import Link from 'next/link';
import MiniCart from '@/components/cart/MiniCart';
import UserMenu from '@/components/auth/UserMenu';
import { getCurrentUser } from '@/lib/actions/auth-actions';
import { Suspense } from 'react';

function MiniCartFallback() {
  return (
    <Link href="/cart" className="btn btn-outline-primary">
      <i className="bi bi-cart3"></i>
      <span className="d-none d-md-inline ms-2">Cart</span>
    </Link>
  );
}

export default async function Navigation() {
  const user = await getCurrentUser();

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-primary">
      <div className="container">
        <Link className="navbar-brand fw-bold" href="/">
          <i className="bi bi-shop me-2"></i>
          BlueBerry24
        </Link>

        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
          aria-controls="navbarNav"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>

        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav me-auto">
            <li className="nav-item">
              <Link className="nav-link" href="/">
                <i className="bi bi-house me-1"></i>
                Home
              </Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" href="/products">
                <i className="bi bi-box-seam me-1"></i>
                Products
              </Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" href="/categories">
                <i className="bi bi-grid me-1"></i>
                Categories
              </Link>
            </li>
          </ul>

          <div className="d-flex gap-2 align-items-center">
            <Suspense fallback={<MiniCartFallback />}>
              <MiniCart />
            </Suspense>
            
            <Link href="/cart" className="btn btn-outline-light">
              <i className="bi bi-cart3 me-1"></i>
              <span className="d-none d-lg-inline">View Cart</span>
            </Link>

            {user ? (
              <UserMenu user={user} />
            ) : (
              <div className="d-flex gap-2">
                <Link href="/auth/login" className="btn btn-outline-light">
                  <i className="bi bi-box-arrow-in-right me-1"></i>
                  <span className="d-none d-md-inline">Sign In</span>
                </Link>
                <Link href="/auth/register" className="btn btn-light">
                  <i className="bi bi-person-plus me-1"></i>
                  <span className="d-none d-md-inline">Sign Up</span>
                </Link>
              </div>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
} 