import { CartStatus } from '@/types/cart';

interface CartStatusProps {
  status: CartStatus;
  className?: string;
}

export default function CartStatusIndicator({ status, className = '' }: CartStatusProps) {
  const getStatusInfo = (status: CartStatus) => {
    switch (status) {
      case CartStatus.Active:
        return {
          label: 'Active',
          color: 'success',
          icon: 'bi-cart-check',
          description: 'Your cart is ready for checkout'
        };
      case CartStatus.Abandoned:
        return {
          label: 'Abandoned',
          color: 'warning',
          icon: 'bi-cart-x',
          description: 'This cart was abandoned and stock has been released'
        };
      case CartStatus.Converted:
        return {
          label: 'Converted',
          color: 'primary',
          icon: 'bi-bag-check',
          description: 'This cart has been converted to an order'
        };
      case CartStatus.Expired:
        return {
          label: 'Expired',
          color: 'secondary',
          icon: 'bi-clock-history',
          description: 'This cart has expired and stock has been released'
        };
      default:
        return {
          label: 'Unknown',
          color: 'secondary',
          icon: 'bi-question-circle',
          description: 'Unknown cart status'
        };
    }
  };

  const statusInfo = getStatusInfo(status);

  return (
    <div className={`cart-status ${className}`}>
      <span 
        className={`badge bg-${statusInfo.color} d-inline-flex align-items-center gap-1`}
        title={statusInfo.description}
      >
        <i className={`bi ${statusInfo.icon}`}></i>
        {statusInfo.label}
      </span>
    </div>
  );
}

interface CartStatusMessageProps {
  status: CartStatus;
  className?: string;
}

export function CartStatusMessage({ status, className = '' }: CartStatusMessageProps) {
  if (status === CartStatus.Active) {
    return null;
  }

  const getStatusMessage = (status: CartStatus) => {
    switch (status) {
      case CartStatus.Abandoned:
        return {
          type: 'warning',
          icon: 'bi-exclamation-triangle',
          title: 'Cart Abandoned',
          message: 'This cart was abandoned and all reserved stock has been released back to inventory.'
        };
      case CartStatus.Converted:
        return {
          type: 'info',
          icon: 'bi-info-circle',
          title: 'Cart Converted',
          message: 'This cart has been successfully converted to an order. Stock has been confirmed and deducted.'
        };
      case CartStatus.Expired:
        return {
          type: 'secondary',
          icon: 'bi-clock',
          title: 'Cart Expired',
          message: 'This cart has expired due to inactivity. All reserved stock has been released.'
        };
      default:
        return null;
    }
  };

  const messageInfo = getStatusMessage(status);
  
  if (!messageInfo) {
    return null;
  }

  return (
    <div className={`alert alert-${messageInfo.type} d-flex align-items-center ${className}`} role="alert">
      <i className={`bi ${messageInfo.icon} me-2`}></i>
      <div>
        <strong>{messageInfo.title}:</strong> {messageInfo.message}
      </div>
    </div>
  );
} 