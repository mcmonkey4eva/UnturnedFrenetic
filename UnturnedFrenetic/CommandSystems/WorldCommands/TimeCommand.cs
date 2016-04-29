using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;

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
        // @Minimum 1
        // @Maximum 1
        // @Description
        // This sets the world time to the specified unsigned integer value.
        // TODO: Explain more!
        // @Example
        // // This sets the time to a daylight hour.
        // time 0;
        // @Example
        // // This sets the time to a nighttime hour.
        // time 25000;
        // -->

        public TimeCommand()
        {
            Name = "time";
            Arguments = "<time>";
            Description = "Changes the current in-game time.";
            MinimumArguments = 1;
            MaximumArguments = 1;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                IntegerTag.TryFor
            };
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            IntegerTag itag = IntegerTag.TryFor(entry.GetArgumentObject(queue, 0));
            uint ti = (uint)itag.Internal;
            SDG.Unturned.LightingManager.time = ti;
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "World time set to " + ti + "!");
            }
        }
    }
}
