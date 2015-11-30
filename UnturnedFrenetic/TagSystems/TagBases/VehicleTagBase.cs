using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class VehicleTagBase : TemplateTags
    {
        // <--[tag]
        // @Base vehicle[<TextTag>]
        // @Group Entities
        // @ReturnType VehicleTag
        // @Returns the vehicle entity corresponding to the given name ID number.
        // -->
        public VehicleTagBase()
        {
            Name = "vehicle";
        }

        public override string Handle(TagData data)
        {
            VehicleTag itag = VehicleTag.For(Utilities.StringToInt(data.GetModifier(0)));
            if (itag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
