using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class ColorTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base color[<TextTag>]
        // @Group Mathematics
        // @ReturnType ColorTag
        // @Returns the color corresponding to the given RGBA value.
        // -->
        public ColorTagBase()
        {
            Name = "color";
        }

        public override TemplateObject Handle(TagData data)
        {
            ColorTag ctag = ColorTag.For(data, data.GetModifierObject(0));
            if (ctag == null)
            {
                return new TextTag("&{NULL}");
            }
            return ctag.Handle(data.Shrink());
        }
    }
}
