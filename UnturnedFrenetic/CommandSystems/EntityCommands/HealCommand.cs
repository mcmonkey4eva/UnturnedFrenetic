using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript.TagHandlers;
using UnityEngine;
using System.Reflection;
using SDG.Unturned;
using FreneticScript.TagHandlers.Objects;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class HealCommand : AbstractCommand
    {
        // <--[command]
        // @Name heal
        // @Arguments <entity> <amount> [bleeding] [bones]
        // @Short Heals an entity by a specified amount.
        // @Updated 2016/04/25
        // @Authors Morphan1
        // @Group Entity
        // @Minimum 2
        // @Maximum 4
        // @Description
        // This heals an entity a certain amount. Must be a positive number.
        // You may also specify boolean values to determine whether you stop
        // a player's bleeding or heal their bones.
        // TODO: Explain more!
        // @Example
        // // This heals the entity with ID 1 by 10.
        // heal 1 10;
        // @Example
        // // This heals the player to full health and stops their bleeding.
        // heal <{player.iid}> 100 true
        // @Example
        // // This heals the player by 1 and fixes their bones.
        // heal <{player.iid}> 1 false true
        // -->
        public HealCommand()
        {
            Name = "heal";
            Arguments = "<entity> <amount> [bleeding] [bones]";
            Description = "Heals an entity by a specified amount.";
            MinimumArguments = 2;
            MaximumArguments = 4;
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            if (entry.Arguments.Count < 2)
            {
                ShowUsage(queue, entry);
                return;
            }
            try
            {
                BooleanTag bones = null;
                BooleanTag bleeding = null;
                if (entry.Arguments.Count > 2) // TODO: Named arguments?
                {
                    if (entry.Arguments.Count > 3)
                    {
                        bones = BooleanTag.TryFor(entry.GetArgumentObject(queue, 3));
                    }
                    bleeding = BooleanTag.TryFor(entry.GetArgumentObject(queue, 2));
                }
                NumberTag num = NumberTag.TryFor(entry.GetArgumentObject(queue, 1));
                if (num.Internal < 0.0)
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
                    player.Internal.player.life.askHeal((byte)num.Internal, bleeding != null && bleeding.Internal, bones != null && bones.Internal);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully healed player " + TagParser.Escape(player.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
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
                        entry.Good(queue, "Successfully healed zombie " + TagParser.Escape(zombie.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
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
                        entry.Good(queue, "Successfully healed animal " + TagParser.Escape(animal.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
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
                        entry.Good(queue, "Successfully healed barricade " + TagParser.Escape(barricade.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                    }
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
                        entry.Good(queue, "Successfully healed resource " + TagParser.Escape(resource.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                    }
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
                        entry.Good(queue, "Successfully healed structure " + TagParser.Escape(structure.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                    }
                }
                VehicleTag vehicle;
                if (entity.TryGetVehicle(out vehicle))
                {
                    vehicle.Internal.askRepair((ushort)num.Internal);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully healed vehicle " + TagParser.Escape(vehicle.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                    }
                }
                queue.HandleError(entry, "That entity can't be healed!");
            }
            catch (Exception ex)
            {
                queue.HandleError(entry, ("Failed to heal entity: " + ex.ToString()));
            }
        }
    }
}
