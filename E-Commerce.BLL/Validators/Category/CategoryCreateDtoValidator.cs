using FluentValidation;

namespace E_Commerce.BLL
{
    public class CategoryDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryDtoValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                    .WithMessage("Category name is required.")
                .MinimumLength(3)
                    .WithMessage("Category name must be at least 3 characters.")
                .MaximumLength(20)
                    .WithMessage("Category name can't exceed 20 characters.");
        }
    }
}
