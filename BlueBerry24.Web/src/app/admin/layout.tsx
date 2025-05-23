import Link from 'next/link';

export default function AdminLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <div className="min-vh-100 bg-light">
      <nav className="navbar navbar-expand-lg navbar-dark bg-dark shadow-sm">
        <div className="container-fluid">
          <Link href="/admin" className="navbar-brand fw-bold">
            <i className="bi bi-shield-check me-2"></i>
            BlueBerry24 Admin
          </Link>
          
          <button 
            className="navbar-toggler" 
            type="button" 
            data-bs-toggle="collapse" 
            data-bs-target="#adminNavbar"
          >
            <span className="navbar-toggler-icon"></span>
          </button>
          
          <div className="collapse navbar-collapse" id="adminNavbar">
            <ul className="navbar-nav me-auto">
              <li className="nav-item dropdown">
                <a 
                  className="nav-link dropdown-toggle" 
                  href="#" 
                  role="button" 
                  data-bs-toggle="dropdown"
                >
                  <i className="bi bi-grid-3x3-gap me-1"></i>
                  Categories
                </a>
                <ul className="dropdown-menu">
                  <li>
                    <Link href="/categories" className="dropdown-item">
                      <i className="bi bi-eye me-2"></i>View All
                    </Link>
                  </li>
                  <li>
                    <Link href="/admin/categories/create" className="dropdown-item">
                      <i className="bi bi-plus-circle me-2"></i>Create New
                    </Link>
                  </li>
                  <li><hr className="dropdown-divider" /></li>
                  <li>
                    <Link href="/admin/categories" className="dropdown-item">
                      <i className="bi bi-gear me-2"></i>Manage
                    </Link>
                  </li>
                </ul>
              </li>
            </ul>
            
            <ul className="navbar-nav">
              <li className="nav-item">
                <Link href="/" className="nav-link">
                  <i className="bi bi-house me-1"></i>
                  Back to Site
                </Link>
              </li>
            </ul>
          </div>
        </div>
      </nav>

      <main className="container-fluid py-4">
        {children}
      </main>
    </div>
  );
} 