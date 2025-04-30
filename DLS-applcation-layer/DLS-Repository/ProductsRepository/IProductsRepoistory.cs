using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLS_applcation_layer.DTO;
using DLS_Domin_layer.DLS_Repository;
using DLS_Domin_layer.Modules;

namespace DLS_applcation_layer.DLS_Repository.ProductsRepository
{
    public interface IProductsRepoistory : IBaseRepo<Product>
    {
        Task Add(Product entity);
        void Update(Product entity);
        void Delete(string id);
        Task<IEnumerable<Product>> GetAllProduct();
        Task<IEnumerable<Product>> GetUserProduct(string id);
        Task<Product> GetProductByid(Guid id);
        Task<double> GetRate(string Id);
        Task<IEnumerable<object>> GetProductsByCategoryId(Guid categoryId);
    }
}
