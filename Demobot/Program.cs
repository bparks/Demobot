using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demobot
{
   static class Program
   {
      static Mutex mutex = new Mutex(true, "{af39805e-889b-4fb5-8a5b-890f7152b35f}");

      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         if (mutex.WaitOne(TimeSpan.Zero, true))
         {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Demobot());
            mutex.ReleaseMutex();
         }
         else
         {
            // send our Win32 message to make the currently running instance
            // jump on top of all the other windows
            NativeMethods.PostMessage(
                (IntPtr)NativeMethods.HWND_BROADCAST,
                NativeMethods.WM_SHOWME,
                IntPtr.Zero,
                IntPtr.Zero);
         }
      }
   }

   // this class just wraps some Win32 stuff that we're going to use
   internal class NativeMethods
   {
      public const int HWND_BROADCAST = 0xffff;
      public static readonly int WM_SHOWME = RegisterWindowMessage("WM_SHOWME");
      [DllImport("user32")]
      public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
      [DllImport("user32")]
      public static extern int RegisterWindowMessage(string message);
   }
}
