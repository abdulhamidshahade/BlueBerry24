import { PaginationDto } from '../../types/pagination';

interface PaginationProps {
  pagination: PaginationDto<any>;
  currentPage: number;
  baseUrl: string;
  searchParams?: Record<string, string>;
}

export default function Pagination({ pagination, currentPage, baseUrl, searchParams = {} }: PaginationProps) {
  if (pagination.totalPages <= 1) {
    return null;
  }

  const createPageUrl = (page: number) => {
    const params = new URLSearchParams(searchParams);
    params.set('page', page.toString());
    return `${baseUrl}?${params.toString()}`;
  };

  const renderPageNumbers = () => {
    const pages = [];
    const maxVisiblePages = 5;
    let startPage = Math.max(1, currentPage - Math.floor(maxVisiblePages / 2));
    let endPage = Math.min(pagination.totalPages, startPage + maxVisiblePages - 1);

    if (endPage - startPage + 1 < maxVisiblePages) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }

    // First page
    if (startPage > 1) {
      pages.push(
        <li key="first" className="page-item">
          <a className="page-link" href={createPageUrl(1)}>
            <i className="bi bi-chevron-double-left"></i>
          </a>
        </li>
      );
    }

    // Previous page
    if (pagination.hasPreviousPage) {
      pages.push(
        <li key="prev" className="page-item">
          <a className="page-link" href={createPageUrl(currentPage - 1)}>
            <i className="bi bi-chevron-left"></i>
          </a>
        </li>
      );
    }

    // Page numbers
    for (let i = startPage; i <= endPage; i++) {
      pages.push(
        <li key={i} className={`page-item ${i === currentPage ? 'active' : ''}`}>
          <a className="page-link" href={createPageUrl(i)}>
            {i}
          </a>
        </li>
      );
    }

    // Next page
    if (pagination.hasNextPage) {
      pages.push(
        <li key="next" className="page-item">
          <a className="page-link" href={createPageUrl(currentPage + 1)}>
            <i className="bi bi-chevron-right"></i>
          </a>
        </li>
      );
    }

    // Last page
    if (endPage < pagination.totalPages) {
      pages.push(
        <li key="last" className="page-item">
          <a className="page-link" href={createPageUrl(pagination.totalPages)}>
            <i className="bi bi-chevron-double-right"></i>
          </a>
        </li>
      );
    }

    return pages;
  };

  return (
    <div className="d-flex justify-content-between align-items-center mt-4">
      <div className="text-muted">
        Showing {pagination.firstItemOnPage} to {pagination.lastItemOnPage} of {pagination.totalCount} products
      </div>
      
      <nav aria-label="Product pagination">
        <ul className="pagination pagination-sm mb-0">
          {renderPageNumbers()}
        </ul>
      </nav>
    </div>
  );
} 