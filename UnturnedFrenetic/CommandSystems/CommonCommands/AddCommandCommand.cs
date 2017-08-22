using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using FreneticScript;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using UnturnedFrenetic.UnturnedCommands;

namespace UnturnedFrenetic.CommandSystems.CommonCommands
{
    class AddCommandCommand : AbstractCommand
    {
        public override void AdaptBlockFollowers(CommandEntry entry, List<CommandEntry> input, List<CommandEntry> fblock)
        {
            input.Clear();
            base.AdaptBlockFollowers(entry, input, fblock);
        }

        // <--[command]
        // @Name addcommand
        // @Arguments 'player'/'server' <name> <help>
        // @Short Registers a command into the game itself.
        // @Updated 2016/04/30
        // @Authors mcmonkey
        // @Group Common
        // @Braces Always
        // @Minimum 3
        // @Maximum 3
        // @Description
        // Registers a command into the game itself.
        // TODO: Explain more!
        // @Example
        // // This adds the "hello" command.
        // addcommand server "hello" "Says hello to the console"
        // {
        //     echo "hello";
        // }
        // @BlockVar player PlayerTag returns the admin player that executed this command, if any.
        // @BlockVar is_server BooleanTag returns whether the server executed this command (if not, an admin did).
        // @BlockVar raw_arguments TextTag returns the plain text of the input arguments.
        // -->

        public AddCommandCommand()
        {
            Name = "addcommand";
            Arguments = "'player'/'server' <name> <help>";
            Description = "Registers a command into the game itself.";
            MinimumArguments = 1;
            MaximumArguments = 3;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TextTag.For,
                TextTag.For,
                TextTag.For
            };
        }

        public override void Execute(FreneticScript.CommandSystem.CommandQueue queue, CommandEntry entry)
        {
            if (entry.Arguments[0].ToString() == "\0CALLBACK")
            {
                return;
            }
            if (entry.Arguments.Count < 3)
            {
                ShowUsage(queue, entry);
                return;
            }
            bool servermode = entry.GetArgument(queue, 0).ToLowerFast() == "server";
            string name = entry.GetArgument(queue, 1).ToLowerFast();
            string help = entry.GetArgument(queue, 2);
            if (entry.InnerCommandBlock == null)
            {
                queue.HandleError(entry, "Event command invalid: No block follows!");
                return;
            }
            // NOTE: Commands are compiled!
            CommandScript script = new CommandScript("ut_command_" + name, entry.InnerCommandBlock, entry.BlockStart, queue.CurrentEntry.Types, true) { Debug = DebugMode.MINIMAL };
            UnturnedCustomCommand ucc = new UnturnedCustomCommand(name, help, script);
            if (servermode)
            {
                Commander.commands.Insert(1, ucc);
            }
            else
            {
                UnturnedFreneticMod.Instance.PlayerCommands.Add(ucc);
            }
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "Registered command!");
            }
        }
    }
}
