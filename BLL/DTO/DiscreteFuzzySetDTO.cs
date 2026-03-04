using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class DiscreteFuzzySetDTO<TDomain>:FuzzySetDTO
    {
        public List<TDomain> valueSet;
        public List<float> membershipDegreeSet;


    }
}
