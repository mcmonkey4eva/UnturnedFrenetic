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
        public static List<AnimalAsset> Animals;
        public static Dictionary<string, AnimalAsset> AnimalsMap;

        public static void Init()
        {
            Animals = new List<AnimalAsset>();
            AnimalsMap = new Dictionary<string, AnimalAsset>();
            Asset[] assets = Assets.find(EAssetType.ANIMAL);
            foreach (Asset asset in assets)
            {
                Animals.Add((AnimalAsset)asset);
                string namelow = asset.name.ToLower();
                if (AnimalsMap.ContainsKey(namelow))
                {
                    SysConsole.Output(OutputType.INIT, "MINOR: multiple animal assets named " + namelow);
                    continue;
                }
                AnimalsMap.Add(namelow, (AnimalAsset)asset);
            }
            SysConsole.Output(OutputType.INIT, "Loaded " + Animals.Count + " base animals!");
        }

        public static AnimalAssetTag For(string nameorid)
        {
            ushort id;
            AnimalAsset asset;
            if (ushort.TryParse(nameorid, out id))
            {
                asset = (AnimalAsset)Assets.find(EAssetType.ANIMAL, id);
            }
            else
            {
                AnimalsMap.TryGetValue(nameorid.ToLower(), out asset);
            }
            if (asset == null)
            {
                return null;
            }
            return new AnimalAssetTag(asset);
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
                // @Name AnimalAssetTag.id
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the ID number of the animal asset.
                // @Example "Cow" .id returns "6".
                // -->
                case "id":
                    return new TextTag(Internal.id).Handle(data.Shrink());
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
