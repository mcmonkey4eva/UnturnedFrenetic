using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;

namespace UnturnedFrenetic.UnturnedCommands
{
    class UnturnedPreCommand : Command
    {
        public UnturnedPreCommand()
        {
            this._command = "precmd";
            this._info = "Fires before all other commands!";
            this._help = "Fires before all other commands!";
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
            // TODO: Fire input command event!
            return false;
        }
    }
}
