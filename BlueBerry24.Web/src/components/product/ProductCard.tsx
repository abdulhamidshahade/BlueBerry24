import { ProductDto } from "@/types/product";
import Link from "next/link";
import AddToCartForm from "@/components/cart/AddToCartForm";

interface ProductCardProps {
  product: ProductDto;
  showAdminActions?: boolean;
  onDelete?: (id: number) => void;
}

export default function ProductCard({
  product,
  showAdminActions = false,
}: ProductCardProps) {
  const stockStatus =
    product.stockQuantity <= product.lowStockThreshold
      ? "danger"
      : product.stockQuantity <= product.lowStockThreshold * 2
      ? "warning"
      : "success";

  const stockIcon =
    stockStatus === "danger"
      ? "bi-exclamation-triangle-fill"
      : stockStatus === "warning"
      ? "bi-exclamation-circle-fill"
      : "bi-check-circle-fill";

  return (
    <div className="col">
      <div className="card h-100 shadow-sm">
        {product.imageUrl && (
          <div
            className="card-img-container"
            style={{ height: "200px", overflow: "hidden" }}
          >
            <img
              src={product.imageUrl}
              className="card-img-top"
              alt={product.name}
              style={{
                width: "100%",
                height: "100%",
                objectFit: "cover",
              }}
            />
          </div>
        )}

        <div className="card-body d-flex flex-column">
          <div className="d-flex justify-content-between align-items-start mb-2">
            <h5 className="card-title mb-0">{product.name}</h5>
            {!product.isActive && (
              <span className="badge bg-secondary">Inactive</span>
            )}
          </div>

          <p className="card-text text-muted mb-2">
            <small>SKU: {product.sku}</small>
          </p>

          <p className="card-text flex-grow-1">{product.description}</p>

          <div className="mb-3">
            <div className="d-flex justify-content-between align-items-center mb-2">
              <span className="h4 text-primary mb-0">
                ${product.price.toFixed(2)}
              </span>
              <span className={`badge bg-${stockStatus}`}>
                <i className={`bi ${stockIcon} me-1`}></i>
                {product.stockQuantity} in stock
              </span>
            </div>

            {showAdminActions && (
              <div className="d-flex gap-1 mb-2">
                <small className="text-muted">
                  Reserved: {product.reservedStock} | Low Stock Alert:{" "}
                  {product.lowStockThreshold}
                </small>
              </div>
            )}
          </div>

          <div className="d-flex gap-2">
            {showAdminActions ? (
              <>
                <a
                  href={`/admin/products/update/${product.id}`}
                  className="btn btn-outline-warning btn-sm flex-fill"
                >
                  <i className="bi bi-pencil-square me-1"></i>Edit
                </a>
                <a
                  href={`/admin/products/delete/${product.id}`}
                  className="btn btn-outline-danger btn-sm flex-fill"
                >
                  <i className="bi bi-trash me-1"></i>Delete
                </a>
              </>
            ) : (
              <>
                <div className="flex-fill">
                  <AddToCartForm
                    productId={product.id}
                    availableStock={product.stockQuantity - product.reservedStock}
                    isInStock={product.isActive && product.stockQuantity > product.reservedStock}
                    buttonSize="sm"
                    className="btn btn-primary w-100"
                  />
                </div>
                <Link
                  href={`/products/${product.id}`}
                  className="btn btn-outline-secondary btn-sm"
                >
                  <i className="bi bi-eye me-1"></i>View
                </Link>
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
