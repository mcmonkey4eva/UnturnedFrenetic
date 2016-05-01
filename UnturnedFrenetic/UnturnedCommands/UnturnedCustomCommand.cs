using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using FreneticScript;
using FreneticScript.CommandSystem;
using Steamworks;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript.TagHandlers.Objects;

namespace UnturnedFrenetic.UnturnedCommands
{
    public class UnturnedCustomCommand : Command
    {
        public CommandScript ExecScript;

        public UnturnedCustomCommand(string name, string help, CommandScript script)
        {
            this._command = name;
            this._help = help;
            this._info = help;
            ExecScript = script;
        }

        protected override void execute(CSteamID executorID, string parameter)
        {
            CommandQueue queue = ExecScript.ToQueue(UnturnedFreneticMod.Instance.CommandSystem.System);
            MapTag map = new MapTag();
            if (executorID == CSteamID.Nil)
            {
                map.Internal["is_server"] = new BooleanTag(true);
            }
            else
            {
                map.Internal["player"] = new PlayerTag(PlayerTool.getSteamPlayer(executorID));
                map.Internal["is_server"] = new BooleanTag(false);
            }
            map.Internal["raw_arguments"] = new TextTag(parameter);
            queue.SetVariable("context", map);
            queue.CommandStack.Peek().Debug = DebugMode.MINIMAL; // Just in case
            queue.Execute();
        }
    }
}
