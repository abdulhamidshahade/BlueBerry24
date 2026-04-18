import { Suspense } from 'react';
import Link from 'next/link';
import { AddCouponToAllUsersForm } from '../../../../components/coupon/AddCouponToAllUsersForm';

interface SearchParams {
  error?: string;
}

interface PageProps {
  searchParams: Promise<SearchParams>;
}

function ErrorAlert({ message }: { message: string }) {
  return (
    <div className="alert alert-danger alert-dismissible fade show" role="alert">
      <i className="bi bi-exclamation-circle-fill me-2"></i>
      {decodeURIComponent(message)}
      <button type="button" className="btn-close" data-bs-dismiss="alert"></button>
    </div>
  );
}



export default async function AddCouponToAllUsersPage({ searchParams }: PageProps) {
  const { error } = await searchParams;

  return (
    <div className="container-fluid">
      <nav aria-label="breadcrumb" className="mb-4">
        <ol className="breadcrumb">
          <li className="breadcrumb-item">
            <Link href="/admin" className="text-decoration-none">
              <i className="bi bi-speedometer2 me-1"></i>Admin
            </Link>
          </li>
          <li className="breadcrumb-item">
            <Link href="/admin/coupons" className="text-decoration-none">
              <i className="bi bi-tag me-1"></i>Coupons
            </Link>
          </li>
          <li className="breadcrumb-item active" aria-current="page">
            <i className="bi bi-globe me-1"></i>Add to All Users
          </li>
        </ol>
      </nav>

      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h1 className="h2 mb-1">
            <i className="bi bi-globe me-2"></i>Add Coupon to All Users
          </h1>
          <p className="text-muted mb-0">Grant ALL users access to a specific coupon</p>
        </div>
        <Link 
          href="/admin/coupons" 
          className="btn btn-outline-secondary"
        >
          <i className="bi bi-arrow-left me-1"></i>Back to Coupons
        </Link>
      </div>

      {error && <ErrorAlert message={error} />}

      <div className="card shadow-sm">
        <div className="card-header bg-light">
          <h5 className="card-title mb-0">
            <i className="bi bi-globe me-2"></i>Add Coupon to All Users
          </h5>
        </div>
        <div className="card-body">
          <Suspense fallback={
            <div className="d-flex justify-content-center py-4">
              <div className="spinner-border text-primary" role="status">
                <span className="visually-hidden">Loading...</span>
              </div>
            </div>
          }>
            <AddCouponToAllUsersForm />
          </Suspense>
        </div>
      </div>
    </div>
  );
}

export const metadata = {
  title: 'Add Coupon to All Users - Admin Dashboard',
  description: 'Grant all users access to a coupon',
}; 