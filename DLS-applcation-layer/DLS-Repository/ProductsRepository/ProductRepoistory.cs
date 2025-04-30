using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLS_applcation_layer.DTO;
using DLS_Domin_layer.hleper;
using DLS_Domin_layer.Modules;
using DLS_Infrastructure__Layer.DLS_Infrastructure__Layer;
using Microsoft.EntityFrameworkCore;

namespace DLS_applcation_layer.DLS_Repository.ProductsRepository
{
    public class ProductRepoistory : IProductsRepoistory
    {
        private readonly APPDbcontext _appContext;
       
       
        public ProductRepoistory(APPDbcontext context, ImageServises imageServises)
        {
         
            _appContext = context;
        }

        public async Task Add(Product entity)
        {
            
                await _appContext.Products.AddAsync(entity);
                await _appContext.SaveChangesAsync();
   
           
        }

        public void Delete(string id)
        {
            var product = _appContext.Products.FirstOrDefault(p => p.ProductID.ToString() == id);
            if (product != null)
            {
                _appContext.Products.Remove(product);
            }
             _appContext.SaveChangesAsync();
        }

        public IEnumerable<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllProduct()
        {

            var x = await _appContext.Products.ToListAsync();
            return x;
        }

        public async Task<Product> GetProductByid(Guid id)
        {
            return await _appContext.Products.FindAsync(id);

        }

        public async Task<IEnumerable<object>> GetProductsByCategoryId(Guid categoryId)
        {
            // Fetch products by category ID  
            var products = await _appContext.Products
                .Where(p => p.CategoryID == categoryId)
                .Select(p => new
                {
                    p.ProductID,
                    p.ProductName,
                    p.Description,
                    p.UnitPrice,
                    p.Stoke
                })
                .ToListAsync();

            // Return the result  
            return products;
        }

        public Task<double> GetRate(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetUserProduct(string id)
        {
           return await _appContext.Products.Where(p => p.ProviderId == id).ToListAsync();
        }

        public void Update(Product entity)
        {
            var existingProduct = _appContext.Products.FirstOrDefault(p => p.ProductID == entity.ProductID);
            if (existingProduct != null)
            {
                // Update fields  
                existingProduct.ProductName = entity.ProductName;
                existingProduct.Description = entity.Description;
                existingProduct.UnitPrice = entity.UnitPrice;
                existingProduct.Stoke = entity.Stoke;
                existingProduct.Image = entity.Image;
                existingProduct.ProviderId = entity.ProviderId;

                // If CategoryID is not provided, keep the existing one  
                if (entity.CategoryID != Guid.Empty)
                {
                    existingProduct.CategoryID = entity.CategoryID;
                }
                if (entity.Image != null)
                {
                    existingProduct.Image = entity.Image;
                }

                _appContext.Products.Update(existingProduct);
                _appContext.SaveChangesAsync();
            }
        }
    }
}

