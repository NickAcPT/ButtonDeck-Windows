using OBSWebsocketDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NickAc.Backend.Utils
{
    public static class OBSUtils
    {
        public const string obswszip = "obs-ws.zip";
        public const string obsWsWebLocation = "https://github.com/Palakis/obs-websocket/releases/download/4.3.0/obs-websocket-4.3.0-Windows.zip";

        public static bool IsConnected { get => OBSConnection != null && OBSConnection.IsConnected; }
        public static OBSWebsocket OBSConnection { get; set; }
        static OBSUtils()
        {
        }

        public static void ConnectToOBS()
        {
            try {
                Thread th = new Thread(ConnectToServer);
                th.Start();
            } catch (Exception) {
            }
        }

        private static void ConnectToServer()
        {
            OBSConnection = new OBSWebsocket();
            OBSConnection.Connect("ws://localhost:4444", "");
        }

        public static void PatchOBS(string obsPath)
        {
            //This method will take a file named obs-ws.zip from the current dir
            //and fix obs to allow the plugin to work
            if (File.Exists(obswszip)) {
                var finalExtractionPath = Path.GetFullPath(Path.Combine(obsPath, "..", ".."));
            }
        }

        public static bool PrepareOBS()
        {
            var obs32List = Process.GetProcessesByName("obs32");
            var obs64List = Process.GetProcessesByName("obs64");
            if (obs32List.Length == 0 && obs64List.Length == 0) {
                //No OBS found. Cancel operation.
                return true;
            }
            List<Process> obsProcesses = new List<Process>();
            obsProcesses.AddRange(obs32List);
            obsProcesses.AddRange(obs64List);

            if (obsProcesses.Count != 1) {
                //Multiple OBS instances found. Cancel operation.
                return false;
            }
            var obsProcess = obsProcesses.First();

            string path = GetProcessPath(obsProcess.Id);
            if (!string.IsNullOrEmpty(path) && path.Contains("studio")) {
                //We have the OBS executable path.
                //We should ask if the user wants to download the obs plugin.
                var obsGlobalFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "obs-studio", "global.ini");
                if (File.Exists(obsGlobalFilePath)/* && !File.ReadAllText(obsGlobalFilePath).Contains("[WebsocketAPI]")*/) {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("An OBS instance was found!");
                    sb.AppendLine("");
                    sb.AppendLine("Do you want to automatically download the necessary OBS plugin to enable compatibility with ButtonDeck?");
                    sb.AppendLine("You won't be prompted again in the future.");
                    if (MessageBox.Show(sb.ToString(), "OBS Interaction", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {

                        //Now we download the websocket plugin from the web
                        using (WebClient client = new WebClient()) {
                            client.DownloadFile(obsWsWebLocation, obswszip);
                        }

                        //TODO: After the download has been completed, try to elevate the process
                        //and extract the plugin files to the obs folder.


                        ProcessInfo newProcess = new System.Diagnostics.ProcessInfo();



                    }
                }

            }

            return true;


        }
        public static string GetProcessPath(int processId)
        {
            string MethodResult = "";
            try {
                string Query = "SELECT ExecutablePath FROM Win32_Process WHERE ProcessId = " + processId;

                using (ManagementObjectSearcher mos = new ManagementObjectSearcher(Query)) {
                    using (ManagementObjectCollection moc = mos.Get()) {
                        string ExecutablePath = (from mo in moc.Cast<ManagementObject>() select mo["ExecutablePath"]).First().ToString();

                        MethodResult = ExecutablePath;
                    }

                }

            } catch {
            }
            return MethodResult;
        }



        public static void Disconnect()
        {
            if (IsConnected) {
                Thread th = new Thread(OBSConnection.Disconnect);
                th.Start();
            }
        }

        public static void PrintScenes()
        {
            Thread th = new Thread(() => {

                Debug.WriteLine("OBS SCENES:");
                List<OBSScene> scenes = OBSConnection.ListScenes();
                foreach (var scene in scenes) {
                    Debug.WriteLine($"   - {scene.Name}");
                }
                Debug.WriteLine("END");
            });
            th.Start();
        }
    }
}
