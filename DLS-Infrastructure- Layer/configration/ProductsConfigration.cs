using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLS_Domin_layer.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DLS_Infrastructure__Layer.configration
{
    public class ProductsConfigration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product", "Product");
            builder.HasKey(p => p.ProductID);
            builder.Property(p => p.ProductName).HasMaxLength(50);
            builder.Property(p => p.Description).HasMaxLength(500);
            builder.HasOne(p => p.ProductCategory).WithMany(c => c.Products).HasForeignKey(p => p.CategoryID);
            builder.HasOne(p => p.ProductUser).WithMany(s => s.Products).HasForeignKey(p => p.ProviderId);
        }
    }
}
