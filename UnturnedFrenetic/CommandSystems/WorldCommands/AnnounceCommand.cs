using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using Steamworks;

namespace UnturnedFrenetic.CommandSystems.WorldCommands
{
    public class AnnounceCommand: AbstractCommand
    {
        // <--[command]
        // @Name announce
        // @Arguments <color> <message> [mode]
        // @Short Announces a message to all players (in chat).
        // @Updated 2015/12/31
        // @Authors Morphan1
        // @Group World
        // @Minimum 2
        // @Maximum 3
        // @Description
        // This sends a chat message to all online players, in a specified color.
        // The default value for the 'mode' parameter is SAY.
        // All possible 'mode' values: GLOBAL, LOCAL, GROUP, WELCOME, SAY.
        // TODO: Explain more!
        // @Example
        // // This announces to all players that a player has logged on.
        // announce YELLOW "<{var[player].name}> has logged on!"
        // -->

        public AnnounceCommand()
        {
            Name = "announce";
            Arguments = "<color> <message> [mode]";
            Description = "Announces a message to all players (in chat).";
            MinimumArguments = 2;
            MaximumArguments = 3;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>(); // TODO: Some validate of data / types?
        }

        public override void Execute(FreneticScript.CommandSystem.CommandQueue queue, CommandEntry entry)
        {
            TemplateObject tcolor = entry.GetArgumentObject(queue, 0);
            ColorTag color = ColorTag.For(tcolor);
            if (color == null)
            {
                queue.HandleError(entry, "Invalid color: " + TagParser.Escape(tcolor.ToString()));
                return;
            }
            string message = entry.GetArgument(queue, 1);
            EChatMode chatMode = EChatMode.SAY;
            if (entry.Arguments.Count > 2)
            {
                string mode = entry.GetArgument(queue, 2);
                try
                {
                    chatMode = (EChatMode)Enum.Parse(typeof(EChatMode), mode.ToUpper());
                } catch (ArgumentException)
                {
                    queue.HandleError(entry, "Invalid chat mode: " + mode);
                    return;
                }
            }
            ChatManager.manager.channel.send("tellChat", ESteamCall.OTHERS, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[]
            {
                CSteamID.Nil,
                (byte)chatMode,
                color.Internal,
                message
            });
        }
    }
}
