using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class VehicleAssetTagBase : TemplateTags
    {
        // <--[tagbase]
        // @Base vehicle_asset[<TextTag>]
        // @Group Assets
        // @ReturnType VehicleTag
        // @Returns the vehicle asset corresponding to the given name or ID.
        // -->
        public VehicleAssetTagBase()
        {
            Name = "vehicle_asset";
        }

        public override string Handle(TagData data)
        {
            string iname = data.GetModifier(0);
            VehicleAssetTag itag = VehicleAssetTag.For(iname);
            if (itag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
