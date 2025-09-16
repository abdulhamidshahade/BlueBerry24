import { notFound } from 'next/navigation';
import Link from 'next/link';
import { getPaymentById, refundPayment } from '../../../../../lib/actions/payment-actions';
import { PaymentStatus, PaymentMethod, Payment } from '../../../../../types/payment';

interface Props {
  params: Promise<{
    id: string;
  }>;
  searchParams: Promise<{ [key: string]: string | string[] | undefined }>;
}

export default async function RefundPaymentPage({ params, searchParams }: Props) {
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

  if (payment.status !== PaymentStatus.Completed) {
    notFound();
  }

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

  async function handleRefundPayment(formData: FormData) {
    'use server'
    await refundPayment(formData);
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
                <i className="bi bi-arrow-return-left me-2 text-warning"></i>
                Refund Payment
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
                  <label className="form-label fw-medium">Original Amount</label>
                  <p className="form-control-plaintext h5 text-success">
                    ${payment.amount.toFixed(2)} {payment.currency}
                  </p>
                </div>
                
                <div className="col-md-6">
                  <label className="form-label fw-medium">Status</label>
                  <p className="form-control-plaintext">
                    <span className="badge bg-success">Completed</span>
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

                <div className="col-md-6">
                  <label className="form-label fw-medium">Processing Fee</label>
                  <p className="form-control-plaintext">${payment.processingFee.toFixed(2)}</p>
                </div>

                <div className="col-md-6">
                  <label className="form-label fw-medium">Net Amount</label>
                  <p className="form-control-plaintext">${payment.netAmount.toFixed(2)}</p>
                </div>
              </div>
            </div>
          </div>

          <div className="card shadow-sm border-0">
            <div className="card-header bg-white py-3">
              <h5 className="card-title mb-0">
                <i className="bi bi-arrow-return-left me-2"></i>
                Refund Details
              </h5>
            </div>
            <div className="card-body p-4">
              <form action={handleRefundPayment}>
                <input type="hidden" name="id" value={payment.id} />
                
                <div className="mb-3">
                  <label htmlFor="refundAmount" className="form-label fw-medium">
                    Refund Amount <span className="text-danger">*</span>
                  </label>
                  <div className="input-group">
                    <span className="input-group-text">$</span>
                    <input 
                      type="number" 
                      name="refundAmount" 
                      id="refundAmount" 
                      className="form-control" 
                      step="0.01" 
                      min="0.01" 
                      max={payment.amount}
                      defaultValue={payment.amount}
                      required
                    />
                  </div>
                  <div className="form-text">
                    Enter the amount to refund (maximum: ${payment.amount.toFixed(2)})
                  </div>
                </div>

                <div className="mb-4">
                  <label htmlFor="reason" className="form-label fw-medium">
                    Refund Reason <span className="text-danger">*</span>
                  </label>
                  <select 
                    name="reason" 
                    id="reason" 
                    className="form-select" 
                    required
                  >
                    <option value="">Select a reason...</option>
                    <option value="Customer Request">Customer Request</option>
                    <option value="Product Defect">Product Defect</option>
                    <option value="Wrong Item Shipped">Wrong Item Shipped</option>
                    <option value="Item Not Received">Item Not Received</option>
                    <option value="Duplicate Payment">Duplicate Payment</option>
                    <option value="Fraudulent Transaction">Fraudulent Transaction</option>
                    <option value="Technical Error">Technical Error</option>
                    <option value="Other">Other</option>
                  </select>
                  <div className="form-text">
                    Select the reason for this refund.
                  </div>
                </div>

                <div className="alert alert-warning" role="alert">
                  <i className="bi bi-exclamation-triangle me-2"></i>
                  <strong>Warning:</strong> This action cannot be undone. The refund will be processed immediately.
                </div>

                <div className="d-flex gap-2">
                  <button type="submit" className="btn btn-warning">
                    <i className="bi bi-arrow-return-left me-2"></i>
                    Process Refund
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