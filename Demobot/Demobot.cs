using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demobot
{
   public partial class Demobot : Form
   {
      public Demobot()
      {
         InitializeComponent();
      }

      private void openToolStripMenuItem_Click(object sender, EventArgs e)
      {
         openFileDialog1.ShowDialog();

         if (String.IsNullOrEmpty(openFileDialog1.FileName))
            return;

         if (!File.Exists(openFileDialog1.FileName))
            return;

         snippets.DataSource = LoadSnippets(openFileDialog1.FileName);
      }

      private string[] LoadSnippets(string fname)
      {
         using (var sw = new StreamReader(fname))
         {
            return sw.ReadToEnd()
               .Split(new[] { "\r\n---\r\n" }, StringSplitOptions.RemoveEmptyEntries)
               .Select(t => t.Trim())
               .Where(t => !String.IsNullOrWhiteSpace(t))
               .ToArray();
         }
      }

      private void snippets_DoubleClick(object sender, EventArgs e)
      {
         Clipboard.SetData(DataFormats.StringFormat, snippets.SelectedItem.ToString());
      }

      protected override void WndProc(ref Message m)
      {
         if (m.Msg == NativeMethods.WM_SHOWME)
         {
            ShowMe();
         }
         base.WndProc(ref m);
      }

      // This form is supposed to be topmost anyway, but I'll leave this here until I know better what everything does...
      private void ShowMe()
      {
         if (WindowState == FormWindowState.Minimized)
         {
            WindowState = FormWindowState.Normal;
         }
         // get our current "TopMost" value (ours will always be false though)
         bool top = TopMost;
         // make our form jump to the top of everything
         TopMost = true;
         // set it back to whatever it was
         TopMost = top;
      }
   }
}
