using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class BaseNaturalJoinQueryData:QueryData
    {
        public List<SelectField> selectList;
        public NaturalJoinList naturalJoinList;
        public SelectionCondition selectionCondition;
        public FPRDBSchema schema;

        public BaseNaturalJoinQueryData(List<SelectField> selectList, NaturalJoinList naturalJoinList, SelectionCondition selectionCondition)
        {
            this.selectList = selectList;
            this.naturalJoinList = naturalJoinList;
            this.selectionCondition = selectionCondition;
        }

        public override FPRDBSchema getSchema() => this.schema;
    }
}
