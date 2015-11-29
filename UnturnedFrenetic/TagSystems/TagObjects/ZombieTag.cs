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
        public Zombie Internal;
        public int InternalRegionID;

        public ZombieTag(Zombie zombie, int regionID)
        {
            Internal = zombie;
            InternalRegionID = regionID;
        }

        public static ZombieTag For(int instanceID)
        {
            for (int i = 0; i < ZombieManager.regions.Length; i++)
            {
                foreach (Zombie zombie in ZombieManager.regions[i].zombies)
                {
                    if (zombie.gameObject.GetInstanceID() == instanceID)
                    {
                        return new ZombieTag(zombie, i);
                    }
                }
            }
            return null;
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
                // @Name ZombieTag.zid
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the zombie ID for this zombie in its region.
                // @Example "2" .zid returns "1".
                // -->
                case "zid":
                    return new TextTag(Internal.id).Handle(data.Shrink());
                // <--[tag]
                // @Name ZombieTag.region
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the zombie region this zombie belongs to.
                // @Example "2" .region returns "4"
                // -->
                case "region":
                    return new TextTag(InternalRegionID).Handle(data.Shrink());
                // <--[tag]
                // @Name ZombieTag.iid
                // @Group General Information
                // @ReturnType TextTag
                // @Returns this zombie's instance ID number.
                // @Example "2" .iid returns "2".
                // -->
                case "iid":
                    return new TextTag(Internal.gameObject.GetInstanceID()).Handle(data.Shrink());
                // <--[tag]
                // @Name ZombieTag.specialty
                // @Group General Information
                // @ReturnType TextTag
                // @Returns this zombie's specialty. Value can be: NORMAL, MEGA, CRAWLER, SPRINTER.
                // @Example "2" .specialty returns "NORMAL".
                // -->
                case "type":
                    return new TextTag(Internal.type).Handle(data.Shrink());
                // <--[tag]
                // @Name ZombieTag.location
                // @Group Status
                // @ReturnType LocationTag
                // @Returns the zombie's current world position.
                // @Example "2" .location returns "(5, 10, 15)".
                // -->
                case "location":
                    return new LocationTag(Internal.transform.position).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.gameObject.GetInstanceID().ToString();
        }
    }
}
