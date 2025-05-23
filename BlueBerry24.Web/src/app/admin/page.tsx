import Link from 'next/link';
import { CategoryService } from "@/lib/services/category/service";
import { ICategoryService } from "@/lib/services/category/interface";

const categoryService: ICategoryService = new CategoryService();

export default async function AdminDashboard() {
  let categoriesCount = 0;
  
  try {
    const categories = await categoryService.getAll();
    categoriesCount = categories.length;
  } catch (error) {
    console.error("Failed to load categories count:", error);
  }

  return (
    <div className="container-fluid">
      <div className="row mb-4">
        <div className="col-12">
          <div className="bg-primary text-white py-5 px-4 rounded-3">
            <div className="container">
              <div className="row align-items-center">
                <div className="col-lg-8">
                  <h1 className="display-5 fw-bold mb-3">
                    <i className="bi bi-speedometer2 me-3"></i>
                    Admin Dashboard
                  </h1>
                  <p className="lead mb-0">
                    Welcome to the BlueBerry24 administration panel. Manage your content and monitor system performance.
                  </p>
                </div>
                <div className="col-lg-4 text-lg-end mt-3 mt-lg-0">
                  <div className="text-white-50">
                    <i className="bi bi-calendar3 me-2"></i>
                    {new Date().toLocaleDateString()}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="row mb-5">
        <div className="col-lg-3 col-md-6 mb-4">
          <div className="card border-0 shadow-sm h-100">
            <div className="card-body text-center">
              <div className="display-6 text-primary mb-3">
                <i className="bi bi-grid-3x3-gap"></i>
              </div>
              <h5 className="card-title">Categories</h5>
              <h2 className="text-primary mb-3">{categoriesCount}</h2>
              <Link href="/admin/categories" className="btn btn-outline-primary btn-sm">
                <i className="bi bi-gear me-1"></i>
                Manage
              </Link>
            </div>
          </div>
        </div>

        <div className="col-lg-3 col-md-6 mb-4">
          <div className="card border-0 shadow-sm h-100">
            <div className="card-body text-center">
              <div className="display-6 text-success mb-3">
                <i className="bi bi-box-seam"></i>
              </div>
              <h5 className="card-title">Products</h5>
              <h2 className="text-success mb-3">—</h2>
              <button className="btn btn-outline-success btn-sm" disabled>
                <i className="bi bi-gear me-1"></i>
                Coming Soon
              </button>
            </div>
          </div>
        </div>

        <div className="col-lg-3 col-md-6 mb-4">
          <div className="card border-0 shadow-sm h-100">
            <div className="card-body text-center">
              <div className="display-6 text-warning mb-3">
                <i className="bi bi-people"></i>
              </div>
              <h5 className="card-title">Users</h5>
              <h2 className="text-warning mb-3">—</h2>
              <button className="btn btn-outline-warning btn-sm" disabled>
                <i className="bi bi-gear me-1"></i>
                Coming Soon
              </button>
            </div>
          </div>
        </div>

        <div className="col-lg-3 col-md-6 mb-4">
          <div className="card border-0 shadow-sm h-100">
            <div className="card-body text-center">
              <div className="display-6 text-info mb-3">
                <i className="bi bi-cart-check"></i>
              </div>
              <h5 className="card-title">Orders</h5>
              <h2 className="text-info mb-3">—</h2>
              <button className="btn btn-outline-info btn-sm" disabled>
                <i className="bi bi-gear me-1"></i>
                Coming Soon
              </button>
            </div>
          </div>
        </div>
      </div>

      <div className="row mb-5">
        <div className="col-12">
          <div className="card border-0 shadow-sm">
            <div className="card-header bg-white py-3">
              <h5 className="card-title mb-0">
                <i className="bi bi-lightning me-2"></i>
                Quick Actions
              </h5>
            </div>
            <div className="card-body">
              <div className="row g-3">
                <div className="col-md-6 col-lg-4">
                  <Link href="/admin/categories/create" className="btn btn-outline-primary w-100 py-3">
                    <i className="bi bi-plus-circle d-block display-6 mb-2"></i>
                    <strong>Create Category</strong>
                    <br />
                    <small className="text-muted">Add a new product category</small>
                  </Link>
                </div>
                
                <div className="col-md-6 col-lg-4">
                  <Link href="/admin/categories" className="btn btn-outline-secondary w-100 py-3">
                    <i className="bi bi-list-ul d-block display-6 mb-2"></i>
                    <strong>Manage Categories</strong>
                    <br />
                    <small className="text-muted">Edit or delete categories</small>
                  </Link>
                </div>

                <div className="col-md-6 col-lg-4">
                  <Link href="/categories" className="btn btn-outline-info w-100 py-3">
                    <i className="bi bi-eye d-block display-6 mb-2"></i>
                    <strong>View Categories</strong>
                    <br />
                    <small className="text-muted">See public category listing</small>
                  </Link>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="row">
        <div className="col-12">
          <div className="card border-0 shadow-sm">
            <div className="card-header bg-white py-3">
              <h5 className="card-title mb-0">
                <i className="bi bi-activity me-2"></i>
                System Status
              </h5>
            </div>
            <div className="card-body">
              <div className="row g-3">
                <div className="col-md-4">
                  <div className="d-flex align-items-center">
                    <div className="badge bg-success rounded-pill me-3" style={{ width: '12px', height: '12px' }}></div>
                    <div>
                      <strong>API Connection</strong>
                      <br />
                      <small className="text-success">Online</small>
                    </div>
                  </div>
                </div>
                
                <div className="col-md-4">
                  <div className="d-flex align-items-center">
                    <div className="badge bg-success rounded-pill me-3" style={{ width: '12px', height: '12px' }}></div>
                    <div>
                      <strong>Database</strong>
                      <br />
                      <small className="text-success">Connected</small>
                    </div>
                  </div>
                </div>

                <div className="col-md-4">
                  <div className="d-flex align-items-center">
                    <div className="badge bg-success rounded-pill me-3" style={{ width: '12px', height: '12px' }}></div>
                    <div>
                      <strong>Web Service</strong>
                      <br />
                      <small className="text-success">Running</small>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
} 