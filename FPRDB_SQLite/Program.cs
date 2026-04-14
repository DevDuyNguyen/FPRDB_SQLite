using BLL.Common;
using DevExpress.Skins;
using DevExpress.UserSkins;
using FPRDB_SQLite.GUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

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
            // Create a custom culture based on English US (which uses the period)
            CultureInfo customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();

            // Force the decimal separator to be a period, just in case
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            customCulture.NumberFormat.NumberGroupSeparator = ",";

            // Apply it to the current thread and all future threads
            Thread.CurrentThread.CurrentCulture = customCulture;
            Thread.CurrentThread.CurrentUICulture = customCulture;
            CultureInfo.DefaultThreadCurrentCulture = customCulture;
            CultureInfo.DefaultThreadCurrentUICulture = customCulture;

            // Force DevExpress to use the Invariant (Period-based) culture for its editors
            DevExpress.Utils.FormatInfo.AlwaysUseThreadFormat = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            CompositionRoot composeRoot = new CompositionRoot();
            Application.Run(new frmMain(composeRoot));
        }
    }
}
