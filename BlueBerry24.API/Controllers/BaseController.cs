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
            string? sessionId = Request.Cookies["cart_session"];

            if (string.IsNullOrEmpty(sessionId) && !GetCurrentUserId().HasValue)
            {
                sessionId = Guid.NewGuid().ToString();
                SetSessionCookie(sessionId);
            }

            return sessionId;
        }

        protected void SetSessionCookie(string sessionId)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax, // Default for security
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                Domain = "localhost"
            };

            Response.Cookies.Append("cart_session", sessionId, cookieOptions);
        }
    }
}
