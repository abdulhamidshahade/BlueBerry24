import { CartDto, CartStatus } from "../../types/cart";
import { proceedToCheckout } from "../../lib/actions/cart-actions";
import ClearCartButton from "./ClearCartButton";

interface CartSummaryProps {
  cart: CartDto;
}

export default function CartSummary({ cart }: CartSummaryProps) {
  const hasItems = cart.cartItems && cart.cartItems.length > 0;
  const itemCount = cart.cartItems?.reduce((sum, item) => sum + item.quantity, 0) || 0;
  const isActiveCart = cart.status === CartStatus.Active;

  return (
    <div className="card sticky-top">
      <div className="card-header">
        <h5 className="mb-0">
          <i className="bi bi-receipt me-2"></i>
          Order Summary
        </h5>
      </div>
      <div className="card-body">
        <div className="d-flex justify-content-between mb-2">
          <span>Items ({itemCount}):</span>
          <span>${cart.subTotal.toFixed(2)}</span>
        </div>

        {cart.discountTotal > 0 && (
          <div className="d-flex justify-content-between mb-2">
            <span className="text-success">Discounts:</span>
            <span className="text-success">-${cart.discountTotal.toFixed(2)}</span>
          </div>
        )}

        <div className="d-flex justify-content-between mb-2">
          <span>Tax:</span>
          <span>${cart.taxAmount.toFixed(2)}</span>
        </div>

        <hr />

        <div className="d-flex justify-content-between mb-3">
          <strong>Total:</strong>
          <strong className="text-primary fs-5">${cart.total.toFixed(2)}</strong>
        </div>

        {hasItems ? (
          <div className="d-grid gap-2">
            {isActiveCart ? (
              <>
                <form action={proceedToCheckout}>
                  <button type="submit" className="btn btn-primary btn-lg w-100">
                    <i className="bi bi-credit-card me-2"></i>
                    Proceed to Checkout
                  </button>
                </form>
                
                <ClearCartButton />
              </>
            ) : (
              <div className="text-center">
                <button type="button" className="btn btn-secondary btn-lg w-100" disabled>
                  <i className="bi bi-lock me-2"></i>
                  Cart Not Active
                </button>
                <small className="text-muted mt-2 d-block">
                  This cart cannot be modified or checked out
                </small>
              </div>
            )}
          </div>
        ) : (
          <div className="text-center text-muted py-3">
            <i className="bi bi-cart-x fs-2 d-block mb-2 opacity-50"></i>
            <p className="mb-0">Your cart is empty</p>
            <small>Add some products to get started</small>
          </div>
        )}

        {cart.discountTotal > 0 && (
          <div className="mt-3 p-2 bg-success bg-opacity-10 rounded">
            <div className="text-center">
              <i className="bi bi-piggy-bank-fill text-success me-1"></i>
              <span className="text-success fw-bold">
                You're saving ${cart.discountTotal.toFixed(2)}!
              </span>
            </div>
          </div>
        )}

        {cart.cartCoupons && cart.cartCoupons.length > 0 && (
          <div className="mt-3">
            <small className="text-muted">Applied coupons:</small>
            {cart.cartCoupons.map((coupon) => (
              <div key={coupon.id} className="d-flex justify-content-between">
                <small className="text-success">{coupon.description}</small>
                <small className="text-success">-${coupon.discountAmount.toFixed(2)}</small>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
} 