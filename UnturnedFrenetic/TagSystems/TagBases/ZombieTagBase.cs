using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class ZombieTagBase : TemplateTags
    {
        // <--[tag]
        // @Base zombie[<TextTag>]
        // @SubType EntityTag
        // @Group Entities
        // @ReturnType ZombieTag
        // @Returns the zombie entity corresponding to the given ID number.
        // -->
        public ZombieTagBase()
        {
            Name = "zombie";
        }

        public override string Handle(TagData data)
        {
            ZombieTag ztag = ZombieTag.For(Utilities.StringToInt(data.GetModifier(0)));
            if (ztag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return ztag.Handle(data.Shrink());
        }
    }
}
