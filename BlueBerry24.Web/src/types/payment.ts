export enum PaymentStatus {
  Pending = 0,
  Processing = 1,
  Completed = 2,
  Failed = 3,
  Cancelled = 4,
  Refunded = 5,
  PartiallyRefunded = 6,
  Disputed = 7,
  Expired = 8
}

export enum PaymentMethod {
  CreditCard = 0,
  DebitCard = 1,
  PayPal = 2,
  BankTransfer = 3,
  DigitalWallet = 4,
  Cryptocurrency = 5,
  GiftCard = 6,
  StoreCredit = 7,
  CashOnDelivery = 8
}

export interface Payment {
  id: number;
  userId?: number;
  orderId?: number;
  sessionId?: string;
  transactionId: string;
  status: PaymentStatus;
  method: PaymentMethod;
  provider: string;
  amount: number;
  currency: string;
  providerTransactionId?: string;
  providerPaymentMethodId?: string;
  cardLast4?: string;
  cardBrand?: string;
  payerEmail?: string;
  payerName?: string;
  billingAddress1?: string;
  billingAddress2?: string;
  billingCity?: string;
  billingState?: string;
  billingPostalCode?: string;
  billingCountry?: string;
  processingFee: number;
  netAmount: number;
  processedAt?: string;
  completedAt?: string;
  failedAt?: string;
  refundedAt?: string;
  errorMessage?: string;
  failureReason?: string;
  metadata?: string;
  notes?: string;
  createdAt: string;
  updatedAt: string;
  userName?: string;
  userEmail?: string;
  orderReferenceNumber?: string;
}

export interface CreatePayment {
  orderId?: number;
  method: PaymentMethod;
  provider: string;
  amount: number;
  currency?: string;
  providerPaymentMethodId?: string;
  cardLast4?: string;
  cardBrand?: string;
  payerEmail?: string;
  payerName?: string;
  billingAddress1?: string;
  billingAddress2?: string;
  billingCity: string;
  billingState: string;
  billingPostalCode: string;
  billingCountry: string;
  metadata?: string; 
  notes?: string;
}

export interface PaymentResponse {
  success: boolean;
  message: string;
  payment?: Payment;
  transactionId?: string;
  status: PaymentStatus;
  redirectUrl?: string;
  metadata?: Record<string, any>;
}

export interface UpdatePaymentStatus {
  status: PaymentStatus;
  notes?: string;
}

export interface RefundPayment {
  refundAmount?: number;
  reason?: string;
}

export interface PaymentStats {
  totalCount: number;
  totalAmount: number;
  completedCount: number;
  failedCount: number;
  refundedCount: number;
}

export interface PaymentSearchParams {
  searchTerm?: string;
  status?: PaymentStatus;
  startDate?: string;
  endDate?: string;
  page?: number;
  pageSize?: number;
} 