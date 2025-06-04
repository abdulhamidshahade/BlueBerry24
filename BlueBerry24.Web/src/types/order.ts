export enum OrderStatus {
  Pending = 0,
  Processing = 1,
  Shipped = 2,
  Delivered = 3,
  Completed = 4,
  Cancelled = 5,
  Refunded = 6
}

export interface OrderItem {
  id: number;
  orderId: number;
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  discountAmount: number;
  createdAt: string;
  updatedAt: string;
}

export interface Order {
  id: number;
  userId: number;
  cartId: number;
  sessionId?: string;
  referenceNumber?: string;
  status: OrderStatus;
  subTotal: number;
  taxAmount: number;
  shippingAmount: number;
  total: number;
  discountTotal: number;
  customerEmail: string;
  customerPhone?: string;
  paymentProvider?: string;
  paymentTransactionId?: number;
  isPaid: boolean;
  paidAt?: string;
  completedAt?: string;
  cancelledAt?: string;
  shippingName?: string;
  shippingAddress1?: string;
  shippingAddress2?: string;
  shippingCity?: string;
  shippingState?: string;
  shippingPostalCode?: string;
  shippingCountry?: string;
  createdAt: string;
  updatedAt: string;
  orderItems: OrderItem[];
}