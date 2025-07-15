import { createProduct } from '../../../../lib/actions/product-actions';
import ProductForm from '../../../../components/product/ProductForm';
import { CategoryService } from '../../../../lib/services/category/service';
import { ICategoryService } from '../../../../lib/services/category/interface';

export const dynamic = 'force-dynamic';

export default async function CreateProductPage() {

  const categoryService: ICategoryService = new CategoryService();
  const categories = await categoryService.getAll();

  return (
    <div className="container-fluid">
      <div className="row">
        <div className="col-12">
          <nav aria-label="breadcrumb" className="mb-4">
            <ol className="breadcrumb">
              <li className="breadcrumb-item">
                <a href="/admin" className="text-decoration-none">
                  <i className="bi bi-house-door me-1"></i>Admin
                </a>
              </li>
              <li className="breadcrumb-item">
                <a href="/admin/products" className="text-decoration-none">
                  <i className="bi bi-box-seam me-1"></i>Products
                </a>
              </li>
              <li className="breadcrumb-item active" aria-current="page">
                <i className="bi bi-plus-circle me-1"></i>Create Product
              </li>
            </ol>
          </nav>

          <ProductForm 
            action={createProduct}
            submitText="Create Product"
            isEdit={false}
            categories={categories}
          />
        </div>
      </div>
    </div>
  );
}