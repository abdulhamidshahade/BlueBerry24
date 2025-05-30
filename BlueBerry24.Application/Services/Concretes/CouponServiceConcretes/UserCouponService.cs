using AutoMapper;
using BlueBerry24.Application.Dtos.AuthDtos;
using BlueBerry24.Application.Dtos.CouponDtos;
using BlueBerry24.Application.Services.Interfaces.AuthServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
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
            if(userId <= 0 || couponId <= 0)
            {
                return null;
            }

            var userExists = await _userService.IsUserExistsByIdAsync(userId);
            var couponExists = await _couponService.ExistsByIdAsync(couponId);

            if (!userExists || !couponExists)
            {
                return null;
            }

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

            List<CouponDto> userHasCoupons = _mapper.Map<List<CouponDto>>
                (await _userCouponRepository.GetCouponsByUserIdAsync(userId));

            if (!userHasCoupons.Any(i => i.Code == couponExists.Code))
            {
                return false;
            }

            var disabledCoupon = await _userCouponRepository.DisableCouponForUserAsync(userId, couponId);

            return disabledCoupon;
        }

        public async Task<List<CouponDto>> GetCouponsByUserIdAsync(int userId)
        {
            if (!await _userService.IsUserExistsByIdAsync(userId))
            {
                return null;
            }

            List<CouponDto> coupons = 
                _mapper.Map<List<CouponDto>>(await _userCouponRepository.GetCouponsByUserIdAsync(userId));

            if(coupons == null)
            {
                return null;
            }

            return coupons.ToList();
        }

        public async Task<List<ApplicationUserDto>> GetUsersByCouponIdAsync(int couponId)
        {
            if (!await _couponService.ExistsByIdAsync(couponId))
            {
                return null;
            }

            List<ApplicationUserDto> userList = 
                _mapper.Map<List<ApplicationUserDto>>(await _userCouponRepository.GetUsersByCouponIdAsync(couponId));

            return userList.ToList();
        }

        public async Task<bool> IsCouponUsedByUser(int userId, string couponCode)
        {
            var userExists = await _userService.IsUserExistsByIdAsync(userId);
            var couponExists = await _couponService.ExistsByCodeAsync(couponCode);

            if (!userExists || !couponExists)
            {
                return false;
            }

            var isCouponUsed = await _userCouponRepository.IsCouponUsedByUserAsync(userId, couponCode);

            return isCouponUsed;
        }
    }
}
