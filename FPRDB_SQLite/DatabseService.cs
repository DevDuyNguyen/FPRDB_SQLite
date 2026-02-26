using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FPRDB_SQLite
{
    public class DatabseService
    {
        private DatabseManager dbMgr = new DatabseManager();
        #region *New Database
        public void SaveDB()
        {
            dbMgr.SaveDB();
        }
        public bool openDB()
        {
            return dbMgr.openDB();
        }
        public List<FPRDBSchema> getFPRDBSchemas()
        {
            return dbMgr.getFPRDBSchemas();
        }
        #endregion
        public List<FPRDBRelation> getFPRDBRelations()
        {
            return dbMgr.getFPRDBRelations();
        }
    }
}
