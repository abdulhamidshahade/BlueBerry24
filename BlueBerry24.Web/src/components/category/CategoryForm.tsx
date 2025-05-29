import { CategoryDto } from '@/types/category';

interface Props {
  category?: CategoryDto;
  action: (formData: FormData) => Promise<void>;
  isEdit?: boolean;
  submitText: string;
  searchParams?: { [key: string]: string | string[] | undefined };
}

export default function CategoryForm({
  category,
  action,
  isEdit = false,
  submitText,
  searchParams
}: Props) {
  const errorName = searchParams?.error_name as string;
  const errorDescription = searchParams?.error_description as string;
  const errorImageUrl = searchParams?.error_imageUrl as string;
  const generalError = searchParams?.error as string;

  const preservedName = searchParams?.name as string;
  const preservedDescription = searchParams?.description as string;
  const preservedImageUrl = searchParams?.imageUrl as string;

  return (
    <div className="container-fluid">
      <div className="row justify-content-center">
        <div className="col-lg-8">
          <div className="card shadow">
            <div className="card-header bg-primary text-white">
              <h4 className="mb-0">
                <i
                  className={`bi ${isEdit ? "bi-pencil-square" : "bi-plus-circle"
                    } me-2`}
                ></i>
                {isEdit ? "Edit Category" : "Create New Category"}
              </h4>
            </div>
            <div className="card-body">
              {generalError && (
                <div className="alert alert-danger mb-3" role="alert">
                  <i className="bi bi-exclamation-triangle-fill me-2"></i>
                  {generalError}
                </div>
              )}

              <form action={action}>
                {isEdit && category && (
                  <input type="hidden" name="id" value={category.id} />
                )}

                <div className="mb-3">
                  <label htmlFor="name" className="form-label">
                    <i className="bi bi-tag me-1"></i>Category Name *
                  </label>
                  <input
                    type="text"
                    className={`form-control ${errorName ? 'is-invalid' : ''}`}
                    id="name"
                    name="name"
                    defaultValue={preservedName || category?.name || ""}
                    required
                    placeholder="Enter category name"
                  />
                  {errorName && (
                    <div className="invalid-feedback">
                      {errorName}
                    </div>
                  )}
                </div>

                <div className="mb-3">
                  <label htmlFor="description" className="form-label">
                    <i className="bi bi-card-text me-1"></i>Description *
                  </label>
                  <textarea
                    className={`form-control ${errorDescription ? 'is-invalid' : ''}`}
                    id="description"
                    name="description"
                    rows={4}
                    defaultValue={preservedDescription || category?.description || ""}
                    required
                    placeholder="Enter category description"
                  ></textarea>
                  {errorDescription && (
                    <div className="invalid-feedback">
                      {errorDescription}
                    </div>
                  )}
                </div>

                <div className="mb-3">
                  <label htmlFor="imageUrl" className="form-label">
                    <i className="bi bi-image me-1"></i>Image URL *
                  </label>
                  <input
                    type="url"
                    className={`form-control ${errorImageUrl ? 'is-invalid' : ''}`}
                    id="imageUrl"
                    name="imageUrl"
                    defaultValue={preservedImageUrl || category?.imageUrl || ""}
                    required
                    placeholder="https://example.com/image.jpg"
                  />
                  {errorImageUrl && (
                    <div className="invalid-feedback">
                      {errorImageUrl}
                    </div>
                  )}
                </div>

                <div className="d-flex gap-2 justify-content-end pt-3 border-top">
                  <a href="/admin/categories" className="btn btn-secondary">
                    <i className="bi bi-x-circle me-1"></i>Cancel
                  </a>
                  <button
                    type="submit"
                    className={`btn ${isEdit ? "btn-warning" : "btn-success"}`}
                  >
                    <i
                      className={`bi ${isEdit ? "bi-pencil-square" : "bi-plus-circle"
                        } me-1`}
                    ></i>
                    {submitText}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}