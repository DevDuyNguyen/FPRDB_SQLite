using System.IO;

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
        public ContinuousFuzzySetDTO(float leftBottom, float leftTop, float rightTop, float rightBottom,
            int oid, string fuzzySetName) : base(oid, fuzzySetName, FieldType.FLOAT)
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
                throw new InvalidDataException($"The membership function of {this.fuzzySetName} doesn't form a point, triangle or trapazoid");
        }
    }

}
