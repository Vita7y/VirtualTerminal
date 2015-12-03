using System.Threading;
using System.Windows.Forms;

namespace VirtualTerminal
{
    internal class Program
    {
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += Application_ThreadException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Stend());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {

        }
    }
}
