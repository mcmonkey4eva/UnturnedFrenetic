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
        // <--[tagbase]
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
            string modif = data.GetModifier(0);
            if (modif.StartsWith("e:"))
            {
                modif = modif.Substring("e:".Length);
            }
            VehicleTag vtag = VehicleTag.For(Utilities.StringToInt(modif));
            if (vtag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return vtag.Handle(data.Shrink());
        }
    }
}
