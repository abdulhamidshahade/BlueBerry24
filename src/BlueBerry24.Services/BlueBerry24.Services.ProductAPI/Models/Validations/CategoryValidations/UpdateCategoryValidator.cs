﻿using BlueBerry24.Services.ProductAPI.Models.DTOs.CategoryDtos;
using FluentValidation;

namespace BlueBerry24.Services.ProductAPI.Models.Validations.CategoryValidations
{
    public class UpdateCategoryValidator : CategoryBaseValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(p => p.Id).GreaterThan(0).WithMessage("ID is required!");
        }
    }
}
