using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UrlLinkChecker
{
    static class Program
    {

        private static frmLinkChecker linkChecker;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            linkChecker = new frmLinkChecker();
            linkChecker.Disposed += thisForm_Disposed;
            Application.Run(linkChecker);
        }

        static void thisForm_Disposed(object sender, EventArgs e)
        {
            linkChecker.CleanUp();
        }
    }
}
