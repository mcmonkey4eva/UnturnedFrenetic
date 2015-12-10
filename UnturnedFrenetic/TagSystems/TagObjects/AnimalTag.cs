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

        public static AnimalTag For(int instanceID)
        {
            foreach (Animal animal in AnimalManager.animals)
            {
                if (animal.gameObject.GetInstanceID() == instanceID)
                {
                    return new AnimalTag(animal);
                }
            }
            return null;
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
                    return new TextTag(Internal.asset.name).Handle(data.Shrink());
                // <--[tag]
                // @Name AnimalTag.aid
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the animal ID number of the animal.
                // @Example "2" .aid returns "1".
                // -->
                case "aid":
                    return new TextTag(Internal.index).Handle(data.Shrink());
                // <--[tag]
                // @Name AnimalTag.health
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the animal's current health.
                // @Example "2" .health returns "96".
                // -->
                case "health":
                    return new TextTag(Internal.health).Handle(data.Shrink());
                // <--[tag]
                // <--[tag]
                // @Name AnimalTag.asset
                // @Group General Information
                // @ReturnType AnimalAssetTag
                // @Returns the animal asset that this animal is based off.
                // @Example "2" .asset returns "Cow".
                // -->
                case "asset":
                    return new AnimalAssetTag(Internal.asset).Handle(data.Shrink());
                default:
                    return new EntityTag(Internal.gameObject).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.gameObject.GetInstanceID().ToString();
        }
    }
}
