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
        public static int maxSelectTop = 100;
        public static void clear()
        {
            createSChemaFieldTypes = null;
            loadFPRDBSchemas = null;
            loadFPRDBSchemaRelations = null;
        }
    }
}
