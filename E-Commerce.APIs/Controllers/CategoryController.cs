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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager _categoryManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoryController(ICategoryManager categoryManager, IWebHostEnvironment webHostEnvironment)
        {
            _categoryManager = categoryManager;
            _webHostEnvironment = webHostEnvironment;
        }

        // Get all categories
        [HttpGet]
        [Authorize(Policy = "UserAccess")]
        public async Task<ActionResult<GeneralResult<IEnumerable<CategoryReadDto>>>> GetAll()
        {
            var result = await _categoryManager.GetCategoriesAsync();
            return Ok(result);
        }

        // Get category by id
        [HttpGet("{id:int}")]
        [Authorize(Policy = "UserAccess")]
        public async Task<ActionResult<GeneralResult<CategoryReadDto>>> GetById([FromRoute] int id)
        {
            var result = await _categoryManager.GetCategoryByIdAsync(id);

            if (result.Status == ResultStatus.NotFound)
                return NotFound(result);

            return Ok(result);
        }

        // Create a new category
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult>> Create([FromBody] CategoryCreateDto categoryCreateDto)
        {
            var result = await _categoryManager.InsertAsync(categoryCreateDto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // Update an existing category
        [HttpPut("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult>> Update([FromRoute] int id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            var result = await _categoryManager.UpdateAsync(id, categoryUpdateDto);

            if (result.Status == ResultStatus.NotFound)
                return NotFound(result);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // Delete a category
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<GeneralResult>> Delete([FromRoute] int id)
        {
            var result = await _categoryManager.DeleteAsync(id);

            if (result.Status == ResultStatus.NotFound)
                return NotFound(result);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        // Upload category image
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

            var result = await _categoryManager.UploadImageAsync(id, imageUploadDto, basePath, schema, host);

            if (result.Status == ResultStatus.NotFound)
                return NotFound(result);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
