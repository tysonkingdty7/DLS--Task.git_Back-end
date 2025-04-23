using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLS_Infrastructure__Layer
{
    using DLS_Domin_layer.hleper;
    using DLS_Domin_layer.Modules;
    using DLS_Infrastructure__Layer;
    using global::DLS_Infrastructure__Layer.configration;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    namespace DLS_Infrastructure__Layer
    {
        public class APPDbcontext : IdentityDbContext<User>
        {
            public APPDbcontext(DbContextOptions<APPDbcontext> options) : base(options)
            {

            }
            protected override void OnModelCreating(ModelBuilder builder)
            {
                builder.ApplyConfiguration<User>(new UserCOnfigration());
                builder.ApplyConfiguration<Cart>(new CartConfigration());
                builder.ApplyConfiguration<CartItem>(new Cartitemcs());
                builder.ApplyConfiguration<Product>(new ProductsConfigration());
                builder.SendRolecs();
                base.OnModelCreating(builder);


            }
            // Add DbSet properties for your models here  
            public DbSet<User> Users { get; set; }
            public DbSet<Product> Products { get; set; }
            public DbSet<Cart> Orders { get; set; }
            public DbSet<CartItem> CartItems { get; set; }
            public DbSet<Category> Categories { get; set; }


        }
    }
}
