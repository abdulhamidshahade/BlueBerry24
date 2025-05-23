import Link from 'next/link';
import { CategoryService } from "@/lib/services/category/service";
import { ICategoryService } from "@/lib/services/category/interface";
import { CategoryDto } from "@/types/category";
import CategoryActionButtons from '@/components/category/CategoryActionButtons';

const categoryService: ICategoryService = new CategoryService();

export default async function AdminCategoriesPage() {
  let categories: CategoryDto[] = [];
  let hasError = false;

  try {
    categories = await categoryService.getAll();
  } catch (error) {
    console.error("Failed to load categories:", error);
    hasError = true;
  }

  return (
    <div className="container-fluid">
      <div className="row mb-4">
        <div className="col-12">
          <div className="d-flex justify-content-between align-items-center">
            <div>
              <h1 className="h2 mb-1">
                <i className="bi bi-gear me-2 text-primary"></i>
                Manage Categories
              </h1>
              <p className="text-muted mb-0">
                Create, edit, and manage product categories
              </p>
            </div>
            <Link 
              href="/admin/categories/create" 
              className="btn btn-primary"
            >
              <i className="bi bi-plus-circle me-2"></i>
              Add New Category
            </Link>
          </div>
        </div>
      </div>

      {hasError && (
        <div className="row mb-4">
          <div className="col-12">
            <div className="alert alert-danger d-flex align-items-center" role="alert">
              <i className="bi bi-exclamation-triangle-fill me-2"></i>
              <div>
                <strong>Error!</strong> Failed to load categories. Please try refreshing the page.
              </div>
            </div>
          </div>
        </div>
      )}

      <div className="row">
        <div className="col-12">
          <div className="card shadow-sm border-0">
            <div className="card-header bg-white py-3">
              <div className="d-flex justify-content-between align-items-center">
                <h5 className="card-title mb-0">
                  <i className="bi bi-list-ul me-2"></i>
                  Categories ({categories.length})
                </h5>
                <div className="input-group" style={{ maxWidth: '300px' }}>
                  <span className="input-group-text bg-light border-end-0">
                    <i className="bi bi-search"></i>
                  </span>
                  <input 
                    type="text" 
                    className="form-control border-start-0" 
                    placeholder="Search categories..."
                  />
                </div>
              </div>
            </div>
            
            <div className="card-body p-0">
              {categories.length === 0 ? (
                <div className="text-center py-5">
                  <i className="bi bi-folder-x display-1 text-muted mb-3"></i>
                  <h5 className="text-muted">No Categories Found</h5>
                  <p className="text-muted mb-4">Get started by creating your first category.</p>
                  <Link 
                    href="/admin/categories/create" 
                    className="btn btn-primary"
                  >
                    <i className="bi bi-plus-circle me-2"></i>
                    Create First Category
                  </Link>
                </div>
              ) : (
                <div className="table-responsive">
                  <table className="table table-hover mb-0">
                    <thead className="table-light">
                      <tr>
                        <th scope="col" style={{ width: '80px' }}>Image</th>
                        <th scope="col">Name</th>
                        <th scope="col">Description</th>
                        <th scope="col" style={{ width: '120px' }}>Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      {categories.map((category) => (
                        <tr key={category.id}>
                          <td>
                            <img 
                              src={category.imageUrl} 
                              alt={category.name}
                              className="rounded"
                              style={{ 
                                width: '50px', 
                                height: '50px', 
                                objectFit: 'cover' 
                              }}
                            />
                          </td>
                          <td>
                            <div className="fw-semibold text-dark">
                              {category.name}
                            </div>
                            <small className="text-muted">ID: {category.id}</small>
                          </td>
                          <td>
                            <span className="text-muted">
                              {category.description.length > 100 
                                ? category.description.substring(0, 100) + '...'
                                : category.description
                              }
                            </span>
                          </td>
                          <td>
                            <CategoryActionButtons categoryId={category.id} />
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
} 