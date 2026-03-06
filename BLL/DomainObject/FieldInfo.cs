using System;

namespace BLL.DomainObject
{
    public class FieldInfo
    {
        private FieldType type;

        private int txtLength { get; set; } = 255;
        public FieldInfo(FieldType type, int length)
        {
            this.type = type;
            this.txtLength = length;
        }
        public FieldType getType()
        {
            return this.type;
        }
        public int getTXTLength()
        {
            return this.txtLength;
        }
    }
}