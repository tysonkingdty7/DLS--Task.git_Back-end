using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLS_Domin_layer.Modules;

namespace Domein_Layer.Models
{
    public class ProductMedia
    {
        public Guid ID { get; set; }
        public string MeadiUrl { get; set; }
        public Guid ProductID { get; set; }
        public virtual Product Products { get; set; }

    }
}
