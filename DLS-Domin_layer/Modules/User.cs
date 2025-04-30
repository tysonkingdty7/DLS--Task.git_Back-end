using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DLS_Domin_layer.Modules
{
    public class User : IdentityUser
    {
        public string name { get; set; }
        public string phone { get; set; }
        public int CartID { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Product> Products { get; set; }
      


    }
}
