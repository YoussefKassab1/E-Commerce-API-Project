using FluentValidation;

namespace E_Commerce.BLL
{
    public class ProductCreateDtoValidator : AbstractValidator<ProductCreateDto>
    {

        public ProductCreateDtoValidator()
        {

            RuleFor(p => p.Title)
            .NotEmpty()
                .WithMessage("Title is required.")
            .MinimumLength(3)
                .WithMessage("Title must be at least 3 characters.")
            .MaximumLength(20)
                .WithMessage("Title can't exceed 20 characters.");

            RuleFor(p => p.Description)
                .NotEmpty()
                    .WithMessage("Description is required.")
                .MinimumLength(5)
                    .WithMessage("Description must be at least 5 characters.")
                .MaximumLength(200)
                    .WithMessage("Description can't exceed 200 characters.");

            RuleFor(p => p.Price)
                .NotEmpty()
                    .WithMessage("Price is required.")
                .InclusiveBetween(1000, 5000)
                    .WithMessage("Price must be between 1000 and 5000.");

            RuleFor(p => p.Count)
                .NotEmpty()
                    .WithMessage("Count is required.")
                .InclusiveBetween(1, 100)
                    .WithMessage("Count must be between 1 and 100.");

            RuleFor(p => p.CategoryId)
                .GreaterThan(0)
                    .WithMessage("A valid CategoryId is required.");

        }
    }
}
