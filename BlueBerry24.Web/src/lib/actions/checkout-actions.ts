'use server'

import { redirect } from 'next/navigation';
import { OrderService } from '../services/order/service';
import { CheckoutRequest } from '../../types/cart';
import { getOrCreateCart } from './cart-actions';
import { CartService } from '../services/cart/service';
import { ICartService } from '../services/cart/interface';

const cartService: ICartService = new CartService();

export async function processCheckout(formData: FormData) {
  try {
    console.log('Starting checkout process...');
    
    console.log('Getting cart...');
    const cart = await getOrCreateCart();
    console.log('Cart retrieved:', { id: cart?.id, itemCount: cart?.cartItems?.length });
    
    if (!cart || !cart.cartItems || cart.cartItems.length === 0) {
      console.error('Cart validation failed:', { cart: cart ? 'exists' : 'null', items: cart?.cartItems?.length });
      redirect('/checkout?error=' + encodeURIComponent('Your cart is empty. Please add items before proceeding to checkout.'));
    }

    console.log('Extracting form data...');
    const checkoutData: CheckoutRequest = {
      cartId: cart.id,
      customerEmail: formData.get('email') as string,
      customerPhone: formData.get('phone') as string || undefined,
      shippingName: `${formData.get('firstName')} ${formData.get('lastName')}`,
      shippingAddressLine1: formData.get('address') as string,
      shippingAddressLine2: formData.get('address2') as string || undefined,
      shippingCity: formData.get('city') as string,
      shippingState: formData.get('state') as string,
      shippingPostalCode: formData.get('zip') as string,
      shippingCountry: formData.get('country') as string || 'US',
      paymentProvider: 'Credit Card',
      paymentTransactionId: Math.floor(Math.random() * 1000000),
      isPaid: true
    };
    
    console.log('Extracted checkout data:', { 
      customerEmail: checkoutData.customerEmail, 
      shippingName: checkoutData.shippingName,
      cartId: cart.id 
    });

    const errors: Record<string, string> = {};
    
    const firstName = formData.get('firstName') as string;
    const lastName = formData.get('lastName') as string;
    
    if (!firstName?.trim()) {
      errors.firstName = 'First name is required';
    }
    
    if (!lastName?.trim()) {
      errors.lastName = 'Last name is required';
    }
    
    if (!checkoutData.customerEmail?.trim()) {
      errors.email = 'Email is required';
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(checkoutData.customerEmail)) {
      errors.email = 'Please enter a valid email address';
    }
    
    if (!checkoutData.shippingAddressLine1?.trim()) {
      errors.address = 'Address is required';
    }
    
    if (!checkoutData.shippingCity?.trim()) {
      errors.city = 'City is required';
    }
    
    if (!checkoutData.shippingState?.trim()) {
      errors.state = 'State is required';
    }
    
    if (!checkoutData.shippingPostalCode?.trim()) {
      errors.zipCode = 'Zip code is required';
    }
    
    const cardNumber = formData.get('cardNumber') as string;
    const expiryDate = formData.get('expiryDate') as string;
    const cvv = formData.get('cvv') as string;
    const cardName = formData.get('cardName') as string;
    
    if (!cardNumber?.trim()) {
      errors.cardNumber = 'Card number is required';
    } else if (!/^\d{4}\s?\d{4}\s?\d{4}\s?\d{4}$/.test(cardNumber.replace(/\s/g, ''))) {
      errors.cardNumber = 'Please enter a valid card number';
    }
    
    if (!expiryDate?.trim()) {
      errors.expiryDate = 'Expiry date is required';
    } else if (!/^(0[1-9]|1[0-2])\/\d{2}$/.test(expiryDate)) {
      errors.expiryDate = 'Please enter a valid expiry date (MM/YY)';
    }
    
    if (!cvv?.trim()) {
      errors.cvv = 'CVV is required';
    } else if (!/^\d{3,4}$/.test(cvv)) {
      errors.cvv = 'Please enter a valid CVV';
    }
    
    if (!cardName?.trim()) {
      errors.cardName = 'Name on card is required';
    }

    if (Object.keys(errors).length > 0) {
      const errorParams = new URLSearchParams();
      Object.entries(errors).forEach(([key, value]) => {
        errorParams.set(`error_${key}`, value);
      });
      
      const formFields = ['firstName', 'lastName', 'email', 'phone', 'address', 'address2', 'city', 'state', 'zip', 'country'];
      formFields.forEach(field => {
        const value = formData.get(field) as string;
        if (value) {
          errorParams.set(field, value);
        }
      });
      
      redirect(`/checkout?${errorParams.toString()}`);
    }

    console.log('Calling order service checkout...');
    const result = await cartService.checkout(cart.id, checkoutData);
    console.log('Checkout result:', result);
    
    console.log('Redirecting to order details page...');
    redirect(`/checkout/order?id=${result.id}`);
  } catch (error) {
    console.error('Error processing checkout:', error);
    console.error('Error stack:', error instanceof Error ? error.stack : 'No stack trace');
    
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    
    const errorMessage = error instanceof Error ? error.message : 'Failed to process checkout. Please try again.';
    redirect('/checkout?error=' + encodeURIComponent(errorMessage));
  }
}

export async function createOrderForPayment(formData: FormData) {
  try {
    console.log('Starting order creation for payment...');
    
    console.log('Getting cart...');
    const cart = await getOrCreateCart();
    console.log('Cart retrieved:', { id: cart?.id, itemCount: cart?.cartItems?.length });
    
    if (!cart || !cart.cartItems || cart.cartItems.length === 0) {
      console.error('Cart validation failed:', { cart: cart ? 'exists' : 'null', items: cart?.cartItems?.length });
      redirect('/checkout?error=' + encodeURIComponent('Your cart is empty. Please add items before proceeding to checkout.'));
    }

    console.log('Extracting form data...');
    const checkoutData: CheckoutRequest = {
      cartId: cart.id,
      customerEmail: formData.get('email') as string,
      customerPhone: formData.get('phone') as string || undefined,
      shippingName: `${formData.get('firstName')} ${formData.get('lastName')}`,
      shippingAddressLine1: formData.get('address') as string,
      shippingAddressLine2: formData.get('address2') as string || undefined,
      shippingCity: formData.get('city') as string,
      shippingState: formData.get('state') as string,
      shippingPostalCode: formData.get('zip') as string,
      shippingCountry: formData.get('country') as string || 'US',
      paymentProvider: 'Pending',
      paymentTransactionId: 0,
      isPaid: false
    };
    
    console.log('Extracted checkout data:', { 
      customerEmail: checkoutData.customerEmail, 
      shippingName: checkoutData.shippingName,
      cartId: cart.id 
    });

    const errors: Record<string, string> = {};
    
    const firstName = formData.get('firstName') as string;
    const lastName = formData.get('lastName') as string;
    
    if (!firstName?.trim()) {
      errors.firstName = 'First name is required';
    }
    
    if (!lastName?.trim()) {
      errors.lastName = 'Last name is required';
    }
    
    if (!checkoutData.customerEmail?.trim()) {
      errors.email = 'Email is required';
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(checkoutData.customerEmail)) {
      errors.email = 'Please enter a valid email address';
    }
    
    if (!checkoutData.shippingAddressLine1?.trim()) {
      errors.address = 'Address is required';
    }
    
    if (!checkoutData.shippingCity?.trim()) {
      errors.city = 'City is required';
    }
    
    if (!checkoutData.shippingState?.trim()) {
      errors.state = 'State is required';
    }
    
    if (!checkoutData.shippingPostalCode?.trim()) {
      errors.zipCode = 'Zip code is required';
    }

    if (Object.keys(errors).length > 0) {
      const errorParams = new URLSearchParams();
      Object.entries(errors).forEach(([key, value]) => {
        errorParams.set(`error_${key}`, value);
      });
      
      const formFields = ['firstName', 'lastName', 'email', 'phone', 'address', 'address2', 'city', 'state', 'zip', 'country'];
      formFields.forEach(field => {
        const value = formData.get(field) as string;
        if (value) {
          errorParams.set(field, value);
        }
      });
      
      redirect(`/checkout?${errorParams.toString()}`);
    }

    console.log('Calling order service checkout...');
    const result = await cartService.checkout(cart.id, checkoutData);
    console.log('Checkout result:', result);
    
    console.log('Redirecting to payment page...');
    redirect(`/checkout/order?id=${result.id}`);
  } catch (error) {
    console.error('Error creating order for payment:', error);
    console.error('Error stack:', error instanceof Error ? error.stack : 'No stack trace');
    
    if (error instanceof Error && error.message === 'NEXT_REDIRECT') {
      throw error;
    }
    
    const errorMessage = error instanceof Error ? error.message : 'Failed to create order. Please try again.';
    redirect('/checkout?error=' + encodeURIComponent(errorMessage));
  }
} 