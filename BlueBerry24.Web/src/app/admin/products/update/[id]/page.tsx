import { notFound } from 'next/navigation';
import { updateProduct, getProduct } from '@/lib/actions/product-actions';
import ProductForm from '@/components/product/ProductForm';
import { ICategoryService } from '@/lib/services/category/interface'
import { CategoryService } from '@/lib/services/category/service'

interface PageProps {
  params: { id: string };
}

export default async function EditProductPage({ params }: PageProps) {
  const productId = parseInt(params.id);
  
  if (isNaN(productId)) {
    notFound();
  }

  const product = await getProduct(productId);

  if (!product) {
    notFound();
    
  }

  const categoryService: ICategoryService = new CategoryService();
  const categories = await categoryService.getAll();
  
  async function handleUpdateProduct(formData: FormData) {
    'use server';
    const product = await getProduct(parseInt(formData.get('id') as string));
    await updateProduct(formData, product!.imageUrl);
  }

  

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
                <i className="bi bi-pencil-square me-1"></i>Edit: {product.name}
              </li>
            </ol>
          </nav>
          
          <ProductForm 
            product={product}
            action={handleUpdateProduct}
            submitText="Update Product"
            isEdit={true}
            categories={categories}
          />
        </div>
      </div>
    </div>
  );
} 