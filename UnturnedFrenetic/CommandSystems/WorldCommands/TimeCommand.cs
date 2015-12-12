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
        // <--[command]
        // @Name time
        // @Arguments <time>
        // @Short Changes the world time.
        // @Updated 2015/11/28
        // @Authors mcmonkey
        // @Group World
        // @Description
        // This sets the world time to the specified unsigned integer value.
        // TODO: Explain more!
        // @Example
        // // This sets the time to a daylight hour.
        // time 0;
        // @Example
        // // This sets the time tot a nighttime hour.
        // time 25000;
        // -->

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
