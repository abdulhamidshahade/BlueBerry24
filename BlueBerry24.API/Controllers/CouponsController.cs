﻿using BlueBerry24.Application.Authorization.Attributes;
using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.AuthDtos;
using BlueBerry24.Application.Dtos.CouponDtos;
using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponsController : BaseController
    {
        private readonly ICouponService _couponService;
        private readonly IUserCouponService _userCouponService;
        private readonly ILogger<CouponsController> _logger;


        public CouponsController(ICouponService couponService,
                                 ILogger<CouponsController> logger,
                                 IUserCouponService userCouponService) : base(logger)
        {
            _couponService = couponService ?? throw new ArgumentNullException(nameof(couponService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userCouponService = userCouponService;
        }


        [HttpGet]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<IEnumerable<CouponDto>>>> GetAll()
        {
            _logger.LogInformation("Getting all coupons");

            var coupons = await _couponService.GetAllAsync();

            if (coupons == null)
            {
                _logger.LogError("Error retrieving all coupons");
                return new ResponseDto<IEnumerable<CouponDto>>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving coupons",
                    Errors = new List<string> { "An unexpected error occurred" }
                };
            }
            var response = new ResponseDto<IEnumerable<CouponDto>>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Coupons retrieved successfully",
                Data = coupons
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<CouponDto>>> GetById(int id)
        {
            _logger.LogInformation($"Getting coupon with ID: {id}");
            var coupon = await _couponService.GetByIdAsync(id);

            if (coupon == null)
            {
                _logger.LogError($"Error retrieving coupon with ID {id}");
                return new ResponseDto<CouponDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto<CouponDto>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Coupon retrieved successfully",
                Data = coupon
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("code/{code}")]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<CouponDto>>> GetByCode(string code)
        {
            _logger.LogInformation($"Getting coupon with code: {code}");
            var coupon = await _couponService.GetByCodeAsync(code);

            if (coupon == null)
            {
                _logger.LogError($"Error retrieving coupon with code {code}");
                return new ResponseDto<CouponDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error retrieving coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto<CouponDto>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Coupon retrieved successfully",
                Data = coupon
            };
            return Ok(response);
        }

        [HttpPost]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<CouponDto>>> Create([FromBody] CreateCouponDto couponDto)
        {
            _logger.LogInformation($"Creating new coupon with code: {couponDto.Code}");
            var createdCoupon = await _couponService.CreateAsync(couponDto);

            if (createdCoupon == null)
            {
                _logger.LogError("Error creating coupon");
                return new ResponseDto<CouponDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error creating coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto<CouponDto>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status201Created,
                StatusMessage = "Coupon created successfully",
                Data = createdCoupon
            };
            return response;


        }

        [HttpPut]
        [Route("{id}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<CouponDto>>> Update(int id, [FromBody] UpdateCouponDto couponDto)
        {
            _logger.LogInformation($"Updating coupon with ID: {id}");
            var updatedCoupon = await _couponService.UpdateAsync(id, couponDto);

            if (updatedCoupon == null)
            {
                _logger.LogError($"Error updating coupon with ID {id}");
                return new ResponseDto<CouponDto>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    StatusMessage = "Error updating coupon",
                    Errors = new List<string> { "An unexpected error occurred" }
                };

            }
            var response = new ResponseDto<CouponDto>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Coupon updated successfully",
                Data = updatedCoupon
            };
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> Delete(int id)
        {
            _logger.LogInformation($"Deleting coupon with ID: {id}");
            var deleted = await _couponService.DeleteAsync(id);

            if (!deleted)
            {
                _logger.LogWarning($"Coupon with ID {id} not found for deletion");
                var notFoundResponse = new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    StatusMessage = "Coupon not found",
                    Errors = new List<string> { $"Coupon with ID {id} not found" },
                    Data = false
                };
                return NotFound(notFoundResponse);
            }

            var response = new ResponseDto<bool>
            {
                IsSuccess = true,
                StatusCode = StatusCodes.Status200OK,
                StatusMessage = "Coupon deleted successfully",
                Data = true
            };
            return Ok(response);
        }

        [HttpGet]
        [Route("exists-by-id/{id}")]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> Exists(int id)
        {
            var exists = await _couponService.ExistsByIdAsync(id);

            if (exists)
            {
                var response = new ResponseDto<bool>
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Coupon exists",
                    Data = true
                };
                return Ok(response);
            }

            var notFoundResponse = new ResponseDto<bool>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                StatusMessage = "Coupon not found",
                Data = false
            };
            return NotFound(notFoundResponse);
        }

        [HttpGet]
        [Route("exists-by-code/{code}")]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> ExistsByCode(string code)
        {
            var exists = await _couponService.ExistsByCodeAsync(code);

            if (exists)
            {
                var response = new ResponseDto<bool>
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    StatusMessage = "Coupon exists",
                    Data = true
                };
                return Ok(response);
            }

            var notFoundResponse = new ResponseDto<bool>
            {
                IsSuccess = false,
                StatusCode = StatusCodes.Status404NotFound,
                StatusMessage = "Coupon not found",
                Data = false
            };
            return NotFound(notFoundResponse);
        }

        [HttpPost]
        [Route("users/{userId}/coupons")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<UserCouponDto>>> AddCouponToUser(int userId, [FromBody] AddCouponToUserDto addCouponToUserDto)
        {
            var entity = await _userCouponService.AddCouponToUserAsync(userId, addCouponToUserDto.CouponId);

            if (entity != null)
            {
                return Ok(new ResponseDto<UserCouponDto>
                {
                    Data = entity,
                    IsSuccess = true,
                    StatusCode = 201
                });
            }

            return BadRequest(new ResponseDto<UserCouponDto>
            {
                IsSuccess = false,
                StatusCode = 400,
                StatusMessage = "An error occurred while apply coupon to user"
            });
        }

        [HttpPut]
        [Route("users/{userId}/coupons/{couponId}/disable")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> DisableUserCoupon(int userId, int couponId)
        {
            var isDisabled = await _userCouponService.DisableCouponToUser(userId, couponId);

            if (isDisabled)
            {
                return Ok(new ResponseDto<bool>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    StatusMessage = $"The coupon has disabled for the user: {userId}",
                    Data = true
                });
            }

            return BadRequest(new ResponseDto<bool>
            {
                IsSuccess = false,
                StatusCode = 400,
                StatusMessage = "En error occurred while disabling the coupon",
                Data = false
            });
        }

        [HttpGet]
        [Route("users/{userId}/coupons")]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<List<CouponDto>>>> GetCouponsByUserId(int userId)
        {
            var coupons = await _userCouponService.GetCouponsByUserIdAsync(userId);

            if (coupons.Count != 0)
            {
                return Ok(new ResponseDto<List<CouponDto>>
                {
                    Data = coupons,
                    IsSuccess = true,
                    StatusCode = 200
                });
            }

            return NotFound(new ResponseDto<List<CouponDto>>
            {
                IsSuccess = false,
                StatusCode = 404,
                StatusMessage = "There is no coupons found by user"
            });
        }

        [HttpGet]
        [Route("{couponId}/users")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<List<ApplicationUserDto>>>> GetUsersByCouponId(int couponId)
        {
            var users = await _userCouponService.GetUsersByCouponIdAsync(couponId);

            if (users.Count != 0)
            {
                return Ok(new ResponseDto<List<ApplicationUserDto>>
                {
                    Data = users,
                    IsSuccess = true,
                    StatusCode = 200
                });
            }

            return NotFound(new ResponseDto<List<ApplicationUserDto>>
            {
                IsSuccess = false,
                StatusCode = 404,
                StatusMessage = "There is no users found by coupon"
            });
        }

        [HttpGet]
        [Route("users/{userId}/coupons/{couponCode}/used")]
        [UserAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> HasUserUsedCoupon(int userId, string couponCode)
        {

            var hasUsed = await _userCouponService.IsCouponUsedByUser(userId, couponCode);

            if (hasUsed)
            {
                return Ok(new ResponseDto<bool>
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    StatusMessage = "The coupon has used",
                    Data = true,
                });
            }
            else if (!hasUsed)
            {
                return Ok(new ResponseDto<bool>
                {
                    StatusMessage = "The coupon has not used",
                    StatusCode = 200,
                    IsSuccess = true,
                    Data = false
                });
            }

            else
            {
                return BadRequest(new ResponseDto<bool>
                {
                    StatusMessage = "Error exists while checking",
                    StatusCode = 400,
                    IsSuccess = false,
                    Data = false
                });
            }
        }


        [HttpPost]
        [Route("add-coupon-to-specific-users/{couponId}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> AddCouponToSpecificUsers(int couponId, [FromQuery]List<int> UserIds)
        {
            var addedCoupon = await _userCouponService.AddCouponToUsersAsync(UserIds, couponId);

            if (!addedCoupon)
            {
                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "An error occurred while applying coupon to users",
                    Data= false,
                });
            }

            return Ok(new ResponseDto<bool>
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "Coupon added to specific users successfully",
                Data = true
            });
        }

        [HttpPost]
        [Route("add-coupon-to-all-users/{couponId}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> AddCouponToAllUsers(int couponId)
        {
            var addedCoupon = await _userCouponService.AddCouponToAllUsersAsync(couponId);

            if (!addedCoupon)
            {
                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "An error occurred while applying coupon to all users",
                    Data = false
                });
            }

            return Ok(new ResponseDto<bool>
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "Coupon added to all users successfully",
                Data = true
            });
        }

        [HttpPost]
        [Route("add-coupon-to-new-users/{couponId}")]
        [AdminAndAbove]
        public async Task<ActionResult<ResponseDto<bool>>> AddCouponToNewUsers(int couponId)
        {
            var addedCoupon = await _userCouponService.AddCouponToNewUsersAsync(couponId);

            if (!addedCoupon)
            {
                return BadRequest(new ResponseDto<bool>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    StatusMessage = "An error occurred while applying coupon to new users",
                    Data = false
                });
            }

            return Ok(new ResponseDto<bool>
            {
                IsSuccess = true,
                StatusCode = 200,
                StatusMessage = "Coupon added to new users successfully",
                Data  = true
            });
        }
    }
}