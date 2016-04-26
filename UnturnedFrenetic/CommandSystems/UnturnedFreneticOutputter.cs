using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;

using System.IO;

namespace UnturnedFrenetic.CommandSystems
{
    public class UnturnedFreneticOutputter : Outputter
    {
        public UnturnedFreneticMod TheMod;

        public Commands Syst;

        public override void Bad(string tagged_text, DebugMode mode)
        {
            if (mode <= DebugMode.MINIMAL)
            {
                SysConsole.Output(OutputType.WARNING, Syst.TagSystem.ParseTagsFromText(tagged_text, "^r^3", new Dictionary<string, TemplateObject>(), mode, (o) => { throw new Exception(o); }, true));
            }
        }

        public override void Good(string tagged_text, DebugMode mode)
        {
            SysConsole.Output(OutputType.INFO, Syst.TagSystem.ParseTagsFromText(tagged_text, "^r^2", new Dictionary<string, TemplateObject>(), mode, (o) => { throw new Exception(o); }, true));
        }

        public override string ReadTextFile(string name)
        {
            return File.ReadAllText(Environment.CurrentDirectory + "/scripts/" + name.Replace("..", "_")); // TODO: Proper sandbox!
        }

        public override void UnknownCommand(CommandQueue queue, string basecommand, string[] arguments)
        {
            Bad("Invalid command: " + TagParser.Escape(basecommand) + "!", queue.CommandStack.Count > 0 ? queue.CommandStack.Peek().Debug : DebugMode.FULL);
        }

        public override void WriteLine(string text)
        {
            SysConsole.WriteLine(text);
        }
    }
}
