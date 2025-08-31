import Link from 'next/link';
import { OrderService } from '../../../lib/services/order/service';
import { redirect } from 'next/navigation';
import { formatCurrency } from '../../../lib/utils/formatCurrency';

interface OrderReviewPageProps {
  searchParams: Promise<{
    id?: string;
  }>;
}
export const dynamic = 'force-dynamic';
export default async function OrderReviewPage({ searchParams }: OrderReviewPageProps) {

  const resolvedSearchParams = await searchParams;

  if (!resolvedSearchParams.id) {
    redirect('/cart?error=' + encodeURIComponent('Order ID is required to view order details.'));
  }

  let order = null;
  try {
    const orderService = new OrderService();
    order = await orderService.getById(parseInt(resolvedSearchParams.id));
  } catch (error) {
    console.error('Error fetching order:', error);
    redirect('/cart?error=' + encodeURIComponent('Unable to find the requested order. Please try again.'));
  }

  if (!order) {
    redirect('/cart?error=' + encodeURIComponent('Order not found. Please check your order details.'));
  }

  return (
    <div className="min-vh-100 bg-light">
      <div className="container py-4">
        <nav aria-label="breadcrumb" className="mb-4">
          <ol className="breadcrumb">
            <li className="breadcrumb-item">
              <Link href="/" className="text-decoration-none">
                <i className="bi bi-house-fill me-2"></i>
                Home
              </Link>
            </li>
            <li className="breadcrumb-item">
              <Link href="/products" className="text-decoration-none">Products</Link>
            </li>
            <li className="breadcrumb-item">
              <Link href="/cart" className="text-decoration-none">Cart</Link>
            </li>
            <li className="breadcrumb-item">
              <Link href="/checkout" className="text-decoration-none">Checkout</Link>
            </li>
            <li className="breadcrumb-item active" aria-current="page">Order Review</li>
          </ol>
        </nav>

        <div className="mb-4">
          <div className="d-flex justify-content-center">
            <div className="d-flex align-items-center text-center">
              <div className="d-flex align-items-center text-primary me-4">
                <i className="bi bi-check-circle-fill me-2"></i>
                <span className="fw-medium">Checkout</span>
              </div>
              <div className="border-top border-primary flex-fill mx-3" style={{width: '50px'}}></div>
              <div className="d-flex align-items-center text-primary me-4">
                <i className="bi bi-check-circle-fill me-2"></i>
                <span className="fw-medium">Order Review</span>
              </div>
              <div className="border-top border-secondary flex-fill mx-3" style={{width: '50px'}}></div>
              <div className="d-flex align-items-center text-muted me-4">
                <span className="fw-medium">Payment</span>
              </div>
              <div className="border-top border-secondary flex-fill mx-3" style={{width: '50px'}}></div>
              <div className="d-flex align-items-center text-muted">
                <span className="me-2">4</span>
                <span>Confirmation</span>
              </div>
            </div>
          </div>
        </div>

        <div className="text-center mb-4">
          <h1 className="display-5 fw-bold text-dark">Order Review</h1>
          <p className="text-muted">Please review your order details before proceeding to payment</p>
        </div>

        <div className="d-flex justify-content-center">
          <div className="col-lg-8">
            <div className="card shadow-sm">
              <div className="card-header bg-white border-bottom">
                <h5 className="card-title mb-0">Order #{order.id}</h5>
              </div>

              <div className="card-body">
                <div className="mb-3">
                  {order.orderItems.map((item: any) => (
                    <div key={item.id} className="d-flex align-items-center py-3 border-bottom">
                      <div className="flex-shrink-0 me-3">
                        <div className="bg-light rounded d-flex align-items-center justify-content-center" style={{width: '64px', height: '64px'}}>
                          <i className="bi bi-image text-muted fs-4"></i>
                        </div>
                      </div>

                      <div className="flex-grow-1">
                        <div className="d-flex justify-content-between align-items-start mb-1">
                          <h6 className="mb-0">{item.productName}</h6>
                          <span className="fw-medium">{formatCurrency(item.totalPrice)}</span>
                        </div>
                        <div className="d-flex justify-content-between align-items-center">
                          <small className="text-muted">Qty {item.quantity}</small>
                          <small className="text-muted">{formatCurrency(item.unitPrice)} each</small>
                        </div>
                      </div>
                    </div>
                  ))}
                </div>
              </div>

              <div className="card-footer bg-white border-top">
                <div className="mb-2">
                  <div className="d-flex justify-content-between mb-2">
                    <span className="fw-medium">Subtotal</span>
                    <span className="fw-medium">{formatCurrency(order.subTotal)}</span>
                  </div>
                  {order.discountTotal > 0 && (
                    <div className="d-flex justify-content-between mb-2 text-success">
                      <span className="fw-medium">Discount</span>
                      <span className="fw-medium">-{formatCurrency(order.discountTotal)}</span>
                    </div>
                  )}
                  <div className="d-flex justify-content-between mb-2">
                    <span className="fw-medium">Shipping</span>
                    <span className="fw-medium">{formatCurrency(order.shippingAmount)}</span>
                  </div>
                  <div className="d-flex justify-content-between mb-3">
                    <span className="fw-medium">Tax</span>
                    <span className="fw-medium">{formatCurrency(order.taxAmount)}</span>
                  </div>
                  <div className="border-top pt-3 d-flex justify-content-between">
                    <span className="fs-5 fw-bold">Total</span>
                    <span className="fs-5 fw-bold">{formatCurrency(order.total)}</span>
                  </div>
                </div>

                <div className="d-grid mt-3">
                  <Link
                    href={`/payment?orderId=${order.id}&amount=${order.total}`}
                    className="btn btn-primary btn-lg d-flex align-items-center justify-content-center text-decoration-none"
                  >
                    Continue to Payment
                  </Link>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}  