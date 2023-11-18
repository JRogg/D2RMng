using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace D2RMulti
{
    internal class D2RHandler
    {
        public static bool startInstanceToken(string Path, string token, out string error)
        {
            string command ="\"" + Path +  "\"" + " - NoProfile - ExecutionPolicy Bypass" + "\"-uid osi\"";
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = "powershell.exe";
            processStartInfo.Arguments = $"-Command \"{command}\"";
            processStartInfo.UseShellExecute = false;
            processStartInfo.RedirectStandardOutput = true;
            
            var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            error = output;

            return true;

          /*
            error = "xx";
            Process d2r = new Process();
            d2r.StartInfo.FileName = @Path;
            d2r.StartInfo.Arguments = @"/c" " -NoProfile -ExecutionPolicy Bypass " + "\"-uid osi\"";
        
            d2r.StartInfo.Verb = "runas";
            d2r.Start();
            return false;*/
        }
        public static bool startInstance(string Path, string User, string Password, string Area, bool Filter, out string error)
        {
            error = "";
            if (!File.Exists(Path)) {
                error = ".exe not found";
                return false;
            };
            if ((User.Length == 0)|| (Area.Length == 0)||(Password.Length == 0))
            {
                error = "Data missing";
                return false;
            } 
            HandleHandler.FindAndDeleteHandler();
            Process d2r = new Process();
            d2r.StartInfo.FileName = @Path; 
            d2r.StartInfo.Arguments = @"/c -username " + User +" -password " + Password + " -address " + Area + ".actual.battle.net";
            if (Filter) {
                d2r.StartInfo.Arguments += " -mod Filter - txt";
            }
            d2r.StartInfo.Verb = "runas";
            d2r.Start();
            return false;
        }

    }
}
