using AutoMapper;
using E_Commerce.Common;
using E_Commerce.DAL;
using E_Commerce.DAL.Data.Models;
using FluentValidation;

namespace E_Commerce.BLL
{
    public class ProductManager : IProductManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ProductCreateDto> _productCreateDtoValidator;
        private readonly IValidator<ProductUpdateDto> _productUpdateDtoValidator;
        private readonly IImageManager _imageManager;

        public ProductManager(
            IUnitOfWork unitOfWork,
            IValidator<ProductCreateDto> productCreateDtoValidator,
            IValidator<ProductUpdateDto> productUpdateDtoValidator,
            IImageManager imageManager)
        {
            _unitOfWork = unitOfWork;
            _productCreateDtoValidator = productCreateDtoValidator;
            _productUpdateDtoValidator = productUpdateDtoValidator;
            _imageManager = imageManager;
        }

        // Get Products with Categories
        public async Task<GeneralResult<IEnumerable<ProductDetailsDto>>> GetProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllWithCategoriesAsync();

            var productsDto = products.Select(p => new ProductDetailsDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Count = p.Count,
                Category = p.Category.Name,
                ImageUrl = p.ImageUrl
            });

            return GeneralResult<IEnumerable<ProductDetailsDto>>.Success(productsDto);
        }

        // Get Product by ID with Category
        public async Task<GeneralResult<ProductDetailsDto>> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdWithCategoriesAsync(id);

            if (product == null)
            {
                return GeneralResult<ProductDetailsDto>.NotFound("Product not found");
            }

            var productDto = new ProductDetailsDto
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Count = product.Count,
                Category = product.Category.Name,
                ImageUrl = product.ImageUrl
            };

            return GeneralResult<ProductDetailsDto>.Success(productDto);
        }

        // Create Product
        public async Task<GeneralResult> InsertAsync(ProductCreateDto productCreateDto)
        {
            var result = await _productCreateDtoValidator.ValidateAsync(productCreateDto);

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(r => r.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToList()
                    );

                return GeneralResult.Failure(errors);
            }

            var product = new Product
            {
                Title = productCreateDto.Title,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                Count = productCreateDto.Count,
                CategoryId = productCreateDto.CategoryId
            };

            _unitOfWork.ProductRepository.Insert(product);
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult.Success("Product created successfully");
        }

        // Update Product
        public async Task<GeneralResult> UpdateAsync(int id, ProductUpdateDto productUpdateDto)
        {
            if (id != productUpdateDto.Id)
            {
                return GeneralResult.Failure("ID in URL does not match ID in body");
            }

            var result = await _productUpdateDtoValidator.ValidateAsync(productUpdateDto);

            if (!result.IsValid)
            {
                var errors = result.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList());

                return GeneralResult.Failure(errors);
            }

            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

            if (product == null)
            {
                return GeneralResult.NotFound("Product not found");
            }

            product.Title = productUpdateDto.Title;
            product.Description = productUpdateDto.Description;
            product.Price = productUpdateDto.Price;
            product.Count = productUpdateDto.Count;
            product.CategoryId = productUpdateDto.CategoryId;

            await _unitOfWork.SaveChangesAsync();
            return GeneralResult.Success("Product updated successfully");
        }

        // Delete Product
        public async Task<GeneralResult> DeleteAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

            if (product == null)
            {
                return GeneralResult.NotFound("Product not found");
            }

            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            return GeneralResult.Success("Product deleted successfully");
        }

        // Get Products with Pagination and Filtering
        public async Task<GeneralResult<PagedResult<ProductDetailsDto>>> GetProductsPaginationAsync
            (
                PaginationParameters paginationParameters,
                ProductFilterParameters productFilterParameters
            )
        {
            var pagedResult = await _unitOfWork.ProductRepository.GetProductsPagination(paginationParameters, productFilterParameters);

            var productsDto = pagedResult.Items.Select(p => new ProductDetailsDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                Count = p.Count,
                Category = p.Category.Name,
                ImageUrl = p.ImageUrl
            });

            var pagedResultDto = new PagedResult<ProductDetailsDto>
            {
                Items = productsDto,
                MetaData = pagedResult.MetaData
            };

            return GeneralResult<PagedResult<ProductDetailsDto>>.Success(pagedResultDto);
        }

        // Upload Product Image
        public async Task<GeneralResult<ImageUploadResultDto>> UploadImageAsync(
            int id,
            ImageUploadDto dto,
            string basePath,
            string? schema,
            string? host)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);

            if (product == null)
            {
                return GeneralResult<ImageUploadResultDto>.NotFound("Product not found");
            }

            var uploadResult = await _imageManager.UploadAsync(dto, basePath, schema, host);

            if (!uploadResult.IsSuccess)
            {
                return GeneralResult<ImageUploadResultDto>.Failure("Image upload failed");
            }

            product.ImageUrl = uploadResult.Data!.ImageUrl;

            await _unitOfWork.SaveChangesAsync();

            return GeneralResult<ImageUploadResultDto>.Success(uploadResult.Data, "Image uploaded and product updated successfully");
        }
    }
}
