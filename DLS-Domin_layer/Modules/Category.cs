namespace DLS_Domin_layer.Modules
{
    public class Category
    {
        public Guid ID { get; set; }
        public string Catagory { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}