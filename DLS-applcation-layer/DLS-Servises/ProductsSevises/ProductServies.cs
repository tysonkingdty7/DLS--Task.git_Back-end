using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using DLS_applcation_layer.DLS_Repository.ProductsRepository;
using DLS_applcation_layer.DLS_Servises.ProductsSevises;
using DLS_applcation_layer.DTO;
using DLS_Domin_layer.DLS_Repository;
using DLS_Domin_layer.hleper;
using DLS_Domin_layer.Modules;
using DLS_Infrastructure__Layer.DLS_Infrastructure__Layer;
using Domein_Layer.Models;

namespace DLS_applcation_layer.DLS_Servises
{
    public class ProductServies : BaseRepository<Product>, IProductServise
    {
        private readonly IProductsRepoistory _productsRepoistory;
        private readonly ImageServises _imageServises;
         private readonly APPDbcontext _appContext;
        public ProductServies(APPDbcontext appContext, IProductsRepoistory productsRepoistory, ImageServises imageServises) : base(appContext)
        {
            _appContext = appContext;
            _productsRepoistory = productsRepoistory;
            _imageServises = imageServises;

        }

        public async Task CreateAsync(ProductsDTO productDto)
        {
            var product = new Product
            {
                ProductID = Guid.NewGuid(),
                ProductName = productDto.ProduectName,
                Description = productDto.Description,
                UnitPrice = productDto.UnitPrice,
                CategoryID = productDto.CategoryID,
                ProviderId = productDto.ProviderId,
                Stoke = productDto.Stoke,
            };
            product.Image = new List<ProductMedia>();
            foreach (var image in productDto.Pics)
            {
                var imagePath = await _imageServises.SaveImageAsync(image);
                product.Image.Add(new ProductMedia
                {
                    MeadiUrl = imagePath,
                    ProductID = product.ProductID
                });
            }


            await _productsRepoistory.Add(product);
             await _appContext.SaveChangesAsync();




        }

        public Task<bool> DeleteAsync(Guid id)
        {
            var product = _productsRepoistory.GetProductByid(id);
            if (product == null)
            {
                return Task.FromResult(false);
            }
            _productsRepoistory.Delete(product.Id.ToString());
            return Task.FromResult(true);

        }

        public async Task<IEnumerable<ProductsDTO>> GetAllAsync()
        {
            var products = await _productsRepoistory.GetAllProduct();
            var productDtos = products.Select(p => new ProductsDTO
            {
                ProviderId = p.ProviderId,
                ProduectName = p.ProductName,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                CategoryID = p.CategoryID,
                Stoke = p.Stoke,
                // IFormFileCollection cannot be directly populated here.  
            }).ToList();

            return productDtos;
        }

        public async Task<IEnumerable<ProductsDTO>> GetByCategoryAsync(Guid categoryId)
        {
            var productCategory = await _productsRepoistory.GetProductsByCategoryId(categoryId);
            var productDtos = productCategory.Cast<ProductsDTO>();
            return productDtos;
        }

        public async Task<ProductsDTO> GetByIdAsync(Guid id)
        {
            var product = await _productsRepoistory.GetProductByid(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }

            // Map the Product entity to ProductsDTO  
            var productDto = new ProductsDTO
            {
                ProviderId = product.ProviderId,
                ProduectName = product.ProductName,
                Description = product.Description,
                UnitPrice = product.UnitPrice,
                CategoryID = product.CategoryID,
                Stoke = product.Stoke,
                Pics = null // Assuming Pics is not directly mappable from Product  
            };

            return productDto;
        }

        public async Task<bool> UpdateAsync(Guid id, ProductsDTO productDto)
        {
            var product = await _productsRepoistory.GetProductByid(id);
            if (product == null)
            {
                return false;
            }
            product.ProductName = productDto.ProduectName;
            product.Description = productDto.Description;
            product.UnitPrice = productDto.UnitPrice;
            product.CategoryID = productDto.CategoryID;
            product.Stoke = productDto.Stoke;
            // Update images if provided
            if (productDto.Pics != null)
            {
                foreach (var image in productDto.Pics)
                {
                    var imagePath = await _imageServises.SaveImageAsync(image);
                    product.Image.Add(new ProductMedia
                    {
                        MeadiUrl = imagePath,
                        ProductID = product.ProductID
                    });
                }
            }
            _productsRepoistory.Update(product);
            return true;


        }
    }
}

