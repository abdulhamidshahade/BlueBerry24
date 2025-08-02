import { cookies } from 'next/headers';
import { Order } from '../../types/order';

export async function validateOrderAccess(order: Order): Promise<boolean> {
  const cookieStore = cookies();
  const sessionId = (await cookieStore).get('cart_session')?.value;
  
  if (!sessionId) {
    return false;
  }
  
  return true;
}

export async function getCurrentSessionId(): Promise<string | null> {
  const cookieStore = cookies();
  return (await cookieStore).get('cart_session')?.value || null;
} 