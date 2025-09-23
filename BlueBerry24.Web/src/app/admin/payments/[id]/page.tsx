import { notFound } from 'next/navigation';
import Link from 'next/link';
import { getPaymentById } from '../../../../lib/actions/payment-actions';
import { PaymentStatus, PaymentMethod, Payment } from '../../../../types/payment';

interface Props {
  params: Promise<{
    id: string;
  }>;
  searchParams: Promise<{ [key: string]: string | string[] | undefined }>;
}

export default async function PaymentDetailsPage({ params, searchParams }: Props) {
  const resolvedParams = await params;
  const paymentId = parseInt(resolvedParams.id);

  if (isNaN(paymentId)) {
    notFound();
  }

  let payment: Payment;
  try {
    payment = await getPaymentById(paymentId);
  } catch (error) {
    console.error("Failed to load payment:", error);
    notFound();
  }

  const getStatusBadgeClass = (status: PaymentStatus) => {
    switch (status) {
      case PaymentStatus.Completed:
        return 'bg-success';
      case PaymentStatus.Processing:
        return 'bg-warning text-dark';
      case PaymentStatus.Failed:
        return 'bg-danger';
      case PaymentStatus.Cancelled:
        return 'bg-secondary';
      case PaymentStatus.Refunded:
        return 'bg-info';
      case PaymentStatus.PartiallyRefunded:
        return 'bg-primary';
      case PaymentStatus.Disputed:
        return 'bg-warning text-dark';
      case PaymentStatus.Expired:
        return 'bg-secondary';
      default:
        return 'bg-light text-dark';
    }
  };

  const getStatusText = (status: PaymentStatus) => {
    switch (status) {
      case PaymentStatus.Pending:
        return 'Pending';
      case PaymentStatus.Processing:
        return 'Processing';
      case PaymentStatus.Completed:
        return 'Completed';
      case PaymentStatus.Failed:
        return 'Failed';
      case PaymentStatus.Cancelled:
        return 'Cancelled';
      case PaymentStatus.Refunded:
        return 'Refunded';
      case PaymentStatus.PartiallyRefunded:
        return 'Partially Refunded';
      case PaymentStatus.Disputed:
        return 'Disputed';
      case PaymentStatus.Expired:
        return 'Expired';
      default:
        return 'Unknown';
    }
  };

  const getMethodText = (method: PaymentMethod) => {
    switch (method) {
      case PaymentMethod.CreditCard:
        return 'Credit Card';
      case PaymentMethod.DebitCard:
        return 'Debit Card';
      case PaymentMethod.PayPal:
        return 'PayPal';
      case PaymentMethod.BankTransfer:
        return 'Bank Transfer';
      case PaymentMethod.DigitalWallet:
        return 'Digital Wallet';
      case PaymentMethod.Cryptocurrency:
        return 'Cryptocurrency';
      case PaymentMethod.GiftCard:
        return 'Gift Card';
      case PaymentMethod.StoreCredit:
        return 'Store Credit';
      case PaymentMethod.CashOnDelivery:
        return 'Cash on Delivery';
      default:
        return 'Other';
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleString();
  };

  return (
    <div className="container-fluid py-4">
      <div className="row mb-4">
        <div className="col-12">
          <div className="d-flex align-items-center">
            <Link
              href="/admin/payments"
              className="btn btn-outline-secondary me-3"
              title="Back to Payments"
            >
              <i className="bi bi-arrow-left"></i>
            </Link>
            <div>
              <h1 className="h2 mb-1">
                <i className="bi bi-credit-card me-2 text-primary"></i>
                Payment Details
              </h1>
              <p className="text-muted mb-0">
                Transaction ID: {payment.transactionId}
              </p>
            </div>
          </div>
        </div>
      </div>

      <div className="row g-4">
        <div className="col-lg-8">
          <div className="card shadow-sm border-0">
            <div className="card-header bg-white py-3">
              <h5 className="card-title mb-0">
                <i className="bi bi-info-circle me-2"></i>
                Payment Information
              </h5>
            </div>
            <div className="card-body">
              <div className="row g-3">
                <div className="col-md-6">
                  <div className="d-flex justify-content-between align-items-center p-3 bg-light rounded">
                    <div>
                      <small className="text-muted d-block">Amount</small>
                      <span className="h4 fw-bold text-success mb-0">
                        ${payment.amount.toFixed(2)}
                      </span>
                      <small className="text-muted d-block">{payment.currency}</small>
                    </div>
                    <i className="bi bi-currency-dollar display-6 text-success opacity-75"></i>
                  </div>
                </div>
                
                <div className="col-md-6">
                  <div className="d-flex justify-content-between align-items-center p-3 bg-light rounded">
                    <div>
                      <small className="text-muted d-block">Status</small>
                      <span className={`badge ${getStatusBadgeClass(payment.status)} fs-6`}>
                        {getStatusText(payment.status)}
                      </span>
                    </div>
                    <i className="bi bi-check-circle display-6 text-success opacity-75"></i>
                  </div>
                </div>

                <div className="col-md-6">
                  <div className="d-flex justify-content-between align-items-center p-3 bg-light rounded">
                    <div>
                      <small className="text-muted d-block">Payment Method</small>
                      <span className="fw-medium">{getMethodText(payment.method)}</span>
                      {payment.cardLast4 && (
                        <small className="text-muted d-block">****{payment.cardLast4}</small>
                      )}
                    </div>
                    <i className="bi bi-credit-card display-6 text-primary opacity-75"></i>
                  </div>
                </div>

                <div className="col-md-6">
                  <div className="d-flex justify-content-between align-items-center p-3 bg-light rounded">
                    <div>
                      <small className="text-muted d-block">Provider</small>
                      <span className="fw-medium">{payment.provider}</span>
                      {payment.providerTransactionId && (
                        <small className="text-muted d-block">ID: {payment.providerTransactionId}</small>
                      )}
                    </div>
                    <i className="bi bi-building display-6 text-info opacity-75"></i>
                  </div>
                </div>
              </div>

              <hr />

              <div className="row g-3">
                <div className="col-md-6">
                  <label className="form-label fw-medium">Transaction ID</label>
                  <p className="form-control-plaintext">{payment.transactionId}</p>
                </div>
                
                <div className="col-md-6">
                  <label className="form-label fw-medium">Processing Fee</label>
                  <p className="form-control-plaintext">${payment.processingFee.toFixed(2)}</p>
                </div>

                <div className="col-md-6">
                  <label className="form-label fw-medium">Net Amount</label>
                  <p className="form-control-plaintext">${payment.netAmount.toFixed(2)}</p>
                </div>

                <div className="col-md-6">
                  <label className="form-label fw-medium">Created At</label>
                  <p className="form-control-plaintext">{formatDate(payment.createdAt)}</p>
                </div>

                {payment.processedAt && (
                  <div className="col-md-6">
                    <label className="form-label fw-medium">Processed At</label>
                    <p className="form-control-plaintext">{formatDate(payment.processedAt)}</p>
                  </div>
                )}

                {payment.completedAt && (
                  <div className="col-md-6">
                    <label className="form-label fw-medium">Completed At</label>
                    <p className="form-control-plaintext">{formatDate(payment.completedAt)}</p>
                  </div>
                )}

                {payment.failedAt && (
                  <div className="col-md-6">
                    <label className="form-label fw-medium">Failed At</label>
                    <p className="form-control-plaintext">{formatDate(payment.failedAt)}</p>
                  </div>
                )}

                {payment.refundedAt && (
                  <div className="col-md-6">
                    <label className="form-label fw-medium">Refunded At</label>
                    <p className="form-control-plaintext">{formatDate(payment.refundedAt)}</p>
                  </div>
                )}

                {payment.updatedAt && (
                  <div className="col-md-6">
                    <label className="form-label fw-medium">Last Updated</label>
                    <p className="form-control-plaintext">{formatDate(payment.updatedAt)}</p>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>

        <div className="col-lg-4">
          <div className="card shadow-sm border-0 mb-4">
            <div className="card-header bg-white py-3">
              <h5 className="card-title mb-0">
                <i className="bi bi-person me-2"></i>
                Customer Information
              </h5>
            </div>
            <div className="card-body">
              <div className="mb-3">
                <label className="form-label fw-medium">Customer Name</label>
                <p className="form-control-plaintext">{payment.payerName || 'N/A'}</p>
              </div>
              
              <div className="mb-3">
                <label className="form-label fw-medium">Email</label>
                <p className="form-control-plaintext">{payment.payerEmail || 'N/A'}</p>
              </div>

              {payment.userName && (
                <div className="mb-3">
                  <label className="form-label fw-medium">User Name</label>
                  <p className="form-control-plaintext">{payment.userName}</p>
                </div>
              )}

              {payment.userEmail && (
                <div className="mb-3">
                  <label className="form-label fw-medium">User Email</label>
                  <p className="form-control-plaintext">{payment.userEmail}</p>
                </div>
              )}

              {payment.orderId && (
                <div className="mb-3">
                  <label className="form-label fw-medium">Order ID</label>
                  <p className="form-control-plaintext">
                    <Link href={`/admin/orders/${payment.orderId}`} className="text-decoration-none">
                      {payment.orderId}
                    </Link>
                  </p>
                </div>
              )}

              {payment.orderReferenceNumber && (
                <div className="mb-3">
                  <label className="form-label fw-medium">Order Reference</label>
                  <p className="form-control-plaintext">{payment.orderReferenceNumber}</p>
                </div>
              )}
            </div>
          </div>

          {(payment.billingAddress1 || payment.billingCity || payment.billingState) && (
            <div className="card shadow-sm border-0">
              <div className="card-header bg-white py-3">
                <h5 className="card-title mb-0">
                  <i className="bi bi-geo-alt me-2"></i>
                  Billing Address
                </h5>
              </div>
              <div className="card-body">
                {payment.billingAddress1 && (
                  <p className="mb-1">{payment.billingAddress1}</p>
                )}
                {payment.billingAddress2 && (
                  <p className="mb-1">{payment.billingAddress2}</p>
                )}
                <p className="mb-1">
                  {payment.billingCity && `${payment.billingCity}, `}
                  {payment.billingState && `${payment.billingState} `}
                  {payment.billingPostalCode}
                </p>
                {payment.billingCountry && (
                  <p className="mb-0">{payment.billingCountry}</p>
                )}
              </div>
            </div>
          )}
        </div>
      </div>

      {(payment.errorMessage || payment.failureReason) && (
        <div className="row mt-4">
          <div className="col-12">
            <div className="card border-danger">
              <div className="card-header bg-danger text-white">
                <h5 className="card-title mb-0">
                  <i className="bi bi-exclamation-triangle me-2"></i>
                  Error Information
                </h5>
              </div>
              <div className="card-body">
                {payment.errorMessage && (
                  <div className="mb-3">
                    <label className="form-label fw-medium">Error Message</label>
                    <p className="form-control-plaintext text-danger">{payment.errorMessage}</p>
                  </div>
                )}
                {payment.failureReason && (
                  <div className="mb-3">
                    <label className="form-label fw-medium">Failure Reason</label>
                    <p className="form-control-plaintext text-danger">{payment.failureReason}</p>
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      )}

      {payment.notes && (
        <div className="row mt-4">
          <div className="col-12">
            <div className="card">
              <div className="card-header bg-white py-3">
                <h5 className="card-title mb-0">
                  <i className="bi bi-sticky me-2"></i>
                  Notes
                </h5>
              </div>
              <div className="card-body">
                <p className="mb-0">{payment.notes}</p>
              </div>
            </div>
          </div>
        </div>
      )}

      <div className="row mt-4">
        <div className="col-12">
          <div className="card">
            <div className="card-header bg-white py-3">
              <h5 className="card-title mb-0">
                <i className="bi bi-gear me-2"></i>
                Actions
              </h5>
            </div>
            <div className="card-body">
              <div className="btn-group" role="group">
                <Link
                  href={`/admin/payments/update/${payment.id}`}
                  className="btn btn-outline-primary"
                >
                  <i className="bi bi-pencil me-2"></i>
                  Update Status
                </Link>
                {payment.status === PaymentStatus.Completed && (
                  <Link
                    href={`/admin/payments/refund/${payment.id}`}
                    className="btn btn-outline-warning"
                  >
                    <i className="bi bi-arrow-return-left me-2"></i>
                    Refund Payment
                  </Link>
                )}
                <Link
                  href={`/admin/payments/delete/${payment.id}`}
                  className="btn btn-outline-danger"
                >
                  <i className="bi bi-trash me-2"></i>
                  Delete Payment
                </Link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
} 