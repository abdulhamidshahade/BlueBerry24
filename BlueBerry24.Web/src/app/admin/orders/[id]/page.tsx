import { OrderService } from '@/lib/services/order/service';
import { formatCurrency } from '@/lib/utils/formatCurrency';
import { format } from 'date-fns';
import Link from 'next/link';
import { OrderStatus } from '@/types/order';
import { notFound } from 'next/navigation';
import { 
  StatusUpdateModal, 
  CancelOrderModal, 
  RefundOrderModal, 
  MarkPaidModal 
} from '@/components/admin/OrderActionModals';

interface SearchParams {
  success?: string;
  error?: string;
}

interface Props {
  params: { id: string };
  searchParams: SearchParams;
}

function SuccessAlert({ message }: { message: string }) {
  const messages = {
    'status-updated': 'Order status updated successfully!',
    'order-cancelled': 'Order cancelled successfully!',
    'order-refunded': 'Order refunded successfully!',
    'order-marked-paid': 'Order marked as paid successfully!',
    'order-processed': 'Order processed successfully!'
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
    'process-failed': 'Failed to process order. Please try again.'
  };

  return (
    <div className="alert alert-danger alert-dismissible fade show" role="alert">
      <i className="bi bi-exclamation-circle-fill me-2"></i>
      {messages[message as keyof typeof messages] || message}
      <button type="button" className="btn-close" data-bs-dismiss="alert"></button>
    </div>
  );
}

export default async function OrderDetailsPage({ params, searchParams }: Props) {
  const { success, error } = searchParams;
  const orderId = parseInt(params.id);

  if (isNaN(orderId)) {
    notFound();
  }

  const orderService = new OrderService();
  
  let order;
  try {
    order = await orderService.getById(orderId);
  } catch (error) {
    notFound();
  }

  if (!order) {
    notFound();
  }

  return (
    <div className="container-fluid py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <nav aria-label="breadcrumb">
            <ol className="breadcrumb mb-2">
              <li className="breadcrumb-item">
                <Link href="/admin" className="text-decoration-none">Admin</Link>
              </li>
              <li className="breadcrumb-item">
                <Link href="/admin/orders" className="text-decoration-none">Orders</Link>
              </li>
              <li className="breadcrumb-item active">
                Order #{order.referenceNumber || order.id}
              </li>
            </ol>
          </nav>
          <h1 className="h3 fw-bold text-dark mb-1">
            Order Details #{order.referenceNumber || order.id}
          </h1>
          <p className="text-muted mb-0">
            Created on {format(new Date(order.createdAt), 'EEEE, MMMM dd, yyyy \'at\' hh:mm a')}
          </p>
        </div>
        <div className="d-flex gap-2">
          <Link href="/admin/orders" className="btn btn-outline-secondary">
            <i className="bi bi-arrow-left me-2"></i>Back to Orders
          </Link>
          <div className="dropdown">
            <button 
              className="btn btn-primary dropdown-toggle" 
              type="button" 
              data-bs-toggle="dropdown"
            >
              <i className="bi bi-gear me-2"></i>Actions
            </button>
            <ul className="dropdown-menu">
              <li>
                <button 
                  className="dropdown-item" 
                  type="button"
                  data-bs-toggle="modal" 
                  data-bs-target={`#statusModal-${order.id}`}
                >
                  <i className="bi bi-pencil me-2"></i>Update Status
                </button>
              </li>
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
                    <i className="bi bi-arrow-counterclockwise me-2"></i>Process Refund
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
                  <i className="bi bi-envelope me-2"></i>Send Email to Customer
                </button>
              </li>
            </ul>
          </div>
        </div>
      </div>

      {success && <SuccessAlert message={success} />}
      {error && <ErrorAlert message={error} />}

      <div className="row">
        <div className="col-lg-8">
          <div className="card border-0 shadow-sm mb-4">
            <div className="card-header bg-white border-bottom">
              <h5 className="card-title mb-0">
                <i className="bi bi-receipt me-2"></i>Order Overview
              </h5>
            </div>
            <div className="card-body">
              <div className="row mb-4">
                <div className="col-md-6">
                  <div className="d-flex align-items-center mb-3">
                    <div className="bg-primary bg-opacity-10 rounded-circle p-2 me-3">
                      <i className="bi bi-hash text-primary"></i>
                    </div>
                    <div>
                      <div className="small text-muted">Order ID</div>
                      <div className="fw-semibold">{order.id}</div>
                    </div>
                  </div>
                  <div className="d-flex align-items-center mb-3">
                    <div className="bg-info bg-opacity-10 rounded-circle p-2 me-3">
                      <i className="bi bi-tag text-info"></i>
                    </div>
                    <div>
                      <div className="small text-muted">Reference Number</div>
                      <div className="fw-semibold">{order.referenceNumber || 'N/A'}</div>
                    </div>
                  </div>
                </div>
                <div className="col-md-6">
                  <div className="d-flex align-items-center mb-3">
                    <div className="bg-warning bg-opacity-10 rounded-circle p-2 me-3">
                      <i className="bi bi-clock text-warning"></i>
                    </div>
                    <div>
                      <div className="small text-muted">Status</div>
                      <span 
                        className="badge px-2 py-1"
                        style={{
                          backgroundColor: getStatusColor(order.status),
                          color: 'white'
                        }}
                      >
                        {getStatusText(order.status)}
                      </span>
                    </div>
                  </div>
                  <div className="d-flex align-items-center mb-3">
                    <div className="bg-success bg-opacity-10 rounded-circle p-2 me-3">
                      <i className="bi bi-credit-card text-success"></i>
                    </div>
                    <div>
                      <div className="small text-muted">Payment Status</div>
                      {order.isPaid ? (
                        <span className="badge bg-success">
                          <i className="bi bi-check-circle me-1"></i>Paid
                        </span>
                      ) : (
                        <span className="badge bg-warning text-dark">
                          <i className="bi bi-clock me-1"></i>Pending
                        </span>
                      )}
                    </div>
                  </div>
                </div>
              </div>

              {order.isPaid && order.paidAt && (
                <div className="alert alert-success border-0">
                  <i className="bi bi-check-circle me-2"></i>
                  Payment received on {format(new Date(order.paidAt), 'MMMM dd, yyyy \'at\' hh:mm a')}
                  {order.paymentProvider && (
                    <span className="ms-2">via <strong>{order.paymentProvider}</strong></span>
                  )}
                </div>
              )}
            </div>
          </div>

          <div className="card border-0 shadow-sm mb-4">
            <div className="card-header bg-white border-bottom">
              <h5 className="card-title mb-0">
                <i className="bi bi-box-seam me-2"></i>Order Items ({order.orderItems?.length || 0})
              </h5>
            </div>
            <div className="table-responsive">
              <table className="table table-hover mb-0">
                <thead className="table-light">
                  <tr>
                    <th className="border-0">Product</th>
                    <th className="border-0">Unit Price</th>
                    <th className="border-0">Quantity</th>
                    <th className="border-0">Discount</th>
                    <th className="border-0 text-end">Total</th>
                  </tr>
                </thead>
                <tbody>
                  {order.orderItems?.map((item) => (
                    <tr key={item.id}>
                      <td className="align-middle">
                        <div>
                          <div className="fw-semibold">{item.productName}</div>
                          <small className="text-muted">Product ID: {item.productId}</small>
                        </div>
                      </td>
                      <td className="align-middle">{formatCurrency(item.unitPrice)}</td>
                      <td className="align-middle">
                        <span className="badge bg-light text-dark">{item.quantity}</span>
                      </td>
                      <td className="align-middle">
                        {item.discountAmount > 0 ? (
                          <span className="text-success">-{formatCurrency(item.discountAmount)}</span>
                        ) : (
                          <span className="text-muted">No discount</span>
                        )}
                      </td>
                      <td className="align-middle text-end fw-semibold">
                        {formatCurrency(item.totalPrice)}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        </div>

        <div className="col-lg-4">
          <div className="card border-0 shadow-sm mb-4">
            <div className="card-header bg-white border-bottom">
              <h5 className="card-title mb-0">
                <i className="bi bi-person me-2"></i>Customer Information
              </h5>
            </div>
            <div className="card-body">
              <div className="mb-3">
                <div className="small text-muted mb-1">Customer Email</div>
                <div className="fw-semibold">{order.customerEmail}</div>
              </div>
              {order.customerPhone && (
                <div className="mb-3">
                  <div className="small text-muted mb-1">Phone Number</div>
                  <div className="fw-semibold">{order.customerPhone}</div>
                </div>
              )}
              <div className="mb-3">
                <div className="small text-muted mb-1">User ID</div>
                <div className="fw-semibold">{order.userId}</div>
              </div>
            </div>
          </div>

          <div className="card border-0 shadow-sm mb-4">
            <div className="card-header bg-white border-bottom">
              <h5 className="card-title mb-0">
                <i className="bi bi-truck me-2"></i>Shipping Information
              </h5>
            </div>
            <div className="card-body">
              {order.shippingName ? (
                <>
                  <div className="mb-3">
                    <div className="small text-muted mb-1">Ship To</div>
                    <div className="fw-semibold">{order.shippingName}</div>
                  </div>
                  <div className="mb-3">
                    <div className="small text-muted mb-1">Address</div>
                    <div>
                      {order.shippingAddress1}<br />
                      {order.shippingAddress2 && <>{order.shippingAddress2}<br /></>}
                      {order.shippingCity}, {order.shippingState} {order.shippingPostalCode}<br />
                      {order.shippingCountry}
                    </div>
                  </div>
                </>
              ) : (
                <div className="text-muted">No shipping information available</div>
              )}
            </div>
          </div>

          <div className="card border-0 shadow-sm">
            <div className="card-header bg-white border-bottom">
              <h5 className="card-title mb-0">
                <i className="bi bi-calculator me-2"></i>Order Summary
              </h5>
            </div>
            <div className="card-body">
              <div className="d-flex justify-content-between mb-2">
                <span>Subtotal:</span>
                <span>{formatCurrency(order.subTotal)}</span>
              </div>
              {order.discountTotal > 0 && (
                <div className="d-flex justify-content-between mb-2 text-success">
                  <span>Discount:</span>
                  <span>-{formatCurrency(order.discountTotal)}</span>
                </div>
              )}
              <div className="d-flex justify-content-between mb-2">
                <span>Tax:</span>
                <span>{formatCurrency(order.taxAmount)}</span>
              </div>
              <div className="d-flex justify-content-between mb-2">
                <span>Shipping:</span>
                <span>{formatCurrency(order.shippingAmount)}</span>
              </div>
              <hr />
              <div className="d-flex justify-content-between fw-bold fs-5">
                <span>Total:</span>
                <span>{formatCurrency(order.total)}</span>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="row mt-4">
        <div className="col-12">
          <div className="card border-0 shadow-sm">
            <div className="card-header bg-white border-bottom">
              <h5 className="card-title mb-0">
                <i className="bi bi-clock-history me-2"></i>Order Timeline
              </h5>
            </div>
            <div className="card-body">
              <div 
                className="timeline"
                style={{
                  position: 'relative',
                  paddingLeft: '30px'
                }}
              >
                <div
                  style={{
                    position: 'absolute',
                    left: '8px',
                    top: '0',
                    bottom: '0',
                    width: '2px',
                    background: '#e9ecef',
                    zIndex: 1
                  }}
                ></div>
                
                <div 
                  className="timeline-item"
                  style={{
                    position: 'relative',
                    marginBottom: '24px'
                  }}
                >
                  <div 
                    className="timeline-marker bg-primary"
                    style={{
                      position: 'absolute',
                      left: '-26px',
                      top: '4px',
                      width: '16px',
                      height: '16px',
                      borderRadius: '50%',
                      border: '3px solid #fff',
                      boxShadow: '0 0 0 2px #e9ecef'
                    }}
                  ></div>
                  <div 
                    className="timeline-content"
                    style={{
                      background: '#f8f9fa',
                      padding: '12px 16px',
                      borderRadius: '8px',
                      borderLeft: '3px solid #0d6efd'
                    }}
                  >
                    <div className="fw-semibold">Order Created</div>
                    <div className="text-muted small">
                      {format(new Date(order.createdAt), 'MMMM dd, yyyy \'at\' hh:mm a')}
                    </div>
                  </div>
                </div>
                
                {order.isPaid && order.paidAt && (
                  <div 
                    className="timeline-item"
                    style={{
                      position: 'relative',
                      marginBottom: '24px'
                    }}
                  >
                    <div 
                      className="timeline-marker bg-success"
                      style={{
                        position: 'absolute',
                        left: '-26px',
                        top: '4px',
                        width: '16px',
                        height: '16px',
                        borderRadius: '50%',
                        border: '3px solid #fff',
                        boxShadow: '0 0 0 2px #e9ecef'
                      }}
                    ></div>
                    <div 
                      className="timeline-content"
                      style={{
                        background: '#f8f9fa',
                        padding: '12px 16px',
                        borderRadius: '8px',
                        borderLeft: '3px solid #0d6efd'
                      }}
                    >
                      <div className="fw-semibold">Payment Received</div>
                      <div className="text-muted small">
                        {format(new Date(order.paidAt), 'MMMM dd, yyyy \'at\' hh:mm a')}
                      </div>
                    </div>
                  </div>
                )}

                {order.completedAt && (
                  <div 
                    className="timeline-item"
                    style={{
                      position: 'relative',
                      marginBottom: '24px'
                    }}
                  >
                    <div 
                      className="timeline-marker bg-success"
                      style={{
                        position: 'absolute',
                        left: '-26px',
                        top: '4px',
                        width: '16px',
                        height: '16px',
                        borderRadius: '50%',
                        border: '3px solid #fff',
                        boxShadow: '0 0 0 2px #e9ecef'
                      }}
                    ></div>
                    <div 
                      className="timeline-content"
                      style={{
                        background: '#f8f9fa',
                        padding: '12px 16px',
                        borderRadius: '8px',
                        borderLeft: '3px solid #0d6efd'
                      }}
                    >
                      <div className="fw-semibold">Order Completed</div>
                      <div className="text-muted small">
                        {format(new Date(order.completedAt), 'MMMM dd, yyyy \'at\' hh:mm a')}
                      </div>
                    </div>
                  </div>
                )}

                {order.cancelledAt && (
                  <div 
                    className="timeline-item"
                    style={{
                      position: 'relative',
                      marginBottom: '24px'
                    }}
                  >
                    <div 
                      className="timeline-marker bg-danger"
                      style={{
                        position: 'absolute',
                        left: '-26px',
                        top: '4px',
                        width: '16px',
                        height: '16px',
                        borderRadius: '50%',
                        border: '3px solid #fff',
                        boxShadow: '0 0 0 2px #e9ecef'
                      }}
                    ></div>
                    <div 
                      className="timeline-content"
                      style={{
                        background: '#f8f9fa',
                        padding: '12px 16px',
                        borderRadius: '8px',
                        borderLeft: '3px solid #0d6efd'
                      }}
                    >
                      <div className="fw-semibold">Order Cancelled</div>
                      <div className="text-muted small">
                        {format(new Date(order.cancelledAt), 'MMMM dd, yyyy \'at\' hh:mm a')}
                      </div>
                    </div>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      </div>

      <StatusUpdateModal 
        orderId={order.id} 
        currentStatus={order.status}
        redirectTo={`/admin/orders/${order.id}`}
      />
      <CancelOrderModal 
        orderId={order.id}
        redirectTo={`/admin/orders/${order.id}`}
      />
      <RefundOrderModal 
        orderId={order.id}
        redirectTo={`/admin/orders/${order.id}`}
      />
      <MarkPaidModal 
        orderId={order.id}
        redirectTo={`/admin/orders/${order.id}`}
      />
    </div>
  );
}

function getStatusColor(status: OrderStatus): string {
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

function getStatusText(status: OrderStatus): string {
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