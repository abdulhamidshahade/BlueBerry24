using Berryfy.Application.Dtos.ProductDtos;

namespace Berryfy.Application.Dtos.CategoryDtos
{
    public class CategoryDto : CategoryBaseDto
    {
        public int Id { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
