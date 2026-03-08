using BLL.Enums;
using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class FuzzySetConstant:Constant
    {
        private string val;
        private int fuzzySetOID;
        private FieldType type;
        public FuzzySetConstant(string val)
        {
            this.val = val;
        }

        public Object getVal() => this.val;
        public float probInterpretationOfRelationsOnFuzzySets(Constant c, CompareOperation compareOperation)
        {
            throw new NotImplementedException();
        }
        public int getFuzzySetOID() => this.fuzzySetOID;
        public void setFuzzySetOID(int oid) => this.fuzzySetOID = oid;
        public FieldType getType() => this.type;
        public void setType(FieldType type) => this.type = type;

    }
}
