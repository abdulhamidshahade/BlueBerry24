import { quickAddToWishlist, quickRemoveFromWishlist, isProductInWishlist } from '@/lib/actions/wishlist-actions';
import { getCurrentUser } from '@/lib/actions/auth-actions';

interface WishlistButtonProps {
  productId: number;
  size?: 'sm' | 'md' | 'lg';
  variant?: 'button' | 'icon';
  className?: string;
  returnUrl?: string;
}

export default async function WishlistButton({ 
  productId, 
  size = 'md', 
  variant = 'button',
  className = '',
  returnUrl
}: WishlistButtonProps) {
  
  const user = await getCurrentUser();
  
  if (!user) {
    return (
      <a 
        href={`/auth/login?redirectTo=${returnUrl || `/products/${productId}`}`}
        className={`btn btn-outline-secondary ${getSizeClass(size)} ${className}`}
        title="Login to add to wishlist"
      >
        {variant === 'icon' ? (
          <i className="bi bi-heart"></i>
        ) : (
          <>
            <i className="bi bi-heart me-1"></i>
            Add to Wishlist
          </>
        )}
      </a>
    );
  }

  const inWishlist = await isProductInWishlist(productId);

  if (inWishlist) {
    return (
      <form action={quickRemoveFromWishlist} style={{ display: 'inline' }}>
        <input type="hidden" name="productId" value={productId} />
        {returnUrl && <input type="hidden" name="returnUrl" value={returnUrl} />}
        <button 
          type="submit" 
          className={`btn btn-success ${getSizeClass(size)} ${className}`}
          title="Remove from wishlist"
        >
          {variant === 'icon' ? (
            <i className="bi bi-heart-fill"></i>
          ) : (
            <>
              <i className="bi bi-heart-fill me-1"></i>
              Remove from Wishlist
            </>
          )}
        </button>
      </form>
    );
  }

  return (
    <form action={quickAddToWishlist} style={{ display: 'inline' }}>
      <input type="hidden" name="productId" value={productId} />
      {returnUrl && <input type="hidden" name="returnUrl" value={returnUrl} />}
      <button 
        type="submit" 
        className={`btn btn-outline-danger ${getSizeClass(size)} ${className}`}
        title="Add to wishlist"
      >
        {variant === 'icon' ? (
          <i className="bi bi-heart"></i>
        ) : (
          <>
            <i className="bi bi-heart me-1"></i>
            Add to Wishlist
          </>
        )}
      </button>
    </form>
  );
}

function getSizeClass(size: 'sm' | 'md' | 'lg'): string {
  switch (size) {
    case 'sm':
      return 'btn-sm';
    case 'lg':
      return 'btn-lg';
    default:
      return '';
  }
} 