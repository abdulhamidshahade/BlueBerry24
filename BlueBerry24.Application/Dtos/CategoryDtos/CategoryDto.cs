using BlueBerry24.Application.Dtos.ProductDtos;

namespace BlueBerry24.Application.Dtos.CategoryDtos
{
    public class CategoryDto : CategoryBaseDto
    {
        public int Id { get; set; }

        public List<ProductDto> Products { get; set; }
    }
}
