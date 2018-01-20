using OBSWebsocketDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NickAc.Backend.Utils
{
    public static class OBSUtils
    {
        #region Fields

        public const string obsWsWebLocation = "https://github.com/Palakis/obs-websocket/releases/download/4.3.0/obs-websocket-4.3.0-Windows.zip";
        public const string obswszip = "obs-ws.zip";

        #endregion

        #region Constructors

        static OBSUtils()
        {
        }

        #endregion

        #region Properties

        public static bool IsConnected { get => OBSConnection != null && OBSConnection.IsConnected; }
        public static OBSWebsocket OBSConnection { get; set; }

        #endregion

        #region Methods

        public static void ConnectToOBS()
        {
            try {
                Thread th = new Thread(ConnectToServer);
                th.Start();
            } catch (Exception) {
            }
        }

        /// <summary>
        /// Directories the copy.
        /// </summary>
        /// <param name="sourceDirPath">The source dir path.</param>
        /// <param name="destDirName">Name of the destination dir.</param>
        /// <param name="isCopySubDirs">if set to <c>true</c> [is copy sub directories].</param>
        /// <returns></returns>
        public static void DirectoryCopy(string sourceDirPath, string destDirName, bool isCopySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirPath);
            DirectoryInfo[] directories = directoryInfo.GetDirectories();
            if (!directoryInfo.Exists) {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: "
                    + sourceDirPath);
            }
            DirectoryInfo parentDirectory = Directory.GetParent(directoryInfo.FullName);
            destDirName = System.IO.Path.Combine(parentDirectory.FullName, destDirName);

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName)) {
                Directory.CreateDirectory(destDirName);
            }
            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = directoryInfo.GetFiles();

            foreach (FileInfo file in files) {
                string tempPath = System.IO.Path.Combine(destDirName, file.Name);

                if (!File.Exists(tempPath)) {
                    file.CopyTo(tempPath, false);
                }
            }
            // If copying subdirectories, copy them and their contents to new location using recursive  function.
            if (isCopySubDirs) {
                foreach (DirectoryInfo item in directories) {
                    string tempPath = System.IO.Path.Combine(destDirName, item.Name);
                    DirectoryCopy(item.FullName, tempPath, isCopySubDirs);
                }
            }
        }

        public static void Disconnect()
        {
            if (IsConnected) {
                Thread th = new Thread(OBSConnection.Disconnect);
                th.Start();
            }
        }

        public static void ExtractZip(string file, string path)
        {
            ZipFile.ExtractToDirectory(file, path);
        }

        public static string GetPathFromOBSExecutable(string execLocation)
        {
            return Path.GetFullPath(Path.Combine(execLocation, "..", "..", ".."));
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

        public static void PatchOBS(string obsPath)
        {
            //This method will take a file named obs-ws.zip from the current dir
            //and fix obs to allow the plugin to work
            if (File.Exists(obswszip)) {
                var finalExtractionPath = Path.GetFullPath(Path.Combine(obsPath, "..", ".."));
            }
        }

        public static bool PrepareOBSIntegration()
        {
            var obs32List = Process.GetProcessesByName("obs32");
            var obs64List = Process.GetProcessesByName("obs64");
            if (obs32List.Length == 0 && obs64List.Length == 0) {
                //No OBS found. Cancel operation.
                return false;
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
                if (File.Exists(obsGlobalFilePath) && !File.ReadAllText(obsGlobalFilePath).Contains("[WebsocketAPI]")) {
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

                        var newProcess = new ProcessStartInfo(Process.GetCurrentProcess().MainModule.FileName, "/armobs")
                        {
                            Verb = "runas"
                        };
                        Process process = Process.Start(newProcess);

                        process.WaitForExit();
                    }
                } else {
                    ConnectToOBS();
                    return true;
                }
            }

            return true;
        }

        private static void ConnectToServer()
        {
            OBSConnection = new OBSWebsocket();
            OBSConnection.Connect("ws://localhost:4444", "");
        }

        #endregion

        #region OBS METHODS

        public static void SwitchScene(string scene)
        {
            Thread th = new Thread(() => {
                List<OBSScene> scenes = OBSConnection.ListScenes();
                foreach (var s in scenes) {
                    if (s.Name == scene) {
                        OBSConnection.SetCurrentScene(scene);
                    }
                }
            });
            th.Start();
        }


        public static List<string> GetScenes()
        {
            ConnectToOBS();
            return OBSConnection.ListScenes().Select(c => c.Name).ToList();
        }


        #endregion
    }
}