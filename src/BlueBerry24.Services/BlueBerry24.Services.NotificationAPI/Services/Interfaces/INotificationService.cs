using BlueBerry24.Services.NotificationAPI.Data.Enums;
using BlueBerry24.Services.NotificationAPI.Models;
using BlueBerry24.Services.NotificationAPI.Models.DTOs;

namespace BlueBerry24.Services.NotificationAPI.Services.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetNotificationsAsync(string userId);
        Task MarkAsReadAsync(string notificationId);
        Task<NotificationDto> SendNotificationAsync(CreateNotificationDto notificationDto, string recipientId);
        Task MarkAllAsReadAsync(string userId);
    }
}
