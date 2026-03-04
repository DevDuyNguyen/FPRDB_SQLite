using BLL.DomainObject;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (fuzzySet is DiscreteFuzzySetDTO<T>)
                {
                    FuzzySet<T> fs = this.fuzzySetDAO.createDiscreteFuzzySet<T>((DiscreteFuzzySetDTO<T>)fuzzySet);
                    FuzzySetDTO dto = fs.toDTO();
                    return dto;
                }
                return null;
                    
            }
            catch(Exception ex)
            {
                throw new Exception("Can't create fuzzy set");
            }
        }
    }
}
