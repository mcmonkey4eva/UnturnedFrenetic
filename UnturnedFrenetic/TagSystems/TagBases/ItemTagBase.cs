using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    class ItemTagBase : TemplateTags
    {
        // <--[tag]
        // @Base item[<TextTag>]
        // @Group Items
        // @ReturnType ItemTag
        // @Returns the item corresponding to the given name or ID.
        // -->
        public ItemTagBase()
        {
            Name = "item";
        }

        public override string Handle(TagData data)
        {
            string iname = data.GetModifier(0);
            ItemTag itag = ItemTag.For(iname);
            if (itag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
