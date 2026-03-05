using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
<<<<<<< HEAD
using BLL;
=======
using BLL.Common;
>>>>>>> f5752036df1bc8ed9e09455da3be034b6d1890ba
using FPRDB_SQLite.GUI;

namespace FPRDB_SQLite
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CompositionRoot composeRoot = new CompositionRoot();
<<<<<<< HEAD
            Application.Run(new frmMain(composeRoot.getDatabaseService()));
=======
            //Application.Run(new Form1(composeRoot.getDatabaseService()));
            Application.Run(new frmMain());
>>>>>>> f5752036df1bc8ed9e09455da3be034b6d1890ba
        }
    }
}
