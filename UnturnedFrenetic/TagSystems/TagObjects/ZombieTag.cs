using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class ZombieTag : TemplateObject
    {
        // <--[object]
        // @Type ZombieTag
        // @SubType EntityTag
        // @Group Entities
        // @Description Represents a spawned zombie in the world.
        // -->

        public Zombie Internal;

        public ZombieTag(Zombie zombie)
        {
            Internal = zombie;
        }

        public static ZombieTag For(int instanceID)
        {
            for (int i = 0; i < ZombieManager.regions.Length; i++)
            {
                foreach (Zombie zombie in ZombieManager.regions[i].zombies)
                {
                    if (zombie.gameObject.GetInstanceID() == instanceID)
                    {
                        return new ZombieTag(zombie);
                    }
                }
            }
            return null;
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
                // @Name ZombieTag.zid
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the zombie ID for this zombie in its region.
                // @Example "2" .zid returns "1".
                // -->
                case "zid":
                    return new NumberTag(Internal.id).Handle(data.Shrink());
                // <--[tag]
                // @Name ZombieTag.region
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the zombie region this zombie belongs to.
                // @Example "2" .region returns "4"
                // -->
                case "region":
                    return new NumberTag(Internal.bound).Handle(data.Shrink());
                // <--[tag]
                // @Name ZombieTag.specialty
                // @Group General Information
                // @ReturnType TextTag
                // @Returns this zombie's specialty. Value can be: NORMAL, MEGA, CRAWLER, SPRINTER.
                // @Example "2" .specialty returns "NORMAL".
                // -->
                case "specialty":
                    return new TextTag(Internal.speciality.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name ZombieTag.health
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the zombie's current health.
                // @Example "2" .health returns "96".
                // -->
                case "health":
                    return new NumberTag(Internal.health).Handle(data.Shrink());
                // <--[tag]
                // @Name ZombieTag.max_health
                // @Group General Information
                // @ReturnType NumberTag
                // @Returns the zombie's maximum health.
                // @Example "2" .max_health returns "100".
                // -->
                case "max_health":
                    return new NumberTag(Internal.maxHealth).Handle(data.Shrink());
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
