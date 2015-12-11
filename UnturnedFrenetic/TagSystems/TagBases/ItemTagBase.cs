using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class ItemTagBase : TemplateTags
    {
        // <--[tag]
        // @Base item[<TextTag>]
        // @SubType EntityTag
        // @Group Entities
        // @ReturnType ItemTag
        // @Returns the item entity corresponding to the given ID number.
        // -->
        public ItemTagBase()
        {
            Name = "item";
        }

        public override string Handle(TagData data)
        {
            ItemTag itag = ItemTag.For(Utilities.StringToInt(data.GetModifier(0)));
            if (itag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
