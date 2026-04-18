import { ProductDto } from "../../types/product";
import { InventoryChangeType, InventoryLogDto } from "../../types/inventory";
import { addStock, quickRestock, updateLowStockThreshold, releaseReservedStock } from "../../lib/actions/inventory-actions";
import Link from 'next/link';

interface InventoryManagerProps {
  product: ProductDto;
  inventoryHistory?: InventoryLogDto[];
}

function getChangeTypeIcon(changeType: InventoryChangeType): string {
  switch (changeType) {
    case InventoryChangeType.Restock:
      return 'bi-arrow-up-circle-fill text-success';
    case InventoryChangeType.Purchase:
      return 'bi-cart-fill text-primary';
    case InventoryChangeType.Return:
      return 'bi-arrow-clockwise text-info';
    case InventoryChangeType.StockAdjustment:
      return 'bi-gear-fill text-warning';
    case InventoryChangeType.Damaged:
      return 'bi-exclamation-triangle-fill text-danger';
    case InventoryChangeType.Reserved:
      return 'bi-lock-fill text-secondary';
    case InventoryChangeType.ReleaseReservation:
      return 'bi-unlock-fill text-secondary';
    case InventoryChangeType.InitialStock:
      return 'bi-plus-circle-fill text-primary';
    default:
      return 'bi-circle-fill text-muted';
  }
}

function getChangeTypeName(changeType: InventoryChangeType): string {
  switch (changeType) {
    case InventoryChangeType.Restock:
      return 'Restock';
    case InventoryChangeType.Purchase:
      return 'Purchase/Sale';
    case InventoryChangeType.Return:
      return 'Return';
    case InventoryChangeType.StockAdjustment:
      return 'Manual Adjustment';
    case InventoryChangeType.Damaged:
      return 'Damaged/Lost';
    case InventoryChangeType.Reserved:
      return 'Reserved';
    case InventoryChangeType.ReleaseReservation:
      return 'Released Reservation';
    case InventoryChangeType.InitialStock:
      return 'Initial Stock';
    default:
      return 'Unknown';
  }
}

function formatQuantityChange(quantity: number): { text: string; className: string } {
  if (quantity > 0) {
    return {
      text: `+${quantity}`,
      className: 'text-success fw-bold'
    };
  } else if (quantity < 0) {
    return {
      text: `${quantity}`,
      className: 'text-danger fw-bold'
    };
  } else {
    return {
      text: '0',
      className: 'text-muted'
    };
  }
}

export default function InventoryManager({ product, inventoryHistory = [] }: InventoryManagerProps) {
  const stockStatus = product.stockQuantity <= product.lowStockThreshold 
    ? product.stockQuantity === 0 ? 'danger' : 'warning'
    : 'success';

  const availableStock = product.stockQuantity - product.reservedStock;

  return (
    <div className="modal fade" id={`inventoryModal${product.id}`} tabIndex={-1} aria-labelledby={`inventoryModalLabel${product.id}`} aria-hidden="true">
      <div className="modal-dialog modal-xl">
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title" id={`inventoryModalLabel${product.id}`}>
              <i className="bi bi-boxes me-2"></i>
              Manage Inventory: {product.name}
            </h5>
            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
          </div>
          <div className="modal-body">
            <div className="row mb-4">
              <div className="col-12">
                <div className="card bg-light">
                  <div className="card-body">
                    <div className="row">
                      <div className="col-md-3">
                        <strong>SKU:</strong><br />
                        <code>{product.sku}</code>
                      </div>
                      <div className="col-md-3">
                        <strong>Current Stock:</strong><br />
                        <span className={`badge bg-${stockStatus} fs-6`}>{product.stockQuantity}</span>
                      </div>
                      <div className="col-md-3">
                        <strong>Available:</strong><br />
                        <span className="badge bg-primary fs-6">{availableStock}</span>
                      </div>
                      <div className="col-md-3">
                        <strong>Reserved:</strong><br />
                        <span className="badge bg-secondary fs-6">{product.reservedStock}</span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>

            {product.stockQuantity <= product.lowStockThreshold && (
              <div className="alert alert-warning d-flex align-items-center mb-4" role="alert">
                <i className="bi bi-exclamation-triangle-fill me-2"></i>
                <div>
                  <strong>Low Stock Warning!</strong> This product is below the low stock threshold of {product.lowStockThreshold} units.
                </div>
              </div>
            )}

            <div className="row">
              <div className="col-md-6">
                <h6>Adjust Stock</h6>
                <form action={addStock}>
                  <input type="hidden" name="productId" value={product.id} />
                  
                  <div className="mb-3">
                    <label htmlFor={`adjustmentType${product.id}`} className="form-label">Adjustment Type</label>
                    <select className="form-select" id={`adjustmentType${product.id}`} name="adjustmentType" required>
                      <option value="">Select adjustment type</option>
                      <option value={InventoryChangeType.Restock}>Restock</option>
                      <option value={InventoryChangeType.StockAdjustment}>Manual Adjustment</option>
                      <option value={InventoryChangeType.Damaged}>Damaged/Lost</option>
                      <option value={InventoryChangeType.Return}>Return</option>
                    </select>
                  </div>
                  
                  <div className="mb-3">
                    <label htmlFor={`quantity${product.id}`} className="form-label">Quantity</label>
                    <input
                      type="number"
                      className="form-control"
                      id={`quantity${product.id}`}
                      name="quantity"
                      min="1"
                      required
                    />
                  </div>
                  
                  <div className="mb-3">
                    <label htmlFor={`notes${product.id}`} className="form-label">Notes</label>
                    <textarea
                      className="form-control"
                      id={`notes${product.id}`}
                      name="notes"
                      rows={3}
                      placeholder="Optional notes about this adjustment"
                    ></textarea>
                  </div>
                  
                  <button type="submit" className="btn btn-primary w-100">
                    <i className="bi bi-plus-circle me-2"></i>
                    Apply Adjustment
                  </button>
                </form>
              </div>

              <div className="col-md-6">
                <h6>Quick Actions</h6>
                <div className="d-grid gap-2 mb-4">
                  <form action={quickRestock}>
                    <input type="hidden" name="productId" value={product.id} />
                    <input type="hidden" name="quantity" value={product.lowStockThreshold * 2} />
                    <button type="submit" className="btn btn-outline-success w-100">
                      <i className="bi bi-arrow-up-circle me-2"></i>
                      Restock to {product.lowStockThreshold * 2} units
                    </button>
                  </form>

                  <form action={updateLowStockThreshold}>
                    <input type="hidden" name="productId" value={product.id} />
                    <input type="hidden" name="lowStockThreshold" value={Math.max(1, Math.floor(product.stockQuantity * 0.2))} />
                    <button type="submit" className="btn btn-outline-info w-100">
                      <i className="bi bi-gear me-2"></i>
                      Update Low Stock Alert
                    </button>
                  </form>

                  <form action={releaseReservedStock}>
                    <input type="hidden" name="productId" value={product.id} />
                    <input type="hidden" name="quantity" value={product.reservedStock} />
                    <input type="hidden" name="referenceId" value="0" />
                    <input type="hidden" name="referenceType" value="manual" />
                    <button 
                      type="submit" 
                      className="btn btn-outline-warning w-100" 
                      disabled={product.reservedStock === 0}
                    >
                      <i className="bi bi-unlock me-2"></i>
                      Release Reserved Stock ({product.reservedStock})
                    </button>
                  </form>

                  <Link 
                    href={`/admin/products/${product.id}/edit`}
                    className="btn btn-outline-secondary w-100"
                  >
                    <i className="bi bi-pencil-square me-2"></i>
                    Edit Product Details
                  </Link>

                  <Link 
                    href={`/admin/inventory/history/${product.id}`}
                    className="btn btn-outline-primary w-100"
                  >
                    <i className="bi bi-clock-history me-2"></i>
                    View Full History
                  </Link>
                </div>
              </div>
            </div>

            <div className="row mt-4">
              <div className="col-12">
                <div className="card">
                  <div className="card-header">
                    <h6 className="mb-0">
                      <i className="bi bi-clock-history me-2"></i>
                      Recent Inventory Activity
                    </h6>
                  </div>
                  <div className="card-body">
                    {inventoryHistory.length > 0 ? (
                      <div className="table-responsive">
                        <table className="table table-sm table-hover">
                          <thead>
                            <tr>
                              <th>Date</th>
                              <th>Type</th>
                              <th>Change</th>
                              <th>Stock After</th>
                              <th>Notes</th>
                            </tr>
                          </thead>
                          <tbody>
                            {inventoryHistory.map((log) => {
                              const quantityChange = formatQuantityChange(log.quantityChanged);
                              return (
                                <tr key={log.id}>
                                  <td>
                                    <small>
                                      {new Date(log.createdAt).toLocaleDateString()}<br />
                                      {new Date(log.createdAt).toLocaleTimeString()}
                                    </small>
                                  </td>
                                  <td>
                                    <div className="d-flex align-items-center">
                                      <i className={`${getChangeTypeIcon(log.changeType)} me-1`} style={{fontSize: '0.8rem'}}></i>
                                      <small>{getChangeTypeName(log.changeType)}</small>
                                    </div>
                                  </td>
                                  <td>
                                    <span className={quantityChange.className}>
                                      {quantityChange.text}
                                    </span>
                                  </td>
                                  <td>
                                    <strong>{log.currentStockQuantity}</strong>
                                  </td>
                                  <td>
                                    <small className="text-muted">
                                      {log.notes ? log.notes.substring(0, 50) + (log.notes.length > 50 ? '...' : '') : '-'}
                                    </small>
                                  </td>
                                </tr>
                              );
                            })}
                          </tbody>
                        </table>
                      </div>
                    ) : (
                      <div className="text-center py-4">
                        <i className="bi bi-clock-history text-muted" style={{ fontSize: '2rem' }}></i>
                        <p className="text-muted mt-2 mb-0">No inventory activity yet</p>
                        <small className="text-muted">
                          Inventory transactions will appear here once you make stock adjustments.
                        </small>
                      </div>
                    )}
                  </div>
                </div>
              </div>
            </div>
          </div>
          
          <div className="modal-footer">
            <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Close</button>
          </div>
        </div>
      </div>
    </div>
  );
} 