using ActDashboard;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing.Configuration;
using System.Text;

public class utils
{
    public static Process? StartProcess(string strPath, string strArg)
    {
        Process? p = null;
        try
        {
            p = System.Diagnostics.Process.Start(strPath, strArg);
            System.Threading.Thread.Sleep(100);
        }
        catch
        {
        }

        return p;
    }

    public static bool DockerLogin(bool bDisplay, TextBox textOutput)
    {
        var xx = RunCommand("docker.exe", "login");
        string strOutput = xx.Item1;
        string strError = xx.Item2;

        if (bDisplay)
        {
            if (!string.IsNullOrEmpty(strOutput))
                WriteToTextBox(textOutput, $"{strOutput}");
            if (!string.IsNullOrEmpty(strError))
                WriteToTextBox(textOutput, $"Error: {strError}");
        }

        if (strOutput == "Need-Docker-Credentials")
        {
            Form_Login fl = new Form_Login();
            if (fl.ShowDialog() == DialogResult.OK)
            {
                var yy = RunCommand("docker.exe", $"login --username {fl.DockerID} --password {fl.Password}");
                strOutput = yy.Item1;
                strError = yy.Item2;
            }
        }


        if (strOutput.Contains("Login Succeeded"))
            return true;
        else
            return false;
    }

    public static bool DockerKillContainer(string strContainer)
    {
        bool bSuccess = true;
        var xx = RunCommand("docker.exe", $"kill {strContainer}");
        string strOutput = xx.Item1;
        string strError = xx.Item2;

        if (strError.Length == 0)
        {
            var yy = RunCommand("docker.exe", $"rm {strContainer}");
            string strOutput2 = yy.Item1;
            string strError2 = yy.Item2;

            bSuccess = (strError2.Length == 0);
        }
        else
        {
            bSuccess = false;
        }

        return bSuccess;
    }

    public static bool DockerRemoveNetwork(string strNetwork)
    {
        bool bSuccess = true;
        var xx = RunCommand("docker.exe", $"network remove --force  {strNetwork}");
        string strOutput = xx.Item1;
        string strError = xx.Item2;

        bSuccess = (strError.Length == 0);

        return bSuccess;
    }

    public static bool IsDockerStarted(bool bDisplaySuccess, bool bDisplayFailure, TextBox textOutput)
    {
        var xx = RunCommand("docker.exe", "ps");
        string strOutput = xx.Item1;
        string strError = xx.Item2;

        if (bDisplaySuccess)
        {
            if (!string.IsNullOrEmpty(strOutput))
                WriteToTextBox(textOutput, $"{strOutput}");
        }
        if (bDisplayFailure)
        {
            if (!string.IsNullOrEmpty(strError))
                WriteToTextBox(textOutput, $"Error: {strError}");
        }


        if (strError.Contains("error during connect"))
            return false;
        else
            return true;
    }


    public static void WriteToTextBox(TextBox tb, string strLine)
    {
        if (!string.IsNullOrWhiteSpace(strLine))
        {
            if (tb.Text.Length > 0)
                tb.AppendText($"\r\n");

            tb.AppendText($"{strLine}");
        }
    }


    public static (string,string) RunCommand(string strCommand, string strArgs, string strWorkingDir = null, string? strOutPath = null)
    {
        System.Diagnostics.Debug.WriteLine($"RunCommand: {strCommand} + {strArgs}");
        bool bLogin = (strArgs == "login");

        Process process = new Process();
        process.StartInfo.FileName = strCommand;
        process.StartInfo.Arguments = strArgs;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.WorkingDirectory = strWorkingDir;
        process.StartInfo.StandardOutputEncoding = Encoding.UTF8;
        process.Start();
        string strOutput = String.Empty;
        string strError = String.Empty;

        if (bLogin)
        {
            RunCommand_LoginHandler(bLogin, process, ref strOutput, ref strError);
        }
        else
        {
            strOutput = process.StandardOutput.ReadToEnd();
            strError = process.StandardError.ReadToEnd();
        }

        if (strOutPath != null)
        {
            string strAllOutput = strOutput;
            if (strError != null)
                strAllOutput += "\r\n" + strError;

            File.WriteAllText(strOutPath, strAllOutput);
        }

        process.WaitForExit();

        //if (strError != null && strError.Length > 0)
        //    System.Diagnostics.Debug.WriteLine(strError);

        return (strOutput, strError);
    }

    private static void RunCommand_LoginHandler(bool bLogin, Process process, ref string strOutput, ref string strError)
    {
        bool bContinue = true;
        bool bFetchCredentials = false;
        while (bContinue)
        {
            try
            {
                var xxx = ReadOutput(process.StandardOutput, process.StandardError, bLogin);
                strOutput = xxx.Item1;
                strError = xxx.Item2;
                bContinue = false;
            }
            catch (Exception e)
            {
                if (e.InnerException.Message == "Need-Docker-Credentials")
                    bFetchCredentials = true;
            }

            if (bFetchCredentials)
            {
                strOutput = "Need-Docker-Credentials";
                process.Kill();
                bContinue = false;
            }

        }
    }

    public static (string, string) ReadOutput(StreamReader srStandardOutput, StreamReader srStandardError, bool bLogin)
    {
        System.Diagnostics.Debug.WriteLine($"ReadOutput");
        ArrayList alStandardOutput = ReadOutputAsync(srStandardOutput, false, bLogin).Result;
        ArrayList alStandardError = ReadOutputAsync(srStandardError, true, bLogin).Result;

        string[] astrStandardOutput = (string[])alStandardOutput.ToArray(typeof(string));
        string[] astrStandardError = (string[])alStandardError.ToArray(typeof(string));

        string strStandardOutput = ConvertToSingleString(astrStandardOutput);
        string strStandardError = ConvertToSingleString(astrStandardError);

        return (strStandardOutput, strStandardError);
    }

    public static string ConvertToSingleString(string[] astrInput)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string str in astrInput)
        {
            sb.AppendLine(str);
        }

        return sb.ToString();
    }

    public static async Task<ArrayList> ReadOutputAsync(StreamReader sr, bool bError, bool bLogin)
    {
        System.Diagnostics.Debug.WriteLine($"ReadOutputAsynch");
        bool bContinue = true;
        string strType = (bError) ? "Error" : "Output";
        ArrayList alReturn = new ArrayList();
        while (sr.EndOfStream == false && bContinue)
        {
            var xx = sr.Peek().ToString();
            System.Diagnostics.Debug.WriteLine($"ReadOutputAsynch: Peek={xx}");
            string? strLine = await sr.ReadLineAsync();
            System.Diagnostics.Debug.WriteLine($"ReadOutputAsynch: strLine={strLine}");
            if (bLogin)
            {
                if (strLine.StartsWith("Log in with your Docker ID or email address"))
                {
                    throw new Exception("Need-Docker-Credentials");
                }
            }

            alReturn.Add(strLine);
        }

        return alReturn;
    }

    public static void Message_Error(string strCaption, string strMessage)
    {
        MessageBox.Show(strMessage, strCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    public static int GetNextValue(string strRootPath, string strActionName, bool bCreateFolders)
    {
        string strActFolder = $"{strRootPath}\\act";
        strActionName = strActionName.Replace(".yml", "").Replace(".YML", "");
        string strOutputFolder = $"{strActFolder}\\{strActionName}";
        if (bCreateFolders)
        {
            if (!Directory.Exists(strActFolder))
                Directory.CreateDirectory(strActFolder);

            if (!Directory.Exists(strOutputFolder))
                Directory.CreateDirectory(strOutputFolder);
        }

        string[] astrActions = Directory.GetFiles(strOutputFolder, "*.yml");
        int iMaxValue = 0;
        foreach (string s in astrActions)
        {
            string strFileName = Path.GetFileName(s);
            string[] astrParts = strFileName.Split(new char[] {'_'});
            if (astrParts.Length > 1)
            {
                int iValue = -1;
                if (int.TryParse(astrParts[0], out iValue))
                {
                    iMaxValue = (iValue > iMaxValue) ? iValue : iMaxValue;
                }
            }
        }

        return iMaxValue+1;
    }

    public static string ConvertToHumanTime(long milliseconds)
    {
        // Only display milliseconds when the duration is greater than 10 seconds.
        bool bDisplayMilliseconds = (milliseconds < 10000);

        StringBuilder sb = new StringBuilder();

        // Hours.
        if (milliseconds > 3600000)
        {
            long hours = milliseconds / 360000;
            sb.Append($"{hours} hrs ");
            milliseconds = milliseconds % 360000;
        }

        // Minutes.
        if (milliseconds > 60000)
        {
            long minutes = milliseconds / 60000;
            sb.Append($"{minutes} min ");
            milliseconds = milliseconds % 60000;
        }

        // Seconds and/or milliseconds
        long seconds = milliseconds / 1000;
        milliseconds = milliseconds % 1000;

        if (bDisplayMilliseconds)
        {
            sb.Append($"{seconds}.{milliseconds} sec");
        }
        else
        {
            sb.Append($"{seconds} sec");
        }

        return sb.ToString();
    }

}
