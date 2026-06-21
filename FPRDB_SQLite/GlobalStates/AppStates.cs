using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.DomainObject;
using BLL.DTO;

namespace GUI.GlobalStates
{
    public static class AppStates
    {
        public static List<string> createSChemaFieldTypes;
        public static List<FPRDBSchemaDTO> loadFPRDBSchemas;
        public static List<FPRDBRelationDTO> loadFPRDBSchemaRelations;
        public static List<string> listOfInDatabaseSQLFiles;
        public static int maxSelectTop = 100;
        private static bool iSAppStateFullyLoad=false;
        public static bool ISAppStateFullyLoad
        {
            get { return iSAppStateFullyLoad; }
            set 
            {
                //if ((bool)value == iSAppStateFullyLoad)
                //{
                //    throw new InvalidOperationException("New state of iSAppStateFullyLoad must be different from the current state");
                //}
                iSAppStateFullyLoad = value;
            }
        }

        public static void clear()
        {
            createSChemaFieldTypes = null;
            loadFPRDBSchemas = null;
            loadFPRDBSchemaRelations = null;
            listOfInDatabaseSQLFiles = null;
        }
        


    }
}
