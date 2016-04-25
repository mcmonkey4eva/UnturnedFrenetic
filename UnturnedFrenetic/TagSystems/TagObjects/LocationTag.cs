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
    public class LocationTag: TemplateObject
    {
        // <--[object]
        // @Type LocationTag
        // @SubType TextTag
        // @Group Mathematics
        // @Description Represents a 3D coordinate in the world, or a 3D vector.
        // -->

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

        public override TemplateObject Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return this;
            }
            switch (data.Input[0])
            {
                // <--[tag]
                // @Name LocationTag.x
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the X coordinate of this location.
                // @Example "0,1,2" .x returns "0".
                // -->
                case "x":
                    return new NumberTag(X).Handle(data.Shrink());
                // <--[tag]
                // @Name LocationTag.y
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the Y coordinate of this location.
                // @Example "0,1,2" .y returns "1".
                // -->
                case "y":
                    return new NumberTag(Y).Handle(data.Shrink());
                // <--[tag]
                // @Name LocationTag.z
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the Z coordinate of this location.
                // @Example "0,1,2" .z returns "2".
                // -->
                case "z":
                    return new NumberTag(Z).Handle(data.Shrink());
                // <--[tag]
                // @Name LocationTag.add[<LocationTag>]
                // @Group Mathematics
                // @ReturnType LocationTag
                // @Returns the result of adding the specified location to this location.
                // @Example "0,1,2" .add[2,1,0] returns "2,2,2".
                // -->
                case "add":
                    {
                        LocationTag modif = LocationTag.For(data.GetModifier(0));
                        return new LocationTag(X + modif.X, Y + modif.Y, Z + modif.Z).Handle(data.Shrink());
                    }
                // <--[tag]
                // @Name LocationTag.subtract[<LocationTag>]
                // @Group Mathematics
                // @ReturnType LocationTag
                // @Returns the result of subtracting the specified location from this location.
                // @Example "0,1,2" .subtract[0,1,2] returns "0,0,0".
                // -->
                case "subtract":
                    {
                        LocationTag modif = LocationTag.For(data.GetModifier(0));
                        return new LocationTag(X - modif.X, Y - modif.Y, Z - modif.Z).Handle(data.Shrink());
                    }
                // <--[tag]
                // @Name LocationTag.length
                // @Group Mathematics
                // @ReturnType NumberTag
                // @Returns the length from this location to the origin.
                // @Warning this tag requires a square root operation, which is a tiny bit slow internally. Consider using <@link tag LocationTag.length_squared>.
                // @Example "0,2,0" .length returns "2".
                // -->
                case "length":
                    return new NumberTag(ToVector3().magnitude).Handle(data.Shrink());
                // <--[tag]
                // @Name LocationTag.length_squared
                // @Group Mathematics
                // @ReturnType NumberTag
                // @Returns the square of the length from this location to the origin.
                // @Example "0,2,0" .length_squared returns "4".
                // -->
                case "length_squared":
                    return new NumberTag(ToVector3().sqrMagnitude).Handle(data.Shrink());
                // <--[tag]
                // @Name LocationTag.find_animals_within[<TextTag>]
                // @Group World
                // @ReturnType ListTag<AnimalTag>
                // @Returns a list of all animals within the specified range (spherical).
                // @Example "0,1,2" .find_animals_within[10] returns "2|3|17".
                // -->
                case "find_animals_within":
                    {
                        List<TemplateObject> animals = new List<TemplateObject>();
                        Vector3 vec3 = ToVector3();
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
                // @Group World
                // @ReturnType ListTag<ZombieTag>
                // @Returns a list of all zombies within the specified range (spherical).
                // @Example "0,1,2" .find_zombies_within[10] returns "2|3|17".
                // -->
                case "find_zombies_within":
                    {
                        List<TemplateObject> zombies = new List<TemplateObject>();
                        Vector3 vec3 = ToVector3();
                        float range = Utilities.StringToFloat(data.GetModifier(0));
                        for (int i = 0; i < ZombieManager.regions.Length; i++)
                        {
                            foreach (Zombie zombie in ZombieManager.regions[i].zombies)
                            {
                                if ((zombie.transform.position - vec3).sqrMagnitude <= range * range)
                                {
                                    zombies.Add(new ZombieTag(zombie));
                                }
                            }
                        }
                        return new ListTag(zombies).Handle(data.Shrink());
                    }
                // <--[tag]
                // @Name LocationTag.find_items_within[<TextTag>]
                // @Group World
                // @ReturnType ListTag<ItemTag>
                // @Returns a list of all items within the specified range (spherical).
                // @Example "0,1,2" .find_items_within[10] returns "2|3|17".
                // -->
                case "find_items_within":
                    {
                        List<TemplateObject> items = new List<TemplateObject>();
                        Vector3 vec3 = ToVector3();
                        float range = Utilities.StringToFloat(data.GetModifier(0));
                        for (byte x = 0; x < Regions.WORLD_SIZE; x++)
                        {
                            for (byte y = 0; y < Regions.WORLD_SIZE; y++)
                            {
                                foreach (ItemDrop drop in ItemManager.regions[x, y].drops)
                                {
                                    Transform item = drop.model;
                                    if ((item.position - vec3).sqrMagnitude <= range * range)
                                    {
                                        items.Add(new ItemTag(item.GetChild(0).gameObject.GetComponent<InteractableItem>()));
                                    }
                                }
                            }
                        }
                        return new ListTag(items).Handle(data.Shrink());
                    }
                // <--[tag]
                // @Name LocationTag.find_resources_within[<TextTag>]
                // @Group World
                // @ReturnType ListTag<ResourceTag>
                // @Returns a list of all resources within the specified range (spherical).
                // @Example "0,1,2" .find_resources_within[10] returns "2|3|17".
                // -->
                case "find_resources_within":
                    {
                        List<TemplateObject> resources = new List<TemplateObject>();
                        Vector3 vec3 = ToVector3();
                        float range = Utilities.StringToFloat(data.GetModifier(0));
                        for (byte x = 0; x < Regions.WORLD_SIZE; x++)
                        {
                            for (byte y = 0; y < Regions.WORLD_SIZE; y++)
                            {
                                foreach (ResourceSpawnpoint resource in LevelGround.trees[x, y])
                                {
                                    if ((resource.model.position - vec3).sqrMagnitude <= range * range)
                                    {
                                        resources.Add(new ResourceTag(resource));
                                    }
                                }
                            }
                        }
                        return new ListTag(resources).Handle(data.Shrink());
                    }
                // <--[tag]
                // @Name LocationTag.find_vehicles_within[<TextTag>]
                // @Group World
                // @ReturnType ListTag<VehicleTag>
                // @Returns a list of all vehicles within the specified range (spherical).
                // @Example "0,1,2" .find_vehicles_within[10] returns "2|3|17".
                // -->
                case "find_vehicles_within":
                    {
                        List<TemplateObject> vehicles = new List<TemplateObject>();
                        Vector3 vec3 = ToVector3();
                        float range = Utilities.StringToFloat(data.GetModifier(0));
                        foreach (InteractableVehicle veh in VehicleManager.vehicles)
                        {
                            if ((veh.gameObject.transform.position - vec3).sqrMagnitude <= range * range)
                            {
                                vehicles.Add(new VehicleTag(veh));
                            }
                        }
                        return new ListTag(vehicles).Handle(data.Shrink());
                    }
                // <--[tag]
                // @Name LocationTag.find_world_objects_within[<TextTag>]
                // @Group World
                // @ReturnType ListTag<WorldObjectTag>
                // @Returns a list of all world objects within the specified range (spherical).
                // @Example "0,1,2" .find_world_objects_within[10] returns "2|3|17".
                // @Note any overlap will count; the object's center does not need to be in the sphere.
                // -->
                case "find_world_objects_within":
                    {
                        // TODO: handle this better, it makes me sad
                        List<TemplateObject> worldObjects = new List<TemplateObject>();
                        Collider[] hitColliders = Physics.OverlapSphere(ToVector3(), Utilities.StringToFloat(data.GetModifier(0)));
                        List<int> ids = new List<int>();
                        foreach (Collider collider in hitColliders)
                        {
                            if (collider.gameObject.transform.parent == LevelObjects.models)
                            {
                                int id = collider.gameObject.GetInstanceID();
                                if (!ids.Contains(id))
                                {
                                    worldObjects.Add(WorldObjectTag.For(id));
                                    ids.Add(id);
                                }
                            }
                        }
                        return new ListTag(worldObjects).Handle(data.Shrink());
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
