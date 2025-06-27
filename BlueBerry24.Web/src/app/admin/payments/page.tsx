import Link from 'next/link';
import { getAllPayments } from '@/lib/actions/payment-actions';
import { PaymentStatus, PaymentMethod, Payment } from '@/types/payment';

interface AdminPaymentsPageProps {
  searchParams: Promise<{
    page?: string;
    success?: string;
    error?: string;
  }>;
}

export default async function AdminPaymentsPage({ searchParams }: AdminPaymentsPageProps) {
  const params = await searchParams;
  const page = parseInt(params.page || '1');
  const success = params.success;
  const error = params.error;

  const payments = await getAllPayments();

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


  const completedPayments = payments.filter((p: Payment) => p.status === PaymentStatus.Completed);
  const totalRevenue = completedPayments.reduce((sum: number, p: Payment) => sum + p.amount, 0);
  const processingPayments = payments.filter((p: Payment) => p.status === PaymentStatus.Processing);
  const failedPayments = payments.filter((p: Payment) => p.status === PaymentStatus.Failed);

  return (
    <div className="container-fluid py-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h1 className="h3 fw-bold mb-2">Payment Management</h1>
          <p className="text-muted mb-0">Track and manage payment transactions</p>
        </div>
        <Link href="/admin/reports" className="btn btn-outline-primary">
          <i className="bi bi-download me-2"></i>
          Export Payments
        </Link>
      </div>

      {success && (
        <div className="alert alert-success alert-dismissible fade show mb-4" role="alert">
          <i className="bi bi-check-circle-fill me-2"></i>
          {decodeURIComponent(success)}
          <button type="button" className="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
      )}

      {error && (
        <div className="alert alert-danger alert-dismissible fade show mb-4" role="alert">
          <i className="bi bi-exclamation-triangle-fill me-2"></i>
          {decodeURIComponent(error)}
          <button type="button" className="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
      )}

      <div className="row g-4 mb-4">
        <div className="col-md-3">
          <div className="card bg-success text-white">
            <div className="card-body">
              <div className="d-flex justify-content-between">
                <div>
                  <h6 className="card-title">Total Revenue</h6>
                  <h4 className="mb-0">${totalRevenue.toFixed(2)}</h4>
                </div>
                <i className="bi bi-currency-dollar display-6 opacity-75"></i>
              </div>
            </div>
          </div>
        </div>

        <div className="col-md-3">
          <div className="card bg-primary text-white">
            <div className="card-body">
              <div className="d-flex justify-content-between">
                <div>
                  <h6 className="card-title">Total Payments</h6>
                  <h4 className="mb-0">{payments.length}</h4>
                </div>
                <i className="bi bi-credit-card display-6 opacity-75"></i>
              </div>
            </div>
          </div>
        </div>

        <div className="col-md-3">
          <div className="card bg-warning text-dark">
            <div className="card-body">
              <div className="d-flex justify-content-between">
                <div>
                  <h6 className="card-title">Processing</h6>
                  <h4 className="mb-0">{processingPayments.length}</h4>
                </div>
                <i className="bi bi-clock display-6 opacity-75"></i>
              </div>
            </div>
          </div>
        </div>

        <div className="col-md-3">
          <div className="card bg-danger text-white">
            <div className="card-body">
              <div className="d-flex justify-content-between">
                <div>
                  <h6 className="card-title">Failed</h6>
                  <h4 className="mb-0">{failedPayments.length}</h4>
                </div>
                <i className="bi bi-x-circle display-6 opacity-75"></i>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="card">
        <div className="card-header d-flex justify-content-between align-items-center">
          <h5 className="mb-0">Payment Transactions</h5>
          <span className="badge bg-primary">{payments.length} total</span>
        </div>

        <div className="card-body">
          {payments.length > 0 ? (
            <div className="table-responsive">
              <table className="table table-hover">
                <thead>
                  <tr>
                    <th>Transaction ID</th>
                    <th>Customer</th>
                    <th>Amount</th>
                    <th>Method</th>
                    <th>Status</th>
                    <th>Date</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {payments.slice((page - 1) * 10, page * 10).map((payment: Payment) => (
                    <tr key={payment.id}>
                      <td>
                        <div className="d-flex flex-column">
                          <span className="fw-medium">{payment.transactionId}</span>
                          {payment.orderId && (
                            <small className="text-muted">Order: {payment.orderId}</small>
                          )}
                        </div>
                      </td>
                      <td>
                        <div className="d-flex flex-column">
                          <span className="fw-medium">{payment.payerName || 'N/A'}</span>
                          <small className="text-muted">{payment.payerEmail || 'N/A'}</small>
                        </div>
                      </td>
                      <td>
                        <div className="d-flex flex-column">
                          <span className="fw-bold">${payment.amount.toFixed(2)}</span>
                          <small className="text-muted">{payment.currency}</small>
                        </div>
                      </td>
                      <td>
                        <div className="d-flex flex-column">
                          <span>{getMethodText(payment.method)}</span>
                          {payment.cardLast4 && (
                            <small className="text-muted">****{payment.cardLast4}</small>
                          )}
                        </div>
                      </td>
                      <td>
                        <span className={`badge ${getStatusBadgeClass(payment.status)}`}>
                          {getStatusText(payment.status)}
                        </span>
                      </td>
                      <td>
                        <div className="d-flex flex-column">
                          <span>{new Date(payment.createdAt).toLocaleDateString()}</span>
                          <small className="text-muted">
                            {new Date(payment.createdAt).toLocaleTimeString()}
                          </small>
                        </div>
                      </td>
                      <td>
                        <div className="btn-group btn-group-sm" role="group">
                          <Link
                            href={`/admin/payments/${payment.id}`}
                            className="btn btn-outline-primary"
                            title="View Details"
                          >
                            <i className="bi bi-eye"></i>
                          </Link>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          ) : (
            <div className="text-center py-5">
              <i className="bi bi-credit-card-2-front display-1 text-muted mb-3"></i>
              <h5 className="text-muted">No payments found</h5>
              <p className="text-muted">No payment transactions available.</p>
            </div>
          )}
        </div>
      </div>
    </div>
  );
} 