using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLS_Domin_layer.Modules
{
    public class CartItem
    {
        public int Id { get; set; }
        public Guid ProductID { get; set; }
        public virtual  ICollection<Product> Products { get; set; }
        public virtual Cart Cart { get; set; }
        public int Qauntety { get; set; } = 1;
        
        public int CartID { get; set; }
    }
}
