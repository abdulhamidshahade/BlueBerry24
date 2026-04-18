using Berryfy.Application.Dtos.CategoryDtos;

namespace Berryfy.Application.Dtos.ProductDtos
{
    public class ProductDto : ProductBaseDto
    {
        public int Id { get; set; }

        public List<CategoryDto> ProductCategories { get; set; }
    }
}
