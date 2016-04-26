using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using Frenetic.TagHandlers;
using UnityEngine;
using System.Reflection;
using SDG.Unturned;
using Frenetic.TagHandlers.Objects;

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
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 2)
            {
                ShowUsage(entry);
                return;
            }
            try
            {
                BooleanTag bones = null;
                BooleanTag bleeding = null;
                if (entry.Arguments.Count > 2) // TODO: BooleanTag.TryFor?
                {
                    if (entry.Arguments.Count > 3)
                    {
                        bones = new BooleanTag(entry.GetArgument(3).StartsWith("t"));
                    }
                    bleeding = new BooleanTag(entry.GetArgument(2).StartsWith("t"));
                }
                NumberTag num = new NumberTag(Utilities.StringToInt(entry.GetArgument(1))); // TODO: NumberTag.TryFor
                if (num.Internal < 0.0)
                {
                    entry.Bad("Must provide a non-negative number!");
                    return;
                }
                EntityTag entity = EntityTag.For(Utilities.StringToInt(entry.GetArgument(0)));
                if (entity == null)
                {
                    entry.Bad("Invalid entity!");
                    return;
                }
                PlayerTag player;
                if (entity.TryGetPlayer(out player))
                {
                    player.Internal.player.life.askHeal((byte)num.Internal, bleeding != null && bleeding.Internal, bones != null && bones.Internal);
                    entry.Good("Successfully healed player " + TagParser.Escape(player.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
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
                    entry.Good("Successfully healed zombie " + TagParser.Escape(zombie.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
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
                    entry.Good("Successfully healed animal " + TagParser.Escape(animal.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
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
                    entry.Good("Successfully healed barricade " + TagParser.Escape(barricade.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
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
                    entry.Good("Successfully healed resource " + TagParser.Escape(resource.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
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
                    entry.Good("Successfully healed structure " + TagParser.Escape(structure.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                }
                VehicleTag vehicle;
                if (entity.TryGetVehicle(out vehicle))
                {
                    vehicle.Internal.askRepair((ushort)num.Internal);
                    entry.Good("Successfully healed vehicle " + TagParser.Escape(vehicle.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                }
                entry.Bad("That entity can't be healed!");
            }
            catch (Exception ex)
            {
                entry.Bad("Failed to heal entity: " + ex.ToString());
            }
        }
    }
}
