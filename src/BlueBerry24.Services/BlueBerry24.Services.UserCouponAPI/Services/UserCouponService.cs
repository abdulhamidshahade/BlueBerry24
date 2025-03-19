using AutoMapper;
using BlueBerry24.Services.UserCouponAPI.Data;
using BlueBerry24.Services.UserCouponAPI.Halpers.Exceptions;
using BlueBerry24.Services.UserCouponAPI.Models;
using BlueBerry24.Services.UserCouponAPI.Models.DTOs;
using BlueBerry24.Services.UserCouponAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Services.UserCouponAPI.Services
{
    public class UserCouponService : IUserCouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserService _userService;
        private readonly ICouponService _couponService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserCouponService(IHttpClientFactory httpClientFactory, IUserService userService, ICouponService couponService,
            ApplicationDbContext context, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _userService = userService;
            _couponService = couponService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserCouponDto> AddCouponToUserAsync(string userId, string couponId)
        {
            var userExists = await _userService.IsUserExistsByIdAsync(userId);
            var couponExists = await _couponService.IsCouponExistsByIdAsync(couponId);

            if (!userExists || !couponExists)
            {
                throw new NotFoundException("User or coupon doesn't exists!");
            }

            var userCoupon = new UserCoupon
            {
                CouponId = couponId,
                UserId = userId,
                IsUsed = false
            };

            await _context.Users_Coupons.AddAsync(userCoupon);

            await _context.SaveChangesAsync();

            var userCouponDto = _mapper.Map<UserCouponDto>(userCoupon);

            return userCouponDto;
        }

        public async Task<bool> DisableUserCouponAsync(string userId, string couponId)
        {
            var userExists = await _userService.IsUserExistsByIdAsync(userId);
            var couponExists = await _couponService.IsCouponExistsByIdAsync(couponId);

            if (!userExists || !couponExists)
            {
                throw new NotFoundException("User or coupon doesn't exists!");
            }

            var userCoupon = await _context.Users_Coupons.FirstOrDefaultAsync(u => u.UserId == userId && u.CouponId == couponId && !u.IsUsed);

            if (userCoupon == null)
            {
                throw new NotFoundException("User's coupon doesn't exists");
            }

            userCoupon.IsUsed = true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IReadOnlyCollection<string>> GetCouponsByUserIdAsync(string userId)
        {
            if(!await _userService.IsUserExistsByIdAsync(userId))
            {
                throw new NotFoundException("User not found!");
            }

            var coupons = await _context.Users_Coupons.Where(u => u.UserId == userId && !u.IsUsed).ToListAsync();

            List<string> couponCodes = new List<string>();

            foreach(var userCoupon in coupons)
            {
                couponCodes.Add(userCoupon.CouponId.ToString());
            }

            return couponCodes;
        }

        public async Task<IReadOnlyCollection<string>> GetUsersByCouponIdAsync(string couponId)
        {
            if(!await _couponService.IsCouponExistsByIdAsync(couponId))
            {
                throw new NotFoundException("Coupon not found");
            }


            var users = await _context.Users_Coupons.Where(u => u.CouponId == couponId).Select(u => u.UserId).Distinct().ToListAsync();

            List<string> userList = new List<string>();

            foreach(var userCoupon in users)
            {
                userList.Add(userCoupon.ToString());
            }

            return userList;
        }

        public async Task<bool> HasUserUsedCouponAsync(string userId, string couponId)
        {
            var userExists = await _userService.IsUserExistsByIdAsync(userId);
            var couponExists = await _couponService.IsCouponExistsByIdAsync(couponId);

            if (!userExists || !couponExists)
            {
                throw new NotFoundException("User or coupon doesn't exists!");
            }

            var used = await _context.Users_Coupons.AnyAsync(u => u.UserId == userId && u.CouponId == couponId && u.IsUsed);

            return used;
        }
    }
}
