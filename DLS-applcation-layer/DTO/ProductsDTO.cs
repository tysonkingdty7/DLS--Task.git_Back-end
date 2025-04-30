using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DLS_applcation_layer.DTO
{
    public class ProductsDTO
    {
        public string ProduectName { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; } = 0;
        public int Stoke { get; set; }
        public Guid CategoryID { get; set; }
        public string? ProviderId { get; set; }
        public IFormFileCollection? Pics { get; set; }

    }
}
