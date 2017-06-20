using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace FileDownloader
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ThreadPool.SetMinThreads(500, 1000);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
