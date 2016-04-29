using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript.TagHandlers;
using UnityEngine;
using SDG.Unturned;
using FreneticScript.TagHandlers.Objects;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class HealCommand : AbstractCommand
    {
        // <--[command]
        // @Name heal
        // @Arguments <entity> <amount>
        // @Short Heals an entity by a specified amount.
        // @Updated 2016/04/25
        // @Authors Morphan1
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // This heals an entity a certain amount. Must be a positive number.
        // TODO: Explain more!
        // @Example
        // // This heals the entity with ID 1 by 10.
        // heal 1 10;
        // -->
        public HealCommand()
        {
            Name = "heal";
            Arguments = "<entity> <amount>";
            Description = "Heals an entity by a specified amount.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                IntegerTag.TryFor
            };
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            try
            {
                IntegerTag num = IntegerTag.TryFor(entry.GetArgumentObject(queue, 1));
                if (num.Internal < 0)
                {
                    queue.HandleError(entry, "Must provide a non-negative number!");
                    return;
                }
                EntityTag entity = EntityTag.For(Utilities.StringToInt(entry.GetArgument(queue, 0)));
                if (entity == null)
                {
                    queue.HandleError(entry, "Invalid entity!");
                    return;
                }
                PlayerTag player;
                if (entity.TryGetPlayer(out player))
                {
                    player.Internal.player.life.askHeal((byte)num.Internal, false, false);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully healed a player to a new health value of " + player.Internal.player.life.health + "!");
                    }
                    return;
                }
                ZombieTag zombie;
                if (entity.TryGetZombie(out zombie))
                {
                    Zombie inZomb = zombie.Internal;
                    inZomb.health += (ushort)num.Internal;
                    if (inZomb.health > inZomb.maxHealth)
                    {
                        inZomb.health = inZomb.maxHealth;
                    }
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully healed a zombie to a new health value of " + inZomb.health + "!");
                    }
                    return;
                }
                AnimalTag animal;
                if (entity.TryGetAnimal(out animal))
                {
                    Animal inAnimal = animal.Internal;
                    inAnimal.health += (ushort)num.Internal;
                    if (inAnimal.health > inAnimal.asset.health)
                    {
                        inAnimal.health = inAnimal.asset.health;
                    }
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully healed an animal to a new health value of " + inAnimal.health + "!");
                    }
                    return;
                }
                BarricadeTag barricade;
                if (entity.TryGetBarricade(out barricade))
                {
                    Barricade inBarricade = barricade.InternalData.barricade;
                    inBarricade.health += (ushort)num.Internal;
                    ushort max = ((ItemBarricadeAsset)Assets.find(EAssetType.ITEM, inBarricade.id)).health;
                    if (inBarricade.health > max)
                    {
                        inBarricade.health = max;
                    }
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully healed a barricade to a new health value of " + inBarricade.health + "!");
                    }
                    return;
                }
                ResourceTag resource;
                if (entity.TryGetResource(out resource))
                {
                    ResourceSpawnpoint inResource = resource.Internal;
                    inResource.health += (ushort)num.Internal;
                    ushort max = ((ResourceAsset)Assets.find(EAssetType.RESOURCE, inResource.id)).health;
                    if (inResource.health > max)
                    {
                        inResource.health = max;
                    }
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully healed a resource to a new health value of " + inResource.health + "!");
                    }
                    return;
                }
                StructureTag structure;
                if (entity.TryGetStructure(out structure))
                {
                    Structure inStructure = structure.InternalData.structure;
                    inStructure.health += (ushort)num.Internal;
                    ushort max = ((ItemStructureAsset)Assets.find(EAssetType.ITEM, inStructure.id)).health;
                    if (inStructure.health > max)
                    {
                        inStructure.health = max;
                    }
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully healed a structure to a new health value of " + inStructure.health + "!");
                    }
                    return;
                }
                VehicleTag vehicle;
                if (entity.TryGetVehicle(out vehicle))
                {
                    vehicle.Internal.askRepair((ushort)num.Internal);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully healed a vehicle to a new health value of " + vehicle.Internal.health + "!");
                    }
                    return;
                }
                queue.HandleError(entry, "That entity can't be healed!");
            }
            catch (Exception ex) // TODO: Necessity?
            {
                queue.HandleError(entry, ("Failed to heal entity: " + ex.ToString()));
            }
        }
    }
}
