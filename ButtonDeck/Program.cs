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
        public static bool Silent { get; set; } = false;
        private static string errorText = "";
        private const string errorFileName = "errors.log";
        public static ServerThread ServerThread { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Silent = args.Any(c => c.ToLower() == "/s");

            Trace.Listeners.Add(new TextWriterTraceListener(errorFileName));
            Trace.AutoFlush = true;

            errorText = $"An error occured! And it was saved to a file called {errorFileName}." + Environment.NewLine + "Please send this to the developer of the application.";

            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ApplicationSettingsManager.LoadSettings();
            DevicePersistManager.LoadDevices();


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (ApplicationSettingsManager.Settings.FirstRun) {
                FirstSetupForm firstRunForm = new FirstSetupForm();
                Application.Run(firstRunForm);
                if (!firstRunForm.FinishedSetup) return;
            }

            ServerThread = new ServerThread();
            ServerThread.Start();

            Application.Run(new MainForm());
            ServerThread.Stop();
            ApplicationSettingsManager.SaveSettings();
            DevicePersistManager.SaveDevices();
            Trace.Flush();


        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex) {
                MessageBox.Show(errorText);
                Trace.WriteLine("An error occured.");
                Trace.WriteLine($"Timestamp: [Local:{DateTime.Now}; UTC: {DateTime.UtcNow}].");
                Trace.WriteLine("Exception:");
                Trace.WriteLine("==================");
                Trace.Indent();
                ex.ToString();
                Trace.Unindent();
                Trace.WriteLine("==================");
                Trace.WriteLine("");

            }
        }



        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(errorText);
            Trace.WriteLine("An error occured.");
            Trace.WriteLine($"Timestamp: [Local:{DateTime.Now}; UTC: {DateTime.UtcNow}].");
            Trace.WriteLine("Exception:");
            Trace.WriteLine("==================");
            Trace.Indent();
            e.Exception.ToString();
            Trace.Unindent();
            Trace.WriteLine("==================");
            Trace.WriteLine("");
        }
    }
}
