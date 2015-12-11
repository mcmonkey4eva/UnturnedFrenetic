using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class ResourceTagBase : TemplateTags
    {
        // <--[tag]
        // @Base resource[<TextTag>]
        // @SubType EntityTag
        // @Group Entities
        // @ReturnType ResourceTag
        // @Returns the resource entity corresponding to the given ID number.
        // -->
        public ResourceTagBase()
        {
            Name = "resource";
        }

        public override string Handle(TagData data)
        {
            ResourceTag itag = ResourceTag.For(Utilities.StringToInt(data.GetModifier(0)));
            if (itag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
