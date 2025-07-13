import CategoryForm from '../../../../components/category/CategoryForm';
import { createCategory } from '../../../../lib/actions/category-actions';

interface Props {
  searchParams: Promise<{ [key: string]: string | string[] | undefined }>;
}

export default async function CreateCategoryPage({ searchParams }: Props) {
  const resolvedSearchParams = await searchParams;
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
                <i className="bi bi-plus-circle me-2 text-success"></i>
                Create New Category
              </h1>
              <p className="text-muted mb-0">
                Add a new category to organize your products
              </p>
            </div>
          </div>
        </div>
      </div>

      <CategoryForm 
        action={createCategory}
        isEdit={false}
        submitText='Create Category'
        searchParams={resolvedSearchParams}

      />
    </div>
  );
}