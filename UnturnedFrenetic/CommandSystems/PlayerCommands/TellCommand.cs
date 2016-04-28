using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
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
        // @Minimum 4
        // @Maximum 4
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
            MinimumArguments = 4;
            MaximumArguments = 4;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>(); // TODO: Some validation of data / types.
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            ListTag players = ListTag.For(entry.GetArgument(queue, 0));
            TemplateObject tcolor = entry.GetArgumentObject(queue, 1);
            ColorTag color = ColorTag.For(tcolor);
            if (color == null)
            {
                queue.HandleError(entry, "Invalid color: " + TagParser.Escape(tcolor.ToString()));
                return;
            }
            string tchatter = entry.GetArgument(queue, 2);
            PlayerTag chatter = PlayerTag.For(tchatter);
            if (chatter == null)
            {
                queue.HandleError(entry, "Invalid chatting player: " + TagParser.Escape(tchatter));
                return;
            }
            string message = entry.GetArgument(queue, 3);
            foreach (TemplateObject tplayer in players.ListEntries)
            {
                PlayerTag player = PlayerTag.For(tplayer.ToString());
                if (player == null)
                {
                    queue.HandleError(entry, "Invalid player: " + TagParser.Escape(tplayer.ToString()));
                    continue;
                }
                ChatManager.manager.channel.send("tellChat", player.Internal.playerID.steamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER,
                    chatter.Internal.playerID.steamID, (byte)0 /* TODO: Configurable mode? */, color.Internal, message);
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully sent a message.");
                }
            }
        }
    }
}
