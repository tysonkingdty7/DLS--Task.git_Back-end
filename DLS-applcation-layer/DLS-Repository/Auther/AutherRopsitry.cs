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


        public AutherRopsitry(APPDbcontext appContext, JwtHelper jwtHelper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : base(appContext)
        {
            _appContext = appContext;
            _jwtHelper = jwtHelper;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<AuthModel> LoginModel(LoginDTO loginModel)
        {
            try
            {
                var AuthUser = new AuthModel();
                var user = await _userManager.FindByEmailAsync(loginModel.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.Password))
                {
                    AuthUser.IsAuthenticated = false;
                    AuthUser.Message = "Invalid email or password";
                    return AuthUser;
                }
                var token = await CreateToken(user);
                var roles = await _userManager.GetRolesAsync(user);
                AuthUser.IsAuthenticated = true;
                AuthUser.UserName = user.name;
                AuthUser.Email = user.Email;
                AuthUser.Id = user.Id;
                AuthUser.Token = new JwtSecurityTokenHandler().WriteToken(token);
                AuthUser.Roles = roles.ToList();
                return AuthUser;
            }
            catch (Exception ex) {
                // تسجيل الخطأ إذا كنت تستخدم نظام تسجيل (مثل Serilog أو NLog)
                return new AuthModel
                {
                    IsAuthenticated = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }

            }


        public async Task<AuthModel> RegisterModelAsync(RegisterDTO regsterModel)
        {
            try
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
                    var error = string.Join(", ", result.Errors.Select(e => e.Description));
                    return new AuthModel
                    {
                        IsAuthenticated = false,
                        Message = $"Error while creating user: {error}"
                    };
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
            catch (Exception ex)
            {
                // تسجيل الخطأ إذا كنت تستخدم نظام تسجيل (مثل Serilog أو NLog)
                return new AuthModel
                {
                    IsAuthenticated = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                };
            }
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
            var jwtKey = _config["JwtSettings:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JwtSettings:Key is not configured.");
            }
            var symkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var signingkey = new SigningCredentials(symkey, SecurityAlgorithms.HmacSha256);
            var jwttoken = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(double.Parse(_config["JwtSettings:DurationInDays"])),
                signingCredentials: signingkey);
            return jwttoken;
        }
    }
}
