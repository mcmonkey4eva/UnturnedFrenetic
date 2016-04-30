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
    public class StructureTag : TemplateObject
    {
        // <--[object]
        // @Type StructureTag
        // @SubType EntityTag
        // @Group Entities
        // @Description Represents a spawned structure in the world.
        // -->

        public Transform Internal;
        public StructureData InternalData;

        public StructureTag(Structure structure)
        {
            for (byte x = 0; x < Regions.WORLD_SIZE; x++)
            {
                for (byte y = 0; y < Regions.WORLD_SIZE; y++)
                {
                    StructureRegion region = StructureManager.regions[x, y];
                    for (int i = 0; i < region.structures.Count; i++)
                    {
                        if (structure == region.structures[i].structure)
                        {
                            Internal = region.models[i];
                            InternalData = region.structures[i];
                            return;
                        }
                    }
                }
            }
        }

        public StructureTag(Transform transform, StructureData data)
        {
            Internal = transform;
            InternalData = data;
        }

        public static List<ItemStructureAsset> Structures;
        public static Dictionary<string, ItemStructureAsset> StructuresMap;

        public static void Init()
        {
            Structures = new List<ItemStructureAsset>();
            StructuresMap = new Dictionary<string, ItemStructureAsset>();
            Asset[] assets = Assets.find(EAssetType.ITEM);
            foreach (Asset asset in assets)
            {
                if (asset is ItemStructureAsset)
                {
                    Structures.Add((ItemStructureAsset)asset);
                    string namelow = "structure_" + asset.name.ToLower();
                    if (StructuresMap.ContainsKey(namelow))
                    {
                        SysConsole.Output(OutputType.INIT, "MINOR: multiple item structure assets named " + namelow);
                        continue;
                    }
                    StructuresMap.Add(namelow, (ItemStructureAsset)asset);
                    EntityType.STRUCTURES.Add(namelow, new EntityType(asset.name, EntityAssetType.STRUCTURE));
                }
            }
            SysConsole.Output(OutputType.INIT, "Loaded " + Structures.Count + " base item structures!");
        }

        public static StructureTag For(int instanceID)
        {
            for (byte x = 0; x < Regions.WORLD_SIZE; x++)
            {
                for (byte y = 0; y < Regions.WORLD_SIZE; y++)
                {
                    StructureRegion region = StructureManager.regions[x, y];
                    for (int i = 0; i < region.structures.Count; i++)
                    {
                        if (instanceID == region.models[i].gameObject.GetInstanceID())
                        {
                            return new StructureTag(region.models[i], region.structures[i]);
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
                // @Name StructureTag.asset
                // @Group General Information
                // @ReturnType ItemAssetTag
                // @Returns the item asset that this structure is based off.
                // @Example "2" .asset returns "Floor_Metal".
                // -->
                case "asset":
                    return ItemAssetTag.For(InternalData.structure.id.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name StructureTag.health
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the current health of the structure.
                // @Example "2" .health returns "1".
                // -->
                case "health":
                    return new NumberTag(InternalData.structure.health).Handle(data.Shrink());
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
