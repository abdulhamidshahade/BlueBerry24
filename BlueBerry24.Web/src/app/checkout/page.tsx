import { getCart } from '../../lib/actions/cart-actions';
import CheckoutForm from '../../components/checkout/CheckoutForm';
import Link from 'next/link';
import { redirect } from 'next/navigation';
import { getCheckoutData } from '../../lib/utils/checkout-storage';
import { CartStatus } from '../../types/cart';
import { OrderService } from '../../lib/services/order/service';

interface CheckoutPageProps {
  searchParams: Promise<{ [key: string]: string | string[] | undefined }>;
}

export const dynamic = 'force-dynamic';

export default async function CheckoutPage({ searchParams }: CheckoutPageProps) {
  const cart = await getCart();

  if (!cart || !cart.cartItems || cart.cartItems.length === 0) {
    redirect('/cart');
  }

  var resolvedSearchParams = await searchParams;

  // Try to get saved checkout data from database
  console.log('🔍 Checkout page loading - fetching saved data...');
  const savedData = await getCheckoutData();
  console.log('📊 Saved data retrieved:', savedData ? 'Data found' : 'No data', savedData);
  
  // If cart is in PendingPayment status, try to get data from existing order
  let orderData: any = null;
  if (cart.status === CartStatus.PendingPayment) {
    try {
      const orderService = new OrderService();
      const orders = await orderService.getUserOrders(cart.userId || 0, 1, 5);
      if (orders && orders.length > 0) {
        const recentOrder = orders.find(o => o.cartId === cart.id);
        if (recentOrder) {
          orderData = recentOrder;
        }
      }
    } catch (error) {
      console.error('Error fetching order data:', error);
    }
  }

  // Merge data: URL params > Order data > Saved database data
  const mergedParams: Record<string, string> = {};
  
  console.log('🔄 Merging data - savedData exists:', !!savedData);
  
  // Start with saved database data
  if (savedData) {
    console.log('✅ Using saved data from database');
    mergedParams.firstName = savedData.firstName;
    mergedParams.lastName = savedData.lastName;
    mergedParams.email = savedData.email;
    mergedParams.phone = savedData.phone || '';
    mergedParams.address = savedData.address;
    mergedParams.address2 = savedData.address2 || '';
    mergedParams.city = savedData.city;
    mergedParams.state = savedData.state;
    mergedParams.zipCode = savedData.zipCode;
    mergedParams.country = savedData.country || 'US';
  } else {
    console.log('ℹ️ No saved data available');
  }
  
  console.log('📋 Merged params:', mergedParams);

  // Override with order data if available
  if (orderData) {
    const nameParts = orderData.shippingName?.split(' ') || [];
    if (nameParts.length >= 2) {
      mergedParams.firstName = nameParts[0];
      mergedParams.lastName = nameParts.slice(1).join(' ');
    }
    if (orderData.customerEmail) mergedParams.email = orderData.customerEmail;
    if (orderData.customerPhone) mergedParams.phone = orderData.customerPhone;
    if (orderData.shippingAddress1) mergedParams.address = orderData.shippingAddress1;
    if (orderData.shippingAddress2) mergedParams.address2 = orderData.shippingAddress2;
    if (orderData.shippingCity) mergedParams.city = orderData.shippingCity;
    if (orderData.shippingState) mergedParams.state = orderData.shippingState;
    if (orderData.shippingPostalCode) mergedParams.zipCode = orderData.shippingPostalCode;
    if (orderData.shippingCountry) mergedParams.country = orderData.shippingCountry;
  }

  // Finally, URL search params take highest priority (including validation errors)
  Object.keys(resolvedSearchParams).forEach(key => {
    if (resolvedSearchParams[key]) {
      mergedParams[key] = resolvedSearchParams[key] as string;
    }
  });

  const error = resolvedSearchParams?.error as string;

  return (
    <div className="container py-4">
      <div className="row">
        <div className="col-12">
          <nav aria-label="breadcrumb">
            <ol className="breadcrumb">
              <li className="breadcrumb-item">
                <Link href="/">Home</Link>
              </li>
              <li className="breadcrumb-item">
                <Link href="/products">Products</Link>
              </li>
              <li className="breadcrumb-item">
                <Link href="/cart">Cart</Link>
              </li>
              <li className="breadcrumb-item active" aria-current="page">
                Checkout
              </li>
            </ol>
          </nav>
        </div>
      </div>

      <div className="row">
        <div className="col-12">
          <h1 className="mb-4">
            <i className="bi bi-credit-card me-2"></i>
            Checkout
          </h1>
        </div>
      </div>

      {(savedData || orderData) && !error && (
        <div className="row">
          <div className="col-12">
            <div className="alert alert-info d-flex align-items-center mb-4" role="alert">
              <i className="bi bi-info-circle-fill me-2"></i>
              <div>
                Your previous checkout information has been restored. You can modify any field before continuing.
              </div>
            </div>
          </div>
        </div>
      )}

      {error && (
        <div className="row">
          <div className="col-12">
            <div className="alert alert-danger d-flex align-items-center mb-4" role="alert">
              <i className="bi bi-exclamation-triangle-fill me-2"></i>
              <div>{error}</div>
              <Link 
                href="/checkout" 
                className="btn btn-sm btn-outline-secondary ms-auto"
              >
                <i className="bi bi-x me-1"></i>
                Dismiss
              </Link>
            </div>
          </div>
        </div>
      )}

      <CheckoutForm cart={cart} searchParams={mergedParams} />
    </div>
  );
} 