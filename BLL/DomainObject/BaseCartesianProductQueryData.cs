using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DomainObject
{
    public class BaseCartesianProductQueryData:QueryData
    {
        public List<SelectField> selectList;
        public List<string> relationList;
        public SelectionCondition selectionCondition;
        public FPRDBSchema schema;

        public BaseCartesianProductQueryData(List<SelectField> selectList, List<string> relationList, SelectionCondition selectionCondition)
        {
            this.selectList = selectList;
            this.relationList = relationList;
            this.selectionCondition = selectionCondition;
        }
        public override FPRDBSchema getSchema() => this.schema;


    }
}
