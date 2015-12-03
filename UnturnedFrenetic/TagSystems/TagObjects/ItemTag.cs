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
    public class ItemTag : TemplateObject
    {
        public InteractableItem Internal;

        public ItemTag(InteractableItem item)
        {
            Internal = item;
        }

        public static ItemTag For(int instanceID)
        {
            for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
            {
                for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
                {
                    foreach (Transform transform in ItemManager.regions[b, b2].models)
                    {
                        InteractableItem item = transform.GetChild(0).gameObject.GetComponent<InteractableItem>();
                        if (instanceID == item.gameObject.GetInstanceID())
                        {
                            return new ItemTag(item);
                        }
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
                // @Name ItemTag.iid
                // @Group General Information
                // @ReturnType TextTag
                // @Returns this item's instance ID number.
                // @Example "2" .iid returns "2".
                // -->
                case "iid":
                    return new TextTag(Internal.gameObject.GetInstanceID()).Handle(data.Shrink());
                // <--[tag]
                // @Name ItemTag.asset
                // @Group General Information
                // @ReturnType ItemAssetTag
                // @Returns the item asset that this item is based off.
                // @Example "2" .asset returns "Rifle_Maple".
                // -->
                case "asset":
                    return new ItemAssetTag(Internal.asset).Handle(data.Shrink());
                // <--[tag]
                // @Name ItemTag.location
                // @Group Status
                // @ReturnType LocationTag
                // @Returns the item's current world position.
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
