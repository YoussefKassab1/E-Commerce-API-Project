using E_Commerce.Common;

namespace E_Commerce.BLL
{
    public interface IImageManager
    {
        Task<GeneralResult<ImageUploadResultDto>> UploadAsync(ImageUploadDto imageUploadDto, string basePath, string? schema, string? host);
    }
}