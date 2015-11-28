using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic;
using Frenetic.CommandSystem;
using Frenetic.TagHandlers;

using System.IO;

namespace UnturnedFrenetic.CommandSystems
{
    public class UnturnedFreneticOutputter : Outputter
    {
        public UnturnedFreneticMod TheMod;

        public override void Bad(string tagged_text, DebugMode mode)
        {
            if (mode <= DebugMode.MINIMAL)
            {
                SysConsole.Output(OutputType.WARNING, TheMod.CommandSystem.System.TagSystem.ParseTagsFromText(tagged_text, "^r^3", null, mode));
            }
        }

        public override void Good(string tagged_text, DebugMode mode)
        {
            if (mode <= DebugMode.FULL)
            {
                SysConsole.Output(OutputType.INFO, TheMod.CommandSystem.System.TagSystem.ParseTagsFromText(tagged_text, "^r^7", null, mode));
            }
        }

        public override string ReadTextFile(string name)
        {
            return File.ReadAllText(name.Replace("..", "_")); // TODO: Proper sandbox!
        }

        public override void UnknownCommand(CommandQueue queue, string basecommand, string[] arguments)
        {
            Bad("Invalid command: " + TagParser.Escape(basecommand) + "!", queue.Debug);
        }

        public override void WriteLine(string text)
        {
            SysConsole.WriteLine(text);
        }
    }
}
