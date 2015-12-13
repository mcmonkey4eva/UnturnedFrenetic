using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class WorldObjectAssetTagBase : TemplateTags
    {
        // <--[tagbase]
        // @Base world_object_asset[<TextTag>]
        // @Group Assets
        // @ReturnType WorldObjectAssetTag
        // @Returns the world object asset corresponding to the given name or ID.
        // -->
        public WorldObjectAssetTagBase()
        {
            Name = "world_object_asset";
        }

        public override string Handle(TagData data)
        {
            string iname = data.GetModifier(0);
            WorldObjectAssetTag itag = WorldObjectAssetTag.For(iname);
            if (itag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
