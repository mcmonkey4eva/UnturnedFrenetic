using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using FreneticScript;
using FreneticScript.CommandSystem;

namespace UnturnedFrenetic.UnturnedCommands
{
    class UnturnedReloadCommand : Command
    {
        public UnturnedReloadCommand()
        {
            this._command = "reload";
            this._info = "Reload: Reloads the FreneticScript system.";
            this._help = "Reload: Reloads the FreneticScript system.";
        }
        
        protected override void execute(Steamworks.CSteamID executorID, string parameter)
        {
            // Dedicator.isDedicated?
            if (!Provider.isServer)
            {
                CommandWindow.LogError("You are not the server!");
                return;
            }
            Commands sys = UnturnedFreneticMod.Instance.CommandSystem.System;
            // TODO: FreneticScript Core methods to ease this.
            foreach (ScriptEvent evt in sys.Events.Values)
            {
                foreach (KeyValuePair<int, CommandScript> handl in new List<KeyValuePair<int, CommandScript>>(evt.Handlers))
                {
                    evt.RemoveEventHandler(handl.Value.Name);
                }
            }
            sys.Functions.Clear();
            for (int i = Commander.commands.Count - 1; i >= 0; i--)
            {
                if (Commander.commands[i] is UnturnedCustomCommand)
                {
                    Commander.commands.RemoveAt(i);
                }
            }
            UnturnedFreneticMod.Instance.PlayerCommands.Clear();
            UnturnedFreneticMod.Instance.AutorunScripts();
            SysConsole.Output(OutputType.INFO, "Reloaded succesfully!");
        }
    }
}
