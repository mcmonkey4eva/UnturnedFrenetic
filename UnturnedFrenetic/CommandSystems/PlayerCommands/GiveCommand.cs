﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.CommandSystem;
using SDG.Unturned;
using UnturnedFrenetic.TagSystems.TagObjects;
using Frenetic.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.PlayerCommands
{
    class GiveCommand: AbstractCommand
    {
        public GiveCommand()
        {
            Name = "give";
            Arguments = "<player> <item> [amount]";
            Description = "Gives a player the specified item.";
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
            ushort item = Utilities.StringToUShort(entry.GetArgument(1));
            byte amount = 1;
            if (entry.Arguments.Count > 2)
            {
                amount = (byte)Utilities.StringToUInt(entry.GetArgument(2));
            }
            if (ItemTool.tryForceGiveItem(player.Internal.player, item, amount))
            {
                ItemAsset itemAsset = (ItemAsset)Assets.find(EAssetType.ITEM, item);
                entry.Good("Successfully gave a " + TagParser.Escape(itemAsset.name) + "!");
            }
            else
            {
                entry.Bad("Failed to give item (is the inventory full?)!");
            }
        }
    }
}