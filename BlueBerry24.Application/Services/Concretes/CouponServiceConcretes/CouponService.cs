﻿using AutoMapper;
using BlueBerry24.Application.Dtos.CouponDtos;
using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using BlueBerry24.Domain.Entities.CouponEntities;

using BlueBerry24.Domain.Repositories.CouponInterfaces;


namespace BlueBerry24.Application.Services.Concretes.CouponServiceConcretes
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        public CouponService(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        public async Task<CouponDto> GetByIdAsync(int id)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);

            if (coupon == null)
            {
                return null;
            }

            return _mapper.Map<CouponDto>(coupon);
        }

        public async Task<CouponDto> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return null;
            }

            var coupon = await _couponRepository.GetByCodeAsync(code);
            if (coupon == null)
            {
                return null;
            }

            return _mapper.Map<CouponDto>(coupon);
        }

        public async Task<IEnumerable<CouponDto>> GetAllAsync()
        {
            var coupons = await _couponRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }

        public async Task<CouponDto> CreateAsync(CreateCouponDto couponDto)
        {
            if (couponDto == null)
            {
                return null;
            }

            //await ValidateCouponDto(couponDto);

            if (await ExistsByCodeAsync(couponDto.Code))
            {
                return null;
            }

            var coupon = _mapper.Map<Coupon>(couponDto);

            var createdCoupon = await _couponRepository.CreateAsync(coupon);
            
            //business doesn't worry about db operations.
            //await _unitOfWork.SaveDbChangesAsync();

            return _mapper.Map<CouponDto>(createdCoupon);
        }


        public async Task<CouponDto> UpdateAsync(int id, UpdateCouponDto couponDto)
        {
            if (couponDto == null)
            {
                return null;
            }



            //var existingCoupon = await _couponRepository.GetByIdAsync(id);
            //if (existingCoupon == null)
            //{
            //    return null;
            //}

            //var couponWithSameCode = await _couponRepository.GetAsync(c => c.Code == couponDto.Code && c.Id != id);

            //if (couponWithSameCode != null)
            //{
            //    return null;
            //}

            //existingCoupon.DiscountAmount = couponDto.DiscountAmount;
            //existingCoupon.MinimumAmount = couponDto.MinimumAmount;

            var mappedCoupon = _mapper.Map<Coupon>(couponDto);

            var updatedCoupon = await _couponRepository.UpdateAsync(id, mappedCoupon);
            return _mapper.Map<CouponDto>(updatedCoupon);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon == null)
            {
                return false;
            }

            var deletedCoupon = await _couponRepository.DeleteAsync(coupon);
            return deletedCoupon;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _couponRepository.ExistsAsync(i => i.Id == id);
        }

        public async Task<bool> ExistsByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }

            return await _couponRepository.ExistsAsync(c => c.Code == code);
        }


        //private async Task ValidateCouponDto(CreateCouponDto couponDto)
        //{
        //    if (string.IsNullOrWhiteSpace(couponDto.Code))
        //    {
        //        throw new ValidationException("Coupon code cannot be empty");
        //    }

        //    if (couponDto.DiscountAmount <= 0)
        //    {
        //        throw new ValidationException("Discount amount must be greater than zero");
        //    }

        //    if (couponDto.MinimumAmount < 0)
        //    {
        //        throw new ValidationException("Minimum amount cannot be negative");
        //    }
        //}
    }
}
