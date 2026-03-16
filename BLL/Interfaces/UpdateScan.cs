using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface UpdateScan
    {
        public void setFieldContent<T>(String fldname, FuzzyProbabilisticValue<T> content);
        public void insert();
        public void delete();
    }
}
