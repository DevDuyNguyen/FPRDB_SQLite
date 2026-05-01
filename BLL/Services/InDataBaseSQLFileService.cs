using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;

namespace BLL.Services
{
    public class InDataBaseSQLFileService
    {
        private InDataBaseSQLFileDAO inDataBaseSQLFileDAO;

        public InDataBaseSQLFileService(InDataBaseSQLFileDAO inDataBaseSQLFileDAO)
        {
            this.inDataBaseSQLFileDAO = inDataBaseSQLFileDAO;
        }

        public void createFile(string fileName, string content)
        {
            this.inDataBaseSQLFileDAO.createFile(fileName, content);
        }
        public string getFileContent(string fileName)
        {
            return this.inDataBaseSQLFileDAO.getFileContent(fileName);
        }
        public void save(string fileName, String content)
        {
            this.inDataBaseSQLFileDAO.save(fileName, content);
        }
        public void deleteFile(string fileName)
        {
            this.inDataBaseSQLFileDAO.deleteFile(fileName);
        }
        public List<string> getInDatabaseSQLFileNames()
        {
            return this.inDataBaseSQLFileDAO.getInDatabaseSQLFileNames();
        }

    }
}
