namespace BlueBerry24.Domain.Entities.Product
{
    public class ProductCategory
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public ProductCategory()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
