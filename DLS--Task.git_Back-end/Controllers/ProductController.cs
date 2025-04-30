using AutoMapper;
using DLS_applcation_layer.DLS_Repository.ProductsRepository;
using DLS_applcation_layer.DLS_Servises.ProductsSevises;
using DLS_applcation_layer.DTO;
using DLS_Domin_layer.hleper;
using DLS_Infrastructure__Layer.DLS_Infrastructure__Layer;
using Microsoft.AspNetCore.Mvc;
using Sermart_Api.Helpers;

namespace DLS__Task.git_Back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServise _productService;
        private readonly ImageServises _imageService;
        private readonly APPDbcontext aPPDbcontext;
        private readonly IMapper _mapper;

        public ProductController(IProductServise productService, IMapper mapper, ImageServises imageService,APPDbcontext aPPDbcontext)
        {
            this.aPPDbcontext = aPPDbcontext;
            _productService = productService;
            _imageService = imageService;
            _mapper = mapper;
        }


        // GET: api/Product
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("GetById{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // POST: api/Product
        [HttpPost("AddProducts")]
        public async Task<IActionResult> Create([FromForm] ProductsDTO productDto)
        {
            try
            {

                await _productService.CreateAsync(productDto);

                return CreatedAtAction(nameof(GetById), new { id = productDto.CategoryID }, productDto);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }

        }

        // PUT: api/Product/{id}  
        [HttpPut("UpDate{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] ProductsDTO productDto)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { Message = $"Product with id {id} not found." });
                }

                // Use AutoMapper to map DTO to the domain model  
                var updatedProduct = _mapper.Map(productDto, product);

                var result = await _productService.UpdateAsync(id, updatedProduct);
                if (!result)
                {
                    return NotFound(new { Message = $"Product with id {id} not found." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        

        // DELETE: api/Product/{id}  
        [HttpDelete("Delete{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { Message = $"Product with id {id} not found." });
                }

                var result = await _productService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new { Message = $"Product with id {id} not found." });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/Product/GetByCategory/{categoryId}  
        [HttpGet("GetByCategory{categoryId}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            try
            {
                var products = await _productService.GetAllAsync();
                var filteredProducts = products.Where(p => p.CategoryID == categoryId);
                return Ok(filteredProducts);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
