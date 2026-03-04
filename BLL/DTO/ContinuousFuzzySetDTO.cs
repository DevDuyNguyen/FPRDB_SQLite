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
            string fuzzySetName) : base(fuzzySetName, FieldType.FLOAT)
        {
            this.leftBottom = leftBottom;
            this.leftTop = leftTop;
            this.rightTop = rightTop;
            this.rightBottom = rightBottom;
        }
        public override bool isValid()
        {
            if (this.leftBottom <= this.leftTop
                && this.leftTop <= this.rightTop
                && this.rightTop <= this.rightBottom)
                return true;
            else
                return false;
        }
    }

}
