using BLL.DomainObject;

namespace BLL.Interfaces
{
    public interface UpdateScan:Scan
    {
        public void setFieldContent<T>(string fldname, FuzzyProbabilisticValue<T> content);
        public void insert();
        public void delete();
    }
}
