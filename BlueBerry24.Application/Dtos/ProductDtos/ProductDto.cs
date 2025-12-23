using BlueBerry24.Application.Dtos.CategoryDtos;

namespace BlueBerry24.Application.Dtos.ProductDtos
{
    public class ProductDto : ProductBaseDto
    {
        public int Id { get; set; }

        public List<CategoryDto> ProductCategories { get; set; }
    }
}
