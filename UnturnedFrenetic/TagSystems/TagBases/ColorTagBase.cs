using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class ColorTagBase : TemplateTags
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

        public override string Handle(TagData data)
        {
            ColorTag ctag = ColorTag.For(data.GetModifier(0));
            if (ctag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return ctag.Handle(data.Shrink());
        }
    }
}
