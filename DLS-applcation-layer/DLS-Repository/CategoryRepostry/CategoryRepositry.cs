using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application_Layer.IRepo.Services;
using Application_Layer.IRepo.Services.Repositery;
using DLS_Domin_layer.Modules;
using DLS_Infrastructure__Layer.DLS_Infrastructure__Layer;
using Microsoft.EntityFrameworkCore;

namespace DLS_applcation_layer.DLS_Repository.CategoryRepostry
{
    public class CategoryRepositry : ICategorytRepository
    {
        private readonly APPDbcontext _appContext;
        public CategoryRepositry(APPDbcontext appContext)
        {
            _appContext = appContext;
        }
        public async Task AddCategory(Category entity)
        {
            await _appContext.Categories.AddAsync(entity);
            await _appContext.SaveChangesAsync();
        }

        public void DeleteCategory(Guid id)
        {
            var model = _appContext.Categories.FirstOrDefault(d=>d.ID==id);
            if (model != null)
            {
                _appContext.Categories.Remove(model);
            }
            _appContext.SaveChangesAsync();

            
        }

        public IEnumerable<Category> GetAll()
        {
            return _appContext.Categories;
            
        }

    
        public async Task<int> getCatgoryCount()
        {
            return await _appContext.Categories.CountAsync();
        }

        public async Task<Category> UPdateCategolry(Category entity)
        {
            var model = _appContext.Categories.Update(entity);
            await _appContext.SaveChangesAsync();
            return model.Entity;
        }
    }
}
