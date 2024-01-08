namespace ActDashboard
{
    partial class Form_Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            label2 = new Label();
            label3 = new Label();
            textDockerID = new TextBox();
            textPassword = new TextBox();
            cmdLogin = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(21, 12);
            label1.Name = "label1";
            label1.Size = new Size(464, 15);
            label1.TabIndex = 0;
            label1.Text = "Log in with your Docker ID or email address to push and pull images from Docker Hub.";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(label3, 0, 1);
            tableLayoutPanel1.Controls.Add(textDockerID, 1, 0);
            tableLayoutPanel1.Controls.Add(textPassword, 1, 1);
            tableLayoutPanel1.Location = new Point(21, 45);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(464, 100);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(13, 17);
            label2.Name = "label2";
            label2.Size = new Size(146, 15);
            label2.TabIndex = 0;
            label2.Text = "Docker ID / Email Address:";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(99, 67);
            label3.Name = "label3";
            label3.Size = new Size(60, 15);
            label3.TabIndex = 1;
            label3.Text = "Password:";
            // 
            // textDockerID
            // 
            textDockerID.Anchor = AnchorStyles.Left;
            textDockerID.Location = new Point(165, 13);
            textDockerID.Name = "textDockerID";
            textDockerID.Size = new Size(223, 23);
            textDockerID.TabIndex = 2;
            // 
            // textPassword
            // 
            textPassword.Anchor = AnchorStyles.Left;
            textPassword.Location = new Point(165, 63);
            textPassword.Name = "textPassword";
            textPassword.PasswordChar = '*';
            textPassword.Size = new Size(223, 23);
            textPassword.TabIndex = 3;
            // 
            // cmdLogin
            // 
            cmdLogin.Location = new Point(214, 173);
            cmdLogin.Name = "cmdLogin";
            cmdLogin.Size = new Size(75, 23);
            cmdLogin.TabIndex = 2;
            cmdLogin.Text = "Login";
            cmdLogin.UseVisualStyleBackColor = true;
            cmdLogin.Click += cmdLogin_Click;
            // 
            // Form_Login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(494, 232);
            Controls.Add(cmdLogin);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form_Login";
            Text = "ACT Dashboard - Docker Credentials Needed";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label2;
        private Label label3;
        private TextBox textDockerID;
        private TextBox textPassword;
        private Button cmdLogin;
    }
}