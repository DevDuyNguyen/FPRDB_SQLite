using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace BLL.DomainObject
{
    public class PossibleValue
    {
        public Constant constant;
        public float lowerBound;
        public float upperBound;

        public PossibleValue(Constant constant, float lowerBound, float upperBound)
        {
            this.constant = constant;
            this.lowerBound = lowerBound;
            this.upperBound = upperBound;
        }
    }
}
