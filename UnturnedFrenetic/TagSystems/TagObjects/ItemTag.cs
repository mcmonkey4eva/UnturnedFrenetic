using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using SDG.Unturned;
using Frenetic.TagHandlers.Objects;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class ItemTag: TemplateObject
    {
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
                    SysConsole.Output(OutputType.INIT, "MINOR: multiple assets named " + namelow);
                    continue;
                }
                ItemsMap.Add(namelow, (ItemAsset)asset);
            }
            SysConsole.Output(OutputType.INIT, "Loaded " + Items.Count + " base items!");
        }

        public static ItemTag For(string nameorid)
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
            return new ItemTag(asset);
        }

        public ItemTag(ItemAsset asset)
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
                // @Name ItemTag.name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the name of the item.
                // @Example "Rifle_Maple" .name returns "Rifle_Maple".
                // -->
                case "name":
                    return new TextTag(Internal.name).Handle(data.Shrink());
                // <--[tag]
                // @Name ItemTag.is_pro
                // @Group General Information
                // @ReturnType TextTag
                // @Returns whether the item is considered pro.
                // @Example "Rifle_Maple" .is_pro returns "false".
                // -->
                case "is_pro":
                    return new TextTag(Internal.isPro).Handle(data.Shrink());
                // <--[tag]
                // @Name ItemTag.id
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the internal ID of the item.
                // @Example "Rifle_Maple" .id returns "474".
                // -->
                case "id":
                    return new TextTag(Internal.id).Handle(data.Shrink());
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
