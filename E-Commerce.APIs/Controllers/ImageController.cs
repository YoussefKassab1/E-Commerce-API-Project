using E_Commerce.BLL;
using E_Commerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class ImageController : ControllerBase
    {
        private readonly IImageManager _imageManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageController(IImageManager imageManager, IWebHostEnvironment webHostEnvironment)
        {
            _imageManager = imageManager;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<ActionResult<GeneralResult<ImageUploadResultDto>>> UploadAsync([FromForm] ImageUploadDto imageUploadDto)
        {
            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var basePath = _webHostEnvironment.ContentRootPath;

            var result = await _imageManager.UploadAsync(imageUploadDto, basePath, schema, host);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
