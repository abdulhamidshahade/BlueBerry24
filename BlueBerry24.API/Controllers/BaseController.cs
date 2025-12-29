using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlueBerry24.API.Controllers
{
    [ApiController]
    [Route("api/base")]
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
            string? sessionId = Request.Headers["X-Session-Id"];

            if (string.IsNullOrEmpty(sessionId) && !GetCurrentUserId().HasValue)
            {
                

                return null;
            }

            return sessionId;
        }
    }
}
