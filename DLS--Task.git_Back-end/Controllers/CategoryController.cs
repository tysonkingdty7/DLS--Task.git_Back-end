using AutoMapper;
using DLS_applcation_layer.DLS_Repository.CategoryRepostry;
using DLS_Domin_layer.Modules;
using Domain_Layer.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DLS__Task.git_Back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategorytRepository _categoryService;
        private readonly IMapper _mapper;
        public CategoryController(ICategorytRepository categoryService, IMapper mapper)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        // GET: api/Category  
        [HttpGet]
        [HttpGet("GETALL")]
        public async Task<ActionResult<IEnumerable<ProductCatgoryViewModel>>> GetAll()
        {
            var result = _categoryService.GetAll(); // Removed 'await' as GetAll() is synchronous.  
            var mapper = _mapper.Map<IEnumerable<Category>, IEnumerable<ProductCatgoryViewModel>>(result);
            if (mapper == null)
            {
                return NotFound();
            }

            return Ok(mapper);
        }

        [HttpPost("AddProducts")]
        public async Task<ActionResult<ProductCatgoryViewModel>> AddCategory([FromForm] ProductCatgoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // تحويل النموذج إلى كيان باستخدام AutoMapper  
                var categoryEntity = _mapper.Map<ProductCatgoryViewModel, Category>(model);

                // إضافة الكيان إلى قاعدة البيانات  
                await _categoryService.AddCategory(categoryEntity);

                // إعادة الكيان المضاف مع حالة Created  
                return CreatedAtAction(nameof(GetAll), new { id = categoryEntity.ID }, categoryEntity);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ إذا كنت تستخدم نظام تسجيل (مثل Serilog أو NLog)  
                return StatusCode(500, new { Message = "An error occurred while adding the category.", Details = ex.Message });
            }
        }
        [HttpPost("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromQuery] Guid id)
        {
            try
            {
                _categoryService.DeleteCategory(id);
                return Ok(new { Message = "Category deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the category.", Details = ex.Message });
            }
        }

        [HttpPut("UpdateCategory")]
        public async Task<ActionResult<ProductCatgoryViewModel>> UpdateCategory([FromQuery] Guid id, [FromBody] ProductCatgoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Map the incoming view model to a Category entity
                var categoryEntity = _mapper.Map<ProductCatgoryViewModel, Category>(model);

                // Set the ID of the category entity to the provided ID
                categoryEntity.ID = id;

                // Pass the Category entity to the repository method
                var updatedCategory = await _categoryService.UPdateCategolry(categoryEntity);

                if (updatedCategory == null)
                {
                    return NotFound(new { Message = "Category not found." });
                }

                // Map the updated Category entity back to the view model
                var updatedViewModel = _mapper.Map<Category, ProductCatgoryViewModel>(updatedCategory);
                return Ok(updatedViewModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the category.", Details = ex.Message });
            }
        }
     

    }

}

