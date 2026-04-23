using FluentValidation;

namespace E_Commerce.BLL
{
    public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
    {
        public CategoryUpdateDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("A valid Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")

                .MinimumLength(3)
                .WithMessage("Category name must be at least 3 characters.")

                .MaximumLength(100)
                .WithMessage("Category name cannot exceed 100 characters.");
        }
    }
}
