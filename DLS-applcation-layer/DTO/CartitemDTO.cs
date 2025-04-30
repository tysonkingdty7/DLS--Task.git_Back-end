using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLS_applcation_layer.DTO
{
    public class CartItemGetDTO
    {

        public int Id { get; set; }
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public int Stoke { get; set; }
        public int Count { get; set; } = 1;
        public decimal SupPrice { get; set; }
        public string PicURL { get; set; }
        public decimal UnitPrice { get; set; }

    }
}
