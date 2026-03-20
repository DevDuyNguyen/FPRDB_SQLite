using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface UpdateScan:Scan
    {
        public void setFieldContent<T>(string fldname, FuzzyProbabilisticValue<T> content);
        public void insert();
        public void delete();
    }
}
