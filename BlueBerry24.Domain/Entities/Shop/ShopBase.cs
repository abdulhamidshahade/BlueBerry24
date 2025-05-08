namespace BlueBerry24.Domain.Entities.Shop
{
    public abstract class ShopBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string LogoUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
