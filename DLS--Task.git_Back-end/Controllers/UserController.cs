using Application_Layer.IRepo.Services;
using DLS_applaction_Layer.DTO;
using DLS_Domin_layer.DLS_Servises.User_Servise;
using Infrastructure_Layer.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLS__Task.git_Back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly IUserServies _userservise;
        private readonly IUnitofWork _unitofWork;
        public UserController(IUserServies userservise, IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;

            _userservise = userservise;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid");
            }

            // Ensure model is not null and contains required data  
            if (model == null || string.IsNullOrEmpty(model.email) || string.IsNullOrEmpty(model.password))
            {
                return BadRequest("Invalid registration data provided.");
            }

            // Validate email and password format  
            if (!model.email.Contains("@") || model.password.Length < 6)
            {
                return BadRequest("Invalid email or password format.");
            }

            var result = await _userservise.RegisterModelAsync(model);

            if (result == null)
            {
                return BadRequest("Registration service returned null.");
            }

            if (!result.IsAuthenticated)
            {
                return BadRequest("Error while registering: " + result.Message);
            }

            // Commit changes to the database  
            try
            {
                _unitofWork.CommitChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while committing changes: " + ex.Message);
            }

            return Ok(new
            {
                result.Token,
                result.ExpiresOn,
                result.Role,
                result.UserName,
                result.Email,
                result.UserID,
                result.Roles
            });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid");
            }

            // Ensure loginModel is not null and contains required data  
            if (loginModel == null || string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest("Invalid login data provided.");
            }

            // Validate email and password format  
            if (!loginModel.Email.Contains("@") || loginModel.Password.Length < 6)
            {
                return BadRequest("Invalid email or password format.");
            }

            var result = await _userservise.LoginModel(loginModel);

            if (result == null)
            {
                return BadRequest("Login service returned null.");
            }

            if (!result.IsAuthenticated)
            {
                return BadRequest("Invalid email or password.");
            }

            _unitofWork.CommitChanges();
            return Ok(new
            {
                token = result.Token,
                user = new
                {
                    id = result.UserID, // إذا كان لديك Id
                    email = result.Email,
                    roles = result.Roles
                }
            });
        }
    }
}