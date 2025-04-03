using BlueBerry24.Services.NotificationAPI.Data.Enums;

namespace BlueBerry24.Services.NotificationAPI.Models
{
    public class Notification
    {
        public string Id { get; set; }

        public string RecipientId { get; set; }
        public Recipient RecipientType { get; set; } = Recipient.Shop;
        public NotificationType NotificationType { get; set; } = NotificationType.LowStockAlert;
        public string Message { get; set; }

        public Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        public NotificationStatus NotificationStatus { get; set; } = NotificationStatus.Unread;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ExpirationDate { get; set; }

        public Notification()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
        }


    }
}
