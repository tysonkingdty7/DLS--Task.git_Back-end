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
        private readonly  IUnitofWork _unitofWork;
        public UserController(IUserServies userservise, IUnitofWork unitofWork)
        {
            _unitofWork = unitofWork;
            
            _userservise = userservise;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid");
            }
            var result = await _userservise.RegisterModelAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest("error while registering" + result.Message);
            _unitofWork.CommitChanges();
            return Ok(new { result.Token, result.ExpiresOn, result.Role, result.UserName, result.Email,  result.UserID });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model state is not valid");
            }
            var result = await _userservise.LoginModel(loginModel);
            if (result.IsAuthenticated)
            {
                _unitofWork.CommitChanges();
            }
            else
            {
                return BadRequest("error while logging in" + result.Message);
            }
                return Ok(new { result.Token, result.UserName, result.Email, result.Roles });

            }

        }
}
