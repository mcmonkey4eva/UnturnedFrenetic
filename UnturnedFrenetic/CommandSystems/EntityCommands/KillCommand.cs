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
    public class KillCommand : AbstractCommand
    {
        // <--[command]
        // @Name kill
        // @Arguments <entity>
        // @Short Immediately kills an entity.
        // @Updated 2016/04/30
        // @Authors mcmonkey
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Immediately kills an entity.
        // TODO: Explain more!
        // @Example
        // // This kills the entity with ID 1.
        // kill 1;
        // -->
        public KillCommand()
        {
            Name = "kill";
            Arguments = "<entity>";
            Description = "Immediately kills an entity.";
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
            EPlayerKill kill; // for use with "out EPlayerKill" parameters
            PlayerTag player;
            if (entity.TryGetPlayer(out player))
            {
                PlayerLife life = player.Internal.player.life;
                life.askDamage(life.health, Vector3.zero, EDeathCause.KILL, ELimb.SPINE, CSteamID.Nil, out kill, null);
                if (entry.ShouldShowGood(queue))
                {

                    entry.Good(queue, "Successfully killed a player!");
                }
                return;
            }
            ZombieTag zombie;
            if (entity.TryGetZombie(out zombie))
            {
                while (!zombie.Internal.isDead)
                {
                    uint xp;
                    zombie.Internal.askDamage((byte)zombie.Internal.health, Vector3.zero, out kill, out xp);
                }
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully killed a zombie!");
                }
                return;
            }
            AnimalTag animal;
            if (entity.TryGetAnimal(out animal))
            {
                while (!animal.Internal.isDead)
                {
                    uint xp;
                    animal.Internal.askDamage((byte)animal.Internal.health, Vector3.zero, out kill, out xp);
                }
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully killed an animal!");
                }
                return;
            }
            BarricadeTag barricade;
            if (entity.TryGetBarricade(out barricade))
            {
                // TODO: Use BarricadeManager?
                barricade.InternalData.barricade.askDamage(barricade.InternalData.barricade.health);
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully destroyed a barricade!");
                }
                return;
            }
            ResourceTag resource;
            if (entity.TryGetResource(out resource))
            {
                // TODO: Use ResourceManager?
                resource.Internal.askDamage(resource.Internal.health);
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully destroyed a resource!");
                }
                return;
            }
            StructureTag structure;
            if (entity.TryGetStructure(out structure))
            {
                // TODO: Use StructureManager?
                structure.InternalData.structure.askDamage(structure.InternalData.structure.health);
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully destroyed a structure!");
                }
                return;
            }
            VehicleTag vehicle;
            if (entity.TryGetVehicle(out vehicle))
            {
                vehicle.Internal.askDamage(vehicle.Internal.health, false);
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully destroyed a vehicle!");
                }
                return;
            }
            queue.HandleError(entry, "That entity can't be damaged!");
        }
    }
}
