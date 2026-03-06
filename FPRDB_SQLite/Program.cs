using DevExpress.Skins;
using DevExpress.UserSkins;
using BLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
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
            Application.Run(new frmMain(composeRoot.getDatabaseService()));
        }
    }
}
