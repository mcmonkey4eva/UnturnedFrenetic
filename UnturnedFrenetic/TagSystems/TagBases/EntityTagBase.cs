using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class EntityTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base entity[<TextTag>]
        // @Group Entities
        // @ReturnType EntityTag
        // @Returns the entity corresponding to the given ID number.
        // -->
        public EntityTagBase()
        {
            Name = "entity";
        }

        public override TemplateObject Handle(TagData data)
        {
            string modif = data.GetModifier(0);
            if (modif.StartsWith("e:"))
            {
                modif = modif.Substring("e:".Length);
            }
            EntityTag itag = EntityTag.For(Utilities.StringToInt(modif));
            if (itag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
