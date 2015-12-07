using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class EntityTagBase : TemplateTags
    {
        // <--[tag]
        // @Base entity[<TextTag>]
        // @Group Entities
        // @ReturnType EntityTag
        // @Returns the entity corresponding to the given ID number.
        // -->
        public EntityTagBase()
        {
            Name = "entity";
        }

        public override string Handle(TagData data)
        {
            EntityTag itag = EntityTag.For(Utilities.StringToInt(data.GetModifier(0)));
            if (itag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
