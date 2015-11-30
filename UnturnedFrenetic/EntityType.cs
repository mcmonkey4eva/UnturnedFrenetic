using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnturnedFrenetic
{
    public class EntityType
    {
        public static EntityType BEAR = new EntityType("Bear", EntityAssetType.ANIMAL);
        public static EntityType COW = new EntityType("Cow", EntityAssetType.ANIMAL);
        public static EntityType DEER = new EntityType("Deer", EntityAssetType.ANIMAL);
        public static EntityType MOOSE = new EntityType("Moose", EntityAssetType.ANIMAL);
        public static EntityType PIG = new EntityType("Pig", EntityAssetType.ANIMAL);
        public static EntityType WOLF = new EntityType("Wolf", EntityAssetType.ANIMAL);
        public static EntityType ZOMBIE = new EntityType("Zombie", EntityAssetType.ZOMBIE);

        public static EntityType ValueOf(string name)
        {
            switch (name.ToLower())
            {
                case "bear":
                    return BEAR;
                case "cow":
                    return COW;
                case "deer":
                    return DEER;
                case "moose":
                    return MOOSE;
                case "pig":
                    return PIG;
                case "wolf":
                    return WOLF;
                case "zombie":
                    return ZOMBIE;
                default:
                    return null;
            }
        }

        public string AssetName;

        public EntityAssetType Type;

        public EntityType(string assetname, EntityAssetType type)
        {
            AssetName = assetname;
            Type = type;
        }
    }

    public enum EntityAssetType
    {
        ANIMAL = 0,
        ZOMBIE = 1
    }
}
