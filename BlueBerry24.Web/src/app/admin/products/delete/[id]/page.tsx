import { notFound } from 'next/navigation';
import Link from 'next/link';
import { deleteProduct, getProduct } from '@/lib/actions/product-actions';

interface PageProps {
  params: { id: string };
}

export default async function DeleteProductPage({ params }: PageProps) {
  const productId = parseInt(params.id);
  
  if (isNaN(productId)) {
    notFound();
  }

  const product = await getProduct(productId);

  if (!product) {
    notFound();
  }

  const stockStatus = product.stockQuantity <= product.lowStockThreshold ? 'danger' : 
                     product.stockQuantity <= product.lowStockThreshold * 2 ? 'warning' : 'success';

  async function handleDeleteProduct(formData: FormData) {
    'use server';
    
    await deleteProduct(formData);
  }

  return (
    <div className="container-fluid">
      <div className="row justify-content-center">
        <div className="col-lg-8">
          <nav aria-label="breadcrumb" className="mb-4">
            <ol className="breadcrumb">
              <li className="breadcrumb-item">
                <a href="/admin" className="text-decoration-none">
                  <i className="bi bi-house-door me-1"></i>Admin
                </a>
              </li>
              <li className="breadcrumb-item">
                <a href="/admin/products" className="text-decoration-none">
                  <i className="bi bi-box-seam me-1"></i>Products
                </a>
              </li>
              <li className="breadcrumb-item active" aria-current="page">
                <i className="bi bi-trash me-1"></i>Delete: {product.name}
              </li>
            </ol>
          </nav>

          <div className="card shadow">
            <div className="card-header bg-danger text-white">
              <h4 className="mb-0">
                <i className="bi bi-exclamation-triangle me-2"></i>
                Confirm Product Deletion
              </h4>
            </div>
            <div className="card-body">
              <div className="alert alert-warning">
                <i className="bi bi-exclamation-triangle-fill me-2"></i>
                <strong>Warning:</strong> This action cannot be undone. The product and all its associated data will be permanently removed.
              </div>

              <div className="row">
                <div className="col-md-4">
                  {product.imageUrl && (
                    <picture>
                      <img 
                        src={product.imageUrl} 
                        className="img-fluid rounded mb-3"
                        alt={product.name}
                        style={{
                          backgroundImage: 'url(/api/placeholder/300/200)',
                          backgroundSize: 'cover',
                          backgroundPosition: 'center'
                        }}
                      />
                    </picture>
                  )}
                </div>
                <div className="col-md-8">
                  <h5 className="card-title">{product.name}</h5>
                  <p className="text-muted mb-2">
                    <strong>SKU:</strong> {product.sku}
                  </p>
                  <p className="mb-3">{product.description}</p>
                  
                  <div className="row mb-3">
                    <div className="col-sm-6">
                      <strong>Price:</strong> ${product.price.toFixed(2)}
                    </div>
                    <div className="col-sm-6">
                      <strong>Stock:</strong> 
                      <span className={`badge bg-${stockStatus} ms-1`}>
                        {product.stockQuantity} units
                      </span>
                    </div>
                  </div>
                  
                  <div className="row mb-3">
                    <div className="col-sm-6">
                      <strong>Reserved:</strong> {product.reservedStock}
                    </div>
                    <div className="col-sm-6">
                      <strong>Status:</strong> 
                      <span className={`badge ${product.isActive ? 'bg-success' : 'bg-secondary'} ms-1`}>
                        {product.isActive ? 'Active' : 'Inactive'}
                      </span>
                    </div>
                  </div>
                </div>
              </div>

              <div className="d-flex gap-2 justify-content-end pt-3 border-top">
                <Link 
                  href="/admin/products" 
                  className="btn btn-secondary"
                >
                  <i className="bi bi-x-circle me-1"></i>Cancel
                </Link>
                <Link 
                  href={`/admin/products/${product.id}/edit`}
                  className="btn btn-outline-warning"
                >
                  <i className="bi bi-pencil-square me-1"></i>Edit Instead
                </Link>
                <form action={handleDeleteProduct} className="d-inline">
                  <input type="hidden" name="id" value={product.id} />
                  <input type="hidden" name="productName" value={product.name} />
                  <button 
                    type="submit" 
                    className="btn btn-danger"
                  >
                    <i className="bi bi-trash me-1"></i>Delete Product
                  </button>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}