import { redirect } from 'next/navigation';
import { revalidatePath } from 'next/cache';
import { CategoryService } from '@/lib/services/category/service';
import { ICategoryService } from '@/lib/services/category/interface';
import { CategoryDto, CreateCategoryDto, UpdateCategoryDto } from '@/types/category';

interface Props {
  mode: 'create' | 'edit';
  categoryId?: number;
  initialData?: CategoryDto;
  searchParams?: { [key: string]: string | string[] | undefined };
}

const categoryService: ICategoryService = new CategoryService();

async function createCategory(formData: FormData) {
  'use server';
  
  const name = formData.get('name') as string;
  const description = formData.get('description') as string;
  const imageUrl = formData.get('imageUrl') as string;
  
  const errors: Record<string, string> = {};
  
  if (!name?.trim()) {
    errors.name = 'Category name is required';
  } else if (name.length < 2) {
    errors.name = 'Category name must be at least 2 characters';
  }
  
  if (!description?.trim()) {
    errors.description = 'Description is required';
  } else if (description.length < 10) {
    errors.description = 'Description must be at least 10 characters';
  }
  
  if (!imageUrl?.trim()) {
    errors.imageUrl = 'Image URL is required';
  } else {
    try {
      new URL(imageUrl);
    } catch {
      errors.imageUrl = 'Please enter a valid URL';
    }
  }
  
  if (Object.keys(errors).length > 0) {
    const errorParams = new URLSearchParams();
    Object.entries(errors).forEach(([key, value]) => {
      errorParams.set(`error_${key}`, value);
    });
    errorParams.set('name', name || '');
    errorParams.set('description', description || '');
    errorParams.set('imageUrl', imageUrl || '');
    
    redirect(`?${errorParams.toString()}`);
  }
  
  try {
    const createData: CreateCategoryDto = {
      name: name.trim(),
      description: description.trim(),
      imageUrl: imageUrl.trim()
    };
    
    await categoryService.create(createData);
    revalidatePath('/admin/categories');
    redirect('/admin/categories?success=Category created successfully!');
  } catch (error) {
    const errorMessage = error instanceof Error ? error.message : 'An error occurred';
    redirect(`?error=${encodeURIComponent(errorMessage)}&name=${name}&description=${description}&imageUrl=${imageUrl}`);
  }
}

async function updateCategory(categoryId: number, formData: FormData) {
  'use server';
  
  const name = formData.get('name') as string;
  const description = formData.get('description') as string;
  const imageUrl = formData.get('imageUrl') as string;
  
  const errors: Record<string, string> = {};
  
  if (!name?.trim()) {
    errors.name = 'Category name is required';
  } else if (name.length < 2) {
    errors.name = 'Category name must be at least 2 characters';
  }
  
  if (!description?.trim()) {
    errors.description = 'Description is required';
  } else if (description.length < 10) {
    errors.description = 'Description must be at least 10 characters';
  }
  
  if (!imageUrl?.trim()) {
    errors.imageUrl = 'Image URL is required';
  } else {
    try {
      new URL(imageUrl);
    } catch {
      errors.imageUrl = 'Please enter a valid URL';
    }
  }
  
  if (Object.keys(errors).length > 0) {
    const errorParams = new URLSearchParams();
    Object.entries(errors).forEach(([key, value]) => {
      errorParams.set(`error_${key}`, value);
    });
    errorParams.set('name', name || '');
    errorParams.set('description', description || '');
    errorParams.set('imageUrl', imageUrl || '');
    
    redirect(`?${errorParams.toString()}`);
  }
  
  try {
    const updateData: UpdateCategoryDto = {
      id: categoryId,
      name: name.trim(),
      description: description.trim(),
      imageUrl: imageUrl.trim()
    };
    
    await categoryService.update(categoryId, updateData);
    revalidatePath('/admin/categories');
    redirect('/admin/categories?success=Category updated successfully!');
  } catch (error) {
    const errorMessage = error instanceof Error ? error.message : 'An error occurred';
    redirect(`?error=${encodeURIComponent(errorMessage)}&name=${name}&description=${description}&imageUrl=${imageUrl}`);
  }
}

function isValidUrl(string: string): boolean {
  try {
    new URL(string);
    return true;
  } catch {
    return false;
  }
}

export default async function CategoryForm({ mode, categoryId, initialData, searchParams }: Props) {
  let categoryData = initialData;
  
  if (mode === 'edit' && categoryId && !initialData) {
    try {
      categoryData = await categoryService.getById(categoryId);
    } catch (error) {
      return (
        <div className="alert alert-danger d-flex align-items-center mb-4" role="alert">
          <i className="bi bi-exclamation-triangle-fill me-2"></i>
          <div>Failed to load category data</div>
        </div>
      );
    }
  }
  
  const formData = {
    name: (searchParams?.name as string) || categoryData?.name || '',
    description: (searchParams?.description as string) || categoryData?.description || '',
    imageUrl: (searchParams?.imageUrl as string) || categoryData?.imageUrl || ''
  };
  
  const validation = {
    name: (searchParams?.error_name as string) || '',
    description: (searchParams?.error_description as string) || '',
    imageUrl: (searchParams?.error_imageUrl as string) || ''
  };
  
  const error = searchParams?.error as string;
  const success = searchParams?.success as string;
  
  const createAction = createCategory;
  const updateAction = updateCategory.bind(null, categoryId!);
  
  return (
    <form action={mode === 'create' ? createAction : updateAction}>
      {error && (
        <div className="alert alert-danger d-flex align-items-center mb-4" role="alert">
          <i className="bi bi-exclamation-triangle-fill me-2"></i>
          <div>{error}</div>
        </div>
      )}

      {success && (
        <div className="alert alert-success d-flex align-items-center mb-4" role="alert">
          <i className="bi bi-check-circle-fill me-2"></i>
          <div>{success}</div>
        </div>
      )}

      <div className="mb-3">
        <label htmlFor="name" className="form-label fw-semibold">
          <i className="bi bi-tag me-2"></i>
          Category Name *
        </label>
        <input
          type="text"
          className={`form-control ${validation.name ? 'is-invalid' : ''}`}
          id="name"
          name="name"
          defaultValue={formData.name}
          placeholder="Enter category name"
          required
        />
        {validation.name && (
          <div className="invalid-feedback">{validation.name}</div>
        )}
      </div>

      <div className="mb-3">
        <label htmlFor="description" className="form-label fw-semibold">
          <i className="bi bi-text-paragraph me-2"></i>
          Description *
        </label>
        <textarea
          className={`form-control ${validation.description ? 'is-invalid' : ''}`}
          id="description"
          name="description"
          rows={4}
          defaultValue={formData.description}
          placeholder="Enter category description"
          required
        />
        {validation.description && (
          <div className="invalid-feedback">{validation.description}</div>
        )}
      </div>

      <div className="mb-4">
        <label htmlFor="imageUrl" className="form-label fw-semibold">
          <i className="bi bi-image me-2"></i>
          Image URL *
        </label>
        <input
          type="url"
          className={`form-control ${validation.imageUrl ? 'is-invalid' : ''}`}
          id="imageUrl"
          name="imageUrl"
          defaultValue={formData.imageUrl}
          placeholder="https://example.com/image.jpg"
          required
        />
        {validation.imageUrl && (
          <div className="invalid-feedback">{validation.imageUrl}</div>
        )}
        
        {formData.imageUrl && isValidUrl(formData.imageUrl) && (
          <div className="mt-3">
            <p className="form-text mb-2">
              <i className="bi bi-eye me-1"></i>
              Preview:
            </p>
            <img 
              src={formData.imageUrl} 
              alt="Preview" 
              className="img-thumbnail"
              style={{ maxWidth: '200px', maxHeight: '150px', objectFit: 'cover' }}
            />
          </div>
        )}
      </div>

      <div className="d-flex gap-3">
        <button 
          type="submit" 
          className="btn btn-primary flex-fill"
        >
          <i className={`bi ${mode === 'create' ? 'bi-plus-circle' : 'bi-check-circle'} me-2`}></i>
          {mode === 'create' ? 'Create Category' : 'Update Category'}
        </button>
        
        <a 
          href="/admin/categories"
          className="btn btn-outline-secondary"
        >
          <i className="bi bi-x-circle me-2"></i>
          Cancel
        </a>
      </div>
    </form>
  );
}