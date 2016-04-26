using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class ZombieTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base zombie[<TextTag>]
        // @Group Entities
        // @ReturnType ZombieTag
        // @Returns the zombie entity corresponding to the given ID number.
        // -->
        public ZombieTagBase()
        {
            Name = "zombie";
        }

        public override TemplateObject Handle(TagData data)
        {
            string modif = data.GetModifier(0);
            if (modif.StartsWith("e:"))
            {
                modif = modif.Substring("e:".Length);
            }
            ZombieTag ztag = ZombieTag.For(Utilities.StringToInt(modif));
            if (ztag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return ztag.Handle(data.Shrink());
        }
    }
}
