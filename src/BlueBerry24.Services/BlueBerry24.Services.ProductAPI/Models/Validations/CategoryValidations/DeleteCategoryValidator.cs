using BlueBerry24.Services.ProductAPI.Models.DTOs.CategoryDtos;
using FluentValidation;

namespace BlueBerry24.Services.ProductAPI.Models.Validations.CategoryValidations
{
    public class DeleteCategoryValidator : CategoryBaseValidator<DeleteCategoryDto>
    {
        public DeleteCategoryValidator()
        {
            
        }
    }
}
