using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface Scan
    {
        public void beforeFirst();
        public bool next();
        public void close();
        public FuzzyProbabilisticValue<T> getFieldContent<T>(String fldName);
        public bool hasField(string fldname);
    }
}
