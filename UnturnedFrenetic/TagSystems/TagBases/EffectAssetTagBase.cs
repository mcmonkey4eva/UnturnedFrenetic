using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class EffectAssetTagBase : TemplateTags
    {
        // <--[tag]
        // @Base effect_asset[<TextTag>]
        // @Group Effects
        // @ReturnType EffectAssetTag
        // @Returns the effect asset corresponding to the given name or ID.
        // -->
        public EffectAssetTagBase()
        {
            Name = "effect_asset";
        }

        public override string Handle(TagData data)
        {
            EffectAssetTag atag = EffectAssetTag.For(data.GetModifier(0));
            if (atag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return atag.Handle(data.Shrink());
        }
    }
}
