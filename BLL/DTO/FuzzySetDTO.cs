using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class FuzzySetDTO
    {
        public int oid;
        public string fuzzySetName;
        public FieldType fuzzySetType;

        public FuzzySetDTO(string fuzzySetName, FieldType fuzzySetType)
        {
            this.fuzzySetName = fuzzySetName;
            this.fuzzySetType = fuzzySetType;
        }
    }
}
