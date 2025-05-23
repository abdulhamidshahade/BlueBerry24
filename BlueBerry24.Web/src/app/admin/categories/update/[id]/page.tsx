import { notFound } from 'next/navigation';
import CategoryForm from "@/components/category/CategoryForm";
import { CategoryService } from "@/lib/services/category/service";
import { ICategoryService } from "@/lib/services/category/interface";

const categoryService: ICategoryService = new CategoryService();

interface Props {
  params: {
    id: string;
  };
}

export default async function EditCategoryPage({ params }: Props) {
  const categoryId = parseInt(params.id);
  
  if (isNaN(categoryId)) {
    notFound();
  }

  let category;
  try {
    category = await categoryService.getById(categoryId);
  } catch (error) {
    console.error("Failed to load category:", error);
    notFound();
  }

  return (
    <div className="container-fluid">
      <div className="row mb-4">
        <div className="col-12">
          <div className="d-flex align-items-center">
            <a 
              href="/admin/categories" 
              className="btn btn-outline-secondary me-3"
              title="Back to Categories"
            >
              <i className="bi bi-arrow-left"></i>
            </a>
            <div>
              <h1 className="h2 mb-1">
                <i className="bi bi-pencil me-2 text-warning"></i>
                Edit Category
              </h1>
              <p className="text-muted mb-0">
                Update information for "{category.name}"
              </p>
            </div>
          </div>
        </div>
      </div>

      <div className="row justify-content-center">
        <div className="col-lg-8 col-xl-6">
          <div className="card shadow-sm border-0">
            <div className="card-header bg-white py-3">
              <h5 className="card-title mb-0">
                <i className="bi bi-info-circle me-2"></i>
                Category Information
              </h5>
            </div>
            <div className="card-body p-4">
              <CategoryForm 
                mode="edit" 
                categoryId={categoryId}
                initialData={category}
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
} 