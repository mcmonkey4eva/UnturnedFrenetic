using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class ItemAssetTag: TemplateObject
    {
        // <--[object]
        // @Type ItemAssetTag
        // @SubType TextTag
        // @Group Assets
        // @Description Represents an asset used to spawn an item.
        // -->

        public static List<ItemAsset> Items;
        public static Dictionary<string, ItemAsset> ItemsMap;

        public static void Init()
        {
            Items = new List<ItemAsset>();
            ItemsMap = new Dictionary<string, ItemAsset>();
            Asset[] assets = Assets.find(EAssetType.ITEM);
            foreach (Asset asset in assets)
            {
                Items.Add((ItemAsset)asset);
                string namelow = asset.name.ToLower();
                if (ItemsMap.ContainsKey(namelow))
                {
                    SysConsole.Output(OutputType.INIT, "MINOR: multiple item assets named " + namelow);
                    continue;
                }
                ItemsMap.Add(namelow, (ItemAsset)asset);
                EntityType.ITEMS.Add(namelow, new EntityType(asset.name, EntityAssetType.ITEM));
            }
            SysConsole.Output(OutputType.INIT, "Loaded " + Items.Count + " base items!");
        }

        public static ItemAssetTag For(string nameorid)
        {
            ushort id;
            ItemAsset asset;
            if (ushort.TryParse(nameorid, out id))
            {
                asset = (ItemAsset)Assets.find(EAssetType.ITEM, id);
            }
            else
            {
                ItemsMap.TryGetValue(nameorid.ToLower(), out asset);
            }
            if (asset == null)
            {
                return null;
            }
            return new ItemAssetTag(asset);
        }

        public ItemAssetTag(ItemAsset asset)
        {
            Internal = asset;
        }

        public ItemAsset Internal;

        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                // <--[tag]
                // @Name ItemAssetTag.name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the name of the item asset.
                // @Example "Rifle_Maple" .name returns "Rifle_Maple".
                // -->
                case "name":
                    return new TextTag(Internal.name).Handle(data.Shrink());
                // <--[tag]
                // @Name ItemAssetTag.formatted_name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the formatted name of the item asset, as in how players see the name.
                // @Example "Rifle_Maple" .formatted_name returns "Maple Rifle".
                // -->
                case "formatted_name":
                    return new TextTag(Internal.itemName).Handle(data.Shrink());
                // <--[tag]
                // @Name ItemAssetTag.description
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the description of the item asset.
                // @Example "Rifle_Maple" .description returns "Maple rifle chambered in Rifle ammunition.".
                // -->
                case "description":
                    return new TextTag(Internal.itemDescription).Handle(data.Shrink());
                // <--[tag]
                // @Name ItemAssetTag.is_pro
                // @Group General Information
                // @ReturnType BooleanTag
                // @Returns whether the item asset is considered pro.
                // @Example "Rifle_Maple" .is_pro returns "false".
                // -->
                case "is_pro":
                    return new BooleanTag(Internal.isPro).Handle(data.Shrink());
                // <--[tag]
                // @Name ItemAssetTag.id
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the internal ID of the item asset.
                // @Example "Rifle_Maple" .id returns "474".
                // -->
                case "id":
                    return new NumberTag(Internal.id).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.name;
        }
    }
}
