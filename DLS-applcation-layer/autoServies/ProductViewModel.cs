using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Sermart_Api.Helpers
{
    public class ProductViewModel
    {

        
        //[Required(ErrorMessage = "Please, Provide a valid Product Name")]
        //[StringLength(50, ErrorMessage = "Must be more 5 letter", MinimumLength = 5)]
        public string ProductName { get; set; }
        //[Required, StringLength(300, MinimumLength = 10)]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Please, Provide a valid Product Price")]
        public decimal UnitPrice { get; set; }
        [Required]
        public int Stoke { get; set; }
        [Required(ErrorMessage = "Please, Provide atleast one image")]
        //public ICollection<IFormFile> ProductMedias { get; set; }
       
        [Display(Name = "Choose Product Category")]
        public Guid CategoryID { get; set; }
        public string? ProviderId { get; set; }

        //public List<IFormFile>? Pics { get; set; } = new List<IFormFile>();
        public IFormFileCollection? Pics { get; set; }
        public List<string> PicsURL { get; set; } = new List<string>();


    }
}
