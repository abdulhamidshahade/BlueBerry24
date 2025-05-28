import { createProduct } from "@/lib/actions/product-actions";
import { CategoryDto } from "@/types/category";
import { ProductDto } from "@/types/product";

interface Category {
  id: number;
  name: string;
  description: string;
  imageUrl: string;
}

interface ProductFormProps {
  product?: ProductDto;
  action: (formData: FormData) => Promise<void>;
  submitText: string;
  isEdit?: boolean;
  categories: CategoryDto[];
}

export default function ProductForm({
  product,
  action,
  submitText,
  isEdit = false,
  categories,
}: ProductFormProps) {
  return (
    <div className="container-fluid">
      <div className="row justify-content-center">
        <div className="col-lg-8">
          <div className="card shadow">
            <div className="card-header bg-primary text-white">
              <h4 className="mb-0">
                <i
                  className={`bi ${
                    isEdit ? "bi-pencil-square" : "bi-plus-circle"
                  } me-2`}
                ></i>
                {isEdit ? "Edit Product" : "Create New Product"}
              </h4>
            </div>
            <div className="card-body">
              <form action={createProduct}>
                {isEdit && product && (
                  <input type="hidden" name="id" value={product.id} />
                )}

                <div className="row">
                  <div className="col-md-6 mb-3">
                    <label htmlFor="name" className="form-label">
                      <i className="bi bi-tag me-1"></i>Product Name *
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      id="name"
                      name="name"
                      defaultValue={product?.name || ""}
                      required
                      placeholder="Enter product name"
                    />
                  </div>

                  <div className="col-md-6 mb-3">
                    <label htmlFor="sku" className="form-label">
                      <i className="bi bi-upc-scan me-1"></i>SKU *
                    </label>
                    <input
                      type="text"
                      className="form-control"
                      id="sku"
                      name="sku"
                      defaultValue={product?.sku || ""}
                      required
                      placeholder="Enter SKU"
                    />
                  </div>
                </div>

                <div className="mb-3">
                  <label htmlFor="description" className="form-label">
                    <i className="bi bi-card-text me-1"></i>Description *
                  </label>
                  <textarea
                    className="form-control"
                    id="description"
                    name="description"
                    rows={4}
                    defaultValue={product?.description || ""}
                    required
                    placeholder="Enter product description"
                  ></textarea>
                </div>

                <div className="row">
                  <div className="col-md-4 mb-3">
                    <label htmlFor="price" className="form-label">
                      <i className="bi bi-currency-dollar me-1"></i>Price *
                    </label>
                    <input
                      type="number"
                      step="0.01"
                      min="0"
                      className="form-control"
                      id="price"
                      name="price"
                      defaultValue={product?.price || ""}
                      required
                      placeholder="0.00"
                    />
                  </div>

                  <div className="col-md-4 mb-3">
                    <label htmlFor="stockQuantity" className="form-label">
                      <i className="bi bi-box-seam me-1"></i>Stock Quantity *
                    </label>
                    <input
                      type="number"
                      min="0"
                      className="form-control"
                      id="stockQuantity"
                      name="stockQuantity"
                      defaultValue={product?.stockQuantity || ""}
                      required
                      placeholder="0"
                    />
                  </div>

                  <div className="col-md-4 mb-3">
                    <label htmlFor="reservedStock" className="form-label">
                      <i className="bi bi-archive me-1"></i>Reserved Stock
                    </label>
                    <input
                      type="number"
                      min="0"
                      className="form-control"
                      id="reservedStock"
                      name="reservedStock"
                      defaultValue={product?.reservedStock || 0}
                      placeholder="0"
                    />
                  </div>
                </div>

                <div className="row">
                  <div className="col-md-6 mb-3">
                    <label htmlFor="lowStockThreshold" className="form-label">
                      <i className="bi bi-exclamation-triangle me-1"></i>Low
                      Stock Threshold
                    </label>
                    <input
                      type="number"
                      min="0"
                      className="form-control"
                      id="lowStockThreshold"
                      name="lowStockThreshold"
                      defaultValue={product?.lowStockThreshold || 10}
                      placeholder="10"
                    />
                  </div>

                  <div className="col-md-6 mb-3">
                    <label htmlFor="imageUrl" className="form-label">
                      <i className="bi bi-image me-1"></i>Image URL
                    </label>
                    <input
                      type="url"
                      className="form-control"
                      id="imageUrl"
                      name="imageUrl"
                      defaultValue={product?.imageUrl || ""}
                      placeholder="https://example.com/image.jpg"
                    />
                  </div>
                </div>

                <div className="row">
                  <div className="col-md-6 mb-3">
                    <label htmlFor="categories" className="form-label">
                      <i className="bi bi-collection me-1"></i>Categories
                    </label>
                    <select
                      multiple
                      className="form-select"
                      id="categories"
                      name="categories"
                      defaultValue={
                        product?.productCategories?.map((cat) =>
                          String(cat.id)
                        ) || []
                      }
                    >
                      {categories.map((category) => (
                        <option key={category.id} value={category.id}>
                          {category.name}
                        </option>
                      ))}
                    </select>
                    <div className="form-text">
                      Hold Ctrl (or Cmd) to select multiple categories
                    </div>
                  </div>

                  <div className="col-md-6 mb-3 d-flex align-items-end">
                    <div className="form-check">
                      <input
                        type="checkbox"
                        className="form-check-input"
                        id="isActive"
                        name="isActive"
                        defaultChecked={product?.isActive !== false}
                      />
                      <label className="form-check-label" htmlFor="isActive">
                        <i className="bi bi-check-circle me-1"></i>Active
                      </label>
                    </div>
                  </div>
                </div>

                <div className="d-flex gap-2 justify-content-end pt-3 border-top">
                  <a href="/admin/products" className="btn btn-secondary">
                    <i className="bi bi-x-circle me-1"></i>Cancel
                  </a>
                  <button
                    type="submit"
                    className={`btn ${isEdit ? "btn-warning" : "btn-success"}`}
                  >
                    <i
                      className={`bi ${
                        isEdit ? "bi-pencil-square" : "bi-plus-circle"
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
