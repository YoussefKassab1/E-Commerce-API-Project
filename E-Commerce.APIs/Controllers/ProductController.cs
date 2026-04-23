using E_Commerce.BLL;
using E_Commerce.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductManager _productManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductManager productManager, IWebHostEnvironment webHostEnvironment)
        {
            _productManager = productManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // Get All Products
        [HttpGet]
        [Authorize(Policy = "UserAccess")]
        public async Task<ActionResult<GeneralResult<PagedResult<ProductDetailsDto>>>> GetAll
            (
                [FromQuery] PaginationParameters paginationParameters,
                [FromQuery] ProductFilterParameters productFilterParameters
            )
        {
            var result = await _productManager.GetProductsPaginationAsync(paginationParameters, productFilterParameters);
            return Ok(result);
        }

        //// Get All Paginated and Filtered Products
        //[HttpGet("pagination")]
        //[Authorize(Policy = "UserAccess")]
        //public async Task<ActionResult<GeneralResult<PagedResult<ProductDetailsDto>>>> GetAllPaginatedProducts
        //    (
        //        [FromQuery] PaginationParameters paginationParameters,
        //        [FromQuery] ProductFilterParameters productFilterParameters
        //    )
        //{
        //    var result = await _productManager.GetProductsPaginationAsync(paginationParameters, productFilterParameters);
        //    return Ok(result);
        //}

        // Get Product By Id
        [HttpGet("{id:int}")]
        [Authorize(Policy = "UserAccess")]
        public async Task<ActionResult<GeneralResult<ProductDetailsDto>>> GetById([FromRoute] int id)
        {
            var result = await _productManager.GetProductByIdAsync(id);

            if (result.Status == ResultStatus.NotFound)
                return NotFound(result);

            return Ok(result);
        }

        // Create Product
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult>> Create([FromBody] ProductCreateDto productCreateDto)
        {
            var result = await _productManager.InsertAsync(productCreateDto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // Update Product
        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult>> Update([FromRoute] int id, [FromBody] ProductUpdateDto productUpdateDto)
        {
            var result = await _productManager.UpdateAsync(id, productUpdateDto);

            if (result.Status == ResultStatus.NotFound)
                return NotFound(result);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // Delete Product
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult>> Delete([FromRoute] int id)
        {
            var result = await _productManager.DeleteAsync(id);

            if (result.Status == ResultStatus.NotFound)
                return NotFound(result);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // upload Product Image
        [HttpPost("{id:int}/image")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult<ImageUploadResultDto>>> UploadImage
            (
                [FromRoute] int id,
                [FromForm] ImageUploadDto imageUploadDto
            )
        {
            var schema = Request.Scheme;
            var host = Request.Host.Value;
            var basePath = _webHostEnvironment.ContentRootPath;

            var result = await _productManager.UploadImageAsync(id, imageUploadDto, basePath, schema, host);

            if (result.Status == ResultStatus.NotFound)
                return NotFound(result);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}