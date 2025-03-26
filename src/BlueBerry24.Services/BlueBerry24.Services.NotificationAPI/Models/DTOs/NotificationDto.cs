using BlueBerry24.Services.NotificationAPI.Data.Enums;

namespace BlueBerry24.Services.NotificationAPI.Models.DTOs
{
    public class NotificationDto
    {
        public string Id { get; set; }

        public string RecipientId { get; set; }
        public Recipient RecipientType { get; set; } = Recipient.Shop;
        public NotificationType NotificationType { get; set; } = NotificationType.LowStockAlert;
        public string Message { get; set; }

        public Dictionary<string, object> Metadata { get; set; }

        public NotificationStatus NotificationStatus { get; set; } = NotificationStatus.Unread;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
