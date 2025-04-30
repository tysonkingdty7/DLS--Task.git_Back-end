using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLS_applcation_layer.DTO;

namespace DLS_applcation_layer.DLS_Servises.ProductsSevises
{
    public interface IProductServise
    {
        Task<IEnumerable<ProductsDTO>> GetAllAsync();
        Task<ProductsDTO> GetByIdAsync(Guid id);
        Task CreateAsync(ProductsDTO productDto);
        Task<bool> UpdateAsync(Guid id, ProductsDTO productDto);
        Task<bool> DeleteAsync(Guid id);

        Task<IEnumerable<ProductsDTO>> GetByCategoryAsync(Guid categoryId);


    }
}
