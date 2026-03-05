using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BLL.Common;
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
            //Application.Run(new Form1(composeRoot.getDatabaseService()));
            Application.Run(new frmMain());
        }
    }
}
