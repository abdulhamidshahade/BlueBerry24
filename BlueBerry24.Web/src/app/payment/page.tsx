import Link from 'next/link';
import { processPayment } from '../../lib/actions/payment-actions';
import { OrderService } from '../../lib/services/order/service';

interface PaymentPageProps {
  searchParams: Promise<{
    orderId?: string;
    amount?: string;
    error?: string;
    error_paymentMethod?: string;
    error_cardNumber?: string;
    error_expiryDate?: string;
    error_cvv?: string;
    error_payerName?: string;
    error_payerEmail?: string;
    error_billingAddress1?: string;
    error_billingCity?: string;
    error_billingState?: string;
    error_billingPostalCode?: string;
  }>;
}

export default async function PaymentPage({ searchParams }: PaymentPageProps) {
  const params = await searchParams;
  const orderId = params.orderId;
  const amount = params.amount;
  const error = params.error;

  // Fetch order details if orderId is provided
  let order = null;
  if (orderId) {
    try {
      const orderService = new OrderService();
      order = await orderService.getById(parseInt(orderId));
    } catch (error) {
      console.error('Error fetching order details:', error);
    }
  }

  // if (!amount) {
  //   redirect('/cart?error=' + encodeURIComponent('Payment amount is required.'));
  // }

  const validation = {
    paymentMethod: params.error_paymentMethod || '',
    cardNumber: params.error_cardNumber || '',
    expiryDate: params.error_expiryDate || '',
    cvv: params.error_cvv || '',
    payerName: params.error_payerName || '',
    payerEmail: params.error_payerEmail || '',
    billingAddress1: params.error_billingAddress1 || '',
    billingCity: params.error_billingCity || '',
    billingState: params.error_billingState || '',
    billingPostalCode: params.error_billingPostalCode || '',
  };

  const orderAmount = parseFloat(amount || '0');
  const processingFee = orderAmount * 0.029 + 0.30;
  const total = orderAmount + processingFee;

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
            <li className="breadcrumb-item">
              <Link href="/checkout/order" className="text-decoration-none">Order Review</Link>
            </li>
            <li className="breadcrumb-item active" aria-current="page">Payment</li>
          </ol>
        </nav>

        <div className="mb-4">
          <div className="d-flex justify-content-center">
            <div className="d-flex align-items-center text-center">
              <div className="d-flex align-items-center text-primary me-4">
                <i className="bi bi-check-circle-fill me-2"></i>
                <span className="fw-medium">Cart</span>
              </div>
              <div className="border-top border-primary flex-fill mx-3" style={{width: '50px'}}></div>
              <div className="d-flex align-items-center text-primary me-4">
                <i className="bi bi-credit-card-fill me-2"></i>
                <span className="fw-medium">Checkout</span>
              </div>
              <div className="border-top border-primary flex-fill mx-3" style={{width: '50px'}}></div>
              <div className="d-flex align-items-center text-primary me-4">
                <i className="bi bi-credit-card-fill me-2"></i>
                <span className="fw-medium">Order Review</span>
              </div>
              <div className="border-top border-secondary flex-fill mx-3" style={{width: '50px'}}></div>
              <div className="d-flex align-items-center text-muted">
                <span>Payment</span>
              </div>
            </div>
          </div>
        </div>

        <div className="text-center mb-4">
          <h1 className="display-5 fw-bold text-dark">Secure Payment</h1>
          <p className="text-muted">Complete your purchase securely</p>
        </div>

        {error && (
          <div className="mb-4">
            <div className="alert alert-danger d-flex align-items-center" role="alert">
              <i className="bi bi-exclamation-triangle-fill me-3"></i>
              <div>
                <h6 className="alert-heading mb-1">Payment Error</h6>
                <p className="mb-0">{error}</p>
              </div>
            </div>
          </div>
        )}

        <div className="row g-4">
          <div className="col-lg-7">
            <div className="card shadow-sm">
              <div className="card-header bg-white border-bottom">
                <h5 className="card-title mb-1">Payment Information</h5>
                <p className="card-text text-muted mb-0">Complete your payment to finalize your order</p>
              </div>

              <div className="card-body">
                <form action={processPayment}>
                  {orderId && <input type="hidden" name="orderId" value={orderId} />}
                  <input type="hidden" name="amount" value={amount} />
                  <input type="hidden" name="currency" value="USD" />
                  <input type="hidden" name="provider" value="stripe" />
                  
                  <div className="mb-4">
                    <fieldset>
                      <legend className="fs-6 fw-medium text-dark mb-3">Payment Method</legend>
                      <div className="mb-3">
                        <div className="form-check d-flex align-items-center justify-content-between">
                          <div className="d-flex align-items-center">
                            <input
                              className="form-check-input"
                              type="radio"
                              name="method"
                              id="credit-card"
                              value="0"
                              defaultChecked
                            />
                            <label className="form-check-label ms-2" htmlFor="credit-card">
                              Credit / Debit Card
                            </label>
                          </div>
                          <div className="d-flex gap-2">
                            <span className="badge bg-primary px-2 py-1">Visa</span>
                            <span className="badge bg-danger px-2 py-1">MC</span>
                            <span className="badge bg-info px-2 py-1">Amex</span>
                          </div>
                        </div>
                      </div>
                      <div className="mb-3">
                        <div className="form-check d-flex align-items-center justify-content-between">
                          <div className="d-flex align-items-center">
                            <input
                              className="form-check-input"
                              type="radio"
                              name="method"
                              id="paypal"
                              value="2"
                            />
                            <label className="form-check-label ms-2" htmlFor="paypal">
                              PayPal
                            </label>
                          </div>
                          <div>
                            <span className="badge bg-primary px-3 py-1">PayPal</span>
                          </div>
                        </div>
                      </div>
                      {validation.paymentMethod && (
                        <div className="text-danger small">{validation.paymentMethod}</div>
                      )}
                    </fieldset>
                  </div>

                  <div className="mb-4">
                    <div className="mb-3">
                      <label htmlFor="cardNumber" className="form-label fw-medium">
                        Card Number
                      </label>
                      <input
                        type="text"
                        className={`form-control ${validation.cardNumber ? 'is-invalid' : ''}`}
                        id="cardNumber"
                        name="cardNumber"
                        placeholder="1234 5678 9012 3456"
                        maxLength={19}
                      />
                      {validation.cardNumber && (
                        <div className="invalid-feedback">{validation.cardNumber}</div>
                      )}
                    </div>

                    <div className="row g-3 mb-3">
                      <div className="col-6">
                        <label htmlFor="expiryDate" className="form-label fw-medium">
                          Expiry Date
                        </label>
                        <input
                          type="text"
                          className={`form-control ${validation.expiryDate ? 'is-invalid' : ''}`}
                          id="expiryDate"
                          name="expiryDate"
                          placeholder="MM/YY"
                          maxLength={5}
                        />
                        {validation.expiryDate && (
                          <div className="invalid-feedback">{validation.expiryDate}</div>
                        )}
                      </div>
                      <div className="col-6">
                        <label htmlFor="cvv" className="form-label fw-medium">
                          CVV
                        </label>
                        <input
                          type="text"
                          className={`form-control ${validation.cvv ? 'is-invalid' : ''}`}
                          id="cvv"
                          name="cvv"
                          placeholder="123"
                          maxLength={4}
                        />
                        {validation.cvv && (
                          <div className="invalid-feedback">{validation.cvv}</div>
                        )}
                      </div>
                    </div>
                  </div>

                  <div className="mb-4">
                    <h6 className="fw-medium text-dark mb-3">Billing Information</h6>
                    
                    <div className="row g-3 mb-3">
                      <div className="col-sm-6">
                        <label htmlFor="payerName" className="form-label fw-medium">
                          Full Name
                        </label>
                        <input
                          type="text"
                          className={`form-control ${validation.payerName ? 'is-invalid' : ''}`}
                          id="payerName"
                          name="payerName"
                          placeholder="John Doe"
                          required
                        />
                        {validation.payerName && (
                          <div className="invalid-feedback">{validation.payerName}</div>
                        )}
                      </div>
                      <div className="col-sm-6">
                        <label htmlFor="payerEmail" className="form-label fw-medium">
                          Email Address
                        </label>
                        <input
                          type="email"
                          className={`form-control ${validation.payerEmail ? 'is-invalid' : ''}`}
                          id="payerEmail"
                          name="payerEmail"
                          placeholder="john@example.com"
                          required
                        />
                        {validation.payerEmail && (
                          <div className="invalid-feedback">{validation.payerEmail}</div>
                        )}
                      </div>
                    </div>

                    <div className="mb-3">
                      <label htmlFor="billingAddress1" className="form-label fw-medium">
                        Address Line 1
                      </label>
                      <input
                        type="text"
                        className={`form-control ${validation.billingAddress1 ? 'is-invalid' : ''}`}
                        id="billingAddress1"
                        name="billingAddress1"
                        placeholder="123 Main Street"
                        required
                      />
                      {validation.billingAddress1 && (
                        <div className="invalid-feedback">{validation.billingAddress1}</div>
                      )}
                    </div>

                    <div className="mb-3">
                      <label htmlFor="billingAddress2" className="form-label fw-medium">
                        Address Line 2 (Optional)
                      </label>
                      <input
                        type="text"
                        className="form-control"
                        id="billingAddress2"
                        name="billingAddress2"
                        placeholder="Apartment, suite, etc."
                      />
                    </div>

                    <div className="row g-3">
                      <div className="col-md-4">
                        <label htmlFor="billingCity" className="form-label fw-medium">
                          City
                        </label>
                        <input
                          type="text"
                          className={`form-control ${validation.billingCity ? 'is-invalid' : ''}`}
                          id="billingCity"
                          name="billingCity"
                          placeholder="New York"
                          required
                        />
                        {validation.billingCity && (
                          <div className="invalid-feedback">{validation.billingCity}</div>
                        )}
                      </div>
                      <div className="col-md-4">
                        <label htmlFor="billingState" className="form-label fw-medium">
                          State
                        </label>
                        <input
                          type="text"
                          className={`form-control ${validation.billingState ? 'is-invalid' : ''}`}
                          id="billingState"
                          name="billingState"
                          placeholder="NY"
                          required
                        />
                        {validation.billingState && (
                          <div className="invalid-feedback">{validation.billingState}</div>
                        )}
                      </div>
                      <div className="col-md-4">
                        <label htmlFor="billingPostalCode" className="form-label fw-medium">
                          Postal Code
                        </label>
                        <input
                          type="text"
                          className={`form-control ${validation.billingPostalCode ? 'is-invalid' : ''}`}
                          id="billingPostalCode"
                          name="billingPostalCode"
                          placeholder="10001"
                          required
                        />
                        {validation.billingPostalCode && (
                          <div className="invalid-feedback">{validation.billingPostalCode}</div>
                        )}
                      </div>
                    </div>

                    <input type="hidden" name="billingCountry" value="US" />
                  </div>

                  <div className="alert alert-success d-flex align-items-start mb-4" role="alert">
                    <i className="bi bi-shield-lock-fill me-3 mt-1"></i>
                    <div>
                      <h6 className="alert-heading mb-1">Your payment is secured</h6>
                      <p className="mb-0">All transactions are encrypted and processed securely through our payment partners.</p>
                    </div>
                  </div>

                  <div className="d-grid">
                    <button
                      type="submit"
                      className="btn btn-primary btn-lg d-flex align-items-center justify-content-center"
                    >
                      <i className="bi bi-lock-fill me-2"></i>
                      Complete Payment - ${total.toFixed(2)}
                    </button>
                    <p className="text-muted text-center mt-2 small">
                      By completing your purchase you agree to our Terms of Service
                    </p>
                  </div>
                </form>
              </div>
            </div>
          </div>

          <div className="col-lg-5">
            <div className="card shadow-sm sticky-top" style={{top: '2rem'}}>
              <div className="card-header bg-white border-bottom">
                <h5 className="card-title mb-0">Payment Summary</h5>
              </div>

              <div className="card-body">
                <div className="border-top pt-3">
                  {order ? (
                    <>
                      <div className="d-flex justify-content-between mb-2">
                        <span className="fw-medium">Subtotal</span>
                        <span className="fw-medium">${order.subTotal.toFixed(2)}</span>
                      </div>
                      {order.discountTotal > 0 && (
                        <div className="d-flex justify-content-between mb-2 text-success">
                          <span className="fw-medium">Discount</span>
                          <span className="fw-medium">-${order.discountTotal.toFixed(2)}</span>
                        </div>
                      )}
                      <div className="d-flex justify-content-between mb-2">
                        <span className="fw-medium">Shipping</span>
                        <span className="fw-medium">${order.shippingAmount.toFixed(2)}</span>
                      </div>
                      <div className="d-flex justify-content-between mb-2">
                        <span className="fw-medium">Tax</span>
                        <span className="fw-medium">${order.taxAmount.toFixed(2)}</span>
                      </div>
                      <div className="border-top pt-3 d-flex justify-content-between mb-2">
                        <span className="fw-medium">Order Total</span>
                        <span className="fw-medium">${order.total.toFixed(2)}</span>
                      </div>
                      <div className="d-flex justify-content-between mb-2">
                        <span className="fw-medium">Processing Fee</span>
                        <span className="fw-medium">${processingFee.toFixed(2)}</span>
                      </div>
                      <div className="border-top pt-3 d-flex justify-content-between">
                        <span className="fs-5 fw-bold">Total</span>
                        <span className="fs-5 fw-bold">${total.toFixed(2)}</span>
                      </div>
                    </>
                  ) : (
                    <>
                      <div className="d-flex justify-content-between mb-2">
                        <span className="fw-medium">Order Total</span>
                        <span className="fw-medium">${amount}</span>
                      </div>
                      <div className="d-flex justify-content-between mb-2">
                        <span className="fw-medium">Processing Fee</span>
                        <span className="fw-medium">${processingFee.toFixed(2)}</span>
                      </div>
                      <div className="border-top pt-3 d-flex justify-content-between">
                        <span className="fs-5 fw-bold">Total</span>
                        <span className="fs-5 fw-bold">${total.toFixed(2)}</span>
                      </div>
                    </>
                  )}
                </div>

                {orderId && (
                  <div className="mt-4 pt-4 border-top">
                    <h6 className="fw-medium mb-2">Order Details</h6>
                    <p className="text-muted mb-1">Order ID: <span className="fw-medium">{orderId}</span></p>
                  </div>
                )}

                <div className="mt-4 pt-4 border-top">
                  <div className="d-flex align-items-center text-muted small">
                    <i className="bi bi-shield-check me-2"></i>
                    <span>256-bit SSL encryption</span>
                  </div>
                  <div className="d-flex align-items-center text-muted small mt-1">
                    <i className="bi bi-lock me-2"></i>
                    <span>PCI DSS compliant</span>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}