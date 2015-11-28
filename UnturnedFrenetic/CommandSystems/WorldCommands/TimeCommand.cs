using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic;
using Frenetic.CommandSystem;

namespace UnturnedFrenetic.CommandSystems.WorldCommands
{
    public class TimeCommand : AbstractCommand
    {
        public TimeCommand()
        {
            Name = "time";
            Arguments = "<time>";
            Description = "Changes the current in-game time.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 1)
            {
                ShowUsage(entry);
                return;
            }
            uint ti = Utilities.StringToUInt(entry.GetArgument(0));
            SDG.Unturned.LightingManager.time = ti;
            entry.Good("World time set to " + ti + "!");
        }
    }
}
