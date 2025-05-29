import { Suspense } from 'react';
import { getProducts } from '@/lib/actions/product-actions';
import ProductCard from '@/components/product/ProductCard';

function ProductsLoadingSkeleton() {
  return (
    <div className="row row-cols-1 row-cols-md-2 row-cols-lg-3 row-cols-xl-4 g-4">
      {Array.from({ length: 12 }, (_, i) => (
        <div key={i} className="col">
          <div className="card h-100">
            <div className="card-img-top bg-light" style={{ height: '250px' }}>
              <div className="d-flex align-items-center justify-content-center h-100">
                <div className="spinner-border text-primary" role="status">
                  <span className="visually-hidden">Loading...</span>
                </div>
              </div>
            </div>
            <div className="card-body">
              <div className="placeholder-glow">
                <span className="placeholder col-7"></span>
                <span className="placeholder col-4"></span>
                <span className="placeholder col-4"></span>
                <span className="placeholder col-6"></span>
                <span className="placeholder col-8"></span>
              </div>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
}

async function ProductsList() {
  const allProducts = await getProducts();
  
  const products = allProducts.filter(product => product.isActive);

  if (products.length === 0) {
    return (
      <div className="text-center py-5">
        <div className="mb-4">
          <i className="bi bi-box-seam display-1 text-muted"></i>
        </div>
        <h3 className="text-muted">No products available</h3>
        <p className="text-muted mb-4">Please check back later for new products.</p>
        <a href="/" className="btn btn-primary">
          <i className="bi bi-house me-2"></i>Back to Home
        </a>
      </div>
    );
  }

  return (
    <div className="row row-cols-1 row-cols-md-2 row-cols-lg-3 row-cols-xl-4 g-4">
      {products.map((product) => (
        <ProductCard 
          key={product.id} 
          product={product} 
          showAdminActions={false}
        />
      ))}
    </div>
  );
}

function ProductsHeader() {
  return (
    <div className="bg-primary text-white py-5 mb-5">
      <div className="container">
        <div className="row align-items-center">
          <div className="col-lg-8">
            <h1 className="display-4 fw-bold mb-3">
              <i className="bi bi-shop me-3"></i>Our Products
            </h1>
            <p className="lead mb-0">
              Discover our amazing collection of high-quality products. 
              Find exactly what you're looking for with great prices and excellent service.
            </p>
          </div>
          <div className="col-lg-4 text-end d-none d-lg-block">
            <i className="bi bi-box-seam-fill display-1 opacity-50"></i>
          </div>
        </div>
      </div>
    </div>
  );
}

function ProductsFilters() {
  return (
    <div className="card shadow-sm mb-4">
      <div className="card-body">
        <div className="row align-items-center">
          <div className="col-md-6">
            <h5 className="mb-0">
              <i className="bi bi-funnel me-2"></i>Filter & Sort
            </h5>
          </div>
          <div className="col-md-6">
            <div className="d-flex gap-2 justify-content-md-end">
              <select className="form-select form-select-sm" style={{ width: 'auto' }}>
                <option value="name">Sort by Name</option>
                <option value="price-low">Price: Low to High</option>
                <option value="price-high">Price: High to Low</option>
                <option value="newest">Newest First</option>
              </select>
              <select className="form-select form-select-sm" style={{ width: 'auto' }}>
                <option value="all">All Categories</option>
                <option value="electronics">Electronics</option>
                <option value="clothing">Clothing</option>
                <option value="books">Books</option>
              </select>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default async function ProductsPage() {
  return (
    <>
      <ProductsHeader />
      
      <div className="container">
        <ProductsFilters />
        
        <div className="mb-4">
          <nav aria-label="breadcrumb">
            <ol className="breadcrumb">
              <li className="breadcrumb-item">
                <a href="/" className="text-decoration-none">
                  <i className="bi bi-house-door me-1"></i>Home
                </a>
              </li>
              <li className="breadcrumb-item active" aria-current="page">
                <i className="bi bi-box-seam me-1"></i>Products
              </li>
            </ol>
          </nav>
        </div>

        <Suspense fallback={<ProductsLoadingSkeleton />}>
          <ProductsList />
        </Suspense>

        <div className="text-center py-5">
          <p className="text-muted">
            Looking for something specific? 
            <a href="/contact" className="text-decoration-none ms-1">Contact us</a> 
            for personalized assistance.
          </p>
        </div>
      </div>
    </>
  );
} 