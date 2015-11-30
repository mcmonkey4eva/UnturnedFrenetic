using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnturnedFrenetic
{
    public class EntityType
    {
        public static EntityType BEAR = new EntityType("Bear", EntityTypeType.ANIMAL);
        public static EntityType COW = new EntityType("Cow", EntityTypeType.ANIMAL);
        public static EntityType DEER = new EntityType("Deer", EntityTypeType.ANIMAL);
        public static EntityType MOOSE = new EntityType("Moose", EntityTypeType.ANIMAL);
        public static EntityType Pig = new EntityType("Pig", EntityTypeType.ANIMAL);
        public static EntityType Wolf = new EntityType("Wolf", EntityTypeType.ANIMAL);
        public static EntityType ZOMBIE = new EntityType("Zombie", EntityTypeType.ZOMBIE);

        public string Name;

        public EntityTypeType Type;

        public EntityType(string name, EntityTypeType type)
        {
            Name = name;
            Type = type;
        }
    }

    public enum EntityTypeType
    {
        ANIMAL = 0,
        ZOMBIE = 1
    }
}
