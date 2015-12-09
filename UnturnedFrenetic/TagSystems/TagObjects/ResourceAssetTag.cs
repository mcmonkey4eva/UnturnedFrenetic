using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class ResourceAssetTag: TemplateObject
    {
        public static List<ResourceAsset> Resources;
        public static Dictionary<string, ResourceAsset> ResourcesMap;

        public static void Init()
        {
            Resources = new List<ResourceAsset>();
            ResourcesMap = new Dictionary<string, ResourceAsset>();
            Asset[] assets = Assets.find(EAssetType.RESOURCE);
            foreach (Asset asset in assets)
            {
                Resources.Add((ResourceAsset)asset);
                string namelow = asset.name.ToLower();
                if (ResourcesMap.ContainsKey(namelow))
                {
                    SysConsole.Output(OutputType.INIT, "MINOR: multiple resource assets named " + namelow);
                    continue;
                }
                ResourcesMap.Add(namelow, (ResourceAsset)asset);
                EntityType.RESOURCES.Add(namelow, new EntityType(asset.name, EntityAssetType.RESOURCE));
            }
            SysConsole.Output(OutputType.INIT, "Loaded " + Resources.Count + " base resources!");
        }

        public static ResourceAssetTag For(string nameorid)
        {
            ushort id;
            ResourceAsset asset;
            if (ushort.TryParse(nameorid, out id))
            {
                asset = (ResourceAsset)Assets.find(EAssetType.RESOURCE, id);
            }
            else
            {
                ResourcesMap.TryGetValue(nameorid.ToLower(), out asset);
            }
            if (asset == null)
            {
                return null;
            }
            return new ResourceAssetTag(asset);
        }

        public ResourceAssetTag(ResourceAsset asset)
        {
            Internal = asset;
        }

        public ResourceAsset Internal;

        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                // <--[tag]
                // @Name ResourceAssetTag.name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the name of the resource asset.
                // @Example "Bush_Jade" .name returns "Bush_Jade".
                // -->
                case "name":
                    return new TextTag(Internal.name).Handle(data.Shrink());
                // <--[tag]
                // @Name ResourceAssetTag.formatted_name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the formatted name of the resource asset.
                // @Example "Bush_Jade" .formatted_name returns "Bush Jade".
                // -->
                case "formatted_name":
                    return new TextTag(Internal.resourceName).Handle(data.Shrink());
                // <--[tag]
                // @Name ResourceAssetTag.id
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the internal ID of the resource asset.
                // @Example "Bush_Jade" .id returns "14".
                // -->
                case "id":
                    return new TextTag(Internal.id).Handle(data.Shrink());
                // <--[tag]
                // @Name ResourceAssetTag.health
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the default health of the resource asset.
                // @Example "Bush_Jade" .type returns "1".
                // -->
                case "health":
                    return new TextTag(Internal.health).Handle(data.Shrink());
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
