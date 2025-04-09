namespace BlueBerry24.Services.NotificationAPI.Models
{
    public class PagedResult <T>
    {
        public List<T> Items { get; set; }
        public string NextCursor { get; set; }
        public DateTime? OldestTimestamp { get; set; }
    }
}
