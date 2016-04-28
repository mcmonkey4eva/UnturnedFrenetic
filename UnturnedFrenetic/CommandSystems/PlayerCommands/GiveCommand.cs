using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using SDG.Unturned;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.PlayerCommands
{
    public class GiveCommand: AbstractCommand
    {
        // <--[command]
        // @Name give
        // @Arguments <player> <item> [amount]
        // @Short Gives the specified item to the player.
        // @Updated 2015/11/28
        // @Authors Morphan1
        // @Group World
        // @Minimum 2
        // @Maximum 3
        // @Description
        // This adds the specified item with an optional amount argument (default 1).
        // TODO: Explain more!
        // @Example
        // // This adds a single carrot to mcmonkey's inventory.
        // give mcmonkey carrot;
        // -->

        public GiveCommand()
        {
            Name = "give";
            Arguments = "<player> <item> [amount]";
            Description = "Gives a player the specified item.";
            MinimumArguments = 2;
            MaximumArguments = 3;
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            PlayerTag player = PlayerTag.For(entry.GetArgument(queue, 0));
            if (player == null)
            {
                queue.HandleError(entry, "Invalid player!");
                return;
            }
            ItemAssetTag item = ItemAssetTag.For(entry.GetArgument(queue, 1));
            if (item == null)
            {
                queue.HandleError(entry, "Invalid item!");
                return;
            }
            byte amount = 1;
            if (entry.Arguments.Count > 2)
            {
                amount = (byte)Utilities.StringToUInt(entry.GetArgument(queue, 2));
            }
            if (ItemTool.tryForceGiveItem(player.Internal.player, item.Internal.id, amount))
            {
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully gave a " + TagParser.Escape(item.Internal.name) + "!");
                }
            }
            else
            {
                queue.HandleError(entry, "Failed to give item (is the inventory full?)!");
            }
        }
    }
}
