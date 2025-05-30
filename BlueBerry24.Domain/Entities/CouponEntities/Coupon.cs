﻿using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.Base;
using BlueBerry24.Domain.Entities.ShoppingCartEntities;

namespace BlueBerry24.Domain.Entities.CouponEntities
{
    public class Coupon : IAuditableEntity
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public CouponType Type { get; set; }

        public decimal Value { get; set; }

        public bool IsForNewUsersOnly { get; set; }

        public List<UserCoupon> UserCoupons { get; set; }

        public List<CartCoupon> CartCoupons { get; set; }
    }
}
