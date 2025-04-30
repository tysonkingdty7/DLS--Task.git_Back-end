using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLS_Infrastructure__Layer;
using DLS_Infrastructure__Layer.DLS_Infrastructure__Layer;
namespace DLS_Domin_layer.DLS_Repository
{
    public class BaseRepository<T> : IBaseRepo<T> where T : class
    {
        private readonly APPDbcontext _appContext;

        public BaseRepository(APPDbcontext appContext)
        {
            _appContext = appContext;
        } 



        public IEnumerable<T> GetAll()
        {
            return _appContext.Set<T>().ToList();
        }
    }
       
    
}
    

