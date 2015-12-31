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

        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
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
