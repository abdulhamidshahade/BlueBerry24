using AutoMapper;
using BlueBerry24.Services.CouponAPI.Exceptions;
using BlueBerry24.Services.CouponAPI.Models.DTOs;
using BlueBerry24.Services.CouponAPI.Models;
using BlueBerry24.Services.CouponAPI.Services.Generic;
using BlueBerry24.Services.CouponAPI.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BlueBerry24.Services.CouponAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly IRepository<Coupon> _couponRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CouponService(IRepository<Coupon> couponRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetByIdAsync(string id)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);

            if (coupon == null)
            {
                throw new NotFoundException($"Coupon with id {id} not found");
            }

            return _mapper.Map<CouponDto>(coupon);
        }

        public async Task<CouponDto> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Coupon code cannot be empty", nameof(code));
            }

            var coupon = await _couponRepository.GetAsync(c => c.Code == code);
            if (coupon == null)
            {
                throw new NotFoundException($"Coupon with code {code} not found");
            }

            return _mapper.Map<CouponDto>(coupon);
        }

        public async Task<IEnumerable<CouponDto>> GetAllAsync()
        {
            var coupons = await _couponRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }

        public async Task<CouponDto> CreateAsync(CouponDto couponDto)
        {
            if (couponDto == null)
            {
                throw new ArgumentNullException(nameof(couponDto));
            }

            await ValidateCouponDto(couponDto);

            if (await ExistsByCodeAsync(couponDto.Code))
            {
                throw new DuplicateEntityException($"Coupon with code {couponDto.Code} already exists");
            }

            var coupon = _mapper.Map<Coupon>(couponDto);

            await _couponRepository.AddAsync(coupon);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CouponDto>(coupon);
        }


        public async Task<CouponDto> UpdateAsync(string id, CouponDto couponDto)
        {
            if (couponDto == null)
            {
                throw new ArgumentNullException(nameof(couponDto));
            }

            await ValidateCouponDto(couponDto);

            var existingCoupon = await _couponRepository.GetByIdAsync(id);
            if (existingCoupon == null)
            {
                throw new NotFoundException($"Coupon with id {id} not found");
            }

            var couponWithSameCode = await _couponRepository.GetAsync(c => c.Code == couponDto.Code && c.Id != id);

            if (couponWithSameCode != null)
            {
                throw new DuplicateEntityException($"Coupon with code {couponDto.Code} already exists");
            }

            existingCoupon.Code = couponDto.Code;
            existingCoupon.DiscountAmount = couponDto.DiscountAmount;
            existingCoupon.MinimumAmount = couponDto.MinimumAmount;

            _couponRepository.Update(existingCoupon);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CouponDto>(existingCoupon);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon == null)
            {
                return false;
            }

            _couponRepository.Delete(coupon);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _couponRepository.ExistsAsync(i => i.Id == id);
        }

        public async Task<bool> ExistsByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Coupon code cannot be empty", nameof(code));
            }

            return await _couponRepository.ExistsAsync(c => c.Code == code);
        }


        private async Task ValidateCouponDto(CouponDto couponDto)
        {
            if (string.IsNullOrWhiteSpace(couponDto.Code))
            {
                throw new ValidationException("Coupon code cannot be empty");
            }

            if (couponDto.DiscountAmount <= 0)
            {
                throw new ValidationException("Discount amount must be greater than zero");
            }

            if (couponDto.MinimumAmount < 0)
            {
                throw new ValidationException("Minimum amount cannot be negative");
            }
        }
    }
}
