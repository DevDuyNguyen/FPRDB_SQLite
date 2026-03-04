using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTO
{
    public class ContinuousFuzzySetDTO:FuzzySetDTO
    {
        public float leftBottom;
        public float leftTop;
        public float rightTop;
        public float rightBottom;

        public ContinuousFuzzySetDTO(float leftBottom, float leftTop, float rightTop, float rightBottom,
            string fuzzySetName, FieldType fuzzySetType) : base(fuzzySetName, fuzzySetType)
        {
            this.leftBottom = leftBottom;
            this.leftTop = leftTop;
            this.rightTop = rightTop;
            this.rightBottom = rightBottom;
        }
    }

}
