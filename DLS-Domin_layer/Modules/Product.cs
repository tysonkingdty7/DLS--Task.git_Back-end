using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domein_Layer.Models;

namespace DLS_Domin_layer.Modules
{
    public  class Product
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
  
        [NotMapped]
        public decimal UnitPrice { get; set; }
        public int Stoke { get; set; }
        public virtual ICollection<ProductMedia> Image { get; set; }
        public Guid CategoryID { get; set; }
        public string ProviderId { get; set; }
        public virtual User ProductUser { get; set; }
        public virtual Category ProductCategory { get; set; }
        
    }
}
