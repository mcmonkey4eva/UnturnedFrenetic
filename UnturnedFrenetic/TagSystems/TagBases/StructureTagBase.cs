﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class StructureTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base structure[<TextTag>]
        // @Group Entities
        // @ReturnType StructureTag
        // @Returns the structure entity corresponding to the given ID number.
        // -->
        public StructureTagBase()
        {
            Name = "structure";
        }

        public override TemplateObject Handle(TagData data)
        {
            string modif = data.GetModifier(0);
            if (modif.StartsWith("e:"))
            {
                modif = modif.Substring("e:".Length);
            }
            StructureTag stag = StructureTag.For(Utilities.StringToInt(modif));
            if (stag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return stag.Handle(data.Shrink());
        }
    }
}
