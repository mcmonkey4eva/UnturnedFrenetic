using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.CommandSystem;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;

namespace UnturnedFrenetic.CommandSystems.PlayerCommands
{
    public class TellCommand: AbstractCommand
    {
        // <--[command]
        // @Name tell
        // @Arguments <receiving player>|... <color> <chatting player> <message>
        // @Short Tells a player a message (in chat).
        // @Updated 2015/12/11
        // @Authors mcmonkey
        // @Group Player
        // @Description
        // This sends a chat message supposedly from a player to an actual player or group of players.
        // TODO: Explain more!
        // @Example
        // // This tells mcmonkey that Morphan1 said "hello there", in white.
        // tell mcmonkey WHITE Morphan1 "hello there";
        // -->

        public TellCommand()
        {
            Name = "tell";
            Arguments = "<receiving player>|... <color> <chatting player> <message>";
            Description = "Tells a player a message (in chat).";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 3)
            {
                ShowUsage(entry);
                return;
            }
            ListTag players = ListTag.For(entry.GetArgument(0));
            string tcolor = entry.GetArgument(1);
            ColorTag color = ColorTag.For(tcolor);
            if (color == null)
            {
                entry.Bad("Invalid color: " + TagParser.Escape(tcolor));
                return;
            }
            string tchatter = entry.GetArgument(2);
            PlayerTag chatter = PlayerTag.For(tchatter);
            if (chatter == null)
            {
                entry.Bad("Invalid chatting player: " + TagParser.Escape(tchatter));
                return;
            }
            string message = entry.GetArgument(3);
            foreach (TemplateObject tplayer in players.ListEntries)
            {
                PlayerTag player = PlayerTag.For(tplayer.ToString());
                if (player == null)
                {
                    entry.Bad("Invalid player: " + TagParser.Escape(tplayer.ToString()));
                    continue;
                }
                ChatManager.manager.channel.send("tellChat", player.Internal.playerID.steamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER,
                    chatter.Internal.playerID.steamID, (byte)0 /* TODO: Configurable mode? */, color.Internal, message);
            }
        }
    }
}
