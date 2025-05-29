import { CategoryDto } from "@/types/category";

interface Props {
  categories: CategoryDto[];
}

export default async function CategoryList({ categories }: Props) {
  if (!categories.length) {
    return (
      <div className="alert alert-info d-flex align-items-center" role="alert">
        <i className="bi bi-info-circle-fill me-2"></i>
        <div>
          No categories available at the moment. Please check back later.
        </div>
      </div>
    );
  }
  return (
    <div className="row g-4">
      {categories.map((category) => (
        <div key={category.id} className="col-12 col-sm-6 col-lg-4">
          <div className="card h-100 shadow-sm border-0 category-card">
            <div className="position-relative overflow-hidden">
              <img
                src={category.imageUrl}
                alt={category.name}
                className="card-img-top"
                style={{ height: '200px', objectFit: 'cover' }}
              />
              <div className="position-absolute top-0 start-0 w-100 h-100 category-overlay"></div>
            </div>
            <div className="card-body d-flex flex-column">
              <h5 className="card-title fw-bold text-primary mb-2">
                {category.name}
              </h5>
              <p className="card-text text-muted flex-grow-1">
                {category.description}
              </p>
              <div className="mt-auto">
                <a href={`/categories/${category.id}`} className="btn btn-primary btn-sm">
                  Explore Category
                  <i className="bi bi-arrow-right ms-2"></i>
                </a>
              </div>
            </div>
          </div>
        </div>
      ))}
    </div>
  );
}