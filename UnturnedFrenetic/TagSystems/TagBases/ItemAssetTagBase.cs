using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class ItemAssetTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base item_asset[<TextTag>]
        // @Group Assets
        // @ReturnType ItemAssetTag
        // @Returns the item asset corresponding to the given name or ID.
        // -->
        public ItemAssetTagBase()
        {
            Name = "item_asset";
        }

        public override TemplateObject Handle(TagData data)
        {
            string iname = data.GetModifier(0);
            ItemAssetTag itag = ItemAssetTag.For(iname);
            if (itag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
