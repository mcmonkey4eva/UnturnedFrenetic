using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using SDG.Unturned;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class WorldObjectTag : TemplateObject
    {
        // <--[object]
        // @Type WorldObjectTag
        // @SubType EntityTag
        // @Group Entities
        // @Description Represents a spawned world object in the world.
        // -->

        public LevelObject Internal;

        public WorldObjectTag(LevelObject obj)
        {
            Internal = obj;
        }

        public static WorldObjectTag For(int instanceID)
        {
            for (byte x = 0; x < Regions.WORLD_SIZE; x++)
            {
                for (byte y = 0; y < Regions.WORLD_SIZE; y++)
                {
                    foreach (LevelObject levelObject in LevelObjects.objects[x, y])
                    {
                        if (levelObject.transform != null && instanceID == levelObject.transform.gameObject.GetInstanceID())
                        {
                            return new WorldObjectTag(levelObject);
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
                // @Name WorldObjectTag.asset
                // @Group General Information
                // @ReturnType ItemAssetTag
                // @Returns the world object asset that this world object is based off.
                // @Example "2" .asset returns "Tower_Military_0".
                // -->
                case "asset":
                    return new WorldObjectAssetTag(Internal.asset).Handle(data.Shrink());
                default:
                    return new EntityTag(Internal.transform.gameObject).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.transform.gameObject.GetInstanceID().ToString();
        }
    }
}
