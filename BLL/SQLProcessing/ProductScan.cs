using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.DomainObject;

namespace BLL.SQLProcessing
{
    public class ProductScan:Scan
    {
        private Scan s1;
        private Scan s2;
        private List<AbstractFuzzyProbabilisticValue> currentTuple;
        public ProductScan(Scan s1, Scan s2)
        {
            this.s1 = s1;
            this.s2 = s2;
            s1.next();
        }
        public void beforeFirst()
        {
            s1.beforeFirst();
            s1.next();
            s2.beforeFirst();
        }
        public bool hasField(string fldname)
        {
            return s1.hasField(fldname) || s2.hasField(fldname);
        }
        public bool next()
        {
            if (s2.next())
            {

                return true;
            }
            else
            {
                throw new NotImplementedException();
                s2.beforeFirst();
                return s1.next() && s2.next();
            }
        }


    }
}
