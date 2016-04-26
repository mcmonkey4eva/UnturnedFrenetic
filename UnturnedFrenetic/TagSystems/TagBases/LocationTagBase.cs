using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    class LocationTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base location[<TextTag>]
        // @Group Mathematics
        // @ReturnType LocationTag
        // @Returns the location at the corresponding coordinates.
        // -->
        public LocationTagBase()
        {
            Name = "location";
        }

        public override TemplateObject Handle(TagData data)
        {
            string lname = data.GetModifier(0);
            LocationTag ltag = LocationTag.For(lname);
            if (ltag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return ltag.Handle(data.Shrink());
        }
    }
}
