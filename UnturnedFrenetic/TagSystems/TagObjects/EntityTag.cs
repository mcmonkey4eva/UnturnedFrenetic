using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using SDG.Unturned;
using UnityEngine;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class EntityTag : TemplateObject
    {
        public GameObject Internal;

        public EntityTag(GameObject obj)
        {
            Internal = obj;
        }

        public static EntityTag For(int instanceID)
        {
            foreach (GameObject obj in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (obj.GetInstanceID() == instanceID)
                {
                    return new EntityTag(obj);
                }
            }
            return null;
        }

        public bool TryGetPlayer(out PlayerTag tag)
        {
            Player component = Internal.GetComponent<Player>();
            if (component != null)
            {
                tag = new PlayerTag(component.channel.owner);
                return true;
            }
            tag = null;
            return false;
        }

        public bool TryGetZombie(out ZombieTag tag)
        {
            Zombie component = Internal.GetComponent<Zombie>();
            if (component != null)
            {
                tag = new ZombieTag(component);
                return true;
            }
            tag = null;
            return false;
        }

        public bool TryGetAnimal(out AnimalTag tag)
        {
            Animal component = Internal.GetComponent<Animal>();
            if (component != null)
            {
                tag = new AnimalTag(component);
                return true;
            }
            tag = null;
            return false;
        }

        public bool TryGetItem(out ItemTag tag)
        {
            InteractableItem component = Internal.GetComponent<InteractableItem>();
            if (component != null)
            {
                tag = new ItemTag(component);
                return true;
            }
            tag = null;
            return false;
        }

        public bool TryGetVehicle(out VehicleTag tag)
        {
            InteractableVehicle component = Internal.GetComponent<InteractableVehicle>();
            if (component != null)
            {
                tag = new VehicleTag(component);
                return true;
            }
            tag = null;
            return false;
        }

        public bool TryGetBarricade(out BarricadeTag tag)
        {
            byte x;
            byte y;
            ushort plant;
            ushort index;
            BarricadeRegion region;
            if (BarricadeManager.tryGetInfo(Internal.transform, out x, out y, out plant, out index, out region))
            {
                tag = new BarricadeTag(Internal.transform, region.barricades[index]);
                return true;
            }
            tag = null;
            return false;
        }

        public bool TryGetStructure(out StructureTag tag)
        {
            byte x;
            byte y;
            ushort index;
            StructureRegion region;
            if (StructureManager.tryGetInfo(Internal.transform, out x, out y, out index, out region))
            {
                tag = new StructureTag(Internal.transform, region.structures[index]);
                return true;
            }
            tag = null;
            return false;
        }

        public bool TryGetResource(out ResourceTag tag)
        {
            Transform transform = Internal.transform;
            byte x;
            byte y;
            if (Regions.tryGetCoordinate(transform.position, out x, out y))
            {
                List<ResourceSpawnpoint> list = LevelGround.trees[x, y];
                foreach (ResourceSpawnpoint resource in list)
                {
                    if (transform == resource.model)
                    {
                        tag = new ResourceTag(resource);
                        return true;
                    }
                }
            }
            tag = null;
            return false;
        }

        public bool TryGetWorldObject(out WorldObjectTag tag)
        {
            Transform transform = Internal.transform;
            byte x;
            byte y;
            if (Regions.tryGetCoordinate(transform.position, out x, out y))
            {
                List<LevelObject> list = LevelObjects.objects[x, y];
                foreach (LevelObject obj in list)
                {
                    if (transform == obj.transform)
                    {
                        tag = new WorldObjectTag(obj);
                        return true;
                    }
                }
            }
            tag = null;
            return false;
        }

        public override TemplateObject Handle(TagData data)
        {
            if (data.Remaining == 0)
            {
                return this;
            }
            switch (data[0])
            {
                // <--[tag]
                // @Name EntityTag.iid
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns this entity's instance ID number.
                // @Example "2" .iid returns "2".
                // -->
                case "iid":
                    return new NumberTag(Internal.GetInstanceID()).Handle(data.Shrink());
                // <--[tag]
                // @Name EntityTag.location
                // @Group Status
                // @ReturnType LocationTag
                // @Returns the entity's current world position.
                // @Example "2" .location returns "(5, 10, 15)".
                // -->
                case "location":
                    return new LocationTag(Internal.transform.position).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data.Shrink());
            }
        }

        public override string ToString()
        {
            return "e:" + Internal.GetInstanceID().ToString();
        }
    }
}
