'use server'

import { revalidatePath } from 'next/cache';
import { redirect } from 'next/navigation';
import { OrderService } from '@/lib/services/order/service';
import { OrderStatus } from '@/types/order';

const orderService = new OrderService();

export async function updateOrderStatus(formData: FormData): Promise<void> {
  try {
    const orderId = parseInt(formData.get('orderId') as string);
    const newStatus = parseInt(formData.get('newStatus') as string) as OrderStatus;
    const redirectTo = formData.get('redirectTo') as string || '/admin/orders';

    if (isNaN(orderId) || isNaN(newStatus)) {
      throw new Error('Invalid order ID or status');
    }

    const success = await orderService.updateOrderStatus(orderId, newStatus);
    
    if (!success) {
      throw new Error('Failed to update order status');
    }

    revalidatePath('/admin/orders');
    revalidatePath(`/admin/orders/${orderId}`);
    redirect(`${redirectTo}?success=status-updated`);
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error updating order status:', error);
    const redirectTo = formData.get('redirectTo') as string || '/admin/orders';
    redirect(`${redirectTo}?error=status-update-failed`);
  }
}

export async function cancelOrder(formData: FormData): Promise<void> {
  try {
    const orderId = parseInt(formData.get('orderId') as string);
    const reason = formData.get('reason') as string;
    const redirectTo = formData.get('redirectTo') as string || '/admin/orders';

    if (isNaN(orderId) || !reason?.trim()) {
      throw new Error('Invalid order ID or cancellation reason');
    }

    const success = await orderService.cancelOrder(orderId, reason);
    
    if (!success) {
      throw new Error('Failed to cancel order');
    }

    revalidatePath('/admin/orders');
    revalidatePath(`/admin/orders/${orderId}`);
    redirect(`${redirectTo}?success=order-cancelled`);
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error cancelling order:', error);
    const redirectTo = formData.get('redirectTo') as string || '/admin/orders';
    redirect(`${redirectTo}?error=cancel-failed`);
  }
}

export async function refundOrder(formData: FormData): Promise<void> {
  try {
    const orderId = parseInt(formData.get('orderId') as string);
    const reason = formData.get('reason') as string;
    const redirectTo = formData.get('redirectTo') as string || '/admin/orders';

    if (isNaN(orderId) || !reason?.trim()) {
      throw new Error('Invalid order ID or refund reason');
    }

    const success = await orderService.refundOrder(orderId, reason);
    
    if (!success) {
      throw new Error('Failed to refund order');
    }

    revalidatePath('/admin/orders');
    revalidatePath(`/admin/orders/${orderId}`);
    redirect(`${redirectTo}?success=order-refunded`);
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error refunding order:', error);
    const redirectTo = formData.get('redirectTo') as string || '/admin/orders';
    redirect(`${redirectTo}?error=refund-failed`);
  }
}

export async function markOrderAsPaid(formData: FormData): Promise<void> {
  try {
    const orderId = parseInt(formData.get('orderId') as string);
    const paymentProvider = formData.get('paymentProvider') as string;
    const paymentTransactionId = parseInt(formData.get('paymentTransactionId') as string) || 0;
    const redirectTo = formData.get('redirectTo') as string || '/admin/orders';

    if (isNaN(orderId) || !paymentProvider?.trim()) {
      throw new Error('Invalid order ID or payment provider');
    }

    const success = await orderService.markOrderAsPaid(orderId, paymentTransactionId, paymentProvider);
    
    if (!success) {
      throw new Error('Failed to mark order as paid');
    }

    revalidatePath('/admin/orders');
    revalidatePath(`/admin/orders/${orderId}`);
    redirect(`${redirectTo}?success=order-marked-paid`);
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error marking order as paid:', error);
    const redirectTo = formData.get('redirectTo') as string || '/admin/orders';
    redirect(`${redirectTo}?error=mark-paid-failed`);
  }
}

export async function processOrder(formData: FormData): Promise<void> {
  try {
    const orderId = parseInt(formData.get('orderId') as string);
    const redirectTo = formData.get('redirectTo') as string || '/admin/orders';

    if (isNaN(orderId)) {
      throw new Error('Invalid order ID');
    }

  
    const success = await orderService.updateOrderStatus(orderId, OrderStatus.Processing);
    
    if (!success) {
      throw new Error('Failed to process order');
    }

    revalidatePath('/admin/orders');
    revalidatePath(`/admin/orders/${orderId}`);
    redirect(`${redirectTo}?success=order-processed`);
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error processing order:', error);
    const redirectTo = formData.get('redirectTo') as string || '/admin/orders';
    redirect(`${redirectTo}?error=process-failed`);
  }
}

export async function exportOrders(formData: FormData): Promise<void> {
  try {
    const status = formData.get('status') as string;
    const format = formData.get('format') as string || 'csv';

    const orderService = new OrderService();
    let orders;
    
    if (status && status !== 'all') {
      orders = await orderService.getOrdersByStatus(parseInt(status) as OrderStatus, 1, 1000);
    } else {
      orders = await orderService.getAllOrders(1, 1000);
    }

    revalidatePath('/admin/orders');
    redirect('/admin/orders?success=export-prepared');
  } catch (error) {
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    console.error('Error exporting orders:', error);
    redirect('/admin/orders?error=export-failed');
  }
}

export async function getOrderByPaymentId(paymentId: number) {
  try {
    return await orderService.getOrderByPaymentId(paymentId);
  } catch (error) {
    console.error("Error getting order by payment ID:", error);
    throw new Error("Failed to fetch order for payment");
  }
} 