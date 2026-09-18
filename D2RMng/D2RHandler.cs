using D2RMng;
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
        /* public static bool startInstanceToken(string Path, string token, out string error)
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

         
            error = "xx";
            Process d2r = new Process();
            d2r.StartInfo.FileName = @Path;
            d2r.StartInfo.Arguments = @"/c" " -NoProfile -ExecutionPolicy Bypass " + "\"-uid osi\"";
        
            d2r.StartInfo.Verb = "runas";
            d2r.Start();
            return false;
        
         }
       */
    
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

            string decryptedPassword = PWHelper.Decrypt(Password);
            if (decryptedPassword.Length > 20)
            {
                error = "Password too long (D2R's -password launch parameter silently truncates at 23 characters, causing login to fail) - shorten the Battle.net password to 20 characters or fewer";
                return false;
            }

            HandleHandler.FindAndDeleteHandler();
            Process d2r = new Process();
            d2r.StartInfo.FileName = @Path;
            d2r.StartInfo.Arguments = "/c -username " + EscapeArgument(User) + " -password " + EscapeArgument(decryptedPassword) + " -address " + EscapeArgument(Area + ".actual.battle.net");
            if (Filter) {
                d2r.StartInfo.Arguments += " -direct -txt";
            }
            d2r.StartInfo.Verb = "runas";
            d2r.Start();
            return true;
        }

        // Quotes/escapes a value for use in a Win32 process command line (CommandLineToArgvW rules),
        // so usernames/passwords containing spaces or quotes are passed as a single argument intact.
        private static string EscapeArgument(string argument)
        {
            if (argument.Length != 0 && argument.IndexOfAny(new[] { ' ', '\t', '\n', '\v', '"' }) == -1)
            {
                return argument;
            }

            var sb = new StringBuilder();
            sb.Append('"');
            int idx = 0;
            while (idx < argument.Length)
            {
                char c = argument[idx++];
                if (c == '\\')
                {
                    int numBackSlash = 1;
                    while (idx < argument.Length && argument[idx] == '\\')
                    {
                        numBackSlash++;
                        idx++;
                    }
                    if (idx == argument.Length)
                    {
                        sb.Append('\\', numBackSlash * 2);
                    }
                    else if (argument[idx] == '"')
                    {
                        sb.Append('\\', numBackSlash * 2 + 1);
                        sb.Append('"');
                        idx++;
                    }
                    else
                    {
                        sb.Append('\\', numBackSlash);
                    }
                }
                else if (c == '"')
                {
                    sb.Append('\\');
                    sb.Append('"');
                }
                else
                {
                    sb.Append(c);
                }
            }
            sb.Append('"');
            return sb.ToString();
        }

    }
}
