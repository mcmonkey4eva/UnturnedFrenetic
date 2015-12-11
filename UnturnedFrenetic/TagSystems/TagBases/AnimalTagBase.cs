using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class AnimalTagBase : TemplateTags
    {
        // <--[tag]
        // @Base animal[<TextTag>]
        // @SubType EntityTag
        // @Group Entities
        // @ReturnType AnimalTag
        // @Returns the animal entity corresponding to the given ID number.
        // -->
        public AnimalTagBase()
        {
            Name = "animal";
        }

        public override string Handle(TagData data)
        {
            string modif = data.GetModifier(0);
            if (modif.StartsWith("e:"))
            {
                modif = modif.Substring("e:".Length);
            }
            AnimalTag atag = AnimalTag.For(Utilities.StringToInt(modif));
            if (atag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return atag.Handle(data.Shrink());
        }
    }
}
