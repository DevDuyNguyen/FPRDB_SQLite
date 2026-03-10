using BLL.DomainObject;
using BLL.Enums;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.SQLProcessing
{
    public class UnionScan:Scan
    {
        private Scan s1;
        private Scan s2;
        private List<AbstractFuzzyProbabilisticValue> currentTuple;
        private ProbabilisticCombinationStrategy probCombinationStrategy;

        public UnionScan(Scan s1, Scan s2, ProbabilisticCombinationStrategy probCombinationStrategy)
        {
            this.s1 = s1;
            this.s2 = s2;
            this.probCombinationStrategy = probCombinationStrategy;
            throw new NotImplementedException();
        }

        public void beforeFirst()=> throw new NotImplementedException();
        public bool next()=> throw new NotImplementedException();
        public void close() { }
        public FuzzyProbabilisticValue<T> getFieldContent<T>(String fldName)=> throw new NotImplementedException();
        public bool hasField(string fldname)=> throw new NotImplementedException();
        //public FPRDBSchema getSchema();
        public List<AbstractFuzzyProbabilisticValue> getCurrentTuple()=> throw new NotImplementedException();
    }
}
