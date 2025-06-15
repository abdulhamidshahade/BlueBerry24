import { getCart } from '@/lib/actions/cart-actions';
import Link from 'next/link';

export default async function MiniCart() {
  try {
    const cart = await getCart();
    
    if (!cart) {
      return (
        <Link href="/cart" className="btn btn-outline-light position-relative">
          <i className="bi bi-cart3"></i>
          <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
            0
          </span>
          <span className="d-none d-md-inline ms-2">Cart</span>
        </Link>
      );
    }

    const itemCount = cart.cartItems?.reduce((sum, item) => sum + item.quantity, 0) || 0;
    const hasItems = itemCount > 0;

    return (
      <Link href="/cart" className="btn btn-outline-light position-relative">
        <i className="bi bi-cart3"></i>
        {hasItems && (
          <span className="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger">
            {itemCount > 99 ? '99+' : itemCount}
          </span>
        )}
        <span className="d-none d-md-inline ms-2">
          Cart{hasItems && ` (${itemCount})`}
        </span>
      </Link>
    );
  } catch (error) {
    console.error('Error loading mini cart:', error);
    return (
      <Link href="/cart" className="btn btn-outline-light">
        <i className="bi bi-cart3"></i>
        <span className="d-none d-md-inline ms-2">Cart</span>
      </Link>
    );
  }
} 