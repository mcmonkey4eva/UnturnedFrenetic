using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.CommandSystem;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
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
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 2)
            {
                ShowUsage(entry);
                return;
            }
            TemplateObject tcolor = entry.GetArgumentObject(0);
            // TODO: better way to get a tagdata
            ColorTag color = ColorTag.For(new TagData(entry.Command.CommandSystem.TagSystem, (List<TagBit>)null, null, null, DebugMode.FULL, (o) => { throw new Exception(o); }), tcolor);
            if (color == null)
            {
                entry.Bad("Invalid color: " + TagParser.Escape(tcolor.ToString()));
                return;
            }
            string message = entry.GetArgument(1);
            EChatMode chatMode = EChatMode.SAY;
            if (entry.Arguments.Count > 2)
            {
                string mode = entry.GetArgument(2);
                try
                {
                    chatMode = (EChatMode)Enum.Parse(typeof(EChatMode), mode.ToUpper());
                } catch (ArgumentException)
                {
                    entry.Bad("Invalid chat mode: " + mode);
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
