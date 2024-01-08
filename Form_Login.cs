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
    public partial class Form_Login : Form
    {
        public string DockerID { get; set; }
        public string Password { get; set; }

        public Form_Login()
        {
            InitializeComponent();
        }

        private void cmdLogin_Click(object sender, EventArgs e)
        {
            DockerID = textDockerID.Text;
            Password = textPassword.Text;
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
