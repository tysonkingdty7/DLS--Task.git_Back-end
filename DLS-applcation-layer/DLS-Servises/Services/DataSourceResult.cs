using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.IRepo.Services
{
    public class DataSourceResult
    {
        [DataMember(Name = "errors")]
        public string Errors { get; set; }

        [DataMember(Name = "data")]
        public IEnumerable Data { get; set; }

        [DataMember(Name = "total")]
        public int Total { get; set; }

        [DataMember(Name = "aggregates")]
        public object Aggregates { get; set; }



        public DataSourceResult()
        {

        }
        public DataSourceResult(IEnumerable data, int total)
        {
            this.Data = data;
            this.Total = total;
        }
    }
}
