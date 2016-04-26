using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class ResourceAssetTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base resource_asset[<TextTag>]
        // @Group Assets
        // @ReturnType ResourceAssetTag
        // @Returns the resource asset corresponding to the given name or ID.
        // -->
        public ResourceAssetTagBase()
        {
            Name = "resource_asset";
        }

        public override TemplateObject Handle(TagData data)
        {
            string iname = data.GetModifier(0);
            ResourceAssetTag itag = ResourceAssetTag.For(iname);
            if (itag == null)
            {
                return new TextTag("&{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
