using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.CommandSystem;
using SDG.Unturned;
using UnturnedFrenetic.TagSystems.TagObjects;
using Frenetic.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.PlayerCommands
{
    public class TakeCommand: AbstractCommand
    {
        // <--[command]
        // @Name take
        // @Arguments <player> <item> [amount]
        // @Short Takes the specified item from the player.
        // @Updated 2015/12/07
        // @Authors Morphan1
        // @Group World
        // @Description
        // This removes the specified item with an optional amount argument (default 1).
        // TODO: Explain more!
        // @Example
        // // This removes a single ham sandwich from mcmonkey's inventory.
        // take mcmonkey sandwich_ham;
        // -->

        public TakeCommand()
        {
            Name = "take";
            Arguments = "<player> <item> [amount]";
            Description = "Takes the specified item from the player.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 2)
            {
                ShowUsage(entry);
                return;
            }
            PlayerTag player = PlayerTag.For(entry.GetArgument(0));
            if (player == null)
            {
                entry.Bad("Invalid player!");
                return;
            }
            ItemAssetTag item = ItemAssetTag.For(entry.GetArgument(1));
            if (item == null)
            {
                entry.Bad("Invalid item!");
                return;
            }
            byte amount = 1;
            if (entry.Arguments.Count > 2)
            {
                amount = (byte)Utilities.StringToUInt(entry.GetArgument(2));
            }
            PlayerInventory inventory = player.Internal.player.inventory;
            byte remainingAmount = amount;
            InventorySearch search;
            while (remainingAmount > 0 && (search = inventory.has(item.Internal.id)) != null)
            {
                if (search.jar.item.amount <= remainingAmount)
                {
                    inventory.removeItem(search.page, inventory.getIndex(search.page, search.jar.x, search.jar.y));
                    remainingAmount -= search.jar.item.amount;
                }
                else
                {
                    inventory.sendUpdateAmount(search.page, search.jar.x, search.jar.y, (byte)(search.jar.item.amount - remainingAmount));
                    remainingAmount = 0;
                }
            }
            if (remainingAmount == 0)
            {
                entry.Good("Successfully took " + amount + " " + TagParser.Escape(item.Internal.name) + "!");
            }
            else if (remainingAmount < amount)
            {
                entry.Good("Successfully took " + (amount - remainingAmount) + " " + TagParser.Escape(item.Internal.name) + "! (" + remainingAmount + " more not found!)");
            }
            else
            {
                entry.Bad("Failed to take item (does the inventory contain any?)!");
            }
        }
    }
}
