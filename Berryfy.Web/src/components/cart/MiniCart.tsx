import { getCart } from '../../lib/actions/cart-actions';
import Link from 'next/link';
import { formatCurrency } from '../../lib/utils/formatCurrency';

export default async function MiniCart() {
  try {
    const cart = await getCart();
    
    if (!cart) {
      return (
        <Link href="/cart" className="btn btn-outline-light d-inline-flex align-items-center gap-2">
          <span className="position-relative d-inline-flex flex-column align-items-center lh-1">
            <i className="bi bi-cart3"></i>
            <small className="mt-1 fw-semibold">$0.00</small>
            <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
              0
            </span>
          </span>
          <span className="d-none d-md-inline ms-2">Cart</span>
        </Link>
      );
    }

    const itemCount = cart.cartItems?.reduce((sum, item) => sum + item.quantity, 0) || 0;
    const hasItems = itemCount > 0;
    const totalPrice = formatCurrency(cart.total || 0);

    return (
      <Link href="/cart" className="btn btn-outline-light d-inline-flex align-items-center gap-2">
        <span className="position-relative d-inline-flex flex-column align-items-center lh-1">
          <i className="bi bi-cart3"></i>
          <small className="mt-1 fw-semibold">{totalPrice}</small>
          {hasItems && (
            <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
              {itemCount > 99 ? '99+' : itemCount}
            </span>
          )}
        </span>
        <span className="d-none d-md-inline ms-2">
          Cart{hasItems && ` (${itemCount})`}
        </span>
      </Link>
    );
  } catch (error) {
    console.error('Error loading mini cart:', error);
    return (
      <Link href="/cart" className="btn btn-outline-light d-inline-flex align-items-center gap-2">
        <span className="d-inline-flex flex-column align-items-center lh-1">
          <i className="bi bi-cart3"></i>
          <small className="mt-1 fw-semibold">$0.00</small>
        </span>
        <span className="d-none d-md-inline ms-2">Cart</span>
      </Link>
    );
  }
} 