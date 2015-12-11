using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class StructureTagBase : TemplateTags
    {
        // <--[tag]
        // @Base structure[<TextTag>]
        // @SubType EntityTag
        // @Group Entities
        // @ReturnType StructureTag
        // @Returns the structure entity corresponding to the given ID number.
        // -->
        public StructureTagBase()
        {
            Name = "structure";
        }

        public override string Handle(TagData data)
        {
            StructureTag stag = StructureTag.For(Utilities.StringToInt(data.GetModifier(0)));
            if (stag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return stag.Handle(data.Shrink());
        }
    }
}
