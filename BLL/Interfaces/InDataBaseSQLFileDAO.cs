using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface InDataBaseSQLFileDAO
    {
        public void createFile(string fileName, string content);
        public string getFileContent(string fileName);
        public void save(string fileName, String content);
        public void deleteFile(string fileName);
    }
}
