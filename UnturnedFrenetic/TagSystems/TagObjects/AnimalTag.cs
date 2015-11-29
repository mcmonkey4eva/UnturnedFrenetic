using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;


namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class AnimalTag : TemplateObject
    {
        public Animal Internal;

        public AnimalTag(Animal animal)
        {
            Internal = animal;
        }

        public static AnimalTag For(ushort aID)
        {
            Animal animal = AnimalManager.getAnimal(aID);
            if (animal == null)
            {
                return null;
            }
            return new AnimalTag(animal);
        }

        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                // <--[tag]
                // @Name AnimalTag.name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the name of the animal's type.
                // @Example "2" .name returns "Cow".
                // -->
                case "name":
                    return new TextTag(Internal.asset.animalName).Handle(data.Shrink());
                // <--[tag]
                // @Name AnimalTag.aid
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the animal ID number of the animal.
                // @Example "2" .id returns "2".
                // -->
                case "aid":
                    return new TextTag(Internal.index).Handle(data.Shrink());
                // <--[tag]
                // @Name AnimalTag.id
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the ID number of the animal asset.
                // @Example "2" .id returns "6".
                // -->
                case "id":
                    return new TextTag(Internal.id).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.index.ToString();
        }
    }
}
