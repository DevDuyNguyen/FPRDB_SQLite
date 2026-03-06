using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.SQLProcessing
{
    public class Preprocessor
    {
        private MetadataManager metadataMgr;

        public Preprocessor(MetadataManager metadataMgr)
        {
            this.metadataMgr = metadataMgr;
        }
        public bool checkSemanticCreateSchema(FPRDBSchema data)
        {
            string schemaName = data.getSchemaName();
            string constraintName = data.getPrimaryConstraintName();
            try
            {
                if (this.metadataMgr.isSchemaExist(schemaName))
                {

                }

                    && data.getPrimaryConstraintName()!=null
                    && data.getPrimarykey()!=null && data.getPrimarykey().Count>=1
                    && !this.metadataMgr.isConstraintExist(constraintName);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


    }
}
