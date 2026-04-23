using E_Commerce.Common;

namespace E_Commerce.BLL
{
    public interface IProductManager
    {
        Task<GeneralResult<IEnumerable<ProductDetailsDto>>> GetProductsAsync();
        Task<GeneralResult<ProductDetailsDto>> GetProductByIdAsync(int id);
        Task<GeneralResult> InsertAsync(ProductCreateDto productCreateDto);
        Task<GeneralResult> UpdateAsync(int id, ProductUpdateDto productUpdateDto);
        Task<GeneralResult> DeleteAsync(int id);
        Task<GeneralResult<PagedResult<ProductDetailsDto>>> GetProductsPaginationAsync
            (
                PaginationParameters paginationParameters,
                ProductFilterParameters productFilterParameters
            );
        Task<GeneralResult<ImageUploadResultDto>> UploadImageAsync
            (
                int id,
                ImageUploadDto dto,
                string basePath,
                string? schema,
                string? host
            );
    }
}
