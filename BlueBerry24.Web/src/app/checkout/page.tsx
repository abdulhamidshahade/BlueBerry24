import { getCart } from '../../lib/actions/cart-actions';
import CheckoutForm from '../../components/checkout/CheckoutForm';
import Link from 'next/link';
import { redirect } from 'next/navigation';

interface CheckoutPageProps {
  searchParams: Promise<{ [key: string]: string | string[] | undefined }>;
}

export const dynamic = 'force-dynamic';

export default async function CheckoutPage({ searchParams }: CheckoutPageProps) {
  const cart = await getCart();

  if (!cart || !cart.cartItems || cart.cartItems.length === 0) {
    redirect('/cart');
  }

  var resolvedSearchParams = await searchParams;

  const error = resolvedSearchParams?.error as string;

  return (
    <div className="container py-4">
      <div className="row">
        <div className="col-12">
          <nav aria-label="breadcrumb">
            <ol className="breadcrumb">
              <li className="breadcrumb-item">
                <Link href="/">Home</Link>
              </li>
              <li className="breadcrumb-item">
                <Link href="/products">Products</Link>
              </li>
              <li className="breadcrumb-item">
                <Link href="/cart">Cart</Link>
              </li>
              <li className="breadcrumb-item active" aria-current="page">
                Checkout
              </li>
            </ol>
          </nav>
        </div>
      </div>

      <div className="row">
        <div className="col-12">
          <h1 className="mb-4">
            <i className="bi bi-credit-card me-2"></i>
            Checkout
          </h1>
        </div>
      </div>

      {error && (
        <div className="row">
          <div className="col-12">
            <div className="alert alert-danger d-flex align-items-center mb-4" role="alert">
              <i className="bi bi-exclamation-triangle-fill me-2"></i>
              <div>{error}</div>
              <Link 
                href="/checkout" 
                className="btn btn-sm btn-outline-secondary ms-auto"
              >
                <i className="bi bi-x me-1"></i>
                Dismiss
              </Link>
            </div>
          </div>
        </div>
      )}

      <CheckoutForm cart={cart} searchParams={resolvedSearchParams} />
    </div>
  );
} 