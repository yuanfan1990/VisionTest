using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionSystem
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool bExist;
            Mutex MyMutex = new Mutex(true, "Form", out bExist);
            if (bExist)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
            else
            {
                MessageBox.Show("程序已经运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
