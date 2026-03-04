using BLL.DomainObject;
using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface FuzzySetDAO
    {
        public DiscreteFuzzySet<T> createDiscreteFuzzySet<T>(DiscreteFuzzySetDTO<T> fuzzySet);
        public ContinuousFuzzySet createContinuousFuzzySet(ContinuousFuzzySetDTO fuzzySet);
    }
}
