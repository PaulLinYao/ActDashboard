namespace ActDashboard
{
    partial class Form_Settings
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
            cmdSave = new Button();
            cmdCancel = new Button();
            textAction = new TextBox();
            textContainers = new TextBox();
            groupBox1 = new GroupBox();
            checkKillContainers = new CheckBox();
            label2 = new Label();
            textGitBranch = new TextBox();
            groupBox2 = new GroupBox();
            label3 = new Label();
            groupBox3 = new GroupBox();
            groupBox4 = new GroupBox();
            textNetworks = new TextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 18);
            label1.Name = "label1";
            label1.Size = new Size(45, 15);
            label1.TabIndex = 0;
            label1.Text = "Action:";
            // 
            // cmdSave
            // 
            cmdSave.Location = new Point(418, 18);
            cmdSave.Name = "cmdSave";
            cmdSave.Size = new Size(75, 23);
            cmdSave.TabIndex = 2;
            cmdSave.Text = "Save";
            cmdSave.UseVisualStyleBackColor = true;
            cmdSave.Click += cmdSave_Click;
            // 
            // cmdCancel
            // 
            cmdCancel.Location = new Point(418, 62);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new Size(75, 23);
            cmdCancel.TabIndex = 3;
            cmdCancel.Text = "Cancel";
            cmdCancel.UseVisualStyleBackColor = true;
            cmdCancel.Click += cmdCancel_Click;
            // 
            // textAction
            // 
            textAction.Location = new Point(64, 15);
            textAction.Name = "textAction";
            textAction.ReadOnly = true;
            textAction.Size = new Size(205, 23);
            textAction.TabIndex = 4;
            // 
            // textContainers
            // 
            textContainers.AcceptsReturn = true;
            textContainers.Location = new Point(20, 22);
            textContainers.Multiline = true;
            textContainers.Name = "textContainers";
            textContainers.Size = new Size(204, 148);
            textContainers.TabIndex = 6;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(textContainers);
            groupBox1.Location = new Point(15, 22);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(242, 191);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Containers";
            // 
            // checkKillContainers
            // 
            checkKillContainers.AutoSize = true;
            checkKillContainers.Location = new Point(86, 219);
            checkKillContainers.Name = "checkKillContainers";
            checkKillContainers.Size = new Size(213, 19);
            checkKillContainers.TabIndex = 7;
            checkKillContainers.Text = "Remove Prior to Running Workflow";
            checkKillContainers.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(15, 25);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 8;
            label2.Text = "Git Branch:";
            // 
            // textGitBranch
            // 
            textGitBranch.Location = new Point(86, 22);
            textGitBranch.Name = "textGitBranch";
            textGitBranch.Size = new Size(221, 23);
            textGitBranch.TabIndex = 9;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(textGitBranch);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(12, 62);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(328, 78);
            groupBox2.TabIndex = 10;
            groupBox2.TabStop = false;
            groupBox2.Text = "Branch to Run On";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(106, 48);
            label3.Name = "label3";
            label3.Size = new Size(196, 15);
            label3.TabIndex = 10;
            label3.Text = " (parameter to --defaultbranch flag)";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(groupBox4);
            groupBox3.Controls.Add(checkKillContainers);
            groupBox3.Controls.Add(groupBox1);
            groupBox3.Location = new Point(12, 158);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(481, 247);
            groupBox3.TabIndex = 11;
            groupBox3.TabStop = false;
            groupBox3.Text = "Created Objects";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(textNetworks);
            groupBox4.Location = new Point(285, 22);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(177, 191);
            groupBox4.TabIndex = 8;
            groupBox4.TabStop = false;
            groupBox4.Text = "Networks";
            // 
            // textNetworks
            // 
            textNetworks.AcceptsReturn = true;
            textNetworks.Location = new Point(19, 22);
            textNetworks.Multiline = true;
            textNetworks.Name = "textNetworks";
            textNetworks.Size = new Size(135, 148);
            textNetworks.TabIndex = 0;
            // 
            // Form_Settings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(551, 430);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(textAction);
            Controls.Add(cmdCancel);
            Controls.Add(cmdSave);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form_Settings";
            Text = "ACT Dashboard - Settings";
            Load += Form_Settings_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button cmdSave;
        private Button cmdCancel;
        private TextBox textAction;
        private TextBox textContainers;
        private GroupBox groupBox1;
        private CheckBox checkKillContainers;
        private Label label2;
        private TextBox textGitBranch;
        private GroupBox groupBox2;
        private Label label3;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private TextBox textNetworks;
    }
}