import { CartItemDto } from "@/types/cart";
import { ProductDto } from "@/types/product";
import { removeFromCart } from "@/lib/actions/cart-actions";
import QuantitySelector from "./QuantitySelector";

interface CartItemProps {
  item: CartItemDto;
  product: ProductDto;
  disabled?: boolean;
}

export default function CartItem({ item, product, disabled = false }: CartItemProps) {
  const itemTotal = item.quantity * item.unitPrice;
  
  const isOutOfStock = product.stockQuantity <= product.reservedStock;
  const maxQuantity = product.stockQuantity - product.reservedStock;

  return (
    <div className={`card mb-3 ${disabled ? 'opacity-75' : ''}`}>
      <div className="card-body">
        <div className="row align-items-center">
          <div className="col-md-2">
            <div className="d-flex justify-content-center">
              {product.imageUrl ? (
                <img
                  src={product.imageUrl}
                  alt={product.name}
                  className="img-fluid rounded"
                  style={{ maxHeight: '80px', objectFit: 'cover' }}
                />
              ) : (
                <div
                  className="bg-light rounded d-flex align-items-center justify-content-center"
                  style={{ width: '80px', height: '80px' }}
                >
                  <i className="bi bi-image text-muted"></i>
                </div>
              )}
            </div>
          </div>

          <div className="col-md-4">
            <h6 className="card-title mb-1">{product.name}</h6>
            <p className="text-muted small mb-1">SKU: {product.sku}</p>
            <p className="text-primary fw-bold mb-0">${item.unitPrice.toFixed(2)}</p>
            {isOutOfStock && (
              <span className="badge bg-danger small">Out of Stock</span>
            )}
            {disabled && (
              <span className="badge bg-secondary small ms-1">Read Only</span>
            )}
          </div>

          <div className="col-md-3">
            {disabled ? (
              <div>
                <span className="fw-bold">Quantity: {item.quantity}</span>
                <br />
                <small className="text-muted">
                  Cart is not active
                </small>
              </div>
            ) : (
              <>
                <QuantitySelector
                  productId={item.productId}
                  currentQuantity={item.quantity}
                  maxQuantity={maxQuantity}
                  isOutOfStock={isOutOfStock}
                />
                <small className="text-muted">
                  Max: {maxQuantity} available
                </small>
              </>
            )}
          </div>

          <div className="col-md-2">
            <div className="text-end">
              <p className="fw-bold mb-0">${itemTotal.toFixed(2)}</p>
              <small className="text-muted">
                {item.quantity} × ${item.unitPrice.toFixed(2)}
              </small>
            </div>
          </div>

          <div className="col-md-1">
            {disabled ? (
              <div className="text-center">
                <i className="bi bi-lock text-muted" title="Cart is not active"></i>
              </div>
            ) : (
              <form action={removeFromCart} className="d-inline">
                <input type="hidden" name="productId" value={item.productId} />
                <button
                  type="submit"
                  className="btn btn-outline-danger btn-sm"
                  title="Remove item"
                >
                  <i className="bi bi-trash"></i>
                </button>
              </form>
            )}
          </div>
        </div>

        {!disabled && product.stockQuantity <= product.lowStockThreshold && !isOutOfStock && (
          <div className="row mt-2">
            <div className="col">
              <div className="alert alert-warning alert-sm py-2 mb-0">
                <i className="bi bi-exclamation-triangle me-1"></i>
                Only {product.stockQuantity - product.reservedStock} left in stock!
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
} 