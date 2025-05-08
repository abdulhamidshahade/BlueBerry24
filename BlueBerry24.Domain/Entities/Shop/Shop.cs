namespace BlueBerry24.Domain.Entities.Shop
{
    public class Shop : ShopBase
    {
        public int Id { get; set; }
       
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Shop()
        {

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
