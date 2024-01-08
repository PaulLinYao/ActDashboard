namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            gridFiles = new DataGridView();
            checkWordWrap = new CheckBox();
            cmdRefresh = new LinkLabel();
            cmdSetFolder = new LinkLabel();
            textFolder = new TextBox();
            label1 = new Label();
            groupBox2 = new GroupBox();
            splitcontainer = new SplitContainer();
            treeView1 = new TreeView();
            rtDisplay = new RichTextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            label2 = new Label();
            label3 = new Label();
            textInputFile = new TextBox();
            textOutputFile = new TextBox();
            groupBox3 = new GroupBox();
            cmdClearConsole = new LinkLabel();
            tableLayoutPanel2 = new TableLayoutPanel();
            label4 = new Label();
            cmdActionSettings = new LinkLabel();
            comboWorkflows = new ComboBox();
            textConsole = new TextBox();
            cmdStartStop = new Button();
            Input = new DataGridViewTextBoxColumn();
            Output = new DataGridViewTextBoxColumn();
            Duration = new DataGridViewTextBoxColumn();
            Notes = new DataGridViewTextBoxColumn();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridFiles).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitcontainer).BeginInit();
            splitcontainer.Panel1.SuspendLayout();
            splitcontainer.Panel2.SuspendLayout();
            splitcontainer.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.Controls.Add(gridFiles);
            groupBox1.Location = new Point(12, 249);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1234, 183);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Workflow History";
            // 
            // gridFiles
            // 
            gridFiles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            gridFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gridFiles.Columns.AddRange(new DataGridViewColumn[] { Input, Output, Duration, Notes });
            gridFiles.Location = new Point(6, 22);
            gridFiles.Name = "gridFiles";
            gridFiles.RowTemplate.Height = 25;
            gridFiles.Size = new Size(1212, 151);
            gridFiles.TabIndex = 3;
            gridFiles.CellValueChanged += gridFiles_CellValueChanged;
            gridFiles.RowHeaderMouseClick += gridFiles_RowHeaderMouseClick;
            // 
            // checkWordWrap
            // 
            checkWordWrap.AutoSize = true;
            checkWordWrap.Location = new Point(656, 32);
            checkWordWrap.Name = "checkWordWrap";
            checkWordWrap.Size = new Size(86, 19);
            checkWordWrap.TabIndex = 5;
            checkWordWrap.Text = "Word Wrap";
            checkWordWrap.UseVisualStyleBackColor = true;
            checkWordWrap.CheckedChanged += checkWordWrap_CheckedChanged;
            // 
            // cmdRefresh
            // 
            cmdRefresh.AutoSize = true;
            cmdRefresh.Location = new Point(562, 70);
            cmdRefresh.Name = "cmdRefresh";
            cmdRefresh.Size = new Size(46, 15);
            cmdRefresh.TabIndex = 4;
            cmdRefresh.TabStop = true;
            cmdRefresh.Text = "Refresh";
            cmdRefresh.LinkClicked += cmdRefresh_LinkClicked;
            // 
            // cmdSetFolder
            // 
            cmdSetFolder.Anchor = AnchorStyles.Left;
            cmdSetFolder.AutoSize = true;
            cmdSetFolder.Location = new Point(377, 9);
            cmdSetFolder.Name = "cmdSetFolder";
            cmdSetFolder.Size = new Size(89, 15);
            cmdSetFolder.TabIndex = 2;
            cmdSetFolder.TabStop = true;
            cmdSetFolder.Text = "Set Repo Folder";
            cmdSetFolder.LinkClicked += cmdSetFolder_LinkClicked;
            // 
            // textFolder
            // 
            textFolder.Anchor = AnchorStyles.Left;
            textFolder.Location = new Point(96, 5);
            textFolder.Name = "textFolder";
            textFolder.Size = new Size(275, 23);
            textFolder.TabIndex = 1;
            textFolder.Text = "C:\\CESMII.github\\Marketplace";
            textFolder.TextChanged += textFolder_TextChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(17, 9);
            label1.Name = "label1";
            label1.Size = new Size(73, 15);
            label1.TabIndex = 0;
            label1.Text = "Repo Folder:";
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(splitcontainer);
            groupBox2.Controls.Add(checkWordWrap);
            groupBox2.Controls.Add(tableLayoutPanel1);
            groupBox2.Location = new Point(12, 438);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1232, 591);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Details";
            // 
            // splitcontainer
            // 
            splitcontainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitcontainer.Location = new Point(6, 65);
            splitcontainer.Name = "splitcontainer";
            // 
            // splitcontainer.Panel1
            // 
            splitcontainer.Panel1.BackColor = SystemColors.Window;
            splitcontainer.Panel1.Controls.Add(treeView1);
            // 
            // splitcontainer.Panel2
            // 
            splitcontainer.Panel2.BackColor = SystemColors.Window;
            splitcontainer.Panel2.Controls.Add(rtDisplay);
            splitcontainer.Size = new Size(1220, 520);
            splitcontainer.SplitterDistance = 239;
            splitcontainer.SplitterWidth = 8;
            splitcontainer.TabIndex = 6;
            // 
            // treeView1
            // 
            treeView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(244, 520);
            treeView1.TabIndex = 0;
            // 
            // rtDisplay
            // 
            rtDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rtDisplay.DetectUrls = false;
            rtDisplay.Font = new Font("Lucida Console", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            rtDisplay.Location = new Point(0, 0);
            rtDisplay.Name = "rtDisplay";
            rtDisplay.Size = new Size(965, 520);
            rtDisplay.TabIndex = 0;
            rtDisplay.Text = "";
            rtDisplay.WordWrap = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37.5F));
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(label3, 2, 0);
            tableLayoutPanel1.Controls.Add(textInputFile, 1, 0);
            tableLayoutPanel1.Controls.Add(textOutputFile, 3, 0);
            tableLayoutPanel1.Location = new Point(20, 22);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(606, 37);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(13, 11);
            label2.Name = "label2";
            label2.Size = new Size(59, 15);
            label2.TabIndex = 0;
            label2.Text = "Input File:";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(305, 11);
            label3.Name = "label3";
            label3.Size = new Size(69, 15);
            label3.TabIndex = 1;
            label3.Text = "Output File:";
            // 
            // textInputFile
            // 
            textInputFile.Anchor = AnchorStyles.Left;
            textInputFile.Location = new Point(78, 7);
            textInputFile.Name = "textInputFile";
            textInputFile.ReadOnly = true;
            textInputFile.Size = new Size(221, 23);
            textInputFile.TabIndex = 2;
            // 
            // textOutputFile
            // 
            textOutputFile.Anchor = AnchorStyles.Left;
            textOutputFile.Location = new Point(380, 7);
            textOutputFile.Name = "textOutputFile";
            textOutputFile.ReadOnly = true;
            textOutputFile.Size = new Size(223, 23);
            textOutputFile.TabIndex = 3;
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3.Controls.Add(cmdClearConsole);
            groupBox3.Controls.Add(cmdRefresh);
            groupBox3.Controls.Add(tableLayoutPanel2);
            groupBox3.Controls.Add(textConsole);
            groupBox3.Controls.Add(cmdStartStop);
            groupBox3.Location = new Point(12, 12);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(1234, 231);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Git Repo";
            // 
            // cmdClearConsole
            // 
            cmdClearConsole.AutoSize = true;
            cmdClearConsole.Location = new Point(20, 208);
            cmdClearConsole.Name = "cmdClearConsole";
            cmdClearConsole.Size = new Size(34, 15);
            cmdClearConsole.TabIndex = 9;
            cmdClearConsole.TabStop = true;
            cmdClearConsole.Text = "Clear";
            cmdClearConsole.LinkClicked += cmdClearConsole_LinkClicked;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(label4, 0, 1);
            tableLayoutPanel2.Controls.Add(cmdActionSettings, 2, 1);
            tableLayoutPanel2.Controls.Add(textFolder, 1, 0);
            tableLayoutPanel2.Controls.Add(comboWorkflows, 1, 1);
            tableLayoutPanel2.Controls.Add(cmdSetFolder, 2, 0);
            tableLayoutPanel2.Location = new Point(16, 22);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(469, 69);
            tableLayoutPanel2.TabIndex = 8;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(29, 44);
            label4.Name = "label4";
            label4.Size = new Size(61, 15);
            label4.TabIndex = 3;
            label4.Text = "Workflow:";
            // 
            // cmdActionSettings
            // 
            cmdActionSettings.Anchor = AnchorStyles.Left;
            cmdActionSettings.AutoSize = true;
            cmdActionSettings.Location = new Point(377, 44);
            cmdActionSettings.Name = "cmdActionSettings";
            cmdActionSettings.Size = new Size(58, 15);
            cmdActionSettings.TabIndex = 6;
            cmdActionSettings.TabStop = true;
            cmdActionSettings.Text = "Settings...";
            cmdActionSettings.LinkClicked += cmdWorkflowSettings_LinkClicked;
            // 
            // comboWorkflows
            // 
            comboWorkflows.Anchor = AnchorStyles.Left;
            comboWorkflows.DropDownStyle = ComboBoxStyle.DropDownList;
            comboWorkflows.FormattingEnabled = true;
            comboWorkflows.Location = new Point(96, 40);
            comboWorkflows.Name = "comboWorkflows";
            comboWorkflows.Size = new Size(275, 23);
            comboWorkflows.TabIndex = 4;
            comboWorkflows.SelectedIndexChanged += comboWorkflows_SelectedIndexChanged;
            // 
            // textConsole
            // 
            textConsole.AcceptsReturn = true;
            textConsole.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textConsole.Location = new Point(16, 97);
            textConsole.Multiline = true;
            textConsole.Name = "textConsole";
            textConsole.ReadOnly = true;
            textConsole.ScrollBars = ScrollBars.Both;
            textConsole.Size = new Size(1202, 108);
            textConsole.TabIndex = 7;
            textConsole.WordWrap = false;
            // 
            // cmdStartStop
            // 
            cmdStartStop.BackColor = Color.FromArgb(0, 192, 0);
            cmdStartStop.Location = new Point(537, 22);
            cmdStartStop.Name = "cmdStartStop";
            cmdStartStop.Size = new Size(100, 33);
            cmdStartStop.TabIndex = 5;
            cmdStartStop.Text = "Start";
            cmdStartStop.UseVisualStyleBackColor = false;
            cmdStartStop.Click += cmdStartStop_Click;
            // 
            // Input
            // 
            Input.HeaderText = "Input";
            Input.Name = "Input";
            Input.ReadOnly = true;
            // 
            // Output
            // 
            Output.HeaderText = "Output";
            Output.Name = "Output";
            Output.ReadOnly = true;
            // 
            // Duration
            // 
            Duration.HeaderText = "Duration";
            Duration.Name = "Duration";
            Duration.ReadOnly = true;
            // 
            // Notes
            // 
            Notes.HeaderText = "Notes";
            Notes.Name = "Notes";
            Notes.Width = 500;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1256, 1078);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "ACT Dashboard";
            Shown += Form1_Shown;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridFiles).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            splitcontainer.Panel1.ResumeLayout(false);
            splitcontainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitcontainer).EndInit();
            splitcontainer.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private DataGridView gridFiles;
        private LinkLabel cmdSetFolder;
        private TextBox textFolder;
        private Label label1;
        private LinkLabel cmdRefresh;
        private GroupBox groupBox2;
        private RichTextBox rtDisplay;
        private CheckBox checkWordWrap;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label2;
        private Label label3;
        private TextBox textInputFile;
        private TextBox textOutputFile;
        private GroupBox groupBox3;
        private Button cmdStartStop;
        private ComboBox comboWorkflows;
        private Label label4;
        private TableLayoutPanel tableLayoutPanel2;
        private LinkLabel cmdActionSettings;
        private TextBox textConsole;
        private LinkLabel cmdClearConsole;
        private SplitContainer splitcontainer;
        private TreeView treeView1;
        private DataGridViewTextBoxColumn Input;
        private DataGridViewTextBoxColumn Output;
        private DataGridViewTextBoxColumn Duration;
        private DataGridViewTextBoxColumn Notes;
    }
}