import { OrderService } from '@/lib/services/order/service';
import { formatCurrency } from '@/lib/utils/formatCurrency';
import { format } from 'date-fns';
import Link from 'next/link';
import { OrderStatus } from '@/types/order';
import { 
  StatusUpdateModal, 
  CancelOrderModal, 
  RefundOrderModal, 
  MarkPaidModal,
  ProcessOrderModal 
} from '@/components/admin/OrderActionModals';
import { ExportOrdersModal } from '@/components/admin/ExportOrdersModal';

interface SearchParams {
  success?: string;
  error?: string;
  status?: string;
}

function SuccessAlert({ message }: { message: string }) {
  const messages = {
    'status-updated': 'Order status updated successfully!',
    'order-cancelled': 'Order cancelled successfully!',
    'order-refunded': 'Order refunded successfully!',
    'order-marked-paid': 'Order marked as paid successfully!',
    'order-processed': 'Order processed successfully!',
    'export-prepared': 'Export prepared successfully!'
  };

  return (
    <div className="alert alert-success alert-dismissible fade show" role="alert">
      <i className="bi bi-check-circle-fill me-2"></i>
      {messages[message as keyof typeof messages] || message}
      <button type="button" className="btn-close" data-bs-dismiss="alert"></button>
    </div>
  );
}

function ErrorAlert({ message }: { message: string }) {
  const messages = {
    'status-update-failed': 'Failed to update order status. Please try again.',
    'cancel-failed': 'Failed to cancel order. Please try again.',
    'refund-failed': 'Failed to refund order. Please try again.',
    'mark-paid-failed': 'Failed to mark order as paid. Please try again.',
    'process-failed': 'Failed to process order. Please try again.',
    'export-failed': 'Failed to export orders. Please try again.'
  };

  return (
    <div className="alert alert-danger alert-dismissible fade show" role="alert">
      <i className="bi bi-exclamation-circle-fill me-2"></i>
      {messages[message as keyof typeof messages] || message}
      <button type="button" className="btn-close" data-bs-dismiss="alert"></button>
    </div>
  );
}

export default async function AdminOrdersPage({
  searchParams,
}: {
  searchParams: SearchParams
}) {
  const { success, error, status } = searchParams;
  const orderService = new OrderService();
  
  let orders;
  if (status && status !== 'all') {
    orders = await orderService.getOrdersByStatus(parseInt(status) as OrderStatus, 1, 50);
  } else {
    orders = await orderService.getAllOrders(1, 50);
  }

  return (
    <div className="container-fluid py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h1 className="h3 fw-bold text-dark mb-1">Orders Management</h1>
          <p className="text-muted mb-0">Manage all customer orders across the platform</p>
        </div>
        <div className="d-flex gap-2">
          <div className="dropdown">
            <button className="btn btn-outline-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown">
              <i className="bi bi-funnel me-2"></i>Filter Status
            </button>
            <ul className="dropdown-menu">
              <li><Link className="dropdown-item" href="/admin/orders">All Orders</Link></li>
              <li><Link className="dropdown-item" href="/admin/orders?status=0">Pending</Link></li>
              <li><Link className="dropdown-item" href="/admin/orders?status=1">Processing</Link></li>
              <li><Link className="dropdown-item" href="/admin/orders?status=2">Shipped</Link></li>
              <li><Link className="dropdown-item" href="/admin/orders?status=3">Delivered</Link></li>
              <li><Link className="dropdown-item" href="/admin/orders?status=4">Completed</Link></li>
              <li><Link className="dropdown-item" href="/admin/orders?status=5">Cancelled</Link></li>
              <li><Link className="dropdown-item" href="/admin/orders?status=6">Refunded</Link></li>
            </ul>
          </div>
          <button className="btn btn-primary" data-bs-toggle="modal" data-bs-target="#exportModal">
            <i className="bi bi-download me-2"></i>Export
          </button>
        </div>
      </div>

      {success && <SuccessAlert message={success} />}
      {error && <ErrorAlert message={error} />}

      {/* Order Statistics Cards */}
      <div className="row mb-4">
        <div className="col-md-3">
          <div className="card border-0 shadow-sm">
            <div className="card-body">
              <div className="d-flex align-items-center">
                <div className="bg-primary bg-opacity-10 rounded-circle p-3 me-3">
                  <i className="bi bi-receipt text-primary fs-4"></i>
                </div>
                <div>
                  <h5 className="card-title mb-1">{orders.length}</h5>
                  <p className="text-muted small mb-0">Total Orders</p>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-md-3">
          <div className="card border-0 shadow-sm">
            <div className="card-body">
              <div className="d-flex align-items-center">
                <div className="bg-warning bg-opacity-10 rounded-circle p-3 me-3">
                  <i className="bi bi-clock text-warning fs-4"></i>
                </div>
                <div>
                  <h5 className="card-title mb-1">{orders.filter(o => o.status === OrderStatus.Pending).length}</h5>
                  <p className="text-muted small mb-0">Pending Orders</p>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-md-3">
          <div className="card border-0 shadow-sm">
            <div className="card-body">
              <div className="d-flex align-items-center">
                <div className="bg-success bg-opacity-10 rounded-circle p-3 me-3">
                  <i className="bi bi-check-circle text-success fs-4"></i>
                </div>
                <div>
                  <h5 className="card-title mb-1">{orders.filter(o => o.status === OrderStatus.Completed).length}</h5>
                  <p className="text-muted small mb-0">Completed Orders</p>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-md-3">
          <div className="card border-0 shadow-sm">
            <div className="card-body">
              <div className="d-flex align-items-center">
                <div className="bg-info bg-opacity-10 rounded-circle p-3 me-3">
                  <i className="bi bi-currency-dollar text-info fs-4"></i>
                </div>
                <div>
                  <h5 className="card-title mb-1">{formatCurrency(orders.reduce((total, order) => total + order.total, 0))}</h5>
                  <p className="text-muted small mb-0">Total Revenue</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      {orders.length === 0 ? (
        <div className="card border-0 shadow-sm">
          <div className="card-body text-center py-5">
            <div className="mb-4">
              <i className="bi bi-receipt display-1 text-muted"></i>
            </div>
            <h3 className="h4 text-dark mb-2">No orders found</h3>
            <p className="text-muted mb-0">Orders will appear here once customers start placing them.</p>
          </div>
        </div>
      ) : (
        <div className="card border-0 shadow-sm">
          <div className="card-header bg-white border-bottom">
            <div className="row align-items-center">
              <div className="col">
                <h5 className="card-title mb-0">Orders List</h5>
              </div>
              <div className="col-auto">
                <div className="d-flex gap-2">
                  <input 
                    type="search" 
                    className="form-control form-control-sm" 
                    placeholder="Search orders..."
                    style={{ width: '200px' }}
                  />
                  <button className="btn btn-outline-secondary btn-sm">
                    <i className="bi bi-search"></i>
                  </button>
                </div>
              </div>
            </div>
          </div>
          <div className="table-responsive">
            <table className="table table-hover mb-0">
              <thead className="table-light">
                <tr>
                  <th className="border-0 fw-semibold">Order ID</th>
                  <th className="border-0 fw-semibold">Customer</th>
                  <th className="border-0 fw-semibold">Date</th>
                  <th className="border-0 fw-semibold">Items</th>
                  <th className="border-0 fw-semibold">Total</th>
                  <th className="border-0 fw-semibold">Status</th>
                  <th className="border-0 fw-semibold">Payment</th>
                  <th className="border-0 fw-semibold">Actions</th>
                </tr>
              </thead>
              <tbody>
                {orders.map((order) => (
                  <tr key={order.id}>
                    <td className="align-middle">
                      <div>
                        <div className="fw-semibold">#{order.referenceNumber || order.id}</div>
                        <small className="text-muted">ID: {order.id}</small>
                      </div>
                    </td>
                    <td className="align-middle">
                      <div>
                        <div className="fw-semibold">{order.shippingName || 'N/A'}</div>
                        <small className="text-muted">{order.customerEmail}</small>
                      </div>
                    </td>
                    <td className="align-middle">
                      <div>
                        <div>{format(new Date(order.createdAt), 'MMM dd, yyyy')}</div>
                        <small className="text-muted">{format(new Date(order.createdAt), 'hh:mm a')}</small>
                      </div>
                    </td>
                    <td className="align-middle">
                      <span className="badge bg-light text-dark">
                        {order.orderItems?.length || 0} items
                      </span>
                    </td>
                    <td className="align-middle">
                      <div className="fw-semibold">{formatCurrency(order.total)}</div>
                    </td>
                    <td className="align-middle">
                      <span 
                        className="badge px-2 py-1"
                        style={{
                          backgroundColor: getStatusColor(order.status),
                          color: 'white'
                        }}
                      >
                        {getStatusText(order.status)}
                      </span>
                    </td>
                    <td className="align-middle">
                      {order.isPaid ? (
                        <span className="badge bg-success">
                          <i className="bi bi-check-circle me-1"></i>Paid
                        </span>
                      ) : (
                        <span className="badge bg-warning text-dark">
                          <i className="bi bi-clock me-1"></i>Pending
                        </span>
                      )}
                    </td>
                    <td className="align-middle">
                      <div className="dropdown">
                        <button 
                          className="btn btn-outline-secondary btn-sm dropdown-toggle" 
                          type="button" 
                          data-bs-toggle="dropdown"
                        >
                          Actions
                        </button>
                        <ul className="dropdown-menu">
                          <li>
                            <Link className="dropdown-item" href={`/admin/orders/${order.id}`}>
                              <i className="bi bi-eye me-2"></i>View Details
                            </Link>
                          </li>
                          <li>
                            <button 
                              className="dropdown-item" 
                              type="button"
                              data-bs-toggle="modal" 
                              data-bs-target={`#statusModal-${order.id}`}
                            >
                              <i className="bi bi-pencil me-2"></i>Edit Status
                            </button>
                          </li>
                          {order.status === OrderStatus.Pending && (
                            <li>
                              <ProcessOrderModal orderId={order.id} />
                            </li>
                          )}
                          {!order.isPaid && (
                            <li>
                              <button 
                                className="dropdown-item text-success" 
                                type="button"
                                data-bs-toggle="modal" 
                                data-bs-target={`#markPaidModal-${order.id}`}
                              >
                                <i className="bi bi-credit-card me-2"></i>Mark as Paid
                              </button>
                            </li>
                          )}
                          {(order.status === OrderStatus.Pending || order.status === OrderStatus.Processing) && (
                            <li>
                              <button 
                                className="dropdown-item text-danger" 
                                type="button"
                                data-bs-toggle="modal" 
                                data-bs-target={`#cancelModal-${order.id}`}
                              >
                                <i className="bi bi-x-circle me-2"></i>Cancel Order
                              </button>
                            </li>
                          )}
                          {(order.status === OrderStatus.Completed || order.status === OrderStatus.Delivered) && order.isPaid && (
                            <li>
                              <button 
                                className="dropdown-item text-warning" 
                                type="button"
                                data-bs-toggle="modal" 
                                data-bs-target={`#refundModal-${order.id}`}
                              >
                                <i className="bi bi-arrow-counterclockwise me-2"></i>Refund Order
                              </button>
                            </li>
                          )}
                          <li><hr className="dropdown-divider" /></li>
                          <li>
                            <button className="dropdown-item" type="button">
                              <i className="bi bi-printer me-2"></i>Print Invoice
                            </button>
                          </li>
                          <li>
                            <button className="dropdown-item" type="button">
                              <i className="bi bi-envelope me-2"></i>Send Email
                            </button>
                          </li>
                        </ul>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
          <div className="card-footer bg-white border-top">
            <div className="d-flex justify-content-between align-items-center">
              <div className="text-muted small">
                Showing {orders.length} orders
              </div>
              <nav>
                <ul className="pagination pagination-sm mb-0">
                  <li className="page-item disabled">
                    <span className="page-link">Previous</span>
                  </li>
                  <li className="page-item active">
                    <span className="page-link">1</span>
                  </li>
                  <li className="page-item">
                    <Link className="page-link" href="/admin/orders?page=2">2</Link>
                  </li>
                  <li className="page-item">
                    <Link className="page-link" href="/admin/orders?page=3">3</Link>
                  </li>
                  <li className="page-item">
                    <Link className="page-link" href="/admin/orders?page=2">Next</Link>
                  </li>
                </ul>
              </nav>
            </div>
          </div>
        </div>
      )}

      {/* Modals for each order */}
      {orders.map((order) => (
        <div key={`modals-${order.id}`}>
          <StatusUpdateModal orderId={order.id} currentStatus={order.status} />
          <CancelOrderModal orderId={order.id} />
          <RefundOrderModal orderId={order.id} />
          <MarkPaidModal orderId={order.id} />
        </div>
      ))}

      {/* Export Modal */}
      <ExportOrdersModal />
    </div>
  );
}

export function getStatusColor(status: OrderStatus): string {
  const colors: { [key in OrderStatus]: string } = {
    [OrderStatus.Pending]: '#F59E0B',      // Amber
    [OrderStatus.Processing]: '#3B82F6',   // Blue
    [OrderStatus.Shipped]: '#10B981',      // Emerald
    [OrderStatus.Delivered]: '#059669',    // Green
    [OrderStatus.Completed]: '#059669',    // Green
    [OrderStatus.Cancelled]: '#EF4444',    // Red
    [OrderStatus.Refunded]: '#6B7280',     // Gray
  };
  return colors[status] || '#6B7280';
}

export function getStatusText(status: OrderStatus): string {
  const statusTexts: { [key in OrderStatus]: string } = {
    [OrderStatus.Pending]: 'Pending',
    [OrderStatus.Processing]: 'Processing',
    [OrderStatus.Shipped]: 'Shipped',
    [OrderStatus.Delivered]: 'Delivered',
    [OrderStatus.Completed]: 'Completed',
    [OrderStatus.Cancelled]: 'Cancelled',
    [OrderStatus.Refunded]: 'Refunded',
  };
  return statusTexts[status] || 'Unknown';
} 