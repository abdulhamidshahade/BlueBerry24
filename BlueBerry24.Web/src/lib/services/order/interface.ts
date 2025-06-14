import { Order, OrderStatus } from "@/types/order";

export interface IOrderService {
  getById(orderId: number): Promise<Order>;
  getOrderByPaymentId(paymentId: number): Promise<Order>;
  getUserOrders(userId: number, page?: number, pageSize?: number): Promise<Order[]>;
  getAllOrders(page?: number, pageSize?: number): Promise<Order[]>;
  getOrdersByStatus(status: OrderStatus, page?: number, pageSize?: number): Promise<Order[]>;
  updateOrderStatus(orderId: number, status: number): Promise<boolean>;
  processOrder(orderId: number): Promise<boolean>;
  cancelOrder(orderId: number, reason: string): Promise<boolean>;
  refundOrder(orderId: number, reason: string): Promise<boolean>;
  markOrderAsPaid(orderId: number, paymentTransactionId: number, paymentProvider: string): Promise<boolean>;
} 