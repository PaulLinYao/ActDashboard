using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActDashboard
{
    public class Settings_Repo
    {
        private const string strTag = "[act]";
        public string? Workflow { get; set; }
        public string? WorkflowFile { get; set; }

        private string? strSettingsFilePath;

        public void Load(string strPath, string strFileName)
        {
            // Default values
            Workflow = "";
            WorkflowFile = "";

            // Create folder where settings goes if it does not already exist.
            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);

            strSettingsFilePath = $"{strPath}\\{strFileName}";
            if (File.Exists(strSettingsFilePath))
            {
                string[] astrLines = File.ReadAllLines(strSettingsFilePath);
                if (astrLines[0].Contains(strTag))
                {
                    foreach (string strLine in astrLines)
                    {
                        string [] astrParts = strLine.Split(new char[] {'='});
                        if (astrParts.Length == 2)
                        {
                            string strLeft = astrParts[0].ToLower().Trim();
                            string strRight = astrParts[1].ToLower().Trim();
                            switch (strLeft)
                            {
                                case "workflow":
                                    Workflow = strRight;
                                    break;
                                case "file":
                                    WorkflowFile = strRight;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public void Save()
        {
            if (!string.IsNullOrEmpty(strSettingsFilePath))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(strTag);
                sb.AppendLine($"workflow={Workflow}");
                sb.AppendLine($"file={WorkflowFile}");
                try
                {
                    File.WriteAllText(strSettingsFilePath, sb.ToString());
                }
                catch { }
            }
        }
    }
}
