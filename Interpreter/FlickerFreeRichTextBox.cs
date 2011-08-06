using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;

namespace Interpreter
{
    // modified version of http://www.c-sharpcorner.com/UploadFile/mgold/ColorSyntaxEditor12012005235814PM/ColorSyntaxEditor.aspx
    public class FlickerFreeRichTextBox : RichTextBox
    {
        /// <summary>
        /// Blocks painting of the control if set true.
        /// </summary>
        private bool IsUpdating;

        public FlickerFreeRichTextBox()
        {
            // set instance non-public property with name "DoubleBuffered" to true
            typeof(Control).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, this, new object[] { true });
        }

        protected override void WndProc(ref Message m)
        {
            // Code courtesy of Mark Mihevc
            // sometimes we want to eat the paint message so we don't have to see all the
            // flicker from when we select the text to change the color.
            if (m.Msg == 15)    // WM_PAINT
            {
                if (!IsUpdating) base.WndProc(ref m); // if we decided to paint this control, just call the RichTextBox WndProc
                else m.Result = IntPtr.Zero; // not painting, must set this to IntPtr.Zero if not painting otherwise serious problems.
            }
            else base.WndProc (ref m); // message other than WM_PAINT, just do what you normally do.
        }

        public void BeginUpdate()
        {
            IsUpdating = true;
        }
        public void EndUpdate()
        {
            IsUpdating = false;
        }

        public void HighlightLine(int line)
        {
            // preserve old selection
            int oldSelectionStart = SelectionStart;
            int oldSelectionLength = SelectionLength;
         

            // notify the control to stop painting any updates
            BeginUpdate();

            // clear any old highlights
            SelectAll();
            SelectionBackColor = BackColor;

            try
            {
                // select the entire line
                int lineStart = GetFirstCharIndexFromLine(line - 1);
                if (lineStart != -1)
                {
                    int lineEnd = GetFirstCharIndexFromLine(line);
                    if (lineEnd == -1)
                    {
                        lineEnd = TextLength;
                    }
                    Select(lineStart, lineEnd - lineStart);

                    // highlight it yellow
                    SelectionBackColor = Color.Yellow;
                }
                else
                {
                    // todo: eol
                }
            }
            catch { }
            finally
            {
                //SelectionStart = GetFirstCharIndexFromLine(line);
                //this.ScrollToCaret();

                // restore the old selection
                SelectionStart = oldSelectionStart;
                SelectionLength = oldSelectionLength;

                // let the control paint again
                EndUpdate();
            }
        }

        public void ClearHighlights()
        {
            // preserve old selection
            int oldSelectionStart = SelectionStart;
            int oldSelectionLength = SelectionLength;

            // notify the control to stop painting any updates
            BeginUpdate();

            // clear any old highlights
            SelectAll();
            SelectionBackColor = BackColor;

            // restore the old selection
            SelectionStart = oldSelectionStart;
            SelectionLength = oldSelectionLength;

            // let the control paint again
            EndUpdate();
        }

        public void HighlightAsmSyntax()
        {
            BeginUpdate();

            // todo: go through each line selecting certain words and coloring accordingly

            EndUpdate();
        }



    }
}
