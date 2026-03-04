using System;

namespace BLL.DomainObject
{
    public class FieldInfo
    {
        private FieldType type;

        private long txtLength { get; set; } = 255;
        public FieldInfo(FieldType type, long length)
        {
            this.type = type;
            this.txtLength = length;
        }
        private FieldType getType()
        {
            return this.type;
        }
        private long getTXTLength()
        {
            return this.txtLength;
        }
    }
}