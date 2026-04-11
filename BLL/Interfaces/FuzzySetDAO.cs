using BLL.DomainObject;
using BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL.Interfaces
{
    public interface FuzzySetDAO
    {
        public DiscreteFuzzySet<T> createDiscreteFuzzySet<T>(DiscreteFuzzySetDTO<T> fuzzySet) where T : IComparable<T>;
        public ContinuousFuzzySet createContinuousFuzzySet(ContinuousFuzzySetDTO fuzzySet);
        public List<BaseFuzzySet> findFuzzySet(string name);
        public List<FPRDBRelation>  getUsingRelations(FuzzySetDTO fuzzySet);
        public void removeFuzzySet(FuzzySetDTO fuzzySet);
        public FuzzySetDTO getExactFuzzySet(int oid);
        public void updateDiscreteFuzzySet<T>(DiscreteFuzzySetDTO<T> fuzzySet);
        public void updateContinuousFuzzySet(ContinuousFuzzySetDTO fuzzySet);
        public bool isFuzzySetExist(string name);

    }
}
