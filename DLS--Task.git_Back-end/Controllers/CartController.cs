using System.Threading.Tasks;
using DLS_Domin_layer.Modules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DLS_applcation_layer.DLS_Repository.CartRepositry;
using DLS_applcation_layer.DTO;
using DLS_Infrastructure__Layer.DLS_Infrastructure__Layer;
using Microsoft.AspNetCore.Identity;

namespace DLS__Task.git_Back_end.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartService;
        private readonly APPDbcontext _AppDbcontext;
        private readonly UserManager<User> _manager;


        public CartController(ICartRepository cartService, APPDbcontext aPPDbcontext,UserManager<User> userManager)
        {
            _cartService = cartService;
            _AppDbcontext = aPPDbcontext;
            _manager = userManager;
        }

        [HttpGet("GetallCart")]
        public async Task<ActionResult<CartItemGetDTO>> GetAll()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.GetAllItems(userID);
            var data = result.Select(i => new CartItemGetDTO
            {
                ProductID = i.ProductID,
                ProductName = i.Product.ProductName,
                UnitPrice = i.Product.UnitPrice,
                Stoke = i.Product.Stoke,
                Count = i.Qauntety,
                Id = i.Id,
                SupPrice = i.Product.UnitPrice * i.Qauntety,
                PicURL = i.Product.Image.Select(i => i.MeadiUrl).FirstOrDefault()
            });
            return Ok(data);
        }
        [HttpPost("addCartProducts")]
public async Task<IActionResult> AddItem(string productid)
        {
            Console.WriteLine("AddItem method called"); // Debug log  

            // Ensure userID is not null or empty  
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userID))
                return Unauthorized("User is not authenticated.");

            // Ensure productid is not null or empty  
            if (string.IsNullOrEmpty(productid))
                return BadRequest("No Product to Add");

            // Call the service to add the product  
            var result = await _cartService.Add(productid, userID);
            if (result == null)
                return BadRequest("Failed to add product to cart.");

            // Ensure user and cart are not null  
            var user = await _manager.FindByIdAsync(userID);
            if (user == null || user.Cart == null)
                return BadRequest("User cart not found.");

            // Check if the cart's Items collection is initialized  
            if (user.Cart.Items == null)
                user.Cart.Items = new List<CartItem>();

            // Add the item to the user's cart  
            user.Cart.Items.Add(result);
            await _AppDbcontext.SaveChangesAsync();

            return Ok(result);
        }
        [HttpPatch("UpDate")]
        public async Task<IActionResult> Update(CartItemUpdateDTO cartItemUpdateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _cartService.Update(cartItemUpdateDTO);
            if (result == null)
                return BadRequest(result);
            _AppDbcontext.SaveChangesAsync();
            return Ok(result);
        }

        [HttpDelete("Empty")]
        public async Task<IActionResult> Empty()
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _cartService.Empty(userID);
            _AppDbcontext.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("Delete {id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _cartService.Delete(id);
            if (result == null)
                return BadRequest(result);
            _AppDbcontext.SaveChangesAsync();
            return Ok(result);
        }
    }
}
