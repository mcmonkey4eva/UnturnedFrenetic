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
    public class BarricadeTag : TemplateObject
    {
        // <--[object]
        // @Type BarricadeTag
        // @SubType EntityTag
        // @Group Entities
        // @Description Represents a spawned barricade in the world.
        // -->

        public Transform Internal;
        public BarricadeData InternalData;

        public BarricadeTag(Barricade barricade)
        {
            for (byte x = 0; x < Regions.WORLD_SIZE; x++)
            {
                for (byte y = 0; y < Regions.WORLD_SIZE; y++)
                {
                    BarricadeRegion region = BarricadeManager.regions[x, y];
                    for (int i = 0; i < region.barricades.Count; i++)
                    {
                        if (barricade == region.barricades[i].barricade)
                        {
                            Internal = region.models[i];
                            InternalData = region.barricades[i];
                            return;
                        }
                    }
                }
            }
        }

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
            }
            SysConsole.Output(OutputType.INIT, "Loaded " + Barricades.Count + " base item barricades!");
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

        public override TemplateObject Handle(TagData data)
        {
            if (data.Remaining == 0)
            {
                return this;
            }
            switch (data[0])
            {
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
                // @Name BarricadeTag.health
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the barricade's current health.
                // @Example "2" .health returns "96".
                // -->
                case "health":
                    return new NumberTag(InternalData.barricade.health).Handle(data.Shrink());
                // <--[tag]
                // @Name BarricadeTag.powered
                // @Group General Information
                // @ReturnType BooleanTag
                // @Returns whether the barricade is currently powered, open, or lit, etc.
                // @Example "2" .powered returns "true".
                // -->
                case "powered":
                    InteractablePower power = Internal.gameObject.GetComponent<InteractablePower>();
                    if (power != null)
                    {
                        switch (power.GetType().Name)
                        {
                            case "InteractableDoor":
                                return new BooleanTag(InternalData.barricade.state[16] == 1).Handle(data.Shrink());
                            case "InteractableFire":
                            case "InteractableGenerator":
                            case "InteractableSafezone":
                            case "InteractableSpot":
                                return new BooleanTag(InternalData.barricade.state[0] == 1).Handle(data.Shrink());
                        }
                    }
                    data.Error("Read 'powered' tag on non-powerable object!");
                    return new NullTag();
                default:
                    return new EntityTag(Internal.gameObject).Handle(data);
            }
        }

        public override string ToString()
        {
            return new EntityTag(Internal.gameObject).ToString();
        }
    }
}
