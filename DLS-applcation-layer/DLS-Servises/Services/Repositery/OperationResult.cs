using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.IRepo.Services.Repositery
{
    public  class OperationResult
    {
        public OperationResult()
        {
            ExtendedProperties = new Dictionary<string, object>();
        }

        [DataMember]
        public QueryResult Result { get; set; }

        [DataMember]
        public string ExceptionMessage { get; set; }

        [DataMember]
        public Dictionary<string, object> ExtendedProperties { get; set; }
    }
}
