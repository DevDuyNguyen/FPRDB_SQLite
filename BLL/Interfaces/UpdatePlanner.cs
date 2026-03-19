using BLL.DomainObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Interfaces
{
    public interface UpdatePlanner
    {
        public bool executeCreateSchema(FPRDBSchema data);
        public bool executeCreateRelation(FPRDBRelation data);
        public int executeInsert(InsertData data);
        public int executeDelete(DeleteData data);
        public void executeDropRelation(string name);
        public void executeDropSchema(string name);
    }
}
