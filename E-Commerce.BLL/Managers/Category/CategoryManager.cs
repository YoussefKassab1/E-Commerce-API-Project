using E_Commerce.Common;
using E_Commerce.DAL;
using E_Commerce.DAL.Data.Models;
using FluentValidation;

namespace E_Commerce.BLL
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CategoryCreateDto> _categoryCreateDtoValidator;
        private readonly IValidator<CategoryUpdateDto> _categoryUpdateDtoValidator;
        private readonly IImageManager _imageManager;

        public CategoryManager(
            IUnitOfWork unitOfWork,
            IValidator<CategoryCreateDto> categoryCreateDtoValidator,
            IValidator<CategoryUpdateDto> categoryUpdateDtoValidator,
            IImageManager imageManager)
        {
            _unitOfWork = unitOfWork;
            _categoryCreateDtoValidator = categoryCreateDtoValidator;
            _categoryUpdateDtoValidator = categoryUpdateDtoValidator;
            _imageManager = imageManager;
        }

        // Get Categories with Products
        public async Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllWithProductsAsync();

            var categoriesDto = categories
                .Select(c => new CategoryReadDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ProductsCount = c.Products.Count,
                    ImageUrl = c.ImageUrl,
                });

            return GeneralResult<IEnumerable<CategoryReadDto>>.Success(categoriesDto);
        }

        // Get Category by id With Products
        public async Task<GeneralResult<CategoryReadDto>> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdWithProductsAsync(id);

            if (category == null)
            {
                return GeneralResult<CategoryReadDto>.NotFound("Category not found");
            }

            var categoryDto = new CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
                ProductsCount = category.Products.Count,
                ImageUrl = category.ImageUrl,
            };

            return GeneralResult<CategoryReadDto>.Success(categoryDto);
        }

        // Insert a new Category
        public async Task<GeneralResult> InsertAsync(CategoryCreateDto categoryDto)
        {
            var result = await _categoryCreateDtoValidator.ValidateAsync(categoryDto);

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList());

                return GeneralResult.Failure(errors);
            }

            var category = new Category
            {
                Name = categoryDto.Name,
            };

            _unitOfWork.CategoryRepository.Insert(category);
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult.Success("Category Created Successfully");
        }

        // Update a Category
        public async Task<GeneralResult> UpdateAsync(int id, CategoryUpdateDto categoryDto)
        {
            if (id != categoryDto.Id)
                return GeneralResult.Failure("ID in URL does not match ID in body.");

            var result = await _categoryUpdateDtoValidator.ValidateAsync(categoryDto);

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList());

                return GeneralResult.Failure(errors);
            }

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return GeneralResult.NotFound("Category not found");
            }

            category.Name = categoryDto.Name;
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult.Success("Category Updated Successfully");
        }

        // Delete a Category
        public async Task<GeneralResult> DeleteAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdWithProductsAsync(id);

            if (category == null)
            {
                return GeneralResult.NotFound("Category not found");
            }

            if (category.Products.Any())
            {
                return GeneralResult.Failure("Cannot delete a category that has products assigned to it.");
            }

            _unitOfWork.CategoryRepository.Delete(category);
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult.Success("Category Deleted Successfully");
        }

        // Upload Category Image
        public async Task<GeneralResult<ImageUploadResultDto>> UploadImageAsync(
            int id,
            ImageUploadDto dto,
            string basePath,
            string? schema,
            string? host)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return GeneralResult<ImageUploadResultDto>.NotFound("Category not found");
            }

            var uploadResult = await _imageManager.UploadAsync(dto, basePath, schema, host);

            if (!uploadResult.IsSuccess)
            {
                return GeneralResult<ImageUploadResultDto>.Failure("Image upload failed");
            }

            category.ImageUrl = uploadResult.Data!.ImageUrl;

            await _unitOfWork.SaveChangesAsync();

            return GeneralResult<ImageUploadResultDto>.Success(uploadResult.Data, "Image uploaded and category updated successfully");
        }
    }
}
