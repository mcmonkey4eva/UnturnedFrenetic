using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class BarricadeTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base barricade[<TextTag>]
        // @Group Entities
        // @ReturnType BarricadeTag
        // @Returns the barricade entity corresponding to the given ID number.
        // -->
        public BarricadeTagBase()
        {
            Name = "barricade";
        }

        public override string Handle(TagData data)
        {
            string modif = data.GetModifier(0);
            if (modif.StartsWith("e:"))
            {
                modif = modif.Substring("e:".Length);
            }
            BarricadeTag btag = BarricadeTag.For(Utilities.StringToInt(modif));
            if (btag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return btag.Handle(data.Shrink());
        }
    }
}
