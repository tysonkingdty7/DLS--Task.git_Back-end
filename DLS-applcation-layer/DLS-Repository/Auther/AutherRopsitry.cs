using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLS_Infrastructure__Layer.DLS_Infrastructure__Layer;
using DLS_Domin_layer.DLS_Repository.Auther;
using DLS_applaction_Layer.DTO;
using DLS_Domin_layer.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Domein_Layer.DTO.AutherDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace DLS_Domin_layer.DLS_Repository.Auther
{
    public class AutherRopsitry : BaseRepository<User>, IAutherReposirty
    {
        private readonly APPDbcontext _appContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtHelper _jwtHelper;
        private readonly IConfiguration _config;


        public AutherRopsitry(APPDbcontext appContext, JwtHelper jwtHelper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager) : base(appContext)
        {
            _appContext = appContext;
            _jwtHelper = jwtHelper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AuthModel> LoginModel(LoginDTO loginModel)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(loginModel.Email);

            if (user == null)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "Invalid email or password.";
                return authModel;
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user, user.password, loginModel.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "Invalid email or password.";
                return authModel;
            }

            var token = await CreateToken(user);
            authModel.Email = user.Email;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(token);
            authModel.UserName = user.UserName;
            authModel.Roles = (await _userManager.GetRolesAsync(user)).ToList();
            authModel.Id = user.Id;

            return authModel;
        }

        public async Task<AuthModel> RegisterModelAsync(RegisterDTO regsterModel)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                name = regsterModel.name,
                Email = regsterModel.email,
                UserName = regsterModel.email, // تعيين اسم المستخدم كالبريد الإلكتروني
                phone = regsterModel.phone
            };

            // إنشاء المستخدم باستخدام UserManager
            var result = await _userManager.CreateAsync(user, regsterModel.password);
            if (!result.Succeeded)
            {
                var erorr = string.Empty;
                foreach (var err in result.Errors)
                {
                    erorr += $"{err.Description},";
                }
                return new AuthModel { Message = erorr };
            }

            // إضافة المستخدم إلى الأدوار
            if (regsterModel.Role != null && regsterModel.Role.Any())
            {
                foreach (var role in regsterModel.Role)
                {
                    if (await _roleManager.RoleExistsAsync(role))
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                    else
                    {
                        return new AuthModel
                        {
                            IsAuthenticated = false,
                            Message = $"Role '{role}' does not exist."
                        };
                    }
                }
            }

            // إنشاء التوكن
            var token = await CreateToken(user);

            return new AuthModel
            {
                IsAuthenticated = true,
                UserName = user.name,
                Email = user.Email,
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Roles = regsterModel.Role
            };
        }

        private async Task<JwtSecurityToken> CreateToken(User user)
        {
            var userclaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleclaim = new List<Claim>();
            foreach (var role in roles)
            {
                roleclaim.Add(new Claim("roles", role));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }
            .Union(userclaims)
            .Union(roleclaim);
            var symkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signingkey = new SigningCredentials(symkey, SecurityAlgorithms.HmacSha256);
            var jwttoken = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(double.Parse(_config["JWT:DurationInDays"])),
                signingCredentials: signingkey);
            return jwttoken;
        }
    }
}
