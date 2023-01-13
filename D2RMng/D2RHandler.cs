using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D2RMulti
{
    internal class D2RHandler
    {
        public static bool startInstance(string Path, string User, string Password, string Area, out string error)
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
            d2r.StartInfo.Verb = "runas";
            d2r.Start();
            return false;
        }

    }
}
