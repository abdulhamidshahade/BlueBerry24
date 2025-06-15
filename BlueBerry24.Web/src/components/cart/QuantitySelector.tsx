import { updateCartItem } from "@/lib/actions/cart-actions";

interface QuantitySelectorProps {
  productId: number;
  currentQuantity: number;
  maxQuantity: number;
  isOutOfStock: boolean;
}

export default function QuantitySelector({
  productId,
  currentQuantity,
  maxQuantity,
  isOutOfStock
}: QuantitySelectorProps) {
  return (
    <div className="d-flex align-items-center">
      <form action={updateCartItem} className="d-inline">
        <input type="hidden" name="productId" value={productId} />
        <button
          type="submit"
          name="quantity"
          value={Math.max(0, currentQuantity - 1)}
          className="btn btn-outline-secondary btn-sm"
          disabled={currentQuantity <= 1}
          title="Decrease quantity"
        >
          <i className="bi bi-dash"></i>
        </button>
      </form>

      <span className="mx-3 fw-bold text-center" style={{ minWidth: '2rem' }}>
        {currentQuantity}
      </span>

      <form action={updateCartItem} className="d-inline">
        <input type="hidden" name="productId" value={productId} />
        <button
          type="submit"
          name="quantity"
          value={currentQuantity + 1}
          className="btn btn-outline-secondary btn-sm"
          disabled={currentQuantity >= maxQuantity || isOutOfStock}
          title="Increase quantity"
        >
          <i className="bi bi-plus"></i>
        </button>
      </form>
    </div>
  );
} 