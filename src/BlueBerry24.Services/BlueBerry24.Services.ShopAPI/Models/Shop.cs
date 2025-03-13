using BlueBerry24.Services.ShopAPI.Halpers.Constants;

namespace BlueBerry24.Services.ShopAPI.Models
{
    public class Shop : ShopBase
    {
        public string Id { get; set; }
       
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Shop()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
