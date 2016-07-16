using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class ItemTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base item[<TextTag>]
        // @Group Entities
        // @ReturnType ItemTag
        // @Returns the item entity corresponding to the given ID number.
        // -->
        public ItemTagBase()
        {
            Name = "item_entity";
        }

        public override TemplateObject Handle(TagData data)
        {
            string modif = data.GetModifier(0);
            if (modif.StartsWith("e:"))
            {
                modif = modif.Substring("e:".Length);
            }
            ItemEntityTag itag = ItemEntityTag.For(Utilities.StringToInt(modif));
            if (itag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
