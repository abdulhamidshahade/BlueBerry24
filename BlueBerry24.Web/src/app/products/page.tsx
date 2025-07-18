import { Suspense } from 'react';
import { getProducts } from '../../lib/actions/product-actions';
import ProductCard from '../../components/product/ProductCard';

export const dynamic = 'force-dynamic';

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

async function ProductsList({ category, sortBy }: any) {
  const allProducts = await getProducts();
  
  let products = allProducts.filter(product => product.isActive);

  if (category && category !== 'all') {
    products = products.filter(product => 
      product?.productCategories.some(c => c.name.toLowerCase() === category.toLowerCase())
    );
  }

  if (sortBy) {
    switch (sortBy) {
      case 'price-low':
        products.sort((a, b) => (a.price || 0) - (b.price || 0));
        break;
      case 'price-high':
        products.sort((a, b) => (b.price || 0) - (a.price || 0));
        break;
      // case 'newest':
      //   products.sort((a, b) => new Date(b.createdAt || 0) - new Date(a.createdAt || 0));
      //   break;
      case 'name':
      default:
        products.sort((a, b) => (a.name || '').localeCompare(b.name || ''));
        break;
    }
  }

  if (products.length === 0) {
    const categoryText = category && category !== 'all' ? ` in ${category}` : '';
    return (
      <div className="text-center py-5">
        <div className="mb-4">
          <i className="bi bi-box-seam display-1 text-muted"></i>
        </div>
        <h3 className="text-muted">No products available{categoryText}</h3>
        <p className="text-muted mb-4">
          {category && category !== 'all' 
            ? `No products found in the ${category} category. Try selecting a different category.`
            : 'Please check back later for new products.'
          }
        </p>
        <a href="/products" className="btn btn-primary">
          <i className="bi bi-arrow-clockwise me-2"></i>Show All Products
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

function ProductsHeader({ category, productsCount }: any) {
  const categoryText = category && category !== 'all' ? `${category}` : 'Our Products';
  const countText = productsCount !== undefined ? ` (${productsCount} items)` : '';
  
  return (
    <div className="bg-primary text-white py-5 mb-5">
      <div className="container">
        <div className="row align-items-center">
          <div className="col-lg-8">
            <h1 className="display-4 fw-bold mb-3">
              <i className="bi bi-shop me-3"></i>{categoryText}
            </h1>
            <p className="lead mb-0">
              Discover our amazing collection of high-quality products{countText}. 
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

function ProductsFilters({ currentCategory, currentSort }: any) {
  const createFilterUrl = (type: any, value: any) => {
    const params = new URLSearchParams();
    
    if (type === 'category') {
      if (value !== 'all') params.set('category', value);
      if (currentSort && currentSort !== 'name') params.set('sort', currentSort);
    } else if (type === 'sort') {
      if (currentCategory && currentCategory !== 'all') params.set('category', currentCategory);
      if (value !== 'name') params.set('sort', value);
    }
    
    return `/products${params.toString() ? '?' + params.toString() : ''}`;
  };

  const categories = [
    { value: 'all', label: 'All Categories' },
    { value: 'Vegetables & Fruits', label: 'Vegetables & Fruits' },
    { value: 'Water & Drinks', label: 'Water & Drinks' },
    { value: 'Snack', label: 'Snack' },
    { value: 'Breakfast', label: 'Breakfast' },
  ];

  const sortOptions = [
    { value: 'name', label: 'Sort by Name' },
    { value: 'price-low', label: 'Price: Low to High' },
    { value: 'price-high', label: 'Price: High to Low' },
    { value: 'newest', label: 'Newest First' }
  ];

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
              <div className="dropdown">
                <button 
                  className="btn btn-outline-secondary btn-sm dropdown-toggle" 
                  type="button" 
                  data-bs-toggle="dropdown"
                >
                  {sortOptions.find(opt => opt.value === (currentSort || 'name'))?.label}
                </button>
                <ul className="dropdown-menu">
                  {sortOptions.map(option => (
                    <li key={option.value}>
                      <a 
                        className={`dropdown-item ${(currentSort || 'name') === option.value ? 'active' : ''}`}
                        href={createFilterUrl('sort', option.value)}
                      >
                        {option.label}
                      </a>
                    </li>
                  ))}
                </ul>
              </div>

              <div className="dropdown">
                <button 
                  className="btn btn-outline-secondary btn-sm dropdown-toggle" 
                  type="button" 
                  data-bs-toggle="dropdown"
                >
                  {categories.find(cat => cat.value === (currentCategory || 'all'))?.label}
                </button>
                <ul className="dropdown-menu">
                  {categories.map(category => (
                    <li key={category.value}>
                      <a 
                        className={`dropdown-item ${(currentCategory || 'all') === category.value ? 'active' : ''}`}
                        href={createFilterUrl('category', category.value)}
                      >
                        {category.label}
                      </a>
                    </li>
                  ))}
                </ul>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default async function ProductsPage({ searchParams }: any) {
  const category = searchParams?.category || 'all';
  const sortBy = searchParams?.sort || 'name';
  
  const allProducts = await getProducts();
  const activeProducts = allProducts.filter(product => product.isActive);
  const filteredProducts = category === 'all' 
    ? activeProducts 
    : activeProducts.filter(product => 
        product.productCategories.some(c => c.name.toLowerCase() === category?.toLowerCase()) 
      );

  return (
    <>
      <ProductsHeader 
        category={category} 
        productsCount={filteredProducts.length}
      />
      
      <div className="container">
        <ProductsFilters 
          currentCategory={category}
          currentSort={sortBy}
        />
        
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
                {category !== 'all' && <span className="text-muted ms-1">({category})</span>}
              </li>
            </ol>
          </nav>
        </div>

        <Suspense fallback={<ProductsLoadingSkeleton />}>
          <ProductsList category={category} sortBy={sortBy} />
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