using BlueBerry24.Services.NotificationAPI.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace BlueBerry24.Services.NotificationAPI.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(INotificationService notificationService, ILogger<NotificationHub> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }
 
        public async Task JoinUserGroup(string userId)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);

                var notifications = await _notificationService.GetNotificationsAsync(userId);
                await Clients.Caller.SendAsync("LoadInitialNotifications", notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error joining user group for user {userId}");
            }
        }

        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        public async Task MarkNotificationAsRead(string notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
        }

        public async Task MarkAllNotificationsAsRead(string userId)
        {
            await _notificationService.MarkAllAsReadAsync(userId);
        }
    }


}
