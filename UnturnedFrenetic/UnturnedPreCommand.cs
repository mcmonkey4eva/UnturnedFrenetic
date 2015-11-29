using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;

namespace UnturnedFrenetic
{
    class UnturnedGenericCommand : Command
    {
        public UnturnedGenericCommand()
        {
            this._command = "generic";
            this._info = "Backup if all other commands fail!";
            this._help = "Backup if all other commands fail!";
        }

        public override bool check(Steamworks.CSteamID executorID, string method, string parameter)
        {
            return Execute(executorID, method, parameter);
        }

        protected override void execute(Steamworks.CSteamID executorID, string parameter)
        {
            Execute(executorID, null, parameter);
        }

        public bool Execute(Steamworks.CSteamID executorID, string method, string parameter)
        {
            // TODO: Fire unknown command event!
            SysConsole.Output(OutputType.WARNING, "Unknown command: " + method + " " + parameter);
            return true;
        }
    }
}
