using BLL.DomainObject;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class NaturalJoinScan:Scan
    {
        public void beforeFirst() => throw new NotImplementedException();
        public bool next() => throw new NotImplementedException();
        public void close() => throw new NotImplementedException();
        public FuzzyProbabilisticValue<T> getFieldContent<T>(String fldName) => throw new NotImplementedException();
        public bool hasField(string fldname) => throw new NotImplementedException();
        //public FPRDBSchema getSchema();
        public List<AbstractFuzzyProbabilisticValue> getCurrentTuple() => throw new NotImplementedException();


    }
}
