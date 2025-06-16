import { Payment, CreatePayment, PaymentResponse, UpdatePaymentStatus, RefundPayment, PaymentStats, PaymentSearchParams, PaymentStatus } from "@/types/payment";

export interface IPaymentService {
  processPayment(paymentData: CreatePayment): Promise<PaymentResponse>;
  getPaymentById(id: number): Promise<Payment>;
  getPaymentByTransactionId(transactionId: string): Promise<Payment>;
  getPaymentByOrderId(orderId: number): Promise<Payment>;
  getAllPayments(): Promise<Payment[]>;
  getPaymentsByUserId(userId: number): Promise<Payment[]>;
  getMyPayments(page?: number, pageSize?: number): Promise<Payment[]>;
  getPaymentsByStatus(status: PaymentStatus): Promise<Payment[]>;
  getPaymentsByDateRange(startDate: string, endDate: string): Promise<Payment[]>;
  getPaginatedPayments(page?: number, pageSize?: number): Promise<Payment[]>;
  updatePaymentStatus(id: number, data: UpdatePaymentStatus): Promise<PaymentResponse>;
  refundPayment(id: number, data: RefundPayment): Promise<PaymentResponse>;
  deletePayment(id: number): Promise<boolean>;
  getTotalPaymentCount(): Promise<number>;
  getTotalAmountByDateRange(startDate: string, endDate: string): Promise<number>;
  searchPayments(params: PaymentSearchParams): Promise<Payment[]>;
  verifyPaymentWithProvider(transactionId: string): Promise<PaymentResponse>;
} 