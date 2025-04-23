using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DLS_Domin_layer.hleper
{
    public static class DLs_sendRolecs
    {
         public static void SendRolecs( this ModelBuilder modelBuilder)

        {
            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "Admin".ToUpper() },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "User".ToUpper() },
                new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Manager", NormalizedName = "Manager".ToUpper() }



            );

        }
    }
}
