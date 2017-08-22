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
using Steamworks;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class RemoveCommand : AbstractCommand
    {
        // <--[command]
        // @Name remove
        // @Arguments <entity>
        // @Short Immediately fully removes an entity.
        // @Updated 2016/04/30
        // @Authors mcmonkey
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Immediately fully removes an entity.
        // TODO: Explain more!
        // @Example
        // // This removes the entity with ID 1.
        // damage 1 10;
        // -->
        public RemoveCommand()
        {
            Name = "remove";
            Arguments = "<entity>";
            Description = "Immediately fully removes an entity.";
            MinimumArguments = 1;
            MaximumArguments = 1;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For
            };
        }

        public override void Execute(FreneticScript.CommandSystem.CommandQueue queue, CommandEntry entry)
        {
            EntityTag entity = EntityTag.For(entry.GetArgumentObject(queue, 0));
            if (entity == null)
            {
                queue.HandleError(entry, "Invalid entity!");
                return;
            }
            PlayerTag player;
            if (entity.TryGetPlayer(out player)) // TODO: Kick?
            {
                Provider.kick(player.Internal.playerID.steamID, "Removed forcibly.");
                if (entry.ShouldShowGood(queue))
                {

                    entry.Good(queue, "Successfully removed a player!");
                }
                return;
            }
            ZombieTag zombie;
            if (entity.TryGetZombie(out zombie)) // TODO: Remove!
            {
                zombie.Internal.health = 0;
                ZombieManager.sendZombieDead(zombie.Internal, new Vector3(9999, 9999, -9999));
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully removed a zombie!");
                }
                return;
            }
            AnimalTag animal;
            if (entity.TryGetAnimal(out animal))
            {
                animal.Internal.health = 0;
                AnimalManager.sendAnimalDead(animal.Internal, new Vector3(9999, 9999, -9999));
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully removed an animal!");
                }
                return;
            }
            BarricadeTag barricade;
            if (entity.TryGetBarricade(out barricade))
            {
                // TODO: Use BarricadeManager magic to remove properly?
                barricade.InternalData.barricade.askDamage(barricade.InternalData.barricade.health);
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully destroyed a barricade!");
                }
                return;
            }
            ResourceTag resource;
            if (entity.TryGetResource(out resource)) // TODO: Remove!
            {
                // TODO: Use ResourceManager magic to remove properly?
                resource.Internal.askDamage(resource.Internal.health);
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully destroyed a resource!");
                }
                return;
            }
            StructureTag structure;
            if (entity.TryGetStructure(out structure)) // TODO: Remove!
            {
                // TODO: Use StructureManager magic to remove properly?
                structure.InternalData.structure.askDamage(structure.InternalData.structure.health);
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully destroyed a structure!");
                }
                return;
            }
            VehicleTag vehicle;
            if (entity.TryGetVehicle(out vehicle)) // TODO: Remove!
            {
                vehicle.Internal.health = 0;
                VehicleManager.sendVehicleExploded(vehicle.Internal);
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully removed a vehicle!");
                }
                return;
            }
            queue.HandleError(entry, "That entity can't be damaged!");
        }
    }
}
