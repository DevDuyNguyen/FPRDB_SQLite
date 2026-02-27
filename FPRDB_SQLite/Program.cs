using DevExpress.Skins;
using DevExpress.UserSkins;
using FPRDB_SQLite.BLL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            //Lexer lexer = new Lexer(" select true \"asdf\" 1⇒2  ⨂");
            //lexer.printAllToken();
            //lexer.eatKeyword("select");
            //Debug.WriteLine(lexer.eatBooleanConstant());
            //Debug.WriteLine(lexer.eatStringConstant());
            //Debug.WriteLine(lexer.eatNumberConstant());
            //Debug.WriteLine(lexer.eatOperator());
            //Debug.WriteLine(lexer.eatNumberConstant());
            //Debug.WriteLine(lexer.eatProbabilisticCombinationStrategy());
        }
    }
}
