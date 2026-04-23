import Link from 'next/link';

interface Props {
  categoryId: number;
  /** When true, Edit is disabled (read-only catalog). */
  editDisabled?: boolean;
}

export default function CategoryActionButtons({ categoryId, editDisabled = false }: Props) {
  return (
    <div className="btn-group" role="group">
      {editDisabled ? (
        <button
          type="button"
          disabled
          className="btn btn-outline-primary btn-sm"
          title="Editing categories is disabled"
        >
          <i className="bi bi-pencil"></i>
        </button>
      ) : (
        <Link
          href={`/admin/categories/update/${categoryId}`}
          className="btn btn-outline-primary btn-sm"
          title="Edit Category"
        >
          <i className="bi bi-pencil"></i>
        </Link>
      )}
      <Link 
        href={`/admin/categories/delete/${categoryId}`}
        className="btn btn-outline-danger btn-sm"
        title="Delete Category"
      >
        <i className="bi bi-trash"></i>
      </Link>
    </div>
  );
} 