using ActDashboard;
using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Xml.Linq;
using static System.Windows.Forms.LinkLabel;

#pragma warning disable CS8600, CS8602, CS8604, CS8605
namespace WinFormsApp1
{
    public partial class Form_Main : Form
    {
        #region Constants
        private const int datacolumn_input = 0;
        private const int datacolumn_output = 1;
        private const int datacolumn_duration = 2;
        private const int datacolumn_date = 3;
        private const int datacolumn_notes = 4;


        #endregion
        #region Fields and Properties
        bool bIgnoreChangeNotification = false;
        public Settings_Repo reposettings = new Settings_Repo();
        public Settings_Workflow workflowsettings = new Settings_Workflow();
        private int iSelectedRow = -1;

        private int []blockCount ;

        public string? strPathToRepos
        {
            get
            {
                return textFolder.Text;
            }
            set
            {
                textFolder.Text = value;
            }
        }

        public string? strCurrentWorkflowFile
        { get; set; }

        // Like this: "C:\\CESMII.github\\Marketplace"
        public string? strPathToWorkflows
        {
            get
            {
                return $"{strPathToRepos}\\.github\\workflows";
            }
        }

        // Like this: "C:\CESMII.github\Marketplace\act"
        public string? strPathToAct
        {
            get
            {
                if (string.IsNullOrEmpty(strPathToRepos))
                    return "";
                return $"{strPathToRepos}\\act";
            }
        }

        public string? strPathToOutput
        {
            get
            {
                if (string.IsNullOrEmpty(strCurrentWorkflowFile))
                    return "";
                string strActionName = strCurrentWorkflowFile.Replace(".yml", "").Replace(".YML", "");
                return $"{strPathToRepos}\\act\\{strActionName}";
            }
        }

        #endregion
        private void listSteps_Click(object sender, EventArgs e)
        {
            int iItem = listSteps.SelectedIndex;
            // Scroll the file to the selected line.
            if (iItem > -1)
            {
                int iFirstLine = 0;
                for (int i = 0; i < iItem; i++)
                {
                    iFirstLine += blockCount[i];
                }
                int iLastLine = iFirstLine + blockCount[iItem] - 1;
                int iSelectLine = rtDisplay.GetFirstCharIndexFromLine(iLastLine);
                rtDisplay.Select(iSelectLine, 0);
                rtDisplay.ScrollToCaret();
            }
        }

        public Form_Main()
        {
            InitializeComponent();
            listSteps.Click += new EventHandler(listSteps_Click);
            

            // At start, open the last repo we had opened.
            string strTemp = Settings1.Default.FolderPath;
            if (!string.IsNullOrEmpty(strTemp) && strTemp != "None")
            {
                if (!Directory.Exists(strTemp))
                {
                    string strMessage = $"Repo Folder {strTemp} not found. Please open a repo folder.";
                    utils.Message_Error(this.Text, strMessage);
                    return;
                }

                // Load last workflow in this repo
                string strTempPathToAct = $"{strTemp}\\act";
                reposettings.Load(strTempPathToAct, "Repo.ini");

                bIgnoreChangeNotification = true;
                strPathToRepos = strTemp;
                bIgnoreChangeNotification = false;
            }

            cmdRefresh_LinkClicked(this, new LinkLabelLinkClickedEventArgs(new Link()));
        }


        private void cmdRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string strFolder = strPathToRepos; // textFolder.Text;
            if (!Directory.Exists(strFolder))
            {
                string strMessage = $"Folder {strFolder} not found.";
                utils.Message_Error(this.Text, strMessage);
                return;
            }

            if (!Directory.Exists(strPathToOutput))
            {
                // MessageBox.Show($"Output Folder {strPathToOutput} not found.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get list of files for the current action.
            string[] astrYml = Directory.GetFiles(strPathToOutput, "*.yml");
            List<string> listYml = new List<string>(astrYml);
            listYml.Sort();

            gridFiles.Rows.Clear();

            foreach (string strFile in listYml)
            {
                string strInput = Path.GetFileName(strFile);
                string strOutput = strInput.Replace("Input.yml", "Output.txt");

                string strOutputPath = $"{strPathToOutput}\\{strOutput}";
                if (!File.Exists(strOutputPath))
                    strOutput = "";


                string strNote = "";
                if (workflowsettings.dictNotes.ContainsKey(strInput))
                    strNote = workflowsettings.dictNotes[strInput];

                string strDuration = "";
                if (workflowsettings.dictDuration.ContainsKey(strInput))
                    strDuration = workflowsettings.dictDuration[strInput];
                
                string strDate = "";
                if (workflowsettings.dictDate.ContainsKey(strInput))
                    strDate = workflowsettings.dictDate[strInput];

                gridFiles.Rows.Add(strInput, strOutput, strDuration, strDate, strNote);
            }

            int iRow = OpenFilesForLastCompleteRow();
            if (iRow > -1)
                iSelectedRow = iRow;
        }

        private void cmdStartStop_Click(object sender, EventArgs e)
        {
            if (cmdStartStop.Text == "Start")
            {
                if (!utils.IsDockerStarted(false, true, textConsole))
                {
                    utils.Message_Error(this.Text, "Docker Daemon not found. Please start Docker Desktop.");
                    return;
                }

                if (!utils.DockerLogin(true, textConsole))
                {
                    utils.Message_Error(this.Text, "Error: Docker Login Failed.");
                    return;
                }

                if (workflowsettings.KillObjectsBeforeRunning)
                {
                    string strContainers = workflowsettings.Containers;
                    string[] astr = strContainers.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in astr)
                    {
                        bool bKilled = utils.DockerKillContainer(str);
                        string strResult = (bKilled) ? "Success." : "Failed";
                        utils.WriteToTextBox(textConsole, $"Removing container {str}. Result: {strResult}.");
                    }

                    string strNetworks = workflowsettings.Networks;
                    string[] astrNetworks = strNetworks.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string str in astrNetworks)
                    {
                        bool bRemoved = utils.DockerRemoveNetwork(str);
                        string strResult = (bRemoved) ? "Success." : "Failed";
                        utils.WriteToTextBox(textConsole, $"Removing network {str}. Result: {strResult}.");
                    }
                }

                string strRootPath = strPathToRepos; // textFolder.Text;
                string strActionNameFile = comboWorkflows.SelectedItem.ToString();
                string strGithubActionPath = $"{strRootPath}\\.github\\workflows\\{strActionNameFile}";
                string strActionName = strActionNameFile.Replace(".yml", "").Replace(".YML", "");
                int iNextValue = utils.GetNextValue(strRootPath, strActionName, true);

                if (!File.Exists(strGithubActionPath))
                {
                    utils.Message_Error(this.Text, $"Error: Unable to find input file {strGithubActionPath}");
                    return;
                }

                string strInputName = $"{iNextValue:0000}_Input.yml";
                string strOutputName = $"{iNextValue:0000}_Output.txt";

                string strInputPath = $"{strRootPath}\\act\\{strActionName}\\{strInputName}";
                string strOutputPath = $"{strRootPath}\\act\\{strActionName}\\{strOutputName}";

                if (File.Exists(strInputPath))
                {
                    utils.Message_Error(this.Text, $"Error: Input archive file already exists: {strInputPath}");
                    return;
                }

                if (File.Exists(strOutputPath))
                {
                    utils.Message_Error(this.Text, $"Error: Output archive file already exists: {strOutputPath}");
                    return;
                }

                bool bSuccess = false;
                string strException = "";
                try
                {
                    File.Copy(strGithubActionPath, strInputPath);
                    bSuccess = true;
                }
                catch (Exception ex)
                {
                    strException = ex.Message;
                }

                if (bSuccess)
                {
                    utils.WriteToTextBox(textConsole, $"Copyied from: {strGithubActionPath}.");
                    utils.WriteToTextBox(textConsole, $"Copyied   to: {strInputPath}.");
                }
                else
                {
                    utils.WriteToTextBox(textConsole, $"Error: Unable to copy YML file: {strException}.");
                }

                // Run workflow by creating process running act.exe
                string strDefaultBranch = (workflowsettings.RunOnGitBranch.Length == 0) ? String.Empty : $"--defaultbranch {workflowsettings.RunOnGitBranch}";
                string strArgs = $"-W {strInputPath} {strDefaultBranch} ";
                utils.WriteToTextBox(textConsole, $"Starting {strActionNameFile} in Act.exe.");
                Stopwatch sw = new Stopwatch();
                sw.Start();
                utils.RunCommand("act.exe", strArgs, strRootPath, strOutputPath);
                sw.Stop();

                // Calculate and save duration details
                string strDuration = utils.ConvertToHumanTime(sw.ElapsedMilliseconds);
                workflowsettings.dictDuration.Add(strInputName, strDuration);
                workflowsettings.Save();

                utils.WriteToTextBox(textConsole, $"Ending {strActionNameFile} in Act.exe in {strDuration}.");

                // Show user the results and scroll into view
                int iNewRow = gridFiles.Rows.Add(strInputName, strOutputName, strDuration, "");
                gridFiles.CurrentCell = gridFiles.Rows[iNewRow].Cells[0];
                DisplayDetailsForRow(iNewRow);
            }
        }




        private void gridFiles_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int iRow = e.RowIndex;
            DisplayDetailsForRow(iRow);
        }

        private int OpenFilesForLastCompleteRow()
        {
            int iRow = -1;

            int iLastRow = gridFiles.Rows.Count - 1;

            if (iLastRow >= 0)
            {
                while (iRow == -1 && iLastRow != -1)
                {
                    object oInput = gridFiles.Rows[iLastRow].Cells[datacolumn_input].Value;
                    object oOutput = gridFiles.Rows[iLastRow].Cells[datacolumn_output].Value;

                    if (oInput != null && oOutput != null)
                    {
                        string strOutput = oOutput.ToString();
                        if (strOutput.Length > 0)
                            iRow = iLastRow;
                    }

                    iLastRow--;
                }

                if (iRow >= 0)
                {
                    gridFiles.Rows[iRow].Selected = true;
                    DisplayDetailsForRow(iRow);
                }
            }
            return iRow;
        }

        private void DisplayDetailsForRow(int iRow)
        {
            listSteps.Items.Clear();
            string strFolder = strPathToRepos; // textFolder.Text;
            string strInput = gridFiles.Rows[iRow].Cells[datacolumn_input].Value.ToString();
            string strOutput = gridFiles.Rows[iRow].Cells[datacolumn_output].Value.ToString();

            string strInputPath = $"{strPathToOutput}\\{strInput}";
            string strOutputPath = $"{strPathToOutput}\\{strOutput}";

            // Reset display of file names.
            textInputFile.Text = "";
            textOutputFile.Text = "";

            // If the File does not exist, raise an error
            if (!File.Exists(strInputPath))
            {
                MessageBox.Show($"Input File Not Found.\r\n\r\nFile {strInput} not found.\r\n\r\nExpected in folder {strPathToOutput}.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!File.Exists(strOutputPath))
            {
                MessageBox.Show($"Output File Not Found.\r\n\r\nFile {strOutput} not found.\r\n\r\nExpected in folder {strPathToOutput}.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Set display of file names.
            textInputFile.Text = strInput;
            textOutputFile.Text = strOutput;

            // Read both input and output files into memory
            string[] astrInput = File.ReadAllLines(strInputPath);
            string[] astrOutput = File.ReadAllLines(strOutputPath);

            InputFile input = Parser.ParseInputFile(astrInput);
            OutputFile output = Parser.ParseOutputFile(astrOutput);
            InputOutputFileLines[] lines = Parser.GetInputBlocks(input, astrInput);
            bool bOk = ValidateInputFileLines(input, astrInput);
            if (bOk)
            {

                Parser.MatchToOutputBlocks(astrInput, output, astrOutput, lines);
                var(sb, lineCounts) = Parser.MergeFiles(input, astrInput, output, astrOutput, lines);

                rtDisplay.Text = sb.ToString();

                blockCount = new int[lines.Length];
                // Add line names to the list
                // foreach (InputOutputFileLines item in lines) {
                //     listSteps.Items.Add(item.strInputName);
                // }
                for (int i = 0; i < lines.Length; i++)
                {
                    listSteps.Items.Add(lines[i].strInputName);
                    // blockCount[i] = lineCounts[i];
                    blockCount[i] = lineCounts[i] + lines[i].iInputLast - lines[i].iInputFirst + 1 + lines[i].iOutputLast - lines[i].iOutputFirst + 1;
                }

                // How to display color!!
                //rtDisplay.Text = "";
                //string strMergedText = sb.ToString().Replace("\r", "");
                //string[] astrLines = strMergedText.Split(new char[] { '\r', '\n' });
                //foreach (string strLine in astrLines)
                //{
                //    rtDisplay.AppendText($"{strLine}\r", Color.FromArgb(87, 166, 74));
                //    //rtDisplay.AppendText($"{strLine}\r", Color.FromArgb(86, 156, 214));
                //}

            }
        }

        public bool ValidateInputFileLines(InputFile input, string[] astrInput)
        {
            // To Do
            // Check whether all step names are unique.
            // Warn user and ask if they want to continue.
            return true;
        }



        private void checkWordWrap_CheckedChanged(object sender, EventArgs e)
        {
            rtDisplay.WordWrap = checkWordWrap.Checked;
        }



        private void Form1_Shown(object sender, EventArgs e)
        {
            if (iSelectedRow != -1)
            {
                gridFiles.CurrentCell = gridFiles.Rows[iSelectedRow].Cells[0];
                gridFiles.Rows[iSelectedRow].Selected = true;
            }
        }

        private void cmdSetFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.SelectedPath = textFolder.Text;

            // Set the help text description for the FolderBrowserDialog.
            dialog.Description = "Select the directory with Github Action input and output files.";

            DialogResult dr = dialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textFolder.Text = dialog.SelectedPath;
                OnReposChangeInitWorkflowList();
                cmdRefresh_LinkClicked(this, new LinkLabelLinkClickedEventArgs(new Link()));
            }
        }

        private void textFolder_TextChanged(object sender, EventArgs e)
        {
            // Only init when user enters a valid folder name.
            if (Directory.Exists(strPathToRepos) && Directory.Exists(strPathToWorkflows))
            {
                OnReposChangeInitWorkflowList();

                // Save the path when it changes.
                Settings1.Default.FolderPath = textFolder.Text;

                if (!bIgnoreChangeNotification)  // No save needed during initialization.
                    Settings1.Default.Save();
            }
        }

        private void OnReposChangeInitWorkflowList()
        {
            // Initialize the list of actions.
            if (Directory.Exists(strPathToRepos) && Directory.Exists(strPathToWorkflows))
            {
                comboWorkflows.SelectedIndex = -1;
                comboWorkflows.Items.Clear();

                // Get last action we used
                string strLastWorkflowFile = reposettings.WorkflowFile;

                string[] astrActions = Directory.GetFiles(strPathToWorkflows, "*.yml");
                for (int i = 0; i < astrActions.Length; i++)
                {
                    string strName = Path.GetFileName(astrActions[i]);
                    int iCurrentItem = comboWorkflows.Items.Add(strName);

                    if (!string.IsNullOrEmpty(strLastWorkflowFile) && strLastWorkflowFile != "None")
                    {
                        if (strName == strLastWorkflowFile)
                        {
                            bIgnoreChangeNotification = true;
                            comboWorkflows.SelectedIndex = iCurrentItem;
                            bIgnoreChangeNotification = false;
                        }
                    }
                }
            }
        }

        private void comboWorkflows_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelected = comboWorkflows.SelectedIndex;
            if (iSelected > -1)
            {
                strCurrentWorkflowFile = comboWorkflows.Items[iSelected].ToString();
                reposettings.WorkflowFile = strCurrentWorkflowFile;
                reposettings.Workflow = strCurrentWorkflowFile.Replace(".yml", "").Replace(".YML", "");

                bIgnoreChangeNotification = true;
                workflowsettings.Load(strPathToOutput, "workflow.ini");
                bIgnoreChangeNotification = false;

                if (!bIgnoreChangeNotification)  // No save needed during initialization.
                    reposettings.Save();
            }

            cmdRefresh_LinkClicked(this, new LinkLabelLinkClickedEventArgs(new Link()));
        }

        private void cmdWorkflowSettings_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form_Settings form = new Form_Settings();
            form.strWorkflowName = comboWorkflows.SelectedItem.ToString();
            form.strContainers = workflowsettings.Containers;
            form.strNetworks = workflowsettings.Networks;
            form.bKillContainers = workflowsettings.KillObjectsBeforeRunning;
            form.strGitBranch = workflowsettings.RunOnGitBranch;

            if (form.ShowDialog() == DialogResult.OK)
            {
                workflowsettings.Containers = form.strContainers;
                workflowsettings.Networks = form.strNetworks;
                workflowsettings.KillObjectsBeforeRunning = form.bKillContainers;
                workflowsettings.RunOnGitBranch = form.strGitBranch;
                workflowsettings.Save();
            }
        }

        private void cmdClearConsole_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textConsole.Text = String.Empty;
        }


        private void gridFiles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == datacolumn_notes)
            {
                System.Diagnostics.Debug.WriteLine("Cell-Value-Changed\r\n");
                int iRow = e.RowIndex;
                int iCol = e.ColumnIndex;

                if (!bIgnoreChangeNotification)
                {
                    if (iRow >= 0)
                    {
                        DataGridViewRow datarow = gridFiles.Rows[iRow];
                        string strKey = (datarow.Cells[datacolumn_input].Value == null) ? "" : datarow.Cells[datacolumn_input].Value.ToString();
                        string strNotes = (datarow.Cells[datacolumn_notes].Value == null) ? "" : datarow.Cells[datacolumn_notes].Value.ToString();
                        if (strKey.Length > 0)
                        {
                            if (workflowsettings.dictNotes.ContainsKey(strKey))
                            {
                                
                                workflowsettings.dictNotes[strKey] = strNotes;
                            }
                            else
                            {
                                workflowsettings.dictNotes.Add(strKey, strNotes);
                            }

                            workflowsettings.Save();
                        }
                    }
                }
            }
        }
    }

    public class StringTrigger
    {
        public string? strTrigger;
        public int iFirstLine;
        public int iLastLine;
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }

}






//public InputFile ParseInputFile(string[] astrInput)
//{
//    InputFile ifile = new InputFile();
//    ifile.cInputLines = astrInput.Length;
//    ifile.iLastLine = astrInput.Length - 1;

//    // Parsing rules changes after we see 'jobs:'
//    bool bInJobPreamble = true;
//    int cInputBlocks = 0;
//    for (int iLine = 0; iLine < astrInput.Length; iLine++)
//    {
//        string strLine = astrInput[iLine];

//        // Clear out comments
//        string[] astrComments = strLine.Split(new char[] { '#' });
//        if (astrComments.Length > 1)
//        {
//            strLine = astrComments[0];
//        }

//        // Ignore empty lines.
//        strLine = strLine.Trim();
//        if (string.IsNullOrEmpty(strLine))
//            continue;

//        string[] astrProcessing = strLine.Split(new char[] { ':' });
//        if (astrProcessing.Length > 1)
//        {
//            string strLeft = astrProcessing[0];
//            string strRight = astrProcessing[1];
//            if (bInJobPreamble)
//            {
//                switch (strLeft)
//                {
//                    case "name":
//                        ifile.iName = iLine;
//                        ifile.strActionName = strRight;
//                        break;
//                    case "env":
//                        ifile.iEnv = iLine;
//                        break;
//                    case "on":
//                        ifile.iOn = iLine;
//                        break;
//                    case "jobs":
//                        ifile.iJobs = iLine;
//                        string strTemp = GetLeftOfColon(astrInput[iLine + 1]);
//                        if (!string.IsNullOrEmpty(strTemp))
//                        {
//                            iLine++;
//                            ifile.strJobName = strTemp;
//                        }
//                        break;
//                    case "steps":
//                        ifile.iSteps = iLine;
//                        bInJobPreamble = false;
//                        break;
//                }
//            }
//            else
//            {
//                if (strLeft.Contains("- name"))
//                    cInputBlocks++;
//            }
//        }
//    } // loop on lines

//    ifile.cInputBlocks = cInputBlocks;
//    return ifile;
//}

//public string GetLeftOfColon(string strInput)
//{
//    string[] astrProcessing = strInput.Split(new char[] { ':' });
//    if (astrProcessing.Length > 1)
//    {
//        string strOutput = astrProcessing[0].Trim();
//        return strOutput;
//    }

//    return "";
//}


//public OutputFile ParseOutputFile(string[] astrOutput)
//{
//    OutputFile oFile = new OutputFile();
//    oFile.cOutputLines = astrOutput.Length;
//    oFile.iLastLine = astrOutput.Length - 1;

//    // Set iFileHeaderLastLine - Last line in header block of file.
//    oFile.iFileHeaderLastLine = astrOutput.Length - 1;                // Default to last line in file

//    for (int iLine = 0; iLine < astrOutput.Length; iLine++)           // But then scan for other lines...
//    {
//        if (astrOutput[iLine].Contains('⭐'))
//        {
//            oFile.iFileHeaderLastLine = iLine - 1;
//            break;
//        }
//    }

//    // Do we need to handle this?
//    // The character / icon for 'skipping unsupported platforms': '🚧'

//    // Set bJobSuccess
//    string strLast = astrOutput[oFile.iLastLine];
//    if (strLast.Contains("Job failed"))
//        oFile.bJobSuccess = false;
//    else
//        oFile.bJobSuccess = true;

//    return oFile;
//}

//public InputOutputFileLines[] GetInputBlocks(InputFile input, string[] astrInput)
//{
//    // Allocate space for expected input blocks.
//    InputOutputFileLines[] atlBlocks = new InputOutputFileLines[input.cInputBlocks];

//    int iBlock = -1;
//    int cBlocksFound = 0;

//    // Loop through lines from start of job to end of file.
//    for (int iLine = input.iSteps; iLine < input.cInputLines; iLine++)
//    {
//        string strLine = astrInput[iLine];

//        // Clear out comments
//        string[] astrComments = strLine.Split(new char[] { '#' });
//        if (astrComments.Length > 1)
//        {
//            strLine = astrComments[0];
//        }

//        // Ignore empty lines.
//        strLine = strLine.Trim();
//        if (string.IsNullOrEmpty(strLine))
//            continue;

//        string[] astrProcessing = strLine.Split(new char[] { ':' });
//        if (astrProcessing.Length > 1)
//        {
//            string strLeft = astrProcessing[0].Trim();
//            string strRight = astrProcessing[1].Trim();
//            if (strLeft == "- name")
//            {
//                iBlock++;
//                cBlocksFound++;
//                atlBlocks[iBlock] = new InputOutputFileLines();
//                // First find uncommented "- name"
//                atlBlocks[iBlock].iInputName = iLine;

//                // Store the name provided by the user
//                atlBlocks[iBlock].strInputName = strRight;

//                // Back up to previous blank line.
//                int cPrev = FindNextBlankLine(astrInput, iLine, -1, input.iSteps + 1, input.iLastLine);

//                // Scan ahead to next blank line.
//                int cNext = FindNextBlankLine(astrInput, iLine, 1, input.iSteps + 1, input.iLastLine);

//                atlBlocks[iBlock].iInputFirst = iLine - cPrev + 1;
//                atlBlocks[iBlock].iInputLast = iLine + cNext - 1;

//                // Set defaults for output file
//                atlBlocks[iBlock].bHasOutput = false;
//                atlBlocks[iBlock].iOutputFirst = -1;
//                atlBlocks[iBlock].iOutputLast = -1;
//            }
//        }
//    }

//    return atlBlocks;
//}

//public void MatchToOutputBlocks(string[] astrInput, OutputFile output, string[] astrOutput, InputOutputFileLines[] lines)
//{
//    // Count all the stars in the output file.
//    int cStars = 0;
//    for (int iOutLine = output.iFileHeaderLastLine; iOutLine < astrOutput.Length; iOutLine++)
//    {
//        string strOutLine = astrOutput[iOutLine];
//        if (strOutLine.Contains('⭐'))
//            cStars++;
//    }

//    // Allocate an array of integers to hold line numbers;
//    int[] aiRunlines = new int[cStars];

//    // Get a list of lines that start with <star> Run Main
//    int iStar = 0;
//    for (int iOutLine = output.iFileHeaderLastLine; iOutLine < astrOutput.Length; iOutLine++)
//    {
//        string strOutLine = astrOutput[iOutLine];
//        if (strOutLine.Contains('⭐'))
//        {
//            aiRunlines[iStar] = iOutLine;
//            iStar++;
//        }
//    }

//    // Walk through all input blocks
//    for (int iBlock = 0; iBlock < lines.Length; iBlock++)
//    {
//        string strBlockName = lines[iBlock].strInputName;

//        // If name has an environment variable, remove it.
//        int iDollar = strBlockName.IndexOf("$");
//        if (iDollar > -1)
//        {
//            strBlockName = strBlockName.Substring(0, iDollar - 1);
//        }

//        // See if there is a match in the output run line titles.
//        for (int iRunlines = 0; iRunlines < aiRunlines.Length; iRunlines++)
//        {
//            int iOutputLine = aiRunlines[iRunlines];
//            string strOutputLine = astrOutput[iOutputLine];
//            if (strOutputLine.Contains(strBlockName))
//            {
//                // We've got a hit
//                lines[iBlock].bHasOutput = true;
//                lines[iBlock].iOutputFirst = iOutputLine;
//                if (iRunlines == aiRunlines.Length - 1)
//                {
//                    // Special case for last entry.
//                    lines[iBlock].iOutputLast = output.iLastLine;
//                }
//                else
//                {
//                    lines[iBlock].iOutputLast = aiRunlines[iRunlines + 1] - 1;
//                }
//            }
//        }
//    }
//}

//public int FindNextBlankLine(string[] astrInput, int iStart, int iStep, int iMin, int iMax)
//{
//    int cLines = 0;
//    for (int iLine = iStart; iLine >= iMin && iLine <= iMax; iLine += iStep)
//    {
//        if (string.IsNullOrEmpty(astrInput[iLine]))
//            break;

//        cLines++;
//    }

//    return cLines;
//}

//public StringBuilder MergeFiles(InputFile input, string[] astrInput, OutputFile output, string[] astrOutput, InputOutputFileLines[] lines)
//{
//    StringBuilder sb = new StringBuilder();
//    int iInput = 0;
//    int iOutput = 0;

//    // Write input file header
//    for (; iInput <= input.iSteps; iInput++)
//        sb.AppendLine(astrInput[iInput]);

//    // Write output file header
//    for (; iOutput < output.iFileHeaderLastLine; iOutput++)
//        sb.AppendLine($"\t\t{astrOutput[iOutput]}");

//    sb.AppendLine($"");
//    sb.AppendLine($"------------------------------------------------------------------------------------");
//    sb.AppendLine($"");
//    // Loop through lines, merging input with output
//    for (int iBlock = 0; iBlock < lines.Length; iBlock++)
//    {
//        // Dump input block
//        for (iInput = lines[iBlock].iInputFirst; iInput <= lines[iBlock].iInputLast; iInput++)
//            sb.AppendLine(astrInput[iInput]);

//        // Dump corresponding output block
//        if (lines[iBlock].bHasOutput)
//        {
//            for (iOutput = lines[iBlock].iOutputFirst; iOutput <= lines[iBlock].iOutputLast; iOutput++)
//                sb.AppendLine($"\t\t{astrOutput[iOutput]}");
//        }
//        else
//        {
//            sb.AppendLine($"\t\t********* No Output for this step. *********");
//        }
//        sb.AppendLine($"");
//        sb.AppendLine($"------------------------------------------------------------------------------------");
//        sb.AppendLine($"");
//    }

//    return sb;
//}