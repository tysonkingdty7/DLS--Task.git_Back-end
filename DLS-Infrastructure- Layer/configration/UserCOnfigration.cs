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
    public class UserCOnfigration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", "User");
            builder.Property(u => u.name).IsRequired().HasMaxLength(50);
            builder.Property(u => u.phone).IsRequired().HasMaxLength(15);
            builder.HasMany(u => u.Products).WithOne(p => p.ProductUser).HasForeignKey(p => p.ProviderId);

        }
    }
}

