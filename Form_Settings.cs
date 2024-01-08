using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActDashboard
{
    public partial class Form_Settings : Form
    {
        public string? strWorkflowName
        {
            set { textAction.Text = value; }
            get { return textAction.Text; }
        }
        public string? strContainers
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string strSet = value.Replace(";", "\r\n");
                    textContainers.Text = strSet;
                }

            }
            get
            {
                string strReturn = textContainers.Text.Replace(" ", ";").Replace("\r", ";").Replace("\n", ";").Replace(";;", ";").Replace(";;", ";");
                return strReturn;
            }
        }

        public string? strNetworks
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    string strSet = value.Replace(";", "\r\n");
                    textNetworks.Text = strSet;
                }

            }
            get
            {
                string strReturn = textNetworks.Text.Replace(" ", ";").Replace("\r", ";").Replace("\n", ";").Replace(";;", ";").Replace(";;", ";");
                return strReturn;
            }
        }

        public bool bKillContainers
        {
            set
            {
                checkKillContainers.Checked = value;
            }
            get
            {
                return checkKillContainers.Checked;
            }
        }

        public string? strGitBranch
        {
            set { textGitBranch.Text = value; }
            get { return textGitBranch.Text; }
        }

        public Form_Settings()
        {
            InitializeComponent();

        }

        private void Form_Settings_Load(object sender, EventArgs e)
        {
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
