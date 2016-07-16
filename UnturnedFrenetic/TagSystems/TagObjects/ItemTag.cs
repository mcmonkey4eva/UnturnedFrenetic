using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class ItemTag : TemplateObject
    {
        // <--[object]
        // @Type ItemTag
        // @SubType ItemAssetTag
        // @Group Inventories
        // @Description Represents an item in an inventory.
        // -->

        public ItemJar Internal;

        public ItemTag(ItemJar item)
        {
            Internal = item;
        }
        
        public ItemAsset GetAsset()
        {
            return (ItemAsset)Assets.find(EAssetType.ITEM, Internal.item.id);
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
                // @ReturnType ItemTag
                // @Returns the item asset that this item is based off.
                // @Example <{var[player].inventory.item[2].asset}> returns "Rifle_Maple".
                // -->
                case "asset":
                    return new ItemAssetTag(GetAsset()).Handle(data.Shrink());
                // <--[tag]
                // @Name ItemTag.amount
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the amount of the item.
                // @Example <{var[player].inventory.item[2].amount}> returns "1".
                // -->
                case "amount":
                    return new NumberTag(Internal.item.amount).Handle(data.Shrink());
                // <--[tag]
                // @Name ItemTag.quality
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the current quality of the item.
                // @Example <{var[player].inventory.item[2].quality}> returns "91".
                // -->
                case "quality":
                    return new NumberTag(Internal.item.quality).Handle(data.Shrink());
                default:
                    return new ItemAssetTag(GetAsset()).Handle(data);
            }
        }

        public override string ToString()
        {
            return new ItemAssetTag(GetAsset()).ToString();
        }
    }
}
