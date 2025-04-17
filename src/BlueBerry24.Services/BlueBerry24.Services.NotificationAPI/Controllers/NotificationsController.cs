using BlueBerry24.Services.NotificationAPI.Data.Context;
using BlueBerry24.Services.NotificationAPI.Models.DTOs;
using BlueBerry24.Services.NotificationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlueBerry24.Services.NotificationAPI.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] CreateNotificationDto notificationDto, [FromQuery] string recipientId)
        {
            if (notificationDto == null || string.IsNullOrEmpty(recipientId))
            {
                return BadRequest("Invalid notification data or recipient ID.");
            }

            try
            {
                var savedNotification = await _notificationService.SendNotificationAsync(notificationDto, recipientId);
                return Ok(savedNotification);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Failed to send notification", details = ex.Message });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetNotifications(string userId)
        {
            var notifications = await _notificationService.GetNotificationsAsync(userId);
            return Ok(notifications);
        }

        [HttpPut("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return Ok(new { message = "Notification marked as read." });
        }

        [HttpPut("mark-all-read/{userId}")]
        public async Task<IActionResult> MarkAllAsRead(string userId)
        {
            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(new { message = "All notifications marked as read." });
        }
    }

}
