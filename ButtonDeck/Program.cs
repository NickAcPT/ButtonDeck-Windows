//#define FORCE_SILENCE

using ButtonDeck.Forms;
using ButtonDeck.Misc;
using NickAc.Backend.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
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
        public static bool SuccessfulServerStart { get; set; } = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Trace.Listeners.Add(new TextWriterTraceListener(errorFileName));
            Trace.AutoFlush = true;

            if (args.Any(c => c.ToLower() == "/armobs")) {
                if (args.Length == 1) {
                    var obs32List = Process.GetProcessesByName("obs32");
                    var obs64List = Process.GetProcessesByName("obs64");
                    if (obs32List.Length == 0 && obs64List.Length == 0) {
                        //No OBS found. Cancel operation.
                        File.Delete(OBSUtils.obswszip);
                        return;
                    }
                    List<Process> obsProcesses = new List<Process>();
                    obsProcesses.AddRange(obs32List);
                    obsProcesses.AddRange(obs64List);

                    if (obsProcesses.Count != 1) {
                        //Multiple OBS instances found. Cancel operation.
                        File.Delete(OBSUtils.obswszip);
                        return;
                    }
                    var obsProcess = obsProcesses.First();

                    string path = OBSUtils.GetProcessPath(obsProcess.Id);
                    string zipTempPath = Path.GetFileNameWithoutExtension(OBSUtils.obswszip);
                    OBSUtils.ExtractZip(OBSUtils.obswszip, zipTempPath);

                    OBSUtils.DirectoryCopy(zipTempPath, OBSUtils.GetPathFromOBSExecutable(path), true);
                    File.Delete(OBSUtils.obswszip);
                    Directory.Delete(zipTempPath, true);

                    var obsGlobalFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "obs-studio", "global.ini");
                    if (File.Exists(obsGlobalFilePath) && !File.ReadAllText(obsGlobalFilePath).Contains("[WebsocketAPI]")) {

                        while (!obsProcess.HasExited) {

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("OBS plugin install completed!");
                            sb.AppendLine("");
                            sb.AppendLine("Please close OBS and click OK to apply the final touches.");
                            MessageBox.Show(sb.ToString());
                        }

                        using (StreamWriter outputFile = new StreamWriter(obsGlobalFilePath)) {
                            outputFile.WriteLine("");
                            outputFile.WriteLine("[WebsocketAPI]");
                            outputFile.WriteLine("ServerPort=4444");
                            outputFile.WriteLine("DebugEnabled=false");
                            outputFile.WriteLine("AlertsEnabled=false");
                            outputFile.WriteLine("AuthRequired=false");
                        }

                        bool shouldRepeat = true;
                        while (shouldRepeat) {
                            var obs32List2 = Process.GetProcessesByName("obs32");
                            var obs64List2 = Process.GetProcessesByName("obs64");

                            shouldRepeat = obs32List2.Length == 0 && obs64List2.Length == 0;
                            if (!shouldRepeat) break;

                            StringBuilder sb = new StringBuilder();
                            sb.AppendLine("OBS plugin install successfully!");
                            sb.AppendLine("");
                            sb.AppendLine("Please open OBS and click OK to continue to the app.");
                            MessageBox.Show(sb.ToString());
                        }

                    }

                    return;
                }
            }

#if FORCE_SILENCE
    Silent = true;
#else
            Silent = args.Any(c => c.ToLower() == "/s");
#endif
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

            OBSUtils.PrepareOBSIntegration();


            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;
            NetworkChange.NetworkAvailabilityChanged += NetworkChange_NetworkAddressChanged;

            ServerThread = new ServerThread();
            ServerThread.Start();

            Application.Run(new MainForm());

            OBSUtils.Disconnect();

            ServerThread.Stop();
            NetworkChange.NetworkAddressChanged -= NetworkChange_NetworkAddressChanged;
            NetworkChange.NetworkAvailabilityChanged -= NetworkChange_NetworkAddressChanged;
            ApplicationSettingsManager.SaveSettings();
            DevicePersistManager.SaveDevices();
            Trace.Flush();
        }

        private static void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            ServerThread.Stop();
            ServerThread = new ServerThread();
            ServerThread.Start();
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
                Trace.TraceError(ex.ToString());
                Trace.Unindent();
                Trace.WriteLine("==================");
                Trace.WriteLine("");

            }
        }



        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show(errorText);
            Trace.WriteLine("An error occured. (Thread exception)");
            Trace.WriteLine($"Timestamp: [Local:{DateTime.Now}; UTC: {DateTime.UtcNow}].");
            Trace.WriteLine("Exception:");
            Trace.WriteLine("==================");
            Trace.Indent();
            Trace.TraceError(e.Exception.ToString());
            Trace.Unindent();
            Trace.WriteLine("==================");
            Trace.WriteLine("");
        }
    }
}
