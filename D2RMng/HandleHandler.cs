using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D2RMulti
{
    internal class HandleHandler
    {
        public static bool FindAndDeleteHandler()
        {
            Process[] allD2R = Process.GetProcessesByName("D2R");
            foreach (Process singleD2r in allD2R)
            {
                FindAndDeleteHandle(singleD2r.Id);                 
            }
            return true;
        }
        public static bool KillAllD2R()        
        {
            Process[] allD2R = Process.GetProcessesByName("D2R");
            foreach (Process singleD2r in allD2R)
            {
                singleD2r.Kill();
            }
            return false;
        }

        private static bool FindAndDeleteHandle(int d2rID)
        {            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = @"/c handle -p " + d2rID.ToString() + " -a"; //handle -p D2R     notepad
            p.StartInfo.Verb = "runas";
            p.Start();
            // Read the output stream first and then wait.
            string results = p.StandardOutput.ReadToEnd();
            string error = p.StandardError.ReadToEnd();
            p.WaitForExit();

            string[] lines = results.Split(
              new string[] { "\r\n", "\r", "\n" },
              StringSplitOptions.None
            );

            var target = "DiabloII Check For Other Instances";
            string[] foundhandles = Array.FindAll(lines, s => s.Contains(target));

            if (foundhandles.Length != 1) { return false; };

            string[] adressarray = foundhandles[0].Split(
                new string[] { ":" },
                StringSplitOptions.None
            );
            var adress = adressarray[0];

            p = new Process();
             
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = @"/c handle -p " + d2rID.ToString() + " -c " + adress.Trim(' ') + " -y";
            p.StartInfo.Verb = "runas";
            p.Start();
            // Read the output stream first and then wait.
            p.WaitForExit();

            return true; 


        }
    }
}
