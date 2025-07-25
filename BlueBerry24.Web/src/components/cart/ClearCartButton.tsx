import { clearCart } from "../../lib/actions/cart-actions";
import Link from "next/link";

interface ClearCartButtonProps {
  showConfirm?: boolean;
}

export default function ClearCartButton({ showConfirm = false }: ClearCartButtonProps) {
  if (showConfirm) {
    return (
      <div className="card border-danger">
        <div className="card-body text-center">
          <h5 className="card-title text-danger">
            <i className="bi bi-exclamation-triangle me-2"></i>
            Clear Cart?
          </h5>
          <p className="card-text">
            Are you sure you want to remove all items from your cart? This action cannot be undone.
          </p>
          <div className="d-flex gap-2 justify-content-center">
            <form action={clearCart} className="d-inline">
              <button type="submit" className="btn btn-danger">
                <i className="bi bi-trash me-2"></i>
                Yes, Clear Cart
              </button>
            </form>
            <Link href="/cart" className="btn btn-secondary">
              Cancel
            </Link>
          </div>
        </div>
      </div>
    );
  }

  return (
    <Link href="/cart?confirm_clear=true" className="btn btn-outline-danger w-100">
      <i className="bi bi-trash me-2"></i>
      Clear Cart
    </Link>
  );
} 