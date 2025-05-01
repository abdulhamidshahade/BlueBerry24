using AutoMapper;
using BlueBerry24.Application.Dtos.CouponDtos;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using BlueBerry24.Domain.Entities.Coupon;
using BlueBerry24.Domain.Repositories.CouponInterfaces;

namespace BlueBerry24.Application.Services.Concretes.CouponServiceConcretes
{
    public class UserCouponService : IUserCouponService
    {
        private readonly IUserService _userService;
        private readonly ICouponService _couponService;
        private readonly IMapper _mapper;
        private readonly IUserCouponRepository _userCouponRepository;

        public UserCouponService(IUserService userService, 
            ICouponService couponService,
            IMapper mapper,
            IUserCouponRepository userCouponRepository)
        {
            _userService = userService;
            _couponService = couponService;
            _mapper = mapper;
            _userCouponRepository = userCouponRepository;
        }

        public async Task<UserCouponDto> AddCouponToUserAsync(int userId, int couponId)
        {
            var userExists = await _userService.IsUserExistsByIdAsync(userId);
            var couponExists = await _couponService.ExistsByIdAsync(couponId);

            if (!userExists || !couponExists)
            {
                return null;
            }

            var userCoupon = new UserCoupon
            {
                CouponId = couponId,
                UserId = userId,
                IsUsed = false
            };

            var addedCouponToUser = await _userCouponRepository.AddCouponToUserAsync(userId, couponId);
            var userCouponDto = _mapper.Map<UserCouponDto>(addedCouponToUser);
            return userCouponDto;
        }

        public async Task<bool> DisableCouponToUser(int userId, int couponId)
        {
            var userExists = await _userService.IsUserExistsByIdAsync(userId);
            var couponExists = await _couponService.GetByIdAsync(couponId);

            if (!userExists || couponExists == null)
            {
                return false;
            }

            //var userCoupon = await _context.Users_Coupons.FirstOrDefaultAsync(u => u.UserId == userId && u.CouponId == couponId && !u.IsUsed);

            List<string> userHasCoupons = await _userCouponRepository.GetCouponsByUserIdAsync(userId);

            if (!userHasCoupons.Contains(couponExists.Code))
            {
                return false;
            }

            var disabledCoupon = await _userCouponRepository.DisableCouponToUserAsync(userId, couponId);

            return disabledCoupon;
        }

        public async Task<List<string>> GetCouponsByUserIdAsync(int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return null;
            }

            //var coupons = await _context.Users_Coupons.Where(u => u.UserId == userId && !u.IsUsed).ToListAsync();

            List<string> couponCodes = await _userCouponRepository.GetCouponsByUserIdAsync(userId);

            if(couponCodes == null)
            {
                return null;
            }

            //foreach (var userCoupon in coupons)
            //{
            //    couponCodes.Add(userCoupon.CouponId.ToString());
            //}

            return couponCodes;
        }

        public async Task<List<string>> GetUsersByCouponIdAsync(int couponId)
        {
            if (!await _couponService.ExistsByIdAsync(couponId))
            {
                return null;
            }


            //var users = await _context.Users_Coupons.Where(u => u.CouponId == couponId).Select(u => u.UserId).Distinct().ToListAsync();

            List<string> userList = await _userCouponRepository.GetUsersByCouponIdAsync(couponId);

            //foreach (var userCoupon in users)
            //{
            //    userList.Add(userCoupon.ToString());
            //}

            return userList;
        }

        public async Task<bool> IsCouponUsedByUser(int userId, int couponId)
        {
            //var userExists = await _userService.IsUserExistsByIdAsync(userId);
            //var couponExists = await _couponService.IsCouponExistsByIdAsync(couponId);

            //var couponClient = new CouponRpcClient(_configuration);
            //await couponClient.InitializeRabbitMqConnection();

            //var userClinet = new UserRpcClient(_configuration);
            //await userClinet.InitializeRabbitMqConnection();

            //var couponExists = await couponClient.IsCouponAvaiableAsync(couponCode);
            //var userExists = await userClinet.IsUserAvailabeAsync(userId);

            //if (!couponExists || !userExists)
            //{
            //    throw new NotFoundException("User or coupon doesn't exists!");
            //}

            //var used = await _context.Users_Coupons.AnyAsync(u => u.UserId == userId && u.CouponId == couponCode && u.IsUsed);

            //return used;

            var userExists = await _userService.IsUserExistsByIdAsync(userId);
            var couponExists = await _couponService.ExistsByIdAsync(couponId);

            if (!userExists || !couponExists)
            {
                return false;
            }

            var isCouponUsed = await _userCouponRepository.IsCouponUsedByUser(userId, couponId);

            return isCouponUsed;
        }
    }
}
