using System;
using System.Text;
using System.Text.Json.Serialization;
using Application_Layer.IRepo.Services;
using DLs.Helpers;
using DLS.Mapper;
using DLS_applcation_layer.DLS_Repository.CartRepository;
using DLS_applcation_layer.DLS_Repository.CartRepositry;
using DLS_applcation_layer.DLS_Repository.CategoryRepostry;
using DLS_applcation_layer.DLS_Repository.ProductsRepository;
using DLS_applcation_layer.DLS_Servises;
using DLS_applcation_layer.DLS_Servises.ProductsSevises;
using DLS_applcation_layer.DLS_Servises.User_Servise;
using DLS_Domin_layer.DLS_Repository;
using DLS_Domin_layer.DLS_Repository.Auther;
using DLS_Domin_layer.DLS_Servises.User_Servise;
using DLS_Domin_layer.hleper;
using DLS_Domin_layer.Modules;
using DLS_Infrastructure__Layer.DLS_Infrastructure__Layer;
using Infrastructure_Layer.Repo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddDbContext<APPDbcontext>(options =>
        {
            options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                m => m.MigrationsAssembly(typeof(APPDbcontext).Assembly.FullName));
        });
        builder.Services.AddControllers().AddJsonOptions(
            x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        // Add services to the container.
        builder.Services.AddScoped<JwtHelper>();
        builder.Services.Configure<JWT>(builder.Configuration.GetSection("JwtSettings"));
        builder.Services.AddScoped<IUnitofWork, UnitofWork>();
        builder.Services.AddScoped<IUserServies,UserServies>();
        builder.Services.AddScoped<IAutherReposirty, AutherRopsitry>();
        builder.Services.AddScoped<ICartRepository, CartRepository>();
        builder.Services.AddAutoMapper(typeof(productProfile), typeof(CatgoryProfile));
        builder.Services.AddScoped<ImageServises>();
        builder.Services.AddScoped<ICategorytRepository, CategoryRepositry>();
        builder.Services.AddScoped<IProductServise,ProductServies>();
        builder.Services.AddScoped<IProductsRepoistory, ProductRepoistory>();

        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        builder.Services.AddScoped(typeof(IBaseRepo<>), typeof(BaseRepository<>));

        builder.Services.AddIdentity<User, IdentityRole>(option =>
        {
            option.Password.RequireDigit = true;
            option.Password.RequireLowercase = true;
            option.Password.RequireUppercase = true;
            option.Password.RequiredLength = 8;
            option.Password.RequireNonAlphanumeric = false; // Disable the requirement for non-alphanumeric characters

        }

    ).AddEntityFrameworkStores<APPDbcontext>()
       .AddDefaultTokenProviders();
        builder.Services.AddCors();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle  
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using bearer scheme",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
        });
        builder.Services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(w =>
            {
                var jwtSettings = builder.Configuration.GetSection("JwtSettings");
                w.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
                };
            }
        );
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.  
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuthorization();
        app.UseCors(builder => builder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed((host) => true) // Allow any origin for testing; tighten this in production
        .AllowCredentials());
        app.MapControllers();

        app.Run();
    }
}
