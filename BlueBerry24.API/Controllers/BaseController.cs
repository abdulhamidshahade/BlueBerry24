using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlueBerry24.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger;


        protected BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        protected int? GetCurrentUserId()
        {
            var userIdClaim = User?.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim != null && int.TryParse(userIdClaim.Value, out int id) ? id : null;
        }

        protected string? GetSessionId()
        {
            string? sessionId = Request.Cookies["ShoppingCartSession"];

            if (string.IsNullOrEmpty(sessionId) && !GetCurrentUserId().HasValue)
            {
                sessionId = Guid.NewGuid().ToString();
            }

            return sessionId;
        }

        protected void SetSessionCookie(string sessionId)
        {
            // Set or update session cookie with a 30-day expiration
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // For HTTPS
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(30)
            };

            Response.Cookies.Append("ShoppingCartSession", sessionId, cookieOptions);
        }
    }
}
