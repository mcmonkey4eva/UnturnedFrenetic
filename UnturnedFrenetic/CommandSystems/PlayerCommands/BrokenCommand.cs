using System;
using System.Collections.Generic;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using FreneticScript.TagHandlers.Objects;
using FreneticScript.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class BrokenCommand : AbstractCommand
    {
        // <--[command]
        // @Name broken
        // @Arguments <player> <boolean>
        // @Short Breaks or mends a player's leg bones.
        // @Updated 2016/04/27
        // @Authors Morphan1
        // @Group Player
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Specify a boolean value that when true breaks a player's legs.
        // When false, it fixes their broken legs.
        // TODO: Explain more!
        // @Example
        // // This breaks the player's legs.
        // broken <{var[player]}> true;
        // -->
        public BrokenCommand()
        {
            Name = "broken";
            Arguments = "<player> <boolean>";
            Description = "Breaks or mends a player's leg bones.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                BooleanTag.TryFor
            };
        }

        public override void Execute(FreneticScript.CommandSystem.CommandQueue queue, CommandEntry entry)
        {
            try
            {
                BooleanTag boolean = BooleanTag.TryFor(entry.GetArgumentObject(queue, 1));
                if (boolean == null)
                {
                    queue.HandleError(entry, "Invalid boolean!");
                    return;
                }
                PlayerTag player = PlayerTag.For(entry.GetArgument(queue, 0));
                if (player == null)
                {
                    queue.HandleError(entry, "Invalid player!");
                    return;
                }
                bool value = boolean.Internal;
                PlayerLife life = player.Internal.player.life;
                if (life.isBroken != value)
                {
                    if (value)
                    {
                        life.breakLegs();
                    }
                    else
                    {
                        life._isBroken = false;
                        life.channel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                        {
                            life._isBroken
                        });
                    }
                    entry.Good(queue, "Successfully adjusted the broken legs of player " + TagParser.Escape(player.ToString()) + " to " + TagParser.Escape(boolean.ToString()) + "!");
                }
                else
                {
                    entry.Good(queue, "Player " + TagParser.Escape(player.ToString()) + " already has their broken legs set to " + TagParser.Escape(boolean.ToString()) + "!");
                }
            }
            catch (Exception ex) // TODO: Necessity?
            {
                queue.HandleError(entry, "Failed to adjust player's broken legs: " + ex.ToString());
            }
        }
    }
}
