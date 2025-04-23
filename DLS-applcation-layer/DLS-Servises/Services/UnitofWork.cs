using Application_Layer.IRepo.Services;
using DLS_Infrastructure__Layer.DLS_Infrastructure__Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Layer.Repo
{
    public class UnitofWork : IUnitofWork
    {
        private  APPDbcontext _appDbContext;
        public  UnitofWork(APPDbcontext appcontext)
        {
            _appDbContext = appcontext; 

        }
    
         
        int IUnitofWork.CommitChanges()
        {
            return _appDbContext.SaveChanges();
        }
    }
}
