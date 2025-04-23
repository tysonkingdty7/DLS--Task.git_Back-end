using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLS_Domin_layer.DLS_Repository
{
    public interface IBaseRepo<T> where T : class
    {
        IEnumerable<T> GetAll();



    }
}
