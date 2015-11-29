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
        // @Group Entities
        // @ReturnType AnimalTag
        // @Returns the animal corresponding to the given ID number.
        // -->
        public AnimalTagBase()
        {
            Name = "animal";
        }

        public override string Handle(TagData data)
        {
            ushort aID;
            bool valid = ushort.TryParse(data.GetModifier(0), out aID);
            AnimalTag atag = AnimalTag.For(aID);
            if (atag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return atag.Handle(data.Shrink());
        }
    }
}
