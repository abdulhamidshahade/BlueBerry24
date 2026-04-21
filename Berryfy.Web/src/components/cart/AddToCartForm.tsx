import { addToCart } from '../../lib/actions/cart-actions';

interface AddToCartFormProps {
  productId: number;
  availableStock: number;
  isInStock: boolean;
  showQuantitySelector?: boolean;
  buttonText?: string;
  buttonSize?: 'sm' | 'lg';
  className?: string;
}

export default function AddToCartForm({
  productId,
  availableStock,
  isInStock,
  showQuantitySelector = false,
  buttonText,
  buttonSize = 'lg',
  className = "btn btn-primary w-100"
}: AddToCartFormProps) {
  return (
    <form action={addToCart} className="w-100">
      <input type="hidden" name="productId" value={productId} />
      
      {showQuantitySelector && isInStock ? (
        <div className="row g-2 mb-3">
          <div className="col-4">
            <label htmlFor={`quantity-${productId}`} className="form-label">Quantity:</label>
            <input
              type="number"
              name="quantity"
              id={`quantity-${productId}`}
              className="form-control"
              defaultValue={1}
              min={1}
              max={20}
            />
          </div>
          <div className="col-8 d-flex align-items-end">
            <button 
              type="submit"
              className={`${className} ${buttonSize === 'lg' ? 'btn-lg' : buttonSize === 'sm' ? 'btn-sm' : ''}`}
              disabled={!isInStock}
            >
              <i className="bi bi-cart-plus me-1"></i>
              {buttonText || (!isInStock ? 'Out of Stock' : 'Add to Cart')}
            </button>
          </div>
        </div>
      ) : (
        <>
          <input type="hidden" name="quantity" value="1" />
          <button 
            type="submit"
            className={`${className} ${buttonSize === 'lg' ? 'btn-lg' : buttonSize === 'sm' ? 'btn-sm' : ''}`}
            disabled={!isInStock}
          >
            <i className="bi bi-cart-plus me-1"></i>
            {buttonText || (!isInStock ? 'Out of Stock' : 'Add to Cart')}
          </button>
        </>
      )}
    </form>
  );
} 