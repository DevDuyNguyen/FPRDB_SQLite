using BLL.DomainObject;

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
        public int executeModify(ModifyData data);
    }
}
