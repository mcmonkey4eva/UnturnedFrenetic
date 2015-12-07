using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class WorldObjectTagBase : TemplateTags
    {
        // <--[tag]
        // @Base world_object[<TextTag>]
        // @Group Entities
        // @ReturnType WorldObjectTag
        // @Returns the world object entity corresponding to the given ID number.
        // -->
        public WorldObjectTagBase()
        {
            Name = "world_object";
        }

        public override string Handle(TagData data)
        {
            WorldObjectTag itag = WorldObjectTag.For(Utilities.StringToInt(data.GetModifier(0)));
            if (itag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
