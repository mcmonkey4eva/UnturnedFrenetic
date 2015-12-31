using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class ResourceTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base resource[<TextTag>]
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
            string modif = data.GetModifier(0);
            if (modif.StartsWith("e:"))
            {
                modif = modif.Substring("e:".Length);
            }
            ResourceTag rtag = ResourceTag.For(Utilities.StringToInt(modif));
            if (rtag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return rtag.Handle(data.Shrink());
        }
    }
}
