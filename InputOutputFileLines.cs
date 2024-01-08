using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    public class InputFile
    {
        public int cInputLines;
        public int iName;
        public string? strActionName;
        public int iOn;
        public int iEnv;
        public int iJobs;
        public string? strJobName;
        public int iSteps;
        public int cInputBlocks;
        public int iLastLine;
    }

    public class OutputFile
    {
        public int iFileHeaderLastLine; // Index of last line before first "Run" statement.
        public int cOutputLines;        // Count of lines in the file.
        public int iLastLine;           // Index of last line in the file.
        public bool bJobSuccess;        // Whether last line says success or failure.
    }

    public class InputOutputFileLines
    {
        public int iInputFirst;         // Input file - first line of block.
        public int iInputLast;          // Input file - last line of block.
        public int iInputName;          // Input file - line with "- name" statement
        public string? strInputName;    // Input file - the string in the "- name:" statement
        public bool bHasOutput;         // Output file - whether we found output for the input.
        public bool bSuccess;           // Output file - whether step ended in success or failure.
        public int iOutputFirst;        // Output file - first line for this block.
        public int iOutputLast;         // Output file - last line for this block.
    }
}
