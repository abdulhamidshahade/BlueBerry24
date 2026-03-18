"use server";

import { apiRequest } from "./api";

export interface CheckoutFormData {
  firstName: string;
  lastName: string;
  email: string;
  phone?: string;
  address: string;
  address2?: string;
  city: string;
  state: string;
  zipCode: string;
  country?: string;
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

export async function saveCheckoutData(data: CheckoutFormData): Promise<void> {
  try {
    console.log('💾 Saving checkout data to database:', data);
    const response = await apiRequest(`${API_ENDPOINT}/checkout`, {
      method: "POST",
      requireAuth: false,
      body: JSON.stringify(data),
    });
    console.log('✅ Checkout data saved successfully:', response);
  } catch (error) {
    console.error("❌ Error saving checkout data to database:", error);
    // Don't throw - we don't want to block the user if save fails
  }
}

export async function getCheckoutData(): Promise<CheckoutFormData | null> {
  try {
    console.log('📥 Fetching checkout data from database:', API_ENDPOINT);
    const response: any = await apiRequest(API_ENDPOINT, {
      requireAuth: false,
    });
    
    console.log('📦 Database response:', response);
    
    if (!response?.isSuccess || !response?.data) {
      console.log('ℹ️ No checkout data found in database');
      return null;
    }
    
    const data: UserCheckoutInfoDto = response.data;
    
    const result = {
      firstName: data.firstName,
      lastName: data.lastName,
      email: data.email,
      phone: data.phone,
      address: data.address,
      address2: data.address2,
      city: data.city,
      state: data.state,
      zipCode: data.zipCode,
      country: data.country
    };
    
    console.log('✅ Checkout data retrieved from database:', result);
    return result;
  } catch (error) {
    console.error("❌ Error retrieving checkout data from database:", error);
    return null;
  }
}

export async function clearCheckoutData(): Promise<void> {
  try {
    console.log('🗑️ Deleting checkout data from database');
    await apiRequest(API_ENDPOINT, {
      method: "DELETE",
      requireAuth: false,
    });
    console.log('✅ Checkout data deleted from database');
  } catch (error) {
    console.error("❌ Error deleting checkout data from database:", error);
  }
}
