using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLS_Domin_layer.Modules
{
    public class Cart
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ICollection<CartItem> Items { get; set; }
        public virtual User User { get; set; }
    }
}
