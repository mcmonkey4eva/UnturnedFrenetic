using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class AnimalAssetTag: TemplateObject
    {
        public static AnimalAssetTag For(string name)
        {
            name = name.ToLower();
            // TODO: Possibly preregister?
            foreach (Asset asset in Assets.find(EAssetType.ANIMAL))
            {
                if (asset.name.ToLower() == name)
                {
                    return new AnimalAssetTag((AnimalAsset)asset);
                }
            }
            return null;
        }

        public AnimalAsset Internal;

        public AnimalAssetTag(AnimalAsset asset)
        {
            Internal = asset;
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
                // @Name AnimalAssetTag.name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the name of the asset.
                // @Example "Cow" .name returns "Cow".
                // -->
                case "name":
                    return new TextTag(Internal.name).Handle(data.Shrink());
                // <--[tag]
                // @Name AnimalAssetTag.animal_name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the name of the animal associated with the asset.
                // @Example "Cow" .animal_name returns "Cow".
                // -->
                case "animal_name":
                    return new TextTag(Internal.animalName).Handle(data.Shrink());
                // <--[tag]
                // @Name AnimalTag.id
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the ID number of the animal asset.
                // @Example "Cow" .id returns "6".
                // -->
                case "id":
                    return new TextTag(Internal.id).Handle(data.Shrink());
                // TODO: Return the actual asset as .asset!
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.name;
        }
    }
}
