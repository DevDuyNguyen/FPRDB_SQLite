using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DTO;

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
    }
}
