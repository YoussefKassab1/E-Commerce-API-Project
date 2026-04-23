using FluentValidation;

namespace E_Commerce.BLL
{
    public class ImageUploadDtoValidator : AbstractValidator<ImageUploadDto>
    {
        private static readonly string[] AllowedExtensions = { ".png", ".jpg", ".jpeg" };

        public ImageUploadDtoValidator()
        {
            RuleFor(i => i.File)
                .NotNull()
                .WithMessage("File is required")
                .WithName("File");

            When(i => i.File != null, () =>
            {
                RuleFor(i => i.File.Length)
                    .GreaterThan(0)
                    .WithMessage("File must not be empty")
                    .WithName("FileSize")

                    .LessThanOrEqualTo(5_000_000)
                    .WithMessage("File must be less than 5MB")
                    .WithName("FileSize");

                RuleFor(i => Path.GetExtension(i.File.FileName).ToLower())
                    .Must(ext => AllowedExtensions.Contains(ext))
                    .WithMessage("Unsupported file extension")
                    .WithName("File Extension");
            });
        }
    }
}
