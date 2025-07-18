import { notFound } from 'next/navigation';
import Link from 'next/link';
import { getProduct } from '../../../lib/actions/product-actions';
import AddToCartForm from '../../../components/cart/AddToCartForm';
import WishlistButton from '../../../components/product/WishlistButton';

interface PageProps {
  params: Promise<{ id: string }>;
}

export default async function ProductDetailPage({ params }: PageProps) {
  const id = await params;
  const productId = parseInt(id.id);
  
  if (isNaN(productId)) {
    notFound();
  }

  const product = await getProduct(productId);

  if (!product || !product.isActive) {
    notFound();
  }

  const stockStatus = product.stockQuantity <= product.lowStockThreshold ? 'danger' : 
                     product.stockQuantity <= product.lowStockThreshold * 2 ? 'warning' : 'success';
  
  const availableStock = product.stockQuantity - product.reservedStock;
  const isInStock = availableStock > 0;

  return (
    <div className="container py-4">
      <nav aria-label="breadcrumb" className="mb-4">
        <ol className="breadcrumb">
          <li className="breadcrumb-item">
            <a href="/" className="text-decoration-none">
              <i className="bi bi-house-door me-1"></i>Home
            </a>
          </li>
          <li className="breadcrumb-item">
            <a href="/products" className="text-decoration-none">
              <i className="bi bi-box-seam me-1"></i>Products
            </a>
          </li>
          <li className="breadcrumb-item active" aria-current="page">
            {product.name}
          </li>
        </ol>
      </nav>

      <div className="row">
        <div className="col-lg-6 mb-4">
          <div className="card shadow-sm">
            <div className="position-relative">
              {product.imageUrl ? (
                <img 
                  src={product.imageUrl} 
                  className="card-img-top"
                  alt={product.name}
                  style={{ height: '500px', objectFit: 'contain' }}
                />
              ) : (
                <div 
                  className="card-img-top bg-light d-flex align-items-center justify-content-center"
                  style={{ height: '500px' }}
                >
                  <i className="bi bi-image display-1 text-muted"></i>
                </div>
              )}
              
              {!isInStock && (
                <div className="position-absolute top-0 start-0 w-100 h-100 d-flex align-items-center justify-content-center bg-dark bg-opacity-50">
                  <span className="badge bg-danger fs-5 px-3 py-2">
                    <i className="bi bi-exclamation-triangle me-2"></i>Out of Stock
                  </span>
                </div>
              )}
            </div>
          </div>
        </div>

        <div className="col-lg-6">
          <div className="h-100">
            <h1 className="display-5 fw-bold mb-3">{product.name}</h1>
            
            <div className="mb-3">
              <span className="text-muted">SKU: {product.sku}</span>
            </div>

            <div className="mb-4">
              <span className="display-4 text-primary fw-bold">
                ${product.price.toFixed(2)}
              </span>
            </div>

            <div className="mb-4">
              <h5>Description</h5>
              <p className="lead">{product.description}</p>
            </div>

            <div className="mb-4">
              <div className="row">
                <div className="col-sm-6">
                  <h6>Availability</h6>
                  <span className={`badge bg-${stockStatus} fs-6`}>
                    <i className="bi bi-box-seam me-1"></i>
                    {isInStock ? `${availableStock} in stock` : 'Out of stock'}
                  </span>
                </div>
                <div className="col-sm-6">
                  <h6>Status</h6>
                  <span className="badge bg-success fs-6">
                    <i className="bi bi-check-circle me-1"></i>Available
                  </span>
                </div>
              </div>
            </div>

            {stockStatus === 'warning' && isInStock && (
              <div className="alert alert-warning">
                <i className="bi bi-exclamation-triangle me-2"></i>
                <strong>Limited Stock:</strong> Only {availableStock} items remaining!
              </div>
            )}

            <div className="d-grid gap-2">
              <AddToCartForm
                productId={product.id}
                availableStock={availableStock}
                isInStock={isInStock}
                showQuantitySelector={true}
                buttonText={isInStock ? 'Add to Cart' : 'Out of Stock'}
                buttonSize="lg"
                className="btn btn-primary w-100"
              />
              
              <div className="row g-2">
                <div className="col">
                  <WishlistButton 
                    productId={product.id}
                    size="md"
                    variant="button"
                    className="w-100"
                    returnUrl={`/products/${product.id}`}
                  />
                </div>
                <div className="col">
                  <Link href="#" className="btn btn-outline-info w-100">
                    <i className="bi bi-share me-2"></i>Share
                  </Link>
                </div>
              </div>
            </div>

            <div className="mt-4 pt-4 border-top">
              <h6>Product Features</h6>
              <ul className="list-unstyled">
                <li className="mb-2">
                  <i className="bi bi-check-circle text-success me-2"></i>
                  High quality materials
                </li>
                <li className="mb-2">
                  <i className="bi bi-check-circle text-success me-2"></i>
                  Fast shipping available
                </li>
                <li className="mb-2">
                  <i className="bi bi-check-circle text-success me-2"></i>
                  30-day return policy
                </li>
                <li className="mb-2">
                  <i className="bi bi-check-circle text-success me-2"></i>
                  Customer support included
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>

      <div className="row mt-5">
        <div className="col">
          <div className="card shadow-sm">
            <div className="card-header">
              <h5 className="mb-0">
                <i className="bi bi-info-circle me-2"></i>Additional Information
              </h5>
            </div>
            <div className="card-body">
              <div className="row">
                <div className="col-md-4">
                  <h6>Shipping</h6>
                  <p className="text-muted mb-3">
                    Free shipping on orders over $50. Standard delivery takes 3-5 business days.
                  </p>
                </div>
                <div className="col-md-4">
                  <h6>Returns</h6>
                  <p className="text-muted mb-3">
                    30-day return policy. Items must be in original condition with tags attached.
                  </p>
                </div>
                <div className="col-md-4">
                  <h6>Support</h6>
                  <p className="text-muted mb-3">
                    24/7 customer support available. Contact us for any questions or concerns.
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="text-center mt-4">
        <Link href="/products" className="btn btn-outline-primary">
          <i className="bi bi-arrow-left me-2"></i>Back to Products
        </Link>
      </div>
    </div>
  );
} 