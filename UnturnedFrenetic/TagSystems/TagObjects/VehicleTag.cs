using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class VehicleTag : TemplateObject
    {
        public InteractableVehicle Internal;

        public VehicleTag(InteractableVehicle vehicle)
        {
            Internal = vehicle;
        }

        public static VehicleTag For(int instanceID)
        {
            foreach (InteractableVehicle vehicle in VehicleManager.vehicles)
            {
                if (vehicle.gameObject.GetInstanceID() == instanceID)
                {
                    return new VehicleTag(vehicle);
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
                // @Name VehicleTag.name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the name of the vehicle's type.
                // @Example "2" .name returns "APC_Forest".
                // -->
                case "name":
                    return new TextTag(Internal.asset.name).Handle(data.Shrink());
                // <--[tag]
                // @Name VehicleTag.vid
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the vehicle ID number of the vehicle.
                // @Example "2" .iid returns "1".
                // -->
                case "vid":
                    return new TextTag(Internal.index).Handle(data.Shrink());
                // <--[tag]
                // @Name VehicleTag.iid
                // @Group General Information
                // @ReturnType TextTag
                // @Returns this vehicle's instance ID number.
                // @Example "2" .iid returns "2".
                // -->
                case "iid":
                    return new TextTag(Internal.gameObject.GetInstanceID()).Handle(data.Shrink());
                // <--[tag]
                // @Name VehicleTag.asset
                // @Group General Information
                // @ReturnType VehicleAssetTag
                // @Returns the vehicle asset that this vehicle is based off.
                // @Example "2" .asset returns "Rifle_Maple".
                // -->
                case "asset":
                    return new VehicleAssetTag(Internal.asset).Handle(data.Shrink());
                // <--[tag]
                // @Name VehicleTag.location
                // @Group Status
                // @ReturnType LocationTag
                // @Returns the vehicle's current world position.
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
