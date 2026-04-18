import { notFound } from 'next/navigation';
import Link from 'next/link';
import { getPaymentById, updatePaymentStatus } from '../../../../../lib/actions/payment-actions';
import { PaymentStatus, PaymentMethod, Payment } from '../../../../../types/payment';

interface Props {
  params: Promise<{
    id: string;
  }>;
  searchParams: Promise<{ [key: string]: string | string[] | undefined }>;
}

export default async function UpdatePaymentPage({ params, searchParams }: Props) {
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

  async function handleUpdatePaymentStatus(formData: FormData) {
    'use server'
    await updatePaymentStatus(formData);
  }

  return (
    <div className="container-fluid py-4">
      <div className="row mb-4">
        <div className="col-12">
          <div className="d-flex align-items-center">
            <Link
              href={`/admin/payments/${payment.id}`}
              className="btn btn-outline-secondary me-3"
              title="Back to Payment Details"
            >
              <i className="bi bi-arrow-left"></i>
            </Link>
            <div>
              <h1 className="h2 mb-1">
                <i className="bi bi-pencil me-2 text-warning"></i>
                Update Payment Status
              </h1>
              <p className="text-muted mb-0">
                Transaction ID: {payment.transactionId}
              </p>
            </div>
          </div>
        </div>
      </div>

      <div className="row justify-content-center">
        <div className="col-lg-8 col-xl-6">
          <div className="card shadow-sm border-0 mb-4">
            <div className="card-header bg-white py-3">
              <h5 className="card-title mb-0">
                <i className="bi bi-info-circle me-2"></i>
                Payment Information
              </h5>
            </div>
            <div className="card-body">
              <div className="row g-3">
                <div className="col-md-6">
                  <label className="form-label fw-medium">Amount</label>
                  <p className="form-control-plaintext h5 text-success">
                    ${payment.amount.toFixed(2)} {payment.currency}
                  </p>
                </div>
                
                <div className="col-md-6">
                  <label className="form-label fw-medium">Current Status</label>
                  <p className="form-control-plaintext">
                    <span className={`badge bg-${payment.status === PaymentStatus.Completed ? 'success' : 
                      payment.status === PaymentStatus.Failed ? 'danger' : 
                      payment.status === PaymentStatus.Processing ? 'warning' : 'secondary'}`}>
                      {getStatusText(payment.status)}
                    </span>
                  </p>
                </div>

                <div className="col-md-6">
                  <label className="form-label fw-medium">Payment Method</label>
                  <p className="form-control-plaintext">{getMethodText(payment.method)}</p>
                </div>

                <div className="col-md-6">
                  <label className="form-label fw-medium">Provider</label>
                  <p className="form-control-plaintext">{payment.provider}</p>
                </div>

                <div className="col-md-6">
                  <label className="form-label fw-medium">Customer</label>
                  <p className="form-control-plaintext">{payment.payerName || 'N/A'}</p>
                </div>

                <div className="col-md-6">
                  <label className="form-label fw-medium">Email</label>
                  <p className="form-control-plaintext">{payment.payerEmail || 'N/A'}</p>
                </div>
              </div>
            </div>
          </div>

          <div className="card shadow-sm border-0">
            <div className="card-header bg-white py-3">
              <h5 className="card-title mb-0">
                <i className="bi bi-gear me-2"></i>
                Update Status
              </h5>
            </div>
            <div className="card-body p-4">
              <form action={handleUpdatePaymentStatus}>
                <input type="hidden" name="id" value={payment.id} />
                
                <div className="mb-3">
                  <label htmlFor="status" className="form-label fw-medium">
                    New Status <span className="text-danger">*</span>
                  </label>
                  <select 
                    name="status" 
                    id="status" 
                    className="form-select" 
                    required
                    defaultValue={payment.status}
                  >
                    <option value={PaymentStatus.Pending}>Pending</option>
                    <option value={PaymentStatus.Processing}>Processing</option>
                    <option value={PaymentStatus.Completed}>Completed</option>
                    <option value={PaymentStatus.Failed}>Failed</option>
                    <option value={PaymentStatus.Cancelled}>Cancelled</option>
                    <option value={PaymentStatus.Refunded}>Refunded</option>
                    <option value={PaymentStatus.PartiallyRefunded}>Partially Refunded</option>
                    <option value={PaymentStatus.Disputed}>Disputed</option>
                    <option value={PaymentStatus.Expired}>Expired</option>
                  </select>
                  <div className="form-text">
                    Select the new status for this payment transaction.
                  </div>
                </div>

                <div className="mb-4">
                  <label htmlFor="notes" className="form-label fw-medium">
                    Notes
                  </label>
                  <textarea 
                    name="notes" 
                    id="notes" 
                    className="form-control" 
                    rows={4}
                    placeholder="Add any notes about this status change..."
                    defaultValue={payment.notes || ''}
                  ></textarea>
                  <div className="form-text">
                    Optional notes explaining the reason for the status change.
                  </div>
                </div>

                <div className="d-flex gap-2">
                  <button type="submit" className="btn btn-primary">
                    <i className="bi bi-check-circle me-2"></i>
                    Update Status
                  </button>
                  <Link
                    href={`/admin/payments/${payment.id}`}
                    className="btn btn-outline-secondary"
                  >
                    <i className="bi bi-x-circle me-2"></i>
                    Cancel
                  </Link>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
} 