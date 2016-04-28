using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using FreneticScript.TagHandlers.Objects;
using FreneticScript.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.PlayerCommands
{
    public class UseItemCommand : AbstractCommand
    {
        // <--[command]
        // @Name useitem
        // @Arguments <player> 'primary'/'secondary' <boolean on/off>
        // @Short Makes a player use their held item.
        // @Warning The player might not see their own actions occur!
        // @Updated 2016/04/28
        // @Authors mcmonkey
        // @Group Player
        // @Minimum 3
        // @Maximum 3
        // @Description
        // Makes a player use their held item.
        // Specify whether to use the 'primary' or 'secondary' mode of the item.
        // 'Primary' would fire a gun, while 'secondary' would aim down the sights.
        // Also specify whether to start or stop the usage with a boolean.
        // TODO: Explain more!
        // @Example
        // // This example causes a player to appear to aim down a gun's sights.
        // useitem <{var[player]}> secondary true;
        // @Example
        // // This example causes a player to fires a gun.
        // useitem <{var[player]}> primary true;
        // wait 1;
        // useitem <{var[player]}> primary false;
        // -->
        public UseItemCommand()
        {
            Name = "useitem";
            Arguments = "<player> 'primary'/'secondary' <boolean on/off>";
            Description = "Makes a player use their held item.";
            MinimumArguments = 3;
            MaximumArguments = 3;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                (input) => input,
                (input) =>
                {
                    string low = input.ToString().ToLowerFast();
                    if (low == "primary" || low == "secondary")
                    {
                        return new TextTag(low);
                    }
                    return null;
                },
                (input) =>
                {
                    return BooleanTag.TryFor(input);
                }
            };
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            PlayerTag player = PlayerTag.For(entry.GetArgument(queue, 0));
            bool primary = entry.GetArgument(queue, 1) == "primary";
            bool start = BooleanTag.TryFor(entry.GetArgumentObject(queue, 2)).Internal;
            if (player.Internal.player.equipment.useable == null)
            {
                entry.Bad(queue, "Failed to use item, holding nothing.");
                return;
            }
            if (primary)
            {
                if (start)
                {
                    player.Internal.player.equipment.useable.startPrimary();
                }
                else
                {
                    player.Internal.player.equipment.useable.stopPrimary();
                }
            }
            else
            {
                if (start)
                {
                    player.Internal.player.equipment.useable.startSecondary();
                }
                else
                {
                    player.Internal.player.equipment.useable.stopSecondary();
                }
            }
            //player.Internal.player.equipment.useable.tick();
            player.Internal.player.equipment.useable.tock(player.Internal.player.input.clock);
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "Player is " + (start ? "now" : "no longer") + " using the " + (primary ? "primary" : "secondary") + " mode on their held item!");
            }
        }
    }
}
