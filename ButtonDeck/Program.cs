using ButtonDeck.Forms;
using ButtonDeck.Misc;
using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ButtonDeck
{
    static class Program
    {
        public static ServerThread ServerThread { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            DevicePersistManager.LoadDevices();

            ServerThread = new ServerThread();
            ServerThread.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            ServerThread.Stop();
            DevicePersistManager.SaveDevices();

        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                MessageBox.Show("An error occured! Please send a screenshot of this message to the developer." + Environment.NewLine + (ex.ToString()));
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show("An error occured! Please send a screenshot of this message to the developer." + Environment.NewLine + (e.Exception.ToString()));
        }
    }
}
