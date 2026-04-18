import Link from 'next/link';

interface Props {
  categoryId: number;
}

export default function CategoryActionButtons({ categoryId }: Props) {
  return (
    <div className="btn-group" role="group">
      <Link 
        href={`/admin/categories/update/${categoryId}`}
        className="btn btn-outline-primary btn-sm"
        title="Edit Category"
      >
        <i className="bi bi-pencil"></i>
      </Link>
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