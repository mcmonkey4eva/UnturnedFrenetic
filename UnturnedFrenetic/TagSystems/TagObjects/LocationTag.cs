using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;
using UnityEngine;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class LocationTag: TemplateTags
    {
        public float X;

        public float Y;

        public float Z;

        public static LocationTag For(string input)
        {
            input = input.Replace("(", "").Replace(")", "");
            string[] inps = input.Split(',');
            if (inps.Length != 3)
            {
                return null;
            }
            return new LocationTag(Utilities.StringToFloat(inps[0].Trim()), Utilities.StringToFloat(inps[1].Trim()), Utilities.StringToFloat(inps[2].Trim()));
        }

        public LocationTag(UnityEngine.Vector3 vec3)
            : this(vec3.x, vec3.y, vec3.z)
        {
        }

        public LocationTag(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
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
                // @Name LocationTag.x
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the X coordinate of this location.
                // @Example "0,1,2" .x returns "0".
                // -->
                case "x":
                    return new TextTag(X).Handle(data.Shrink());
                // <--[tag]
                // @Name LocationTag.y
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the Y coordinate of this location.
                // @Example "0,1,2" .x returns "1".
                // -->
                case "y":
                    return new TextTag(Y).Handle(data.Shrink());
                // <--[tag]
                // @Name LocationTag.z
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the Z coordinate of this location.
                // @Example "0,1,2" .x returns "2".
                // -->
                case "z":
                    return new TextTag(Z).Handle(data.Shrink());
                // <--[tag]
                // @Name LocationTag.find_animals_within[<TextTag>]
                // @Group General Information
                // @ReturnType ListTag<AnimalTag>
                // @Returns a list of all animals within the specified range (spherical).
                // @Example "0,1,2" .find_animals_within[10] returns "2|3|17".
                // -->
                case "find_animals_within":
                    {
                        List<TemplateObject> animals = new List<TemplateObject>();
                        Vector3 vec3 = new Vector3(X, Y, Z);
                        float range = Utilities.StringToFloat(data.GetModifier(0));
                        foreach (Animal animal in AnimalManager.animals)
                        {
                            if ((animal.transform.position - vec3).sqrMagnitude <= range * range)
                            {
                                animals.Add(new AnimalTag(animal));
                            }
                        }
                        return new ListTag(animals).Handle(data.Shrink());
                    }
                // <--[tag]
                // @Name LocationTag.find_zombies_within[<TextTag>]
                // @Group General Information
                // @ReturnType ListTag<ZombieTag>
                // @Returns a list of all zombies within the specified range (spherical).
                // @Example "0,1,2" .find_zombies_within[10] returns "2|3|17".
                // -->
                case "find_zombies_within":
                    {
                        List<TemplateObject> zombies = new List<TemplateObject>();
                        Vector3 vec3 = new Vector3(X, Y, Z);
                        float range = Utilities.StringToFloat(data.GetModifier(0));
                        for (int i = 0; i < ZombieManager.regions.Length; i++)
                        {
                            foreach (Zombie zombie in ZombieManager.regions[i].zombies)
                            {
                                if ((zombie.transform.position - vec3).sqrMagnitude <= range * range)
                                            {
                                    zombies.Add(new ZombieTag(zombie, i));
                                }
                            }
                        }
                        return new ListTag(zombies).Handle(data.Shrink());
                    }
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }

        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, Z);
        }
    }
}
