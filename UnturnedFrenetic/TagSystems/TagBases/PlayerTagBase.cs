using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class PlayerTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base player[<TextTag>]
        // @Group Entities
        // @ReturnType PlayerTag
        // @Returns the player corresponding to the given name.
        // -->
        public PlayerTagBase()
        {
            Name = "player";
        }

        public override string Handle(TagData data)
        {
            string pname = data.GetModifier(0);
            PlayerTag ptag = PlayerTag.For(pname);
            if (ptag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return ptag.Handle(data.Shrink());
        }
    }
}
