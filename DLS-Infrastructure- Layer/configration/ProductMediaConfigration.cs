using Domein_Layer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer.Configuration
{
    public class ProductMediaConfigration : IEntityTypeConfiguration<ProductMedia>
    {
        public void Configure(EntityTypeBuilder<ProductMedia> builder)
        {
            builder.ToTable("ProductMedia", "ProductsMdc");
            builder.HasKey(x => x.ID);
            builder.Property(x=>x.MeadiUrl).IsRequired().HasMaxLength(500);
            builder.HasOne(z=>z.Products).WithMany(x=>x.Image).HasForeignKey(x=>x.ProductID).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
