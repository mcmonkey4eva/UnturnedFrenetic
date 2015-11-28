using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class PlayerTagBase : TemplateTags
    {
        // <--[tag]
        // @Base player[<TextTag>]
        // @Group Entities
        // @ReturnType PlayerTag
        // @Returns the input text as a PlayerTag. (Soon: A player object!)
        // -->
        public PlayerTagBase()
        {
            Name = "player";
        }

        public override string Handle(TagData data)
        {
            string pname = data.GetModifier(0);
            // TODO: validate name
            return new PlayerTag(pname).Handle(data.Shrink());
        }
    }
}
