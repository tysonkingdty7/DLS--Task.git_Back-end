using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using DLS_Domin_layer.Modules;

namespace DLS_Infrastructure__Layer.configration
{
    public class ProductCategoryConfigurstion : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category", "Category");
            builder.HasKey(x => x.ID);
            builder.Property(i => i.ID).ValueGeneratedOnAdd();
            builder.Property(i => i.Catagory).IsRequired().HasMaxLength(500);
        }
    }
}
