﻿using BlueBerry24.Services.UserCouponAPI.Models.DTOs;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace BlueBerry24.Services.UserCouponAPI.Models
{
    public class UserCoupon
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public int CouponId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public UserCoupon()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }


    }
}
