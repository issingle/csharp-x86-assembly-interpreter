namespace Interpreter
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLoadSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveSnapshot = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExecution = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStepInto = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStepOver = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuReset = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpEditor = new System.Windows.Forms.GroupBox();
            this.grpRegisters = new System.Windows.Forms.GroupBox();
            this.txtEfl = new System.Windows.Forms.TextBox();
            this.lblEFL = new System.Windows.Forms.Label();
            this.txtEbp = new System.Windows.Forms.TextBox();
            this.lblEbp = new System.Windows.Forms.Label();
            this.txtEsp = new System.Windows.Forms.TextBox();
            this.lblEsp = new System.Windows.Forms.Label();
            this.txtEdi = new System.Windows.Forms.TextBox();
            this.lblEdi = new System.Windows.Forms.Label();
            this.txtEsi = new System.Windows.Forms.TextBox();
            this.lblEsi = new System.Windows.Forms.Label();
            this.txtEdx = new System.Windows.Forms.TextBox();
            this.lblEdx = new System.Windows.Forms.Label();
            this.txtEcx = new System.Windows.Forms.TextBox();
            this.lblEcx = new System.Windows.Forms.Label();
            this.txtEbx = new System.Windows.Forms.TextBox();
            this.lblEbx = new System.Windows.Forms.Label();
            this.txtEax = new System.Windows.Forms.TextBox();
            this.lblEax = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.grpMemory = new System.Windows.Forms.GroupBox();
            this.grpEventLog = new System.Windows.Forms.GroupBox();
            this.txtEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this.txtMemory = new Interpreter.FlickerFreeRichTextBox();
            this.txtEventLog = new Interpreter.FlickerFreeRichTextBox();
            this.mnuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpEditor.SuspendLayout();
            this.grpRegisters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.grpMemory.SuspendLayout();
            this.grpEventLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuExecution,
            this.mnuAbout});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(703, 24);
            this.mnuMain.TabIndex = 1;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSaveSnapshot,
            this.mnuLoadSnapshot,
            this.mnuExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            // 
            // mnuLoadSnapshot
            // 
            this.mnuLoadSnapshot.Name = "mnuLoadSnapshot";
            this.mnuLoadSnapshot.Size = new System.Drawing.Size(152, 22);
            this.mnuLoadSnapshot.Text = "Load Snapshot";
            this.mnuLoadSnapshot.Click += new System.EventHandler(this.mnuLoadSnapshot_Click);
            // 
            // mnuSaveSnapshot
            // 
            this.mnuSaveSnapshot.Name = "mnuSaveSnapshot";
            this.mnuSaveSnapshot.Size = new System.Drawing.Size(152, 22);
            this.mnuSaveSnapshot.Text = "Save Snapshot";
            this.mnuSaveSnapshot.Click += new System.EventHandler(this.mnuSaveSnapshot_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(152, 22);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuExecution
            // 
            this.mnuExecution.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuStart,
            this.mnuStepInto,
            this.mnuStepOver,
            this.mnuStop,
            this.mnuReset});
            this.mnuExecution.Name = "mnuExecution";
            this.mnuExecution.Size = new System.Drawing.Size(70, 20);
            this.mnuExecution.Text = "Execution";
            // 
            // mnuStart
            // 
            this.mnuStart.Name = "mnuStart";
            this.mnuStart.ShortcutKeyDisplayString = "";
            this.mnuStart.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.mnuStart.Size = new System.Drawing.Size(180, 22);
            this.mnuStart.Text = "Start";
            this.mnuStart.Click += new System.EventHandler(this.mnuStart_Click);
            // 
            // mnuStepInto
            // 
            this.mnuStepInto.Enabled = false;
            this.mnuStepInto.Name = "mnuStepInto";
            this.mnuStepInto.ShortcutKeys = System.Windows.Forms.Keys.F11;
            this.mnuStepInto.Size = new System.Drawing.Size(180, 22);
            this.mnuStepInto.Text = "Step Into";
            this.mnuStepInto.Click += new System.EventHandler(this.mnuStepInto_Click);
            // 
            // mnuStepOver
            // 
            this.mnuStepOver.Enabled = false;
            this.mnuStepOver.Name = "mnuStepOver";
            this.mnuStepOver.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.mnuStepOver.Size = new System.Drawing.Size(180, 22);
            this.mnuStepOver.Text = "Step Over";
            this.mnuStepOver.Click += new System.EventHandler(this.mnuStepOver_Click);
            // 
            // mnuStop
            // 
            this.mnuStop.Enabled = false;
            this.mnuStop.Name = "mnuStop";
            this.mnuStop.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F5)));
            this.mnuStop.Size = new System.Drawing.Size(180, 22);
            this.mnuStop.Text = "Stop";
            this.mnuStop.Click += new System.EventHandler(this.mnuStop_Click);
            // 
            // mnuReset
            // 
            this.mnuReset.Enabled = false;
            this.mnuReset.Name = "mnuReset";
            this.mnuReset.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.F5)));
            this.mnuReset.Size = new System.Drawing.Size(180, 22);
            this.mnuReset.Text = "Reset";
            this.mnuReset.Click += new System.EventHandler(this.mnuReset_Click);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(52, 20);
            this.mnuAbout.Text = "About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 619);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(703, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grpEditor);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(703, 595);
            this.splitContainer1.SplitterDistance = 255;
            this.splitContainer1.TabIndex = 3;
            // 
            // grpEditor
            // 
            this.grpEditor.Controls.Add(this.txtEditor);
            this.grpEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEditor.Location = new System.Drawing.Point(0, 0);
            this.grpEditor.Name = "grpEditor";
            this.grpEditor.Size = new System.Drawing.Size(703, 255);
            this.grpEditor.TabIndex = 4;
            this.grpEditor.TabStop = false;
            this.grpEditor.Text = "Editor";
            // 
            // grpRegisters
            // 
            this.grpRegisters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.grpRegisters.Controls.Add(this.txtEfl);
            this.grpRegisters.Controls.Add(this.lblEFL);
            this.grpRegisters.Controls.Add(this.txtEbp);
            this.grpRegisters.Controls.Add(this.lblEbp);
            this.grpRegisters.Controls.Add(this.txtEsp);
            this.grpRegisters.Controls.Add(this.lblEsp);
            this.grpRegisters.Controls.Add(this.txtEdi);
            this.grpRegisters.Controls.Add(this.lblEdi);
            this.grpRegisters.Controls.Add(this.txtEsi);
            this.grpRegisters.Controls.Add(this.lblEsi);
            this.grpRegisters.Controls.Add(this.txtEdx);
            this.grpRegisters.Controls.Add(this.lblEdx);
            this.grpRegisters.Controls.Add(this.txtEcx);
            this.grpRegisters.Controls.Add(this.lblEcx);
            this.grpRegisters.Controls.Add(this.txtEbx);
            this.grpRegisters.Controls.Add(this.lblEbx);
            this.grpRegisters.Controls.Add(this.txtEax);
            this.grpRegisters.Controls.Add(this.lblEax);
            this.grpRegisters.Location = new System.Drawing.Point(3, 3);
            this.grpRegisters.Name = "grpRegisters";
            this.grpRegisters.Size = new System.Drawing.Size(177, 252);
            this.grpRegisters.TabIndex = 0;
            this.grpRegisters.TabStop = false;
            this.grpRegisters.Text = "Registers";
            // 
            // txtEfl
            // 
            this.txtEfl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEfl.Font = new System.Drawing.Font("Lucida Console", 9.75F);
            this.txtEfl.Location = new System.Drawing.Point(44, 224);
            this.txtEfl.Name = "txtEfl";
            this.txtEfl.ReadOnly = true;
            this.txtEfl.Size = new System.Drawing.Size(121, 20);
            this.txtEfl.TabIndex = 17;
            // 
            // lblEFL
            // 
            this.lblEFL.AutoSize = true;
            this.lblEFL.Location = new System.Drawing.Point(6, 227);
            this.lblEFL.Name = "lblEFL";
            this.lblEFL.Size = new System.Drawing.Size(32, 13);
            this.lblEFL.TabIndex = 16;
            this.lblEFL.Text = "EFL: ";
            // 
            // txtEbp
            // 
            this.txtEbp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEbp.Font = new System.Drawing.Font("Lucida Console", 9.75F);
            this.txtEbp.Location = new System.Drawing.Point(44, 198);
            this.txtEbp.Name = "txtEbp";
            this.txtEbp.ReadOnly = true;
            this.txtEbp.Size = new System.Drawing.Size(121, 20);
            this.txtEbp.TabIndex = 15;
            // 
            // lblEbp
            // 
            this.lblEbp.AutoSize = true;
            this.lblEbp.Location = new System.Drawing.Point(6, 201);
            this.lblEbp.Name = "lblEbp";
            this.lblEbp.Size = new System.Drawing.Size(34, 13);
            this.lblEbp.TabIndex = 14;
            this.lblEbp.Text = "EBP: ";
            // 
            // txtEsp
            // 
            this.txtEsp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEsp.Font = new System.Drawing.Font("Lucida Console", 9.75F);
            this.txtEsp.Location = new System.Drawing.Point(44, 172);
            this.txtEsp.Name = "txtEsp";
            this.txtEsp.ReadOnly = true;
            this.txtEsp.Size = new System.Drawing.Size(121, 20);
            this.txtEsp.TabIndex = 13;
            // 
            // lblEsp
            // 
            this.lblEsp.AutoSize = true;
            this.lblEsp.Location = new System.Drawing.Point(6, 175);
            this.lblEsp.Name = "lblEsp";
            this.lblEsp.Size = new System.Drawing.Size(34, 13);
            this.lblEsp.TabIndex = 12;
            this.lblEsp.Text = "ESP: ";
            // 
            // txtEdi
            // 
            this.txtEdi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEdi.Font = new System.Drawing.Font("Lucida Console", 9.75F);
            this.txtEdi.Location = new System.Drawing.Point(44, 146);
            this.txtEdi.Name = "txtEdi";
            this.txtEdi.ReadOnly = true;
            this.txtEdi.Size = new System.Drawing.Size(121, 20);
            this.txtEdi.TabIndex = 11;
            // 
            // lblEdi
            // 
            this.lblEdi.AutoSize = true;
            this.lblEdi.Location = new System.Drawing.Point(6, 149);
            this.lblEdi.Name = "lblEdi";
            this.lblEdi.Size = new System.Drawing.Size(31, 13);
            this.lblEdi.TabIndex = 10;
            this.lblEdi.Text = "EDI: ";
            // 
            // txtEsi
            // 
            this.txtEsi.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEsi.Font = new System.Drawing.Font("Lucida Console", 9.75F);
            this.txtEsi.Location = new System.Drawing.Point(44, 120);
            this.txtEsi.Name = "txtEsi";
            this.txtEsi.ReadOnly = true;
            this.txtEsi.Size = new System.Drawing.Size(121, 20);
            this.txtEsi.TabIndex = 9;
            // 
            // lblEsi
            // 
            this.lblEsi.AutoSize = true;
            this.lblEsi.Location = new System.Drawing.Point(6, 123);
            this.lblEsi.Name = "lblEsi";
            this.lblEsi.Size = new System.Drawing.Size(30, 13);
            this.lblEsi.TabIndex = 8;
            this.lblEsi.Text = "ESI: ";
            // 
            // txtEdx
            // 
            this.txtEdx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEdx.Font = new System.Drawing.Font("Lucida Console", 9.75F);
            this.txtEdx.Location = new System.Drawing.Point(44, 94);
            this.txtEdx.Name = "txtEdx";
            this.txtEdx.ReadOnly = true;
            this.txtEdx.Size = new System.Drawing.Size(121, 20);
            this.txtEdx.TabIndex = 7;
            // 
            // lblEdx
            // 
            this.lblEdx.AutoSize = true;
            this.lblEdx.Location = new System.Drawing.Point(6, 97);
            this.lblEdx.Name = "lblEdx";
            this.lblEdx.Size = new System.Drawing.Size(35, 13);
            this.lblEdx.TabIndex = 6;
            this.lblEdx.Text = "EDX: ";
            // 
            // txtEcx
            // 
            this.txtEcx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEcx.Font = new System.Drawing.Font("Lucida Console", 9.75F);
            this.txtEcx.Location = new System.Drawing.Point(44, 68);
            this.txtEcx.Name = "txtEcx";
            this.txtEcx.ReadOnly = true;
            this.txtEcx.Size = new System.Drawing.Size(121, 20);
            this.txtEcx.TabIndex = 5;
            // 
            // lblEcx
            // 
            this.lblEcx.AutoSize = true;
            this.lblEcx.Location = new System.Drawing.Point(6, 71);
            this.lblEcx.Name = "lblEcx";
            this.lblEcx.Size = new System.Drawing.Size(34, 13);
            this.lblEcx.TabIndex = 4;
            this.lblEcx.Text = "ECX: ";
            // 
            // txtEbx
            // 
            this.txtEbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEbx.Font = new System.Drawing.Font("Lucida Console", 9.75F);
            this.txtEbx.Location = new System.Drawing.Point(44, 42);
            this.txtEbx.Name = "txtEbx";
            this.txtEbx.ReadOnly = true;
            this.txtEbx.Size = new System.Drawing.Size(121, 20);
            this.txtEbx.TabIndex = 3;
            // 
            // lblEbx
            // 
            this.lblEbx.AutoSize = true;
            this.lblEbx.Location = new System.Drawing.Point(6, 45);
            this.lblEbx.Name = "lblEbx";
            this.lblEbx.Size = new System.Drawing.Size(34, 13);
            this.lblEbx.TabIndex = 2;
            this.lblEbx.Text = "EBX: ";
            // 
            // txtEax
            // 
            this.txtEax.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEax.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEax.Location = new System.Drawing.Point(44, 16);
            this.txtEax.Name = "txtEax";
            this.txtEax.ReadOnly = true;
            this.txtEax.Size = new System.Drawing.Size(121, 20);
            this.txtEax.TabIndex = 1;
            // 
            // lblEax
            // 
            this.lblEax.AutoSize = true;
            this.lblEax.Location = new System.Drawing.Point(6, 19);
            this.lblEax.Name = "lblEax";
            this.lblEax.Size = new System.Drawing.Size(34, 13);
            this.lblEax.TabIndex = 0;
            this.lblEax.Text = "EAX: ";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.grpRegisters);
            this.splitContainer3.Panel1.Controls.Add(this.grpMemory);
            this.splitContainer3.Panel1MinSize = 60;
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.grpEventLog);
            this.splitContainer3.Panel2MinSize = 40;
            this.splitContainer3.Size = new System.Drawing.Size(703, 336);
            this.splitContainer3.SplitterDistance = 261;
            this.splitContainer3.TabIndex = 1;
            // 
            // grpMemory
            // 
            this.grpMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpMemory.Controls.Add(this.txtMemory);
            this.grpMemory.Location = new System.Drawing.Point(186, 3);
            this.grpMemory.Name = "grpMemory";
            this.grpMemory.Size = new System.Drawing.Size(514, 252);
            this.grpMemory.TabIndex = 0;
            this.grpMemory.TabStop = false;
            this.grpMemory.Text = "Memory";
            // 
            // grpEventLog
            // 
            this.grpEventLog.Controls.Add(this.txtEventLog);
            this.grpEventLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpEventLog.Location = new System.Drawing.Point(0, 0);
            this.grpEventLog.Name = "grpEventLog";
            this.grpEventLog.Size = new System.Drawing.Size(703, 71);
            this.grpEventLog.TabIndex = 0;
            this.grpEventLog.TabStop = false;
            this.grpEventLog.Text = "Event Log";
            // 
            // txtEditor
            // 
            this.txtEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEditor.IsReadOnly = false;
            this.txtEditor.Location = new System.Drawing.Point(3, 16);
            this.txtEditor.Name = "txtEditor";
            this.txtEditor.Size = new System.Drawing.Size(697, 236);
            this.txtEditor.TabIndex = 0;
            this.txtEditor.Text = resources.GetString("txtEditor.Text");
            this.txtEditor.TextChanged += new System.EventHandler(this.txtEditor_TextChanged);
            // 
            // txtMemory
            // 
            this.txtMemory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMemory.Font = new System.Drawing.Font("Lucida Console", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMemory.Location = new System.Drawing.Point(3, 16);
            this.txtMemory.Name = "txtMemory";
            this.txtMemory.ReadOnly = true;
            this.txtMemory.Size = new System.Drawing.Size(508, 233);
            this.txtMemory.TabIndex = 0;
            this.txtMemory.Text = "";
            this.txtMemory.WordWrap = false;
            // 
            // txtEventLog
            // 
            this.txtEventLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEventLog.Location = new System.Drawing.Point(3, 16);
            this.txtEventLog.Name = "txtEventLog";
            this.txtEventLog.ReadOnly = true;
            this.txtEventLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.txtEventLog.Size = new System.Drawing.Size(697, 52);
            this.txtEventLog.TabIndex = 0;
            this.txtEventLog.Text = "";
            this.txtEventLog.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 641);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mnuMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuMain;
            this.MinimumSize = new System.Drawing.Size(520, 600);
            this.Name = "Form1";
            this.Text = "x86 Assembly Interpreter";
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpEditor.ResumeLayout(false);
            this.grpRegisters.ResumeLayout(false);
            this.grpRegisters.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.grpMemory.ResumeLayout(false);
            this.grpEventLog.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private ICSharpCode.TextEditor.TextEditorControl txtEditor;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox grpEditor;
        private System.Windows.Forms.GroupBox grpRegisters;
        private System.Windows.Forms.GroupBox grpMemory;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.GroupBox grpEventLog;
        private FlickerFreeRichTextBox txtEventLog;
        private System.Windows.Forms.TextBox txtEfl;
        private System.Windows.Forms.Label lblEFL;
        private System.Windows.Forms.TextBox txtEbp;
        private System.Windows.Forms.Label lblEbp;
        private System.Windows.Forms.TextBox txtEsp;
        private System.Windows.Forms.Label lblEsp;
        private System.Windows.Forms.TextBox txtEdi;
        private System.Windows.Forms.Label lblEdi;
        private System.Windows.Forms.TextBox txtEsi;
        private System.Windows.Forms.Label lblEsi;
        private System.Windows.Forms.TextBox txtEdx;
        private System.Windows.Forms.Label lblEdx;
        private System.Windows.Forms.TextBox txtEcx;
        private System.Windows.Forms.Label lblEcx;
        private System.Windows.Forms.TextBox txtEbx;
        private System.Windows.Forms.Label lblEbx;
        private System.Windows.Forms.TextBox txtEax;
        private System.Windows.Forms.Label lblEax;
        private System.Windows.Forms.ToolStripMenuItem mnuExecution;
        private System.Windows.Forms.ToolStripMenuItem mnuStart;
        private System.Windows.Forms.ToolStripMenuItem mnuStepInto;
        private System.Windows.Forms.ToolStripMenuItem mnuStepOver;
        private System.Windows.Forms.ToolStripMenuItem mnuStop;
        private System.Windows.Forms.ToolStripMenuItem mnuReset;
        private FlickerFreeRichTextBox txtMemory;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveSnapshot;
        private System.Windows.Forms.ToolStripMenuItem mnuLoadSnapshot;
    }
}

