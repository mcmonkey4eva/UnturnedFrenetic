﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using SDG.Unturned;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class WorldObjectAssetTag: TemplateObject
    {
        // <--[object]
        // @Type WorldObjectAssetTag
        // @SubType TextTag
        // @Group Assets
        // @Description Represents an asset used to spawn a world object.
        // -->

        public static List<ObjectAsset> WorldObjects;
        public static Dictionary<string, ObjectAsset> WorldObjectsMap;

        public static void Init()
        {
            WorldObjects = new List<ObjectAsset>();
            WorldObjectsMap = new Dictionary<string, ObjectAsset>();
            Asset[] assets = Assets.find(EAssetType.OBJECT);
            foreach (Asset asset in assets)
            {
                WorldObjects.Add((ObjectAsset)asset);
                string namelow = asset.name.ToLower();
                if (WorldObjectsMap.ContainsKey(namelow))
                {
                    SysConsole.Output(OutputType.INIT, "MINOR: multiple world object assets named " + namelow);
                    continue;
                }
                WorldObjectsMap.Add(namelow, (ObjectAsset)asset);
                EntityType.WORLD_OBJECTS.Add(namelow, new EntityType(asset.name, EntityAssetType.WORLD_OBJECT));
            }
            SysConsole.Output(OutputType.INIT, "Loaded " + WorldObjects.Count + " base world objects!");
        }

        public static WorldObjectAssetTag For(string nameorid)
        {
            ushort id;
            ObjectAsset asset;
            if (ushort.TryParse(nameorid, out id))
            {
                asset = (ObjectAsset)Assets.find(EAssetType.OBJECT, id);
            }
            else
            {
                WorldObjectsMap.TryGetValue(nameorid.ToLower(), out asset);
            }
            if (asset == null)
            {
                return null;
            }
            return new WorldObjectAssetTag(asset);
        }

        public WorldObjectAssetTag(ObjectAsset asset)
        {
            Internal = asset;
        }

        public ObjectAsset Internal;

        public override TemplateObject Handle(TagData data)
        {
            if (data.Remaining == 0)
            {
                return this;
            }
            switch (data[0])
            {
                // <--[tag]
                // @Name WorldObjectAssetTag.name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the name of the world object asset.
                // @Example "Tower_Military_0" .name returns "Tower_Military_0".
                // -->
                case "name":
                    return new TextTag(Internal.name).Handle(data.Shrink());
                // <--[tag]
                // @Name WorldObjectAssetTag.formatted_name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the formatted name of the world object asset.
                // @Example "Tower_Military_0" .formatted_name returns "Military Tower #1".
                // -->
                case "formatted_name":
                    return new TextTag(Internal.name).Handle(data.Shrink());
                // <--[tag]
                // @Name WorldObjectAssetTag.id
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the internal ID of the world object asset.
                // @Example "Tower_Military_0" .id returns "227".
                // -->
                case "id":
                    return new NumberTag(Internal.id).Handle(data.Shrink());
                // <--[tag]
                // @Name WorldObjectAssetTag.type
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the type of the world object asset.
                // @Example "Tower_Military_0" .type returns "LARGE".
                // -->
                case "type":
                    return new TextTag(Internal.type.ToString()).Handle(data.Shrink());
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
