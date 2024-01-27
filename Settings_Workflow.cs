using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActDashboard
{
    public class Settings_Workflow
    {
        private const string strTag = "[workflow]";
        private const string strTag2 = "[notes]";
        private const string strTag3 = "[duration]";
        private const string strTag4 = "[date]";
        public string? Containers {  get; set; }
        public string? Networks { get; set; }
        public string? RunOnGitBranch {  get; set; }
        public bool KillObjectsBeforeRunning { get; set; }

        private string? strSettingsFilePath;

        public Dictionary<string,string> dictNotes = new Dictionary<string,string>();
        public Dictionary<string, string> dictDate = new Dictionary<string, string>();
        public Dictionary<string, string> dictDuration = new Dictionary<string, string>();
        

        public void Load(string strPath, string strFileName)
        {
            // Default Values
            Containers = "";
            Networks = "";
            RunOnGitBranch = "";
            KillObjectsBeforeRunning = true;

            // Empty Dictionaries
            dictNotes.Clear();
            dictDate.Clear();
            dictDuration.Clear();

            // Create folder where settings goes if it does not already exist.
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);

            strSettingsFilePath = $"{strPath}\\{strFileName}";

            if (File.Exists(strSettingsFilePath))
            {
                string[] astrLines = File.ReadAllLines(strSettingsFilePath);
                bool bNotes = false;
                bool bDuration = false;
                if (astrLines[0].Contains(strTag))
                {
                    foreach (string strLine in astrLines)
                    {
                        if (strLine.Contains(strTag2))
                        {
                            bNotes = true;
                            continue;
                        }

                        if (strLine.Contains(strTag3))
                        {
                            bDuration = true;
                            bNotes = false;
                            continue;
                        }

                        string[] astrParts = strLine.Split(new char[] { '=' });
                        if (astrParts.Length == 2)
                        {
                            string strLeft = astrParts[0].Trim();
                            string strRight = astrParts[1].Trim();

                            if (bNotes)
                            {
                                if (!dictNotes.ContainsKey(strLeft))
                                    dictNotes.Add(strLeft, strRight);
                            }
                            else if (bDuration)
                            {
                                if (!dictDuration.ContainsKey(strLeft))
                                    dictDuration.Add(strLeft, strRight);
                            }
                            else
                            {
                                switch (strLeft)
                                {
                                    case "containers":
                                        Containers = strRight;
                                        break;
                                    case "networks":
                                        Networks = strRight;
                                        break;
                                    case "runongitbranch":
                                        RunOnGitBranch = strRight;
                                        break;
                                    case "killobjects":
                                        KillObjectsBeforeRunning = strRight.ToLower() == "true";
                                        break;
                                }
                            }
                        }
                    }

                }

                // Added this
                for (int iNextValue = 1; iNextValue < 1000; iNextValue++)
                {
                    string strInputName = $"{iNextValue:0000}_Input.yml";
                    string strOutputName = $"{iNextValue:0000}_Output.txt";

                    string strInputFilePath = $"{strPath}\\{strInputName}";
                    string strOutputFilePath = $"{strPath}\\{strOutputName}";
                    if (File.Exists(strInputFilePath) && File.Exists(strOutputFilePath)) 
                    {
                        dictDate.Add(strInputName, File.GetCreationTime(strInputFilePath).ToString());
                    }
                }
            }
            else
            {
                Save();
            }

        }

        public void Save()
        {
            if (!string.IsNullOrEmpty(strSettingsFilePath))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(strTag);
                sb.AppendLine($"containers={Containers}");
                sb.AppendLine($"networks={Networks}");
                sb.AppendLine($"runongitbranch={RunOnGitBranch}");
                sb.AppendLine($"killobjects={KillObjectsBeforeRunning.ToString().ToLower()}");

                // Store notes.
                sb.AppendLine(strTag2);
                foreach (KeyValuePair<string,string> n in dictNotes)
                {
                    sb.AppendLine($"{n.Key}={n.Value}");
                }

                // Store durations.
                sb.AppendLine(strTag3);
                foreach (KeyValuePair<string, string> n in dictDuration)
                {
                    sb.AppendLine($"{n.Key}={n.Value}");
                }
                
                // sb.AppendLine(strTag4);
                // foreach (KeyValuePair<string, string> n in dictDate)
                // {
                //     sb.AppendLine($"{n.Key}={n.Value}");
                // }

                try
                {
                    File.WriteAllText(strSettingsFilePath, sb.ToString());
                }
                catch { }
            }
        }

    }
}
