using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class OfflinePlayerTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base offline_player[<TextTag>]
        // @Group Server Information
        // @ReturnType OfflinePlayerTag
        // @Returns the offline player corresponding to the given name.
        // -->
        public OfflinePlayerTagBase()
        {
            Name = "offline_player";
        }

        public override TemplateObject Handle(TagData data)
        {
            OfflinePlayerTag ptag = OfflinePlayerTag.For(Utilities.StringToULong(data.GetModifier(0)));
            if (ptag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return ptag.Handle(data.Shrink());
        }
    }
}
