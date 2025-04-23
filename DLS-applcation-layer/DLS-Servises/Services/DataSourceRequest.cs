using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.IRepo.Services
{
    public class DataSourceRequest
    {
        public int skip { get; set; }
        public int take { get; set; }
        //public int page { get; set; }
        //public int pageSize { get; set; }

        public IEnumerable<Sort> Sort { get; set; }
        public Filter Filter { get; set; }
        //public string sort { get; set; }
        //public string filter { get; set; }
    }

    [DataContract]
    public class Sort
    {
        [DataMember(Name = "field")]
        public string Field { get; set; }
        [DataMember(Name = "dir")]
        public string Dir { get; set; }

        public string ToExpression()
        {
            return Field + " " + Dir;
        }
    }

    [DataContract]
    public class Filter
    {
        [DataMember(Name = "field")]
        public string Field { get; set; }

        [DataMember(Name = "operator")]
        public string Operator { get; set; }

        [DataMember(Name = "value")]
        public object Value { get; set; }

        [DataMember(Name = "logic")]
        public string Logic { get; set; }

        [DataMember(Name = "filters")]
        public IEnumerable<Filter> Filters { get; set; }

        private static readonly IDictionary<string, string> operators = new Dictionary<string, string>
        {
            {"numeq", "numeq"},
            {"numneq", "numneq"},
            {"eq", "="},
            {"ideq" , "Equals"}, // Used with Guid fields
            {"idneq" , "nEquals"}, // Used with Guid fields
            {"ideq?" , "Equals?"}, // Used with Nullable Guid fields
            {"dteq" , "dteq"},      //Used with DateTime fields 
             {"dtneq" , "dtneq"},    //Used with DateTime fields 
            
            {"dtgt" , "dtgt"},      //Used with DateTime fields 
            {"dtgte" , "dtgte"},      //Used with DateTime fields 
            {"dtlt" , "dtlt"},      //Used with DateTime fields 
            {"dtlte" , "dtlte"},      //Used with DateTime fields 


            {"neq", "!="},
            {"lt", "lt"},
            {"lte", "lte"},
            {"gt", "gt"},
            {"gte", "gte"},

            {"startswith", "StartsWith"},
            {"endswith", "EndsWith"},
            {"contains", "Contains"}
        };

        public IList<Filter> All()
        {
            var filters = new List<Filter>();

            Collect(filters);

            return filters;
        }

        private void Collect(IList<Filter> filters)
        {
            if (Filters != null && Filters.Any())
            {
                foreach (Filter filter in Filters)
                {
                    filters.Add(filter);

                    filter.Collect(filters);
                }
            }
            else
            {
                filters.Add(this);
            }
        }

        /// Converts the filter expression to a predicate suitable for Dynamic Linq e.g. "Field1 = @1 and Field2.Contains(@2)"
        public string ToExpression(IList<Filter> filters)
        {
            if (Filters != null && Filters.Any())
            {
                return "(" + String.Join(" " + Logic + " ", Filters.Select(filter => filter.ToExpression(filters)).ToArray()) + ")";
            }

            int index = filters.IndexOf(this);

            string comparison = operators[Operator];

            if (comparison == "StartsWith" || comparison == "EndsWith" || comparison == "Contains")
            {
                if (this.Value == null)
                {
                    this.Value = string.Empty;
                }
                return String.Format("{0}.{1}(@{2})", Field, comparison, index);
            }

            if (comparison == "Equals")
            {
                if (this.Value == null)
                    return String.Format("{0}.{1}(Guid(\"{2}\"))", Field, comparison, Guid.Empty);
                else
                    return String.Format("{0}.{1}(Guid(\"{2}\"))", Field, comparison, this.Value.ToString());
            }
            if (comparison == "nEquals")
            {
                if (this.Value == null)
                    return String.Format("!{0}.Equals(Guid(\"{1}\"))", Field, Guid.Empty);
                else
                    return String.Format("!{0}.Equals(Guid(\"{1}\"))", Field, this.Value.ToString());
            }
            if (comparison == "Equals?")
            {
                if (this.Value == null)
                    return String.Format("{0}.HasValue && {0}.Value.Equals(Guid(\"{2}\"))", Field, comparison, Guid.Empty);
                else
                    return String.Format("{0}.HasValue && {0}.Value.Equals(Guid(\"{2}\"))", Field, comparison, this.Value.ToString());
            }

            if (comparison == "dteq")
            {
                var dt = Convert.ToDateTime(this.Value.ToString());
                return String.Format("{0} == DateTime({2} , {3} , {4})", Field, comparison, dt.Year, dt.Month, dt.Day);
            }
            if (comparison == "dtneq")
            {
                var dt = Convert.ToDateTime(this.Value.ToString());
                return String.Format("{0} != DateTime({2} , {3} , {4})", Field, comparison, dt.Year, dt.Month, dt.Day);
            }

            if (comparison == "dtgt")
            {
                var dt = Convert.ToDateTime(this.Value.ToString());
                return String.Format("{0} > DateTime({2} , {3} , {4})", Field, comparison, dt.Year, dt.Month, dt.Day);
            }

            if (comparison == "dtgte")
            {
                var dt = Convert.ToDateTime(this.Value.ToString());
                return String.Format("{0} >= DateTime({2} , {3} , {4})", Field, comparison, dt.Year, dt.Month, dt.Day);
            }

            if (comparison == "dtlt")
            {
                var dt = Convert.ToDateTime(this.Value.ToString());
                return String.Format("{0} < DateTime({2} , {3} , {4})", Field, comparison, dt.Year, dt.Month, dt.Day);
            }

            if (comparison == "dtlte")
            {
                var dt = Convert.ToDateTime(this.Value.ToString());
                return String.Format("{0} <= DateTime({2} , {3} , {4})", Field, comparison, dt.Year, dt.Month, dt.Day);
            }


            if (comparison == "numeq")
            {
                return String.Format("{0} == {2}", Field, comparison, this.Value.ToString());
            }

            if (comparison == "numneq")
            {
                return String.Format("{0} != {2}", Field, comparison, this.Value.ToString());
            }
            if (comparison == "lt")
            {
                return String.Format("{0} < {2}", Field, comparison, this.Value.ToString());
            }
            if (comparison == "lte")
            {
                return String.Format("{0} <= {2}", Field, comparison, this.Value.ToString());
            }
            if (comparison == "gt")
            {
                return String.Format("{0} > {2}", Field, comparison, this.Value.ToString());
            }
            if (comparison == "gte")
            {
                return String.Format("{0} >= {2}", Field, comparison, this.Value.ToString());
            }
            return String.Format("{0} {1} @{2}", Field, comparison, index);
        }
    }
}
