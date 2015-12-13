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
        // <--[tagbase]
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
            string modif = data.GetModifier(0);
            if (modif.StartsWith("e:"))
            {
                modif = modif.Substring("e:".Length);
            }
            WorldObjectTag wotag = WorldObjectTag.For(Utilities.StringToInt(modif));
            if (wotag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return wotag.Handle(data.Shrink());
        }
    }
}
