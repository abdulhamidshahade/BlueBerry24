import { CouponDto, CouponType } from '../../types/coupon';

interface CouponTypeDisplayProps {
  type: CouponType;
}

export function CouponTypeDisplay({ type }: CouponTypeDisplayProps) {
  const typeInfo = {
    [CouponType.Percentage]: { 
      label: 'Percentage', 
      icon: 'bi-percent', 
      class: 'bg-success' 
    },
    [CouponType.FixedAmount]: { 
      label: 'Fixed Amount', 
      icon: 'bi-currency-dollar', 
      class: 'bg-primary' 
    },
  };

  const info = typeInfo[type];

  if (!info) {
    return (
      <span className="badge bg-secondary">
        <i className="bi bi-question-circle me-1"></i>
        Unknown
      </span>
    );
  }

  return (
    <span className={`badge ${info.class}`}>
      <i className={`${info.icon} me-1`}></i>
      {info.label}
    </span>
  );
}

interface CouponValueDisplayProps {
  coupon: CouponDto;
}

export function CouponValueDisplay({ coupon }: CouponValueDisplayProps) {
  switch (coupon.type) {
    case CouponType.Percentage:
      return `${coupon.value}%`;
    case CouponType.FixedAmount:
      return `$${coupon.value.toFixed(2)}`;
    default:
      return `$${coupon.value.toFixed(2)}`;
  }
}

interface CouponStatusDisplayProps {
  isActive: boolean;
}

export function CouponStatusDisplay({ isActive }: CouponStatusDisplayProps) {
  return (
    <span className={`badge ${isActive ? 'bg-success' : 'bg-secondary'}`}>
      <i className={`bi ${isActive ? 'bi-check-circle' : 'bi-x-circle'} me-1`}></i>
      {isActive ? 'Active' : 'Inactive'}
    </span>
  );
}

interface CouponNewUserDisplayProps {
  isForNewUsersOnly: boolean;
}

export function CouponNewUserDisplay({ isForNewUsersOnly }: CouponNewUserDisplayProps) {
  if (isForNewUsersOnly) {
    return (
      <span className="badge bg-info">
        <i className="bi bi-person-plus me-1"></i>
        New Users Only
      </span>
    );
  }
  
  return <span className="text-muted">All Users</span>;
}

interface CouponMinimumAmountDisplayProps {
  minimumAmount: number;
}

export function CouponMinimumAmountDisplay({ minimumAmount }: CouponMinimumAmountDisplayProps) {
  if (minimumAmount > 0) {
    return `$${minimumAmount.toFixed(2)}`;
  }
  
  return 'No minimum';
}

interface CouponFormSelectProps {
  name: string;
  required?: boolean;
  defaultValue?: CouponType;
}

export function CouponTypeSelect({ name, required = false, defaultValue }: CouponFormSelectProps) {
  return (
    <select
      className="form-select"
      id={name}
      name={name}
      required={required}
      defaultValue={defaultValue}
    >
      <option value="">Select coupon type</option>
      <option value={CouponType.Percentage}>Percentage Discount</option>
      <option value={CouponType.FixedAmount}>Fixed Amount Discount</option>
    </select>
  );
} 