using Microsoft.AspNetCore.Http;

namespace E_Commerce.BLL
{
    public sealed record ImageUploadDto(IFormFile? File);
}
