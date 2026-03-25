using BLL.DomainObject;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BLL.Services
{
    public class FuzzySetService
    {
        private FuzzySetDAO fuzzySetDAO;
        public FuzzySetService() { }
        public FuzzySetService(FuzzySetDAO fuzzySetDAO)
        {
            this.fuzzySetDAO = fuzzySetDAO;
        }
        public bool checkIfFuzzySetValid(FuzzySetDTO fuzzyset)
        {
            return fuzzyset.isValid();
        }
        public FuzzySetDTO createFuzzySet<T>(FuzzySetDTO fuzzySet)
        {
            try
            {
                FuzzySetDTO dto;
                if (fuzzySet is DiscreteFuzzySetDTO<T>)
                {
                    FuzzySet<T> fs = this.fuzzySetDAO.createDiscreteFuzzySet<T>((DiscreteFuzzySetDTO<T>)fuzzySet);
                    dto = fs.toDTO();
                    
                }
                else
                {
                    FuzzySet<float> fs = this.fuzzySetDAO.createContinuousFuzzySet((ContinuousFuzzySetDTO)fuzzySet);
                    dto = fs.toDTO();
                }
                return dto;

            }
            catch(SQLExecutionException ex)
            {
                throw ex;
            }
        }
        public List<FuzzySetDTO> findFuzzySet(string name)
        {
            List<FuzzySetDTO> ans = new List<FuzzySetDTO>();
            List<BaseFuzzySet> fsList = this.fuzzySetDAO.findFuzzySet(name);
            foreach(BaseFuzzySet fs in fsList)
            {
                if(fs is FuzzySet<int>)
                {
                    ans.Add((fs as FuzzySet<int>).toDTO());
                }
                else if (fs is FuzzySet<float>)
                {
                    ans.Add((fs as FuzzySet<float>).toDTO());
                }
                else if (fs is FuzzySet<string>)
                {
                    ans.Add((fs as FuzzySet<string>).toDTO());
                }
            }
            return ans;
        }
        public void removeFuzzySet(FuzzySetDTO fuzzySet)
        {
            List<FPRDBRelation> usingRelations = this.fuzzySetDAO.getUsingRelations(fuzzySet);
            if(usingRelations!=null && usingRelations.Count > 0)
            {
                string errorMessage = $"Can't delete the fuzzy set {fuzzySet.fuzzySetName}, because relations";
                foreach(FPRDBRelation rel in usingRelations)
                {
                    errorMessage += $" {rel.getRelName()},";
                }
                errorMessage = errorMessage.TrimEnd(',');
                errorMessage += " are using it";
                throw new InvalidOperationException(errorMessage);
            }
            else
            {
                this.fuzzySetDAO.removeFuzzySet(fuzzySet);
            }
        }


    }
}
