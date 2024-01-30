using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1;

namespace ActDashboard
{
    public class Parser
    {
        public static InputFile ParseInputFile(string[] astrInput)
        {
            InputFile ifile = new InputFile();
            ifile.cInputLines = astrInput.Length;
            ifile.iLastLine = astrInput.Length - 1;

            // Parsing rules changes after we see 'jobs:'
            bool bInJobPreamble = true;
            int cInputBlocks = 0;
            for (int iLine = 0; iLine < astrInput.Length; iLine++)
            {
                string strLine = astrInput[iLine];

                // Clear out comments
                string[] astrComments = strLine.Split(new char[] { '#' });
                if (astrComments.Length > 1)
                {
                    strLine = astrComments[0];
                }

                // Ignore empty lines.
                strLine = strLine.Trim();
                if (string.IsNullOrEmpty(strLine))
                    continue;

                string[] astrProcessing = strLine.Split(new char[] { ':' });
                if (astrProcessing.Length > 1)
                {
                    string strLeft = astrProcessing[0];
                    string strRight = astrProcessing[1];
                    if (bInJobPreamble)
                    {
                        switch (strLeft)
                        {
                            case "name":
                                ifile.iName = iLine;
                                ifile.strActionName = strRight;
                                break;
                            case "env":
                                ifile.iEnv = iLine;
                                break;
                            case "on":
                                ifile.iOn = iLine;
                                break;
                            case "jobs":
                                ifile.iJobs = iLine;
                                string strTemp = GetLeftOfColon(astrInput[iLine + 1]);
                                if (!string.IsNullOrEmpty(strTemp))
                                {
                                    iLine++;
                                    ifile.strJobName = strTemp;
                                }
                                break;
                            case "steps":
                                ifile.iSteps = iLine;
                                bInJobPreamble = false;
                                break;
                        }
                    }
                    else
                    {
                        if (strLeft.Contains("- name"))
                            cInputBlocks++;
                    }
                }
            } // loop on lines

            ifile.cInputBlocks = cInputBlocks;
            return ifile;
        }

        public static string GetLeftOfColon(string strInput)
        {
            string[] astrProcessing = strInput.Split(new char[] { ':' });
            if (astrProcessing.Length > 1)
            {
                string strOutput = astrProcessing[0].Trim();
                return strOutput;
            }

            return "";
        }

        public static InputOutputFileLines[] GetInputBlocks(InputFile input, string[] astrInput)
        {
            // Allocate space for expected input blocks.
            InputOutputFileLines[] atlBlocks = new InputOutputFileLines[input.cInputBlocks];

            int iBlock = -1;
            int cBlocksFound = 0;

            // Loop through lines from start of job to end of file.
            for (int iLine = input.iSteps; iLine < input.cInputLines; iLine++)
            {
                string strLine = astrInput[iLine];

                // Clear out comments
                string[] astrComments = strLine.Split(new char[] { '#' });
                if (astrComments.Length > 1)
                {
                    strLine = astrComments[0];
                }

                // Ignore empty lines.
                strLine = strLine.Trim();
                if (string.IsNullOrEmpty(strLine))
                    continue;

                string[] astrProcessing = strLine.Split(new char[] { ':' });
                if (astrProcessing.Length > 1)
                {
                    string strLeft = astrProcessing[0].Trim();
                    string strRight = astrProcessing[1].Trim();
                    if (strLeft == "- name")
                    {
                        iBlock++;
                        cBlocksFound++;
                        atlBlocks[iBlock] = new InputOutputFileLines();
                        // First find uncommented "- name"
                        atlBlocks[iBlock].iInputName = iLine;

                        // Store the name provided by the user
                        atlBlocks[iBlock].strInputName = strRight;

                        // Back up to previous blank line.
                        int cPrev = FindNextBlankLine(astrInput, iLine, -1, input.iSteps + 1, input.iLastLine);

                        // Scan ahead to next blank line.
                        int cNext = FindNextBlankLine(astrInput, iLine, 1, input.iSteps + 1, input.iLastLine);

                        atlBlocks[iBlock].iInputFirst = iLine - cPrev + 1;
                        atlBlocks[iBlock].iInputLast = iLine + cNext - 1;

                        // Set defaults for output file
                        atlBlocks[iBlock].bHasOutput = false;
                        atlBlocks[iBlock].iOutputFirst = -1;
                        atlBlocks[iBlock].iOutputLast = -1;
                    }
                }
            }

            return atlBlocks;
        }

        public static int FindNextBlankLine(string[] astrInput, int iStart, int iStep, int iMin, int iMax)
        {
            int cLines = 0;
            for (int iLine = iStart; iLine >= iMin && iLine <= iMax; iLine += iStep)
            {
                if (string.IsNullOrEmpty(astrInput[iLine]))
                    break;

                cLines++;
            }

            return cLines;
        }

        public static OutputFile ParseOutputFile(string[] astrOutput)
        {
            OutputFile oFile = new OutputFile();
            oFile.cOutputLines = astrOutput.Length;
            oFile.iLastLine = astrOutput.Length - 1;

            // Set iFileHeaderLastLine - Last line in header block of file.
            oFile.iFileHeaderLastLine = astrOutput.Length - 1;                // Default to last line in file

            for (int iLine = 0; iLine < astrOutput.Length; iLine++)           // But then scan for other lines...
            {
                if (astrOutput[iLine].Contains('⭐'))
                {
                    oFile.iFileHeaderLastLine = iLine - 1;
                    break;
                }
            }

            // Do we need to handle this?
            // The character / icon for 'skipping unsupported platforms': '🚧'

            // Set bJobSuccess
            string strLast = astrOutput[oFile.iLastLine];
            if (strLast.Contains("Job failed"))
                oFile.bJobSuccess = false;
            else
                oFile.bJobSuccess = true;

            return oFile;
        }

        public static void MatchToOutputBlocks(string[] astrInput, OutputFile output, string[] astrOutput, InputOutputFileLines[] lines)
        {
            // Count all the stars in the output file.
            int cStars = 0;
            for (int iOutLine = output.iFileHeaderLastLine; iOutLine < astrOutput.Length; iOutLine++)
            {
                string strOutLine = astrOutput[iOutLine];
                if (strOutLine.Contains('⭐'))
                    cStars++;
            }

            // Allocate an array of integers to hold line numbers;
            int[] aiRunlines = new int[cStars];

            // Get a list of lines that start with <star> Run Main
            int iStar = 0;
            for (int iOutLine = output.iFileHeaderLastLine; iOutLine < astrOutput.Length; iOutLine++)
            {
                string strOutLine = astrOutput[iOutLine];
                if (strOutLine.Contains('⭐'))
                {
                    aiRunlines[iStar] = iOutLine;
                    iStar++;
                }
            }

            // Walk through all input blocks
            for (int iBlock = 0; iBlock < lines.Length; iBlock++)
            {
                string strBlockName = lines[iBlock].strInputName;

                // If name has an environment variable, remove it.
                int iDollar = strBlockName.IndexOf("$");
                if (iDollar > -1)
                {
                    strBlockName = strBlockName.Substring(0, iDollar - 1);
                }

                // See if there is a match in the output run line titles.
                for (int iRunlines = 0; iRunlines < aiRunlines.Length; iRunlines++)
                {
                    int iOutputLine = aiRunlines[iRunlines];
                    string strOutputLine = astrOutput[iOutputLine];
                    if (strOutputLine.Contains(strBlockName))
                    {
                        // We've got a hit
                        lines[iBlock].bHasOutput = true;
                        lines[iBlock].iOutputFirst = iOutputLine;
                        if (iRunlines == aiRunlines.Length - 1)
                        {
                            // Special case for last entry.
                            lines[iBlock].iOutputLast = output.iLastLine;
                        }
                        else
                        {
                            lines[iBlock].iOutputLast = aiRunlines[iRunlines + 1] - 1;
                        }
                    }
                }
            }
        }

        public static StringBuilder MergeFiles(InputFile input, string[] astrInput, OutputFile output, string[] astrOutput, InputOutputFileLines[] lines)
        {
            StringBuilder sb = new StringBuilder();
            int iInput = 0;
            int iOutput = 0;

            // Write input file header
            for (; iInput <= input.iSteps; iInput++)
                sb.AppendLine(astrInput[iInput]);

            // Write output file header
            for (; iOutput < output.iFileHeaderLastLine; iOutput++)
                sb.AppendLine($"\t\t{astrOutput[iOutput]}");

            sb.AppendLine($".");
            sb.AppendLine($"------------------------------------------------------------------------------------");
            sb.AppendLine($".");
            // Loop through lines, merging input with output
            for (int iBlock = 0; iBlock < lines.Length; iBlock++)
            {
                // Dump input block
                for (iInput = lines[iBlock].iInputFirst; iInput <= lines[iBlock].iInputLast; iInput++)
                    sb.AppendLine(astrInput[iInput]);

                // Dump corresponding output block
                if (lines[iBlock].bHasOutput)
                {
                    for (iOutput = lines[iBlock].iOutputFirst; iOutput <= lines[iBlock].iOutputLast; iOutput++)
                        sb.AppendLine($"\t\t{astrOutput[iOutput]}");
                }
                else
                {
                    sb.AppendLine($"\t\t********* No Output for this step. *********");
                }
                sb.AppendLine($".");
                sb.AppendLine($"------------------------------------------------------------------------------------");
                sb.AppendLine($".");
            }

            string strTemp = sb.ToString();
            string[] astrLines = strTemp.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sbTemp = new StringBuilder();
            for (int iLine = 0; iLine < astrLines.Length; iLine++)
            {
                sbTemp.AppendLine($"{iLine:0000}  {astrLines[iLine]}");
            }

            return sbTemp;
        }


    }
}
