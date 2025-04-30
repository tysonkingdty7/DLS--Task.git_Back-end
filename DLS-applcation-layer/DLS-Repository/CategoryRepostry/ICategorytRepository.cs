using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Layer.IRepo.Services;
using Application_Layer.IRepo.Services.Repositery;
using DLS_Domin_layer.DLS_Repository;
using DLS_Domin_layer.Modules;

namespace DLS_applcation_layer.DLS_Repository.CategoryRepostry
{
    public interface ICategorytRepository : IBaseRepo<Category>
    {
        Task AddCategory(Category entity);
       
        void DeleteCategory (Guid id );
        Task<Category> UPdateCategolry (Category entity);
        Task<int> getCatgoryCount();
    }
}
