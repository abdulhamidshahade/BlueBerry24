import { redirect } from 'next/navigation';
import { revalidatePath } from 'next/cache';
import { CategoryService } from '@/lib/services/category/service';
import { ICategoryService } from '@/lib/services/category/interface';
import { CategoryDto } from '@/types/category';

interface Props {
  category: CategoryDto;
  searchParams?: { [key: string]: string | string[] | undefined };
}

const categoryService: ICategoryService = new CategoryService();


async function deleteCategory(categoryId: number, formData: FormData) {
  'use server';
  
  const confirmationText = formData.get('confirmationText') as string;
  const expectedConfirmation = 'DELETE';
  
  if (!confirmationText || confirmationText !== expectedConfirmation) {
    const errorParam = confirmationText 
      ? 'Please type "DELETE" exactly as shown'
      : 'Please type "DELETE" to confirm';
    redirect(`?error=${encodeURIComponent(errorParam)}&confirmationText=${encodeURIComponent(confirmationText || '')}`);
  }
  
  try {
    await categoryService.delete(categoryId);
    revalidatePath('/admin/categories');
    redirect('/admin/categories?deleted=true');
  } catch (error) {
    const errorMessage = error instanceof Error ? error.message : 'Failed to delete category';
    redirect(`?error=${encodeURIComponent(errorMessage)}&confirmationText=${encodeURIComponent(confirmationText)}`);
  }
}

export default function DeleteCategoryForm({ category, searchParams }: Props) {
  const expectedConfirmation = 'DELETE';
  
  const confirmationText = (searchParams?.confirmationText as string) || '';
  const error = searchParams?.error as string;
  const isConfirmationValid = confirmationText === expectedConfirmation;
  
  const deleteAction = deleteCategory.bind(null, category.id);

  return (
    <div>
      {error && (
        <div className="alert alert-danger d-flex align-items-center mb-4" role="alert">
          <i className="bi bi-exclamation-triangle-fill me-2"></i>
          <div>{error}</div>
        </div>
      )}

      <form action={deleteAction}>
        <div className="mb-4">
          <h6 className="fw-semibold mb-3">
            <i className="bi bi-shield-exclamation me-2"></i>
            Confirmation Required
          </h6>
          <p className="text-muted mb-3">
            To confirm deletion, please type <strong>{expectedConfirmation}</strong> in the field below:
          </p>
          <input
            type="text"
            name="confirmationText"
            className={`form-control ${confirmationText && !isConfirmationValid ? 'is-invalid' : ''}`}
            placeholder={`Type "${expectedConfirmation}" to confirm`}
            defaultValue={confirmationText}
            required
          />
          {confirmationText && !isConfirmationValid && (
            <div className="invalid-feedback">
              Please type "{expectedConfirmation}" exactly as shown
            </div>
          )}
        </div>

        <div className="d-flex gap-3">
          <button
            type="submit"
            className="btn btn-danger flex-fill"
            disabled={!isConfirmationValid}
          >
            <i className="bi bi-trash me-2"></i>
            Delete Category
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

      <div className="mt-4 p-3 bg-light rounded">
        <h6 className="text-danger mb-2">
          <i className="bi bi-exclamation-triangle me-2"></i>
          This action will:
        </h6>
        <ul className="text-muted mb-0 small">
          <li>Permanently delete the category "{category.name}"</li>
          <li>Remove all references to this category</li>
          <li>This action cannot be undone</li>
        </ul>
      </div>
    </div>
  );
}