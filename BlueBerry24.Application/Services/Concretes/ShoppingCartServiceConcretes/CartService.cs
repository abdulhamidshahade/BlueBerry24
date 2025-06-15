using AutoMapper;
using BlueBerry24.Application.Dtos.ShoppingCartDtos;
using BlueBerry24.Application.Services.Interfaces.CouponServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.InventoryServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ProductServiceInterfaces;
using BlueBerry24.Application.Services.Interfaces.ShoppingCartServiceInterfaces;
using BlueBerry24.Domain.Constants;
using BlueBerry24.Domain.Entities.ProductEntities;
using BlueBerry24.Domain.Entities.ShoppingCartEntities;
using BlueBerry24.Domain.Repositories;
using BlueBerry24.Domain.Repositories.ShoppingCartInterfaces;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace BlueBerry24.Application.Services.Concretes.ShoppingCartServiceConcretes
{
    public class CartService : ICartService
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;
        private readonly ILogger<CartService> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;
        private readonly ICouponService _couponService;
        private readonly IUserCouponService _userCouponService;

        public CartService(
                           IMapper mapper,
                           IUnitOfWork unitOfWork,
                           ICartRepository cartRepository,
                           ILogger<CartService> logger,
                           IConfiguration configuration,
                           IInventoryService inventoryService,
                           IProductService productService,
                           ICouponService couponService,
                           IUserCouponService userCouponService
                           )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
            _logger = logger;
            _inventoryService = inventoryService;
            _productService = productService;
            _couponService = couponService;
            _userCouponService = userCouponService;
        }


        public async Task<CartDto> GetCartByUserIdAsync(int userId, CartStatus? status = CartStatus.Active)
        {
            if (userId <= 0)
            {
                return null;
            }
            var dbCart = await _cartRepository.GetCartByUserIdAsync(userId, status);

            if (dbCart == null)
            {
                return null;
            }

            var mappedCart = _mapper.Map<CartDto>(dbCart);

            return mappedCart;
        }
        public async Task<CartDto> GetCartBySessionIdAsync(string sessionId, CartStatus? status = CartStatus.Active)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return null;
            }

            var dbCart = await _cartRepository.GetCartBySessionIdAsync(sessionId, status);

            if (dbCart == null)
            {
                await CreateCartAsync(null, sessionId);
            }

            var mappedCart = _mapper.Map<CartDto>(dbCart);

            return mappedCart;
        }

        public async Task<CartDto> MergeCartAsync(int userId, string sessionId)
        {

            var cartById = await _cartRepository.GetCartByUserIdAsync(userId, CartStatus.Active);

            if(cartById != null)
            {
                return null;
            }


            var cart = await _cartRepository.GetCartBySessionIdAsync(sessionId);

            cart.SessionId = null;
            cart.UserId = userId;

            var cartItems = cart.CartItems.Where(i => i.ShoppingCartId == cart.Id).ToList();

            foreach(var item in cartItems)
            {
                item.SessionId = null;
                item.UserId = userId;
            }

            //var updatedItems = await _cartRepository.UpdateItemsAsync(cartItems);

            //TODO here the shopping cart's items will update successfully because the cart has navigation property for items List<CartItem>
            var updatedCart = await _cartRepository.UpdateCartAsync(cart);
            
            return _mapper.Map<CartDto>(updatedCart);
        }

        public async Task<CartDto> GetCartByIdAsync(int cartId, CartStatus status)
        {

            var dbCart = await _cartRepository.GetCartByIdAsync(cartId, status);

            if (dbCart == null)
            {
                return null;
            }

            var mappedCart = _mapper.Map<CartDto>(dbCart);

            return mappedCart;
        }


        public async Task<CartDto> CreateCartAsync(int? userId, string? sessionId)
        {
            CartDto? cart = null;

            if (userId.HasValue)
            {
                cart = _mapper.Map<CartDto>(await _cartRepository.GetCartByUserIdAsync(userId, CartStatus.Active));

                if (cart != null)
                {
                    return cart;
                }

                else
                {
                    var createdCart = await _cartRepository.CreateCartAsync(userId, CartStatus.Active);
                    return _mapper.Map<CartDto>(createdCart);
                }

            }

            else if (!string.IsNullOrEmpty(sessionId))
            {

                cart = _mapper.Map<CartDto>(await _cartRepository.GetCartBySessionIdAsync(sessionId, CartStatus.Active));

                if (cart != null)
                {
                    return cart;
                }
                else
                {
                    var createdCart = await _cartRepository.CreateCartAsync(sessionId, CartStatus.Active);
                    return _mapper.Map<CartDto>(createdCart);
                }
            }

            else
            {
                return null;
            }
        }
        public async Task<CartDto?> AddItemAsync(int cartId, int? userId, string? sessionId, int productId, int quantity)
        {
            try
            {
                _logger.LogDebug("Adding item to cart: CartId={CartId}, ProductId={ProductId}, Quantity={Quantity}",
                    cartId, productId, quantity);

                if (quantity <= 0)
                {
                    _logger.LogWarning("Invalid quantity {Quantity} for product {ProductId}", quantity, productId);
                    return null;
                }

                var product = await _productService.GetByIdAsync(productId);
                var mappedProduct = _mapper.Map<Product>(product);

                if (mappedProduct == null)
                {
                    _logger.LogWarning("Product not found: {ProductId}", productId);
                    return null;
                }

                if (!mappedProduct.IsActive)
                {
                    _logger.LogWarning("Product {ProductId} is not active", productId);
                    return null;
                }

                var existingItem = await GetItemAsync( cartId, productId);
                int totalQuantityNeeded = quantity;

                if (existingItem != null)
                {
                    totalQuantityNeeded = existingItem.Quantity + quantity;
                    _logger.LogDebug("Existing item found. Current quantity: {CurrentQuantity}, Adding: {AddQuantity}, Total needed: {TotalQuantity}",
                        existingItem.Quantity, quantity, totalQuantityNeeded);
                }


                if (!await _inventoryService.IsInStockAsync(productId, totalQuantityNeeded))
                {
                    _logger.LogWarning("Insufficient stock for product {ProductId}. Requested: {Quantity}, Total needed: {TotalQuantity}",
                        productId, quantity, totalQuantityNeeded);
                    return null;
                }

                var stockReserved = await _inventoryService.ReserveStockAsync(
                    productId,
                    quantity,
                    cartId,
                    "CartItem");

                if (!stockReserved)
                {
                    _logger.LogError("Failed to reserve stock for product {ProductId}, quantity {Quantity}", productId, quantity);
                    return null;
                }

                Cart updatedCart = null;

                try
                {
                    if (existingItem != null)
                    {
                        _logger.LogDebug("Updating existing cart item quantity");
                        updatedCart = await _cartRepository.UpdateItemQuantityAsync(userId, sessionId, productId, totalQuantityNeeded);
                    }
                    else
                    {
                        _logger.LogDebug("Creating new cart item");
                        var createdItem = await _cartRepository.CreateItemAsync(cartId, userId, sessionId, productId, quantity, product.Price);

                        if (createdItem == null)
                        {
                            throw new InvalidOperationException("Failed to create cart item");
                        }

                        if (userId.HasValue)
                        {
                            updatedCart = await _cartRepository.GetCartByUserIdAsync(userId, CartStatus.Active);
                        }
                        else if (!string.IsNullOrEmpty(sessionId))
                        {
                            updatedCart = await _cartRepository.GetCartBySessionIdAsync(sessionId, CartStatus.Active);
                        }
                    }

                    if (updatedCart == null)
                    {
                        throw new InvalidOperationException("Failed to retrieve updated cart");
                    }

                    _logger.LogInformation("Successfully added item to cart: CartId={CartId}, ProductId={ProductId}, Quantity={Quantity}",
                        cartId, productId, quantity);

                    return _mapper.Map<CartDto>(updatedCart);
                }
                catch
                {
                    _logger.LogWarning("Cart operation failed, releasing reserved stock for product {ProductId}, quantity {Quantity}",
                        productId, quantity);

                    await _inventoryService.ReleaseReservedStockAsync(
                        productId,
                        quantity,
                        cartId,
                        "CartItem");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart: CartId={CartId}, ProductId={ProductId}, Quantity={Quantity}",
                    cartId, productId, quantity);
                return null;
            }
        }
        public async Task<CartDto> UpdateItemQuantityAsync(int cartId, int? userId, string? sessionId, int productId, int quantity)
        {

            if (quantity <= 0)
            {
                return null;
            }

            Cart? cart = null;

            if (userId.HasValue)
            {
                cart = await _cartRepository.GetCartByUserIdAsync(userId);
            }

            else if (!string.IsNullOrEmpty(sessionId))
            {
                cart = await _cartRepository.GetCartBySessionIdAsync(sessionId);
            }

            else
            {
                return null;
            }

            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

            if (item == null)
            {
                return null;
            }

            int quantityDifference = quantity - item.Quantity;

            if (quantityDifference == 0)
            {
                return _mapper.Map<CartDto>(cart);
            }

            if (quantityDifference > 0)
            {
                if (!await _inventoryService.IsInStockAsync(productId, quantityDifference))
                {
                    return null;
                }

                await _inventoryService.ReserveStockAsync(
                    productId,
                    quantityDifference,
                    cartId,
                    "CartItem");
            }
            else
            {
                await _inventoryService.ReleaseReservedStockAsync(
                    productId,
                    Math.Abs(quantityDifference),
                    cartId,
                    "CartItem");
            }

            var updatedQuantity = await _cartRepository.UpdateItemQuantityAsync(userId, sessionId, productId, quantity);


            if (updatedQuantity == null && quantityDifference > 0)
            {
                await _inventoryService.ReleaseReservedStockAsync(
                    productId,
                    quantityDifference,
                    cartId,
                    "CartItem");
                return null;
            }

            return _mapper.Map<CartDto>(updatedQuantity);
        }


        public async Task<bool> RemoveItemAsync(int cartId, int? userId, string? sessionId, int productId)
        {
            try
            {
                _logger.LogDebug("Removing item from cart: CartId={CartId}, ProductId={ProductId}", cartId, productId);

                Cart cart = null;

                if (userId.HasValue)
                {
                    cart = await _cartRepository.GetCartByUserIdAsync(userId);
                }
                else if (!string.IsNullOrEmpty(sessionId))
                {
                    cart = await _cartRepository.GetCartByIdAsync(cartId, CartStatus.Active);
                }
                else
                {
                    _logger.LogWarning("No userId or sessionId provided for cart item removal");
                    return false;
                }

                if (cart == null)
                {
                    _logger.LogWarning("Cart not found for removal: CartId={CartId}", cartId);
                    return false;
                }

                var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

                if (item == null)
                {
                    _logger.LogWarning("Cart item not found: CartId={CartId}, ProductId={ProductId}", cartId, productId);
                    return false;
                }

                var quantityToRelease = item.Quantity;
                _logger.LogDebug("Releasing reserved stock: ProductId={ProductId}, Quantity={Quantity}", productId, quantityToRelease);

                await _inventoryService.ReleaseReservedStockAsync(
                    productId,
                    quantityToRelease,
                    cartId,
                    "CartItem");

                var removedItem = await _cartRepository.RemoveItemAsync(userId, sessionId, productId);

                if (removedItem)
                {
                    _logger.LogInformation("Successfully removed item from cart: CartId={CartId}, ProductId={ProductId}, ReleasedQuantity={Quantity}",
                        cartId, productId, quantityToRelease);
                }
                else
                {
                    _logger.LogError("Failed to remove item from cart: CartId={CartId}, ProductId={ProductId}", cartId, productId);

                    await _inventoryService.ReserveStockAsync(
                        productId,
                        quantityToRelease,
                        cartId,
                        "CartItem");
                }

                return removedItem;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart: CartId={CartId}, ProductId={ProductId}", cartId, productId);
                return false;
            }
        }


        public async Task<bool> ClearCartAsync(int cartId, int? userId, string? sessionId)
        {
            try
            {
                _logger.LogDebug("Clearing cart: CartId={CartId}", cartId);

                if (cartId <= 0)
                {
                    _logger.LogWarning("Invalid cart ID provided: {CartId}", cartId);
                    return false;
                }

                Cart cart = null;

                if (userId.HasValue)
                {
                    cart = await _cartRepository.GetCartByUserIdAsync(userId);
                }
                else if (!string.IsNullOrEmpty(sessionId))
                {
                    cart = await _cartRepository.GetCartByIdAsync(cartId, CartStatus.Active);
                }

                if (cart == null)
                {
                    _logger.LogWarning("Cart not found for clearing: CartId={CartId}", cartId);
                    return false;
                }

                if (cart.CartItems?.Any() == true)
                {
                    _logger.LogDebug("Releasing reserved stock for {ItemCount} items in cart {CartId}", cart.CartItems.Count, cartId);

                    var stockReleaseTasks = cart.CartItems.Select(async item =>
                    {
                        try
                        {
                            await _inventoryService.ReleaseReservedStockAsync(
                                item.ProductId,
                                item.Quantity,
                                cartId,
                                "CartItem");

                            _logger.LogDebug("Released stock for product {ProductId}, quantity {Quantity}",
                                item.ProductId, item.Quantity);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to release stock for product {ProductId} in cart {CartId}",
                                item.ProductId, cartId);
                        }
                    });

                    await Task.WhenAll(stockReleaseTasks);
                }

                cart.CartItems.Clear();
                cart.CartCoupons.Clear();
                cart.UpdatedAt = DateTime.UtcNow;

                var updatedCart = await _cartRepository.UpdateCartAsync(cart);
                var success = updatedCart != null;

                if (success)
                {
                    _logger.LogInformation("Successfully cleared cart {CartId}", cartId);
                }
                else
                {
                    _logger.LogError("Failed to clear cart {CartId}", cartId);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart: CartId={CartId}", cartId);
                return false;
            }
        }

        public async Task<bool> CompleteCartAsync(int cartId, int? userId)
        {
            var updatedCart = await _cartRepository.UpdateCartStatusAsync(userId, CartStatus.Converted);
            return updatedCart != null;
        }

        public async Task<bool> ConvertCartAsync(int cartId)
        {
            try
            {
                _logger.LogInformation("Starting cart conversion for cart ID: {CartId}", cartId);

                var cart = await _cartRepository.GetCartByIdAsync(cartId, CartStatus.Active);
                if (cart == null)
                {
                    _logger.LogWarning("Cart not found or not active for conversion: {CartId}", cartId);
                    return false;
                }

                if (cart.CartItems == null || !cart.CartItems.Any())
                {
                    _logger.LogWarning("Cannot convert empty cart: {CartId}", cartId);
                    return false;
                }

                var processedItems = new List<CartItem>();

                foreach (var item in cart.CartItems)
                {
                    try
                    {
                        var isInStock = await _inventoryService.IsInStockAsync(item.ProductId, item.Quantity);
                        if (!isInStock)
                        {
                            _logger.LogError("Insufficient stock for product {ProductId} in cart {CartId}. Quantity needed: {Quantity}",
                                item.ProductId, cartId, item.Quantity);

                            if (processedItems.Any())
                            {
                                await RollbackStockConversions(processedItems, cartId);
                            }
                            return false;
                        }

                        var deductionConfirmed = await _inventoryService.ConfirmStockDeductionAsync(
                            item.ProductId,
                            item.Quantity,
                            cartId,
                            "Order");

                        if (!deductionConfirmed)
                        {
                            _logger.LogError("Failed to confirm stock deduction for product {ProductId} in cart {CartId}",
                                item.ProductId, cartId);


                            if (processedItems.Any())
                            {
                                await RollbackStockConversions(processedItems, cartId);
                            }
                            return false;
                        }

                        _logger.LogDebug("Stock deduction confirmed for product {ProductId}, quantity {Quantity} in cart {CartId}",
                            item.ProductId, item.Quantity, cartId);

                        processedItems.Add(item);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error converting stock for product {ProductId} in cart {CartId}",
                            item.ProductId, cartId);

                        if (processedItems.Any())
                        {
                            await RollbackStockConversions(processedItems, cartId);
                        }
                        return false;
                    }
                }

                cart.Status = CartStatus.Converted;
                cart.UpdatedAt = DateTime.UtcNow;

                var updatedCart = await _cartRepository.UpdateCartAsync(cart);
                if (updatedCart == null)
                {
                    _logger.LogError("Failed to update cart status to Converted for cart {CartId}. Rolling back stock...", cartId);

                    await RollbackStockConversions(processedItems, cartId);
                    return false;
                }

                _logger.LogInformation("Successfully converted cart {CartId} to order. Stock has been confirmed for {ItemCount} items",
                    cartId, cart.CartItems.Count);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during cart conversion for cart {CartId}", cartId);
                return false;
            }
        }

        private async Task RollbackStockConversions(IList<CartItem> processedItems, int cartId)
        {
            if (processedItems == null || !processedItems.Any())
            {
                _logger.LogDebug("No items to rollback for cart {CartId}", cartId);
                return;
            }

            try
            {
                _logger.LogWarning("Rolling back stock conversions for {ItemCount} processed items in cart {CartId}",
                    processedItems.Count, cartId);

                foreach (var item in processedItems)
                {
                    try
                    {
                        await _inventoryService.AddStockAsync(
                            item.ProductId,
                            item.Quantity,
                            $"Rollback from failed cart conversion - Cart ID: {cartId}",
                            null);

                        _logger.LogDebug("Rolled back stock for product {ProductId}, quantity {Quantity} from cart {CartId}",
                            item.ProductId, item.Quantity, cartId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to rollback stock for product {ProductId} in cart {CartId}. " +
                            "Manual inventory adjustment may be required.",
                            item.ProductId, cartId);
                    }
                }

                _logger.LogInformation("Completed rollback attempts for {ItemCount} items in cart {CartId}",
                    processedItems.Count, cartId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during rollback for cart {CartId}. Manual inventory review recommended.", cartId);
            }
        }

        public async Task<CartDto> RefreshCartAsync(int cartId)
        {
            Cart? cart = null;



            return _mapper.Map<CartDto>(cart);
        }


        public async Task<CartItemDto> GetItemAsync(int cartId, int productId)
        {
            CartDto? cart = null;

            cart = await GetCartByIdAsync(cartId, CartStatus.Active);

            var item = cart.CartItems.Where(i => i.ProductId == productId).FirstOrDefault();

            if (item == null)
            {
                return null;
            }

            return _mapper.Map<CartItemDto>(item);
        }


        public async Task<CartDto> ApplyCouponAsync(int cartId, int? userId, string couponCode)
        {
            try
            {

                var cart = await _cartRepository.GetCartByUserIdAsync(userId);

                if (cart == null)
                {
                    return null;
                }

                if (string.IsNullOrWhiteSpace(couponCode))
                {
                    return null;
                }

                if (cart.CartCoupons.Any(cc => cc.Coupon.Code == couponCode))
                {
                    return null;
                }

                var coupon = await _couponService.GetByCodeAsync(couponCode);

                if (coupon == null || !coupon.IsActive || await _userCouponService.IsCouponUsedByUser(userId.Value ,couponCode))
                {
                    return null;
                }

                if (!cart.CartItems.Any())
                {
                    return null;
                }

                decimal cartSubTotal = cart.SubTotal;


                if (coupon.MinimumOrderAmount > 0 && cartSubTotal < coupon.MinimumOrderAmount)
                {
                    return null;
                }

                decimal discountAmount = 0;

                if(coupon.MinimumOrderAmount == 0)
                {
                    switch (coupon.Type)
                    {
                        case CouponType.Percentage:
                            discountAmount = cartSubTotal * (coupon.Value / 100);
                            break;
                        case CouponType.FixedAmount:
                            discountAmount = Math.Min(coupon.DiscountAmount, cartSubTotal);
                            break;
                    }
                }
                else
                {
                    if(coupon.MinimumOrderAmount <= cartSubTotal)
                        switch (coupon.Type)
                        {
                            case CouponType.Percentage:
                                discountAmount = cartSubTotal * (coupon.Value / 100);
                                break;
                            case CouponType.FixedAmount:
                                discountAmount = coupon.DiscountAmount;
                                break;
                        }
                    else
                    {
                        return null;
                    }
                }


                    var cartCoupon = new CartCoupon
                    {
                        CartId = cart.Id,
                        CouponId = coupon.Id,
                        UserId = userId,
                        DiscountAmount = discountAmount,
                        AppliedAt = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                cart.CartCoupons.Add(cartCoupon);

                var updatedCart = await _cartRepository.UpdateCartAsync(cart);
                if (updatedCart == null)
                {
                    return null;
                }

                return _mapper.Map<CartDto>(updatedCart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying coupon {CouponCode} to cart {CartId}", couponCode, cartId);
                return null;
            }
        }

        public async Task<CartDto> RemoveCouponAsync(int cartId, int? userId, string? sessionId, int couponId)
        {
            try
            {
                Cart? cart = null;

                if (userId.HasValue)
                {
                    cart = await _cartRepository.GetCartByUserIdAsync(userId, CartStatus.Active);
                }
                else if (!string.IsNullOrEmpty(sessionId))
                {
                    cart = await _cartRepository.GetCartBySessionIdAsync(sessionId, CartStatus.Active);
                }


                if (cart == null)
                {
                    return null;
                }

                var cartCoupon = cart.CartCoupons.FirstOrDefault(cc => cc.CouponId == couponId);
                if (cartCoupon == null)
                {
                    return null;
                }

                cart.CartCoupons.Remove(cartCoupon);

                var updatedCart = await _cartRepository.UpdateCartAsync(cart);
                if (updatedCart == null)
                {
                    throw new InvalidOperationException("Failed to remove coupon from cart");
                }

                return _mapper.Map<CartDto>(updatedCart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing coupon {CouponId} from cart {CartId}", couponId, cartId);
                throw;
            }
        }

        public async Task<bool> HandleAbandonedCartAsync(int cartId)
        {
            try
            {
                _logger.LogInformation("Handling abandoned cart: {CartId}", cartId);

                var cart = await _cartRepository.GetCartByIdAsync(cartId, CartStatus.Active);
                if (cart == null)
                {
                    _logger.LogWarning("Cart not found or not active: {CartId}", cartId);
                    return false;
                }

                if (cart.CartItems?.Any() == true)
                {
                    _logger.LogDebug("Releasing reserved stock for abandoned cart {CartId} with {ItemCount} items",
                        cartId, cart.CartItems.Count);

                    var stockReleaseTasks = cart.CartItems.Select(async item =>
                    {
                        try
                        {
                            await _inventoryService.ReleaseReservedStockAsync(
                                item.ProductId,
                                item.Quantity,
                                cartId,
                                "CartItem");

                            _logger.LogDebug("Released stock for abandoned cart - Product: {ProductId}, Quantity: {Quantity}",
                                item.ProductId, item.Quantity);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to release stock for product {ProductId} in abandoned cart {CartId}",
                                item.ProductId, cartId);
                        }
                    });

                    await Task.WhenAll(stockReleaseTasks);
                }

                cart.Status = CartStatus.Abandoned;
                cart.UpdatedAt = DateTime.UtcNow;

                var updatedCart = await _cartRepository.UpdateCartAsync(cart);
                var success = updatedCart != null;

                if (success)
                {
                    _logger.LogInformation("Successfully handled abandoned cart {CartId}", cartId);
                }
                else
                {
                    _logger.LogError("Failed to update abandoned cart status: {CartId}", cartId);
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling abandoned cart: {CartId}", cartId);
                return false;
            }
        }

        public async Task<int> CleanupExpiredCartsAsync()
        {
            try
            {
                _logger.LogInformation("Starting cleanup of expired carts");

                var allCarts = await _cartRepository.GetCartsAsync();
                var expiredCarts = allCarts.Where(c =>
                    c.Status == CartStatus.Active &&
                    c.UpdatedAt < DateTime.UtcNow.AddHours(-24))
                    .ToList();

                if (!expiredCarts.Any())
                {
                    _logger.LogInformation("No expired carts found for cleanup");
                    return 0;
                }

                _logger.LogInformation("Found {ExpiredCartCount} expired carts for cleanup", expiredCarts.Count);

                int cleanedUpCount = 0;
                var cleanupTasks = expiredCarts.Select(async cart =>
                {
                    try
                    {
                        var success = await HandleAbandonedCartAsync(cart.Id);
                        if (success)
                        {
                            Interlocked.Increment(ref cleanedUpCount);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error cleaning up expired cart: {CartId}", cart.Id);
                    }
                });

                await Task.WhenAll(cleanupTasks);

                _logger.LogInformation("Completed cleanup of expired carts. Cleaned up: {CleanedUpCount}/{TotalExpired}",
                    cleanedUpCount, expiredCarts.Count);

                return cleanedUpCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during expired cart cleanup");
                return 0;
            }
        }
    }
}