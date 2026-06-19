using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.HandlingException
{
    static public class HandlingExceptionViaErrorNotification
    {
        public static void handlingInvalidOperationException(Exception ex)
        {
            XtraMessageBox.Show(ex.Message, "Invalid Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
