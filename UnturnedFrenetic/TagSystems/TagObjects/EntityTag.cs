using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;


namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class EntityTag : TemplateObject
    {
        public TemplateObject TagObject;

        public EntityTag(TemplateObject tagObject)
        {
            TagObject = tagObject;
        }

        public static EntityTag For(int instanceID)
        {
            // TODO: better way to handle this?
            TemplateObject obj = null;
            obj = AnimalTag.For(instanceID);
            if (obj == null)
            {
                obj = BarricadeTag.For(instanceID);
            }
            if (obj == null)
            {
                obj = ItemTag.For(instanceID);
            }
            if (obj == null)
            {
                obj = ResourceTag.For(instanceID);
            }
            if (obj == null)
            {
                obj = VehicleTag.For(instanceID);
            }
            if (obj == null)
            {
                obj = WorldObjectTag.For(instanceID);
            }
            if (obj == null)
            {
                obj = ZombieTag.For(instanceID);
            }
            if (obj == null)
            {
                return null;
            }
            return new EntityTag(obj);
        }

        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                // TODO: generic entity tags? such as entity_type to return "ZombieTag" etc?..
                default:
                    return TagObject.Handle(data);
            }
        }

        public override string ToString()
        {
            return TagObject.ToString();
        }
    }
}
