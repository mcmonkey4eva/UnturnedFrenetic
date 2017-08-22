using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using SDG.Unturned;
using UnityEngine;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class ItemTag : TemplateObject
    {
        // <--[object]
        // @Type ItemTag
        // @SubType EntityTag
        // @Group Entities
        // @Description Represents a spawned item in the world.
        // -->

        public InteractableItem Internal;

        public ItemTag(InteractableItem item)
        {
            Internal = item;
        }

        public static ItemTag For(int instanceID)
        {
            for (byte x = 0; x < Regions.WORLD_SIZE; x++)
            {
                for (byte y = 0; y < Regions.WORLD_SIZE; y++)
                {
                    foreach (ItemDrop drop in ItemManager.regions[x, y].drops)
                    {
                        InteractableItem item = drop.interactableItem;
                        if (instanceID == item.gameObject.GetInstanceID())
                        {
                            return new ItemTag(item);
                        }
                    }
                }
            }
            return null;
        }

        public override TemplateObject Handle(TagData data)
        {
            if (data.Remaining == 0)
            {
                return this;
            }
            switch (data[0])
            {
                // <--[tag]
                // @Name ItemTag.asset
                // @Group General Information
                // @ReturnType ItemAssetTag
                // @Returns the item asset that this item is based off.
                // @Example "2" .asset returns "Rifle_Maple".
                // -->
                case "asset":
                    return new ItemAssetTag(Internal.asset).Handle(data.Shrink());
                // <--[tag]
                // @Name ItemTag.amount
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the amount of the item.
                // @Example "2" .amount returns "1".
                // -->
                case "amount":
                    return new NumberTag(Internal.item.amount).Handle(data.Shrink());
                // <--[tag]
                // @Name ItemTag.quality
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the current quality of the item.
                // @Example "2" .quality returns "91".
                // -->
                case "quality":
                    return new NumberTag(Internal.item.quality).Handle(data.Shrink());
                default:
                    return new EntityTag(Internal.gameObject).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.gameObject.GetInstanceID().ToString();
        }
    }
}
