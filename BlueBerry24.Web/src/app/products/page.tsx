import { Suspense } from 'react';
import { getPaginatedProducts } from '../../lib/actions/product-actions';
import ProductCard from '../../components/product/ProductCard';
import Pagination from '../../components/shared/Pagination';
import ProductSearchFilters from '../../components/product/ProductSearchFilters';
import { ProductFilterDto } from '../../types/pagination';

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

async function ProductsList({ filter }: { filter: ProductFilterDto }) {
  const paginationResult = await getPaginatedProducts(filter);
  
  if (paginationResult.data.length === 0) {
    const categoryText = filter.category && filter.category !== 'all' ? ` in ${filter.category}` : '';
    const searchText = filter.searchTerm ? ` matching "${filter.searchTerm}"` : '';
    
    return (
      <div className="text-center py-5">
        <div className="mb-4">
          <i className="bi bi-box-seam display-1 text-muted"></i>
        </div>
        <h3 className="text-muted">No products found{categoryText}{searchText}</h3>
        <p className="text-muted mb-4">
          {filter.category && filter.category !== 'all' 
            ? `No products found in the ${filter.category} category. Try selecting a different category or search term.`
            : filter.searchTerm
            ? `No products found matching "${filter.searchTerm}". Try a different search term or browse all categories.`
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
    <>
      <div className="row row-cols-1 row-cols-md-2 row-cols-lg-3 row-cols-xl-4 g-4">
        {paginationResult.data.map((product) => (
          <ProductCard 
            key={product.id} 
            product={product} 
            showAdminActions={false}
          />
        ))}
      </div>
      
      <Pagination 
        pagination={paginationResult}
        currentPage={filter.pageNumber}
        baseUrl="/products"
        searchParams={{
          ...(filter.category && filter.category !== 'all' && { category: filter.category }),
          ...(filter.sortBy && filter.sortBy !== 'name' && { sort: filter.sortBy }),
          ...(filter.searchTerm && { search: filter.searchTerm })
        }}
      />
    </>
  );
}

function ProductsHeader({ category, searchTerm, totalCount }: { 
  category: string; 
  searchTerm?: string;
  totalCount: number;
}) {
  const categoryText = category && category !== 'all' ? `${category}` : 'Our Products';
  const searchText = searchTerm ? ` matching "${searchTerm}"` : '';
  const countText = totalCount > 0 ? ` (${totalCount} items)` : '';
  
  return (
    <div className="bg-primary text-white py-5 mb-5">
      <div className="container">
        <div className="row align-items-center">
          <div className="col-lg-8">
            <h1 className="display-4 fw-bold mb-3">
              <i className="bi bi-shop me-3"></i>{categoryText}{searchText}
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

export default async function ProductsPage({ searchParams }: { 
  searchParams: Promise<{ 
    page?: string; 
    category?: string; 
    sort?: string; 
    search?: string;
  }>
}) {
  const params = await searchParams;
  const page = Math.max(1, parseInt(params.page || '1'));
  const category = params.category || 'all';
  const sortBy = params.sort || 'name';
  const searchTerm = params.search || '';
  
  const filter: ProductFilterDto = {
    pageNumber: page,
    pageSize: 12,
    searchTerm: searchTerm || undefined,
    category: category === 'all' ? undefined : category,
    sortBy: sortBy === 'name' ? undefined : sortBy,
    isActive: true
  };

  const paginationResult = await getPaginatedProducts(filter);

  return (
    <>
      <ProductsHeader 
        category={category} 
        searchTerm={searchTerm}
        totalCount={paginationResult.totalCount}
      />
      
      <div className="container">
        <ProductSearchFilters 
          currentCategory={category}
          currentSort={sortBy}
          currentSearch={searchTerm}
          currentPage={page}
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
                {searchTerm && <span className="text-muted ms-1">(Search: {searchTerm})</span>}
              </li>
            </ol>
          </nav>
        </div>

        <Suspense fallback={<ProductsLoadingSkeleton />}>
          <ProductsList filter={filter} />
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