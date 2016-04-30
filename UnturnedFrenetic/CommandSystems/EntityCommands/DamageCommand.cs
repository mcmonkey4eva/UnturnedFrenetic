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
    public class DamageCommand : AbstractCommand
    {
        // <--[command]
        // @Name damage
        // @Arguments <entity> <amount>
        // @Short Damages an entity by a specified amount.
        // @Updated 2016/04/25
        // @Authors Morphan1
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // This damages an entity a certain amount. Must be a positive number.
        // TODO: Explain more!
        // @Example
        // // This damages the entity with ID 1 by 10.
        // damage 1 10;
        // -->
        public DamageCommand()
        {
            Name = "damage";
            Arguments = "<entity> <amount>";
            Description = "Damages an entity by a specified amount.";
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
                    UFMHealthController healthController = player.Internal.player.GetComponent<UFMHealthController>();
                    uint health = healthController != null ? healthController.health : life.health;
                    if (num.Internal >= health)
                    {
                        uint amount = (uint)num.Internal;
                        if (healthController != null)
                        {
                            healthController.health = 0;
                        }
                        if (amount >= byte.MaxValue) // TODO: better handling
                        {
                            life._health = 0;
                            amount = 1;
                        }
                        life.askDamage((byte)amount, Vector3.zero, EDeathCause.KILL, ELimb.SPINE, CSteamID.Nil, out kill, null);
                    }
                    else
                    {
                        uint amount = (uint)num.Internal;
                        if (healthController != null)
                        {
                            healthController.Damage((uint)num.Internal);
                            amount = (uint)(((double)amount / healthController.maxHealth) * 100.0);
                        }
                        life._health -= (byte)amount;
                        life.channel.send("tellHealth", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                        {
                            life.health
                        });
                    }
                    if (entry.ShouldShowGood(queue))
                    {

                        entry.Good(queue, "Successfully damaged a player by " + TagParser.Escape(num.ToString()) + "!");
                    }
                    return;
                }
                ZombieTag zombie;
                if (entity.TryGetZombie(out zombie))
                {
                    zombie.Internal.askDamage((byte)num.Internal, Vector3.zero, out kill);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully damaged a zombie by " + TagParser.Escape(num.ToString()) + "!");
                    }
                    return;
                }
                AnimalTag animal;
                if (entity.TryGetAnimal(out animal))
                {
                    animal.Internal.askDamage((byte)num.Internal, Vector3.zero, out kill);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully damaged an animal by " + TagParser.Escape(num.ToString()) + "!");
                    }
                    return;
                }
                BarricadeTag barricade;
                if (entity.TryGetBarricade(out barricade))
                {
                    // TODO: Use BarricadeManager?
                    barricade.InternalData.barricade.askDamage((ushort)num.Internal);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully damaged a barricade by " + TagParser.Escape(num.ToString()) + "!");
                    }
                    return;
                }
                ResourceTag resource;
                if (entity.TryGetResource(out resource))
                {
                    // TODO: Use ResourceManager?
                    resource.Internal.askDamage((ushort)num.Internal);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully damaged a resource by " + TagParser.Escape(num.ToString()) + "!");
                    }
                    return;
                }
                StructureTag structure;
                if (entity.TryGetStructure(out structure))
                {
                    // TODO: Use StructureManager?
                    structure.InternalData.structure.askDamage((ushort)num.Internal);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully damaged a structure by " + TagParser.Escape(num.ToString()) + "!");
                    }
                    return;
                }
                VehicleTag vehicle;
                if (entity.TryGetVehicle(out vehicle))
                {
                    vehicle.Internal.askDamage((ushort)num.Internal, false);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully damaged a vehicle by " + TagParser.Escape(num.ToString()) + "!");
                    }
                    return;
                }
                queue.HandleError(entry, "That entity can't be damaged!");
            }
            catch (Exception ex) // TODO: Necessity?
            {
                queue.HandleError(entry, "Failed to damage entity: " + ex.ToString());
            }
        }
    }
}
