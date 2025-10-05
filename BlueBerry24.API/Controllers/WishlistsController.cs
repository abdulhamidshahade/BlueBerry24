﻿using BlueBerry24.Application.Authorization.Attributes;
using BlueBerry24.Application.Dtos.WishlistDtos;
using BlueBerry24.Application.Services.Concretes.WishlistServiceConcretes;
using BlueBerry24.Application.Services.Interfaces.WishlistServiceInterfaces;
using BlueBerry24.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlueBerry24.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistsController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistsController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        private int GetUserId()
        {
            return Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        // GET: api/wishlists
        [HttpGet]
        [UserAndAbove]
        public async Task<IActionResult> GetUserWishlists()
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var wishlists = await _wishlistService.GetUserWishlistsAsync(userId);
                return Ok(new { IsSuccess = true, Data = wishlists });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [UserAndAbove]
        public async Task<IActionResult> GetWishlist(int id)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var wishlist = await _wishlistService.GetByIdAsync(id);
                if (wishlist == null)
                    return NotFound(new { IsSuccess = false, StatusMessage = "Wishlist not found" });

                if (wishlist.UserId != userId && !wishlist.IsPublic)
                    return Forbid();

                return Ok(new { IsSuccess = true, Data = wishlist });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }


        [HttpGet("default")]
        [UserAndAbove]
        public async Task<IActionResult> GetDefaultWishlist()
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var wishlist = await _wishlistService.GetUserDefaultWishlistAsync(userId);
                return Ok(new { IsSuccess = true, Data = wishlist });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }


        [HttpGet("summary")]
        [UserAndAbove]
        public async Task<IActionResult> GetUserSummary()
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var summary = await _wishlistService.GetUserSummaryAsync(userId);
                return Ok(new { IsSuccess = true, Data = summary });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }


        [HttpPost]
        [UserAndAbove]
        public async Task<IActionResult> CreateWishlist([FromBody] CreateWishlistDto createWishlistDto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var wishlist = await _wishlistService.CreateAsync(userId, createWishlistDto);
                return CreatedAtAction(nameof(GetWishlist), new { id = wishlist.Id },
                    new { IsSuccess = true, Data = wishlist });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }


        [HttpPut("{id}")]
        [UserAndAbove]
        public async Task<IActionResult> UpdateWishlist(int id, [FromBody] UpdateWishlistDto updateWishlistDto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingWishlist = await _wishlistService.GetByIdAsync(id);
                if (existingWishlist == null)
                    return NotFound(new { IsSuccess = false, StatusMessage = "Wishlist not found" });

                if (existingWishlist.UserId != userId)
                    return Forbid();

                var updatedWishlist = await _wishlistService.UpdateAsync(id, updateWishlistDto);
                return Ok(new { IsSuccess = true, Data = updatedWishlist });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [UserAndAbove]
        public async Task<IActionResult> DeleteWishlist(int id)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var existingWishlist = await _wishlistService.GetByIdAsync(id);
                if (existingWishlist == null)
                    return NotFound(new { IsSuccess = false, StatusMessage = "Wishlist not found" });

                if (existingWishlist.UserId != userId)
                    return Forbid();

                var result = await _wishlistService.DeleteAsync(id);
                if (!result)
                    return BadRequest(new { IsSuccess = false, StatusMessage = "Cannot delete the default wishlist" });

                return Ok(new { IsSuccess = true, StatusMessage = "Wishlist deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpPost("items/add")]
        [UserAndAbove]
        public async Task<IActionResult> AddItem([FromBody] AddToWishlistDto addToWishlistDto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var item = await _wishlistService.AddItemAsync(userId, addToWishlistDto);
                if (item == null)
                    return BadRequest(new { IsSuccess = false, StatusMessage = "Failed to add item to wishlist" });

                return Ok(new { IsSuccess = true, Data = item, StatusMessage = "Item added to wishlist successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }


        [HttpPut("{wishlistId}/items/{productId}")]
        [UserAndAbove]
        public async Task<IActionResult> UpdateItem(int wishlistId, int productId, [FromBody] UpdateWishlistItemDto updateItemDto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var wishlist = await _wishlistService.GetByIdAsync(wishlistId);
                if (wishlist == null || wishlist.UserId != userId)
                    return Forbid();

                var updatedItem = await _wishlistService.UpdateItemAsync(wishlistId, productId, updateItemDto);
                if (updatedItem == null)
                    return NotFound(new { IsSuccess = false, StatusMessage = "Wishlist item not found" });

                return Ok(new { IsSuccess = true, Data = updatedItem });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpDelete("{wishlistId}/items/{productId}")]
        [UserAndAbove]
        public async Task<IActionResult> RemoveItem(int wishlistId, int productId)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var wishlist = await _wishlistService.GetByIdAsync(wishlistId);
                if (wishlist == null || wishlist.UserId != userId)
                    return Forbid();

                var result = await _wishlistService.RemoveItemAsync(wishlistId, productId);
                if (!result)
                    return NotFound(new { IsSuccess = false, StatusMessage = "Wishlist item not found" });

                return Ok(new { IsSuccess = true, StatusMessage = "Item removed from wishlist successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpGet("check-product/{productId}")]
        [UserAndAbove]
        public async Task<IActionResult> CheckProductInWishlist(int productId)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var isInWishlist = await _wishlistService.IsProductInWishlistAsync(userId, productId);
                return Ok(new { IsSuccess = true, Data = new { IsInWishlist = isInWishlist } });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpPost("{wishlistId}/items/bulk-add")]
        [UserAndAbove]
        public async Task<IActionResult> AddMultipleItems(int wishlistId, [FromBody] List<int> productIds)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var wishlist = await _wishlistService.GetByIdAsync(wishlistId);
                if (wishlist == null || wishlist.UserId != userId)
                    return Forbid();

                var result = await _wishlistService.AddMultipleItemsAsync(userId, wishlistId, productIds);
                if (!result)
                    return BadRequest(new { IsSuccess = false, StatusMessage = "Failed to add items to wishlist" });

                return Ok(new { IsSuccess = true, StatusMessage = "Items added to wishlist successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpDelete("{wishlistId}/items/bulk-remove")]
        [UserAndAbove]
        public async Task<IActionResult> RemoveMultipleItems(int wishlistId, [FromBody] List<int> productIds)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var wishlist = await _wishlistService.GetByIdAsync(wishlistId);
                if (wishlist == null || wishlist.UserId != userId)
                    return Forbid();

                var result = await _wishlistService.RemoveMultipleItemsAsync(wishlistId, productIds);
                return Ok(new { IsSuccess = true, StatusMessage = "Items removed from wishlist successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpPut("{wishlistId}/share")]
        [UserAndAbove]
        public async Task<IActionResult> ShareWishlist(int wishlistId, [FromBody] bool isPublic)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var wishlist = await _wishlistService.GetByIdAsync(wishlistId);
                if (wishlist == null || wishlist.UserId != userId)
                    return Forbid();

                var result = await _wishlistService.ShareWishlistAsync(wishlistId, isPublic);
                return Ok(new { IsSuccess = true, StatusMessage = $"Wishlist {(isPublic ? "shared" : "made private")} successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpPost("{wishlistId}/duplicate")]
        [UserAndAbove]
        public async Task<IActionResult> DuplicateWishlist(int wishlistId, [FromBody] string newName)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var wishlist = await _wishlistService.GetByIdAsync(wishlistId);
                if (wishlist == null || wishlist.UserId != userId)
                    return Forbid();

                var duplicatedWishlist = await _wishlistService.DuplicateWishlistAsync(wishlistId, newName);
                if (duplicatedWishlist == null)
                    return BadRequest(new { IsSuccess = false, StatusMessage = "Failed to duplicate wishlist" });

                return Ok(new { IsSuccess = true, Data = duplicatedWishlist, StatusMessage = "Wishlist duplicated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpDelete("{wishlistId}/clear")]
        [UserAndAbove]
        public async Task<IActionResult> ClearWishlist(int wishlistId)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                    return Unauthorized();

                var wishlist = await _wishlistService.GetByIdAsync(wishlistId);
                if (wishlist == null || wishlist.UserId != userId)
                    return Forbid();

                var result = await _wishlistService.ClearWishlistAsync(wishlistId);
                return Ok(new { IsSuccess = true, StatusMessage = "Wishlist cleared successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        // ADMIN ENDPOINTS

        [HttpGet("admin/all")]
        [AdminAndAbove]
        public async Task<IActionResult> GetAllWishlistsForAdmin()
        {
            try
            {
                //TODO: for test purposes, remove later
                var allWishlists = await _wishlistService.GetAllWishlistsAsync();
                return Ok(new { IsSuccess = true, Data = allWishlists });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpGet("admin/stats")]
        [AdminAndAbove]
        public async Task<IActionResult> GetGlobalWishlistStats()
        {
            try
            {
                var stats = await _wishlistService.GetGlobalStatsAsync();
                return Ok(new { IsSuccess = true, Data = stats });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpDelete("admin/{id}")]
        [AdminAndAbove]
        public async Task<IActionResult> AdminDeleteWishlist(int id)
        {
            try
            {
                var result = await _wishlistService.DeleteAsync(id);
                if (!result)
                    return BadRequest(new { IsSuccess = false, StatusMessage = "Cannot delete the default wishlist" });

                return Ok(new { IsSuccess = true, StatusMessage = "Wishlist deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpDelete("admin/{id}/clear")]
        [AdminAndAbove]
        public async Task<IActionResult> AdminClearWishlist(int id)
        {
            try
            {
                var result = await _wishlistService.ClearWishlistAsync(id);
                return Ok(new { IsSuccess = true, StatusMessage = "Wishlist cleared successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }

        [HttpPut("admin/{id}/visibility")]
        [AdminAndAbove]
        public async Task<IActionResult> AdminToggleWishlistVisibility(int id, [FromBody] bool isPublic)
        {
            try
            {
                var result = await _wishlistService.ShareWishlistAsync(id, isPublic);
                return Ok(new { IsSuccess = true, StatusMessage = $"Wishlist {(isPublic ? "shared" : "made private")} successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { IsSuccess = false, StatusMessage = ex.Message });
            }
        }
    }
}