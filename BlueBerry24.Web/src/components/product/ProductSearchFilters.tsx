interface ProductSearchFiltersProps {
  currentCategory: string;
  currentSort: string;
  currentSearch: string;
  currentPage: number;
}

export default function ProductSearchFilters({ 
  currentCategory, 
  currentSort, 
  currentSearch,
  currentPage 
}: ProductSearchFiltersProps) {
  const createFilterUrl = (type: string, value: string, resetPage: boolean = true) => {
    const params = new URLSearchParams();
    
    if (type !== 'category' && currentCategory && currentCategory !== 'all') {
      params.set('category', currentCategory);
    }
    if (type !== 'sort' && currentSort && currentSort !== 'name') {
      params.set('sort', currentSort);
    }
    if (type !== 'search' && currentSearch) {
      params.set('search', currentSearch);
    }
    
    if (type === 'category') {
      if (value !== 'all') params.set('category', value);
    } else if (type === 'sort') {
      if (value !== 'name') params.set('sort', value);
    } else if (type === 'search') {
      if (value) params.set('search', value);
    }
    
    if (resetPage && currentPage > 1) {
      params.set('page', '1');
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
    <div className="card shadow-sm mb-4 overflow-visible">
      <div className="card-body overflow-visible">
        <div className="row g-3">
          <div className="col-12">
            <form action="/products" method="GET" className="d-flex">
              <input 
                type="hidden" 
                name="category" 
                value={currentCategory === 'all' ? '' : currentCategory} 
              />
              <input 
                type="hidden" 
                name="sort" 
                value={currentSort === 'name' ? '' : currentSort} 
              />
              <input 
                type="hidden" 
                name="page" 
                value="1" 
              />
              <input
                type="text"
                name="search"
                className="form-control me-2"
                placeholder="Search products..."
                defaultValue={currentSearch}
              />
              <button type="submit" className="btn btn-primary">
                <i className="bi bi-search"></i>
              </button>
            </form>
          </div>

          <div className="col-md-6">
            <div className="dropdown">
              <button 
                className="btn btn-outline-secondary dropdown-toggle w-100" 
                type="button" 
                data-bs-toggle="dropdown"
              >
                <i className="bi bi-funnel me-2"></i>
                {categories.find(cat => cat.value === (currentCategory || 'all'))?.label}
              </button>
              <ul className="dropdown-menu w-100">
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

          <div className="col-md-6">
            <div className="dropdown">
              <button 
                className="btn btn-outline-secondary dropdown-toggle w-100" 
                type="button" 
                data-bs-toggle="dropdown"
              >
                <i className="bi bi-sort-down me-2"></i>
                {sortOptions.find(opt => opt.value === (currentSort || 'name'))?.label}
              </button>
              <ul className="dropdown-menu w-100">
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
          </div>

          {(currentCategory !== 'all' || currentSort !== 'name' || currentSearch) && (
            <div className="col-12 text-center">
              <a href="/products" className="btn btn-outline-secondary btn-sm">
                <i className="bi bi-x-circle me-1"></i>
                Clear All Filters
              </a>
            </div>
          )}
        </div>
      </div>
    </div>
  );
} 