using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using x86;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

using ICSharpCode.TextEditor;

namespace Interpreter
{
    public partial class Form1 : Form
    {
        private bool IsExecutionStarted;
        private ExecutionEnvironment env;


        public Form1()
        {
            InitializeComponent();
            this.Icon = Icon.FromHandle(Properties.Resources.Icon.GetHicon());

            // initialize the environment
            env = new ExecutionEnvironment(0, 1024 * 16);

            txtEditor.LineViewerStyle = ICSharpCode.TextEditor.Document.LineViewerStyle.FullRow;
            txtEditor.TextEditorProperties.EnableFolding = false;
            this.txtEditor.ActiveTextAreaControl.TextArea.Caret.PositionChanged += new System.EventHandler(Caret_PositionChanged);

            UpdateUI();
            return;
        }

        private void UpdateUI()
        {
            if (IsExecutionStarted)
            {
                // update registers
                txtEax.Text = string.Format("0x{0:X8}", env.Eax.Value);
                txtEbx.Text = string.Format("0x{0:X8}", env.Ebx.Value);
                txtEcx.Text = string.Format("0x{0:X8}", env.Ecx.Value);
                txtEdx.Text = string.Format("0x{0:X8}", env.Edx.Value);
                txtEsi.Text = string.Format("0x{0:X8}", env.Esi.Value);
                txtEdi.Text = string.Format("0x{0:X8}", env.Edi.Value);
                txtEsp.Text = string.Format("0x{0:X8}", env.Esp.Value);
                txtEbp.Text = string.Format("0x{0:X8}", env.Ebp.Value);

                // update eflags
                txtEfl.ResetText();
                if (env.EFlags.HasFlag(EFlags.OverflowFlag))    txtEfl.Text += "OF, ";
                if (env.EFlags.HasFlag(EFlags.DirectionFlag))   txtEfl.Text += "DF, ";
                if (env.EFlags.HasFlag(EFlags.SignFlag))        txtEfl.Text += "SF, ";
                if (env.EFlags.HasFlag(EFlags.ZeroFlag))        txtEfl.Text += "ZF, ";
                if (env.EFlags.HasFlag(EFlags.AdjustFlag))      txtEfl.Text += "AF, ";
                if (env.EFlags.HasFlag(EFlags.ParityFlag))      txtEfl.Text += "PF, ";
                if (env.EFlags.HasFlag(EFlags.CarryFlag))       txtEfl.Text += "CF, ";
                txtEfl.Text = txtEfl.Text.TrimEnd(new char[] { ' ', ',' });

                // update memory
                // starting from start address, each line displays 16 bytes
                uint lines = (uint)env.Memory.Length / 16;
                uint bytesRemaining = (uint)env.Memory.Length % 16;
                uint offset = 0;

                StringBuilder sb = new StringBuilder((int)((lines + 1) * 62));
                for (uint i = 0; i < lines; i++)
                {
                    sb.Append(string.Format("0x{0:X8} | {1}\r\n", env.BaseAddress + offset, BitConverter.ToString(env.Memory, (int)offset, 16).Replace('-', ' ')));
                    offset += 16;
                }
                txtMemory.Text = sb.ToString();


                // todo: keep track of last memory address written so it can scroll to it
                //txtMemory.SelectionStart = 150;
                //txtMemory.ScrollToCaret();

                SelectLine((int)env.Eip);


                // todo: color code memory
                // heap - black
                // on top of stack - green
                // changed values - red

            }
            else
            {
                // reset all memory and register text, but keep logs
                txtMemory.ResetText();
                txtEax.ResetText();
                txtEbx.ResetText();
                txtEcx.ResetText();
                txtEdx.ResetText();
                txtEsi.ResetText();
                txtEdi.ResetText();
                txtEsp.ResetText();
                txtEbp.ResetText();
                txtEfl.ResetText();
            }
        }

        // todo: if in the middle of execution, only allow changes at or after the current line to be executed

        private void txtEditor_TextChanged(object sender, EventArgs e)
        {

            // todo: syntax highlighting
            // int curline = txtEditor.ActiveTextAreaControl.Caret.Line;
        }

        private void mnuStart_Click(object sender, EventArgs e)
        {
            StartExecution();
        }

        private void mnuStepInto_Click(object sender, EventArgs e)
        {
            try
            {
                bool completed = env.Execute(txtEditor.Text);
                UpdateUI();
            }
            catch (Exception ex)
            {
                // todo: log it
                txtEventLog.Text = ex.Message + System.Environment.NewLine + txtEventLog.Text;

                StopExecution();
            }
        }

        private void mnuStepOver_Click(object sender, EventArgs e)
        {
            try
            {
                while (!env.Execute(txtEditor.Text));
                UpdateUI();
            }
            catch (Exception ex)
            {
                // todo: log it
                txtEventLog.Text = ex.Message + System.Environment.NewLine + txtEventLog.Text;

                StopExecution();
            }
        }

        private void mnuStop_Click(object sender, EventArgs e)
        {
            StopExecution();


        }

        private void StopExecution()
        {
            // refresh the UI
            UpdateUI();

            mnuStart.Enabled = true;
            mnuStepInto.Enabled = false;
            mnuStepOver.Enabled = false;
            mnuStop.Enabled = false;
            mnuReset.Enabled = false;
            IsExecutionStarted = false;
        }

        private void StartExecution()
        {
            // update menu options
            mnuStart.Enabled = false;
            mnuStepInto.Enabled = true;
            mnuStepOver.Enabled = true;
            mnuStop.Enabled = true;
            mnuReset.Enabled = true;
            IsExecutionStarted = true;

            // reset the execution environment
            env.Reset();

            SelectLine(1);

            // refresh the UI
            UpdateUI();
        }

        private void mnuReset_Click(object sender, EventArgs e)
        {
            // reset the execution state
            env.Reset();

            // refresh the UI
            UpdateUI();
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            DotNetPerls.BetterDialog.ShowDialog("x86 Assembly Interpreter", "About", "Created by Mike Davis" + Environment.NewLine + "© Copyright 2011", null, "OK", Interpreter.Properties.Resources.Icon);
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void Caret_PositionChanged(object sender, System.EventArgs e)
        {
            //string[] lines = txtEditor.Text.Split(new string[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
            //env.GoTo(ref lines, txtEditor.ActiveTextAreaControl.Caret.Line);
            //txtEditor.ActiveTextAreaControl.Caret.Position = new TextLocation(0, (int)env.Eip - 1);
            env.Eip = (uint)txtEditor.ActiveTextAreaControl.Caret.Line + 1;
        }

        private void SelectLine(int line)
        {
            // select next line
            txtEditor.ActiveTextAreaControl.Caret.Position = new TextLocation(0, line - 1);
        }

        /// <summary>
        /// Saves the execution state to the specified file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuSaveSnapshot_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Snapshot Files (*.snp)|*.snp";
                sfd.FileName = "x86 Assembly Interpreter Snapshot.snp";
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    using (GZipStream gz = new GZipStream(fs, CompressionMode.Compress))
                    using (StreamWriter sw = new StreamWriter(gz))
                    {
                        // store execution environment
                        new BinaryFormatter().Serialize(gz, env);

                        // store code
                        sw.Write(txtEditor.Text);
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to load a saved execution state from the specified file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuLoadSnapshot_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Snapshot Files (*.snp)|*.snp";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (GZipStream gz = new GZipStream(fs, CompressionMode.Decompress))
                    using (StreamReader sr = new StreamReader(gz))
                    {
                        // load execution environment
                        env = (ExecutionEnvironment)new BinaryFormatter().Deserialize(gz);
                        uint eip = env.Eip;

                        // load code
                        txtEditor.Text = sr.ReadToEnd();
                        StartExecution();
                        SelectLine((int)eip);
                    }
                }
            }
        }
    }
}
