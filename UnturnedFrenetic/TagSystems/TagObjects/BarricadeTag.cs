using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;
using UnityEngine;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class BarricadeTag : TemplateObject
    {
        public Transform Internal;
        public BarricadeData InternalData;

        public BarricadeTag(Transform transform, BarricadeData data)
        {
            Internal = transform;
            InternalData = data;
        }

        public static List<ItemBarricadeAsset> Barricades;
        public static Dictionary<string, ItemBarricadeAsset> BarricadesMap;

        public static void Init()
        {
            Barricades = new List<ItemBarricadeAsset>();
            BarricadesMap = new Dictionary<string, ItemBarricadeAsset>();
            Asset[] assets = Assets.find(EAssetType.ITEM);
            foreach (Asset asset in assets)
            {
                if (asset is ItemBarricadeAsset)
                {
                    Barricades.Add((ItemBarricadeAsset)asset);
                    string namelow = "barricade_" + asset.name.ToLower();
                    if (BarricadesMap.ContainsKey(namelow))
                    {
                        SysConsole.Output(OutputType.INIT, "MINOR: multiple item barricade assets named " + namelow);
                        continue;
                    }
                    BarricadesMap.Add(namelow, (ItemBarricadeAsset)asset);
                    EntityType.BARRICADES.Add(namelow, new EntityType(asset.name, EntityAssetType.BARRICADE));
                }
                SysConsole.Output(OutputType.INIT, "Loaded " + Barricades.Count + " base item barricades!");
            }
        }

        public static BarricadeTag For(int instanceID)
        {
            for (byte x = 0; x < Regions.WORLD_SIZE; x++)
            {
                for (byte y = 0; y < Regions.WORLD_SIZE; y++)
                {
                    BarricadeRegion region = BarricadeManager.regions[x, y];
                    for (int i = 0; i < region.barricades.Count; i++)
                    {
                        if (instanceID == region.models[i].gameObject.GetInstanceID())
                        {
                            return new BarricadeTag(region.models[i], region.barricades[i]);
                        }
                    }
                }
            }
            return null;
        }

        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                // <--[tag]
                // @Name BarricadeTag.iid
                // @Group General Information
                // @ReturnType TextTag
                // @Returns this barricade's instance ID number.
                // @Example "2" .iid returns "2".
                // -->
                case "iid":
                    return new TextTag(Internal.gameObject.GetInstanceID()).Handle(data.Shrink());
                // <--[tag]
                // @Name BarricadeTag.asset
                // @Group General Information
                // @ReturnType ItemAssetTag
                // @Returns the item asset that this barricade is based off.
                // @Example "2" .asset returns "Bush_Jade".
                // -->
                case "asset":
                    return ItemAssetTag.For(InternalData.barricade.id.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name BarricadeTag.location
                //// @Group Status
                // @ReturnType LocationTag
                // @Returns the resource's current world position.
                // @Example "2" .location returns "(5, 10, 15)".
                // -->
                case "location":
                    return new LocationTag(Internal.transform.position).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.gameObject.GetInstanceID().ToString();
        }
    }
}
