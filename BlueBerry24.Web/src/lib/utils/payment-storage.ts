"use server";

import { apiRequest } from "./api";

export interface PaymentBillingData {
  payerName: string;
  payerEmail: string;
  billingAddress1: string;
  billingAddress2?: string;
  billingCity: string;
  billingState: string;
  billingPostalCode: string;
  billingCountry?: string;
}

interface UserCheckoutInfoDto {
  id: number;
  userId: number;
  sessionId?: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  address: string;
  address2?: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  payerName?: string;
  payerEmail?: string;
  billingAddress1?: string;
  billingAddress2?: string;
  billingCity?: string;
  billingState?: string;
  billingPostalCode?: string;
  billingCountry?: string;
  lastUsedAt?: string;
}

const API_BASE_URL = process.env.NEXT_PUBLIC_API_BASE_URL || 'https://localhost:7105/api';
const API_ENDPOINT = `${API_BASE_URL}/UserCheckoutInfo`;

export async function savePaymentBillingData(data: PaymentBillingData): Promise<void> {
  try {
    console.log('💳 Saving payment billing data to database:', data);
    const response = await apiRequest(`${API_ENDPOINT}/billing`, {
      method: "POST",
      requireAuth: false,
      body: JSON.stringify(data),
    });
    console.log('✅ Payment billing data saved successfully:', response);
  } catch (error) {
    console.error("❌ Error saving payment billing data to database:", error);
    // Don't throw - we don't want to block the user if save fails
  }
}

export async function getPaymentBillingData(): Promise<PaymentBillingData | null> {
  try {
    console.log('📥 Fetching payment billing data from database:', API_ENDPOINT);
    const response: any = await apiRequest(API_ENDPOINT, {
      requireAuth: false,
    });
    
    console.log('📦 Database response:', response);
    
    if (!response?.isSuccess || !response?.data) {
      console.log('ℹ️ No payment billing data found in database');
      return null;
    }
    
    const data: UserCheckoutInfoDto = response.data;
    
    if (!data.payerName) {
      console.log('ℹ️ No payment billing info in database record');
      return null;
    }
    
    const result = {
      payerName: data.payerName,
      payerEmail: data.payerEmail || '',
      billingAddress1: data.billingAddress1 || '',
      billingAddress2: data.billingAddress2,
      billingCity: data.billingCity || '',
      billingState: data.billingState || '',
      billingPostalCode: data.billingPostalCode || '',
      billingCountry: data.billingCountry
    };
    
    console.log('✅ Payment billing data retrieved from database:', result);
    return result;
  } catch (error) {
    console.error("❌ Error retrieving payment billing data from database:", error);
    return null;
  }
}

export async function clearPaymentBillingData(): Promise<void> {
  try {
    console.log('🗑️ Deleting payment billing data from database');
    await apiRequest(API_ENDPOINT, {
      method: "DELETE",
      requireAuth: false,
    });
    console.log('✅ Payment billing data deleted from database');
  } catch (error) {
    console.error("❌ Error deleting payment billing data from database:", error);
  }
}
