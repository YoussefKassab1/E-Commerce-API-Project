using E_Commerce.BLL;
using E_Commerce.Common;

namespace E_Commerce.BLL
{
    public interface ICategoryManager
    {
        Task<GeneralResult<IEnumerable<CategoryReadDto>>> GetCategoriesAsync();
        Task<GeneralResult<CategoryReadDto>> GetCategoryByIdAsync(int id);
        Task<GeneralResult> InsertAsync(CategoryCreateDto categoryCreateDto);
        Task<GeneralResult> UpdateAsync(int id, CategoryUpdateDto categoryUpdateDto);
        Task<GeneralResult> DeleteAsync(int id);
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
