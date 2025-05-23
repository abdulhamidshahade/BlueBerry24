using BlueBerry24.Application.Dtos;
using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlueBerry24.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : BaseController
    {
        private readonly ILogger<ShoppingCartsController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int? _userId;
        private readonly IUserCouponService _userCouponService;
        private readonly ICartService _cartService;

        public ShoppingCartsController(
            ILogger<ShoppingCartsController> logger,
            IHttpContextAccessor httpContextAccessor,
            IUserCouponService userCouponService,
            ICartService cartService) : base(logger)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _userId = Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _userCouponService = userCouponService;
            _cartService = cartService;
        }

        [HttpGet("{cartId:int}")]
        public async Task<IActionResult> GetCartById(int cartId)
        {
            var cart = await _cartService.GetCartByIdAsync(cartId);
            if (cart == null) return NotFound();

            return Ok(cart);
        }

        [HttpGet("session/{sessionId}")]
        public async Task<IActionResult> GetCartBySessionId(string sessionId)
        {
            var cart = await _cartService.GetCartBySessionIdAsync(sessionId);
            if (cart == null) return NotFound();

            return Ok(cart);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCart([FromQuery] int? userId, [FromQuery] string? sessionId)
        {
            var cart = await _cartService.CreateCartAsync(userId, sessionId);
            if (cart == null) return BadRequest("Cart could not be created.");

            return Ok(cart);
        }

        [HttpPost("{cartId}/add")]
        public async Task<IActionResult> AddItemToCart(int cartId, [FromQuery] int productId, [FromQuery] int quantity)
        {
            var updatedCart = await _cartService.AddItemAsync(cartId, productId, quantity);
            if (updatedCart == null) return BadRequest("Failed to add item to cart.");

            return Ok(updatedCart);
        }

        [HttpPut("{cartId}/update")]
        public async Task<IActionResult> UpdateItemQuantity(int cartId, [FromQuery] int productId, [FromQuery] int quantity)
        {
            var updatedCart = await _cartService.UpdateItemQuantityAsync(cartId, productId, quantity);
            if (updatedCart == null) return BadRequest("Failed to update item quantity.");

            return Ok(updatedCart);
        }

        [HttpDelete("{cartId}/remove/{productId}")]
        public async Task<IActionResult> RemoveItemFromCart(int cartId, int productId)
        {
            var success = await _cartService.RemoveItemAsync(cartId, productId);
            if (!success) return NotFound("Item not found in cart.");

            return NoContent();
        }

        [HttpDelete("{cartId}/clear")]
        public async Task<IActionResult> ClearCart(int cartId)
        {
            var success = await _cartService.ClearCartAsync(cartId);
            if (!success) return BadRequest("Failed to clear the cart.");

            return NoContent();
        }

        [HttpPost("{cartId}/complete")]
        public async Task<IActionResult> CompleteCart(int cartId)
        {
            var success = await _cartService.CompleteCartAsync(cartId);
            if (!success) return BadRequest("Failed to complete the cart.");

            return Ok("Cart completed successfully.");
        }

        [HttpGet("{cartId}/refresh")]
        public async Task<IActionResult> RefreshCart(int cartId)
        {
            var cart = await _cartService.RefreshCartAsync(cartId);
            if (cart == null) return NotFound();

            return Ok(cart);
        }

        [HttpGet("{cartId}/item/{productId}")]
        public async Task<IActionResult> GetCartItem(int cartId, int productId)
        {
            var item = await _cartService.GetItemAsync(cartId, productId);
            if (item == null) return NotFound();

            return Ok(item);
        }



    }
}

