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
                EPlayerKill kill; // for use with "out EPlayerKill" parameters
                PlayerTag player;
                if (entity.TryGetPlayer(out player))
                {
                    player.Internal.player.life.askDamage((byte)num.Internal, Vector3.zero, EDeathCause.KILL, ELimb.SPINE, CSteamID.Nil, out kill);
                    entry.Good("Successfully damaged player " + TagParser.Escape(player.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                    return;
                }
                ZombieTag zombie;
                if (entity.TryGetZombie(out zombie))
                {
                    zombie.Internal.askDamage((byte)num.Internal, Vector3.zero, out kill);
                    entry.Good("Successfully damaged zombie " + TagParser.Escape(zombie.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                    return;
                }
                AnimalTag animal;
                if (entity.TryGetAnimal(out animal))
                {
                    animal.Internal.askDamage((byte)num.Internal, Vector3.zero, out kill);
                    entry.Good("Successfully damaged animal " + TagParser.Escape(animal.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                    return;
                }
                BarricadeTag barricade;
                if (entity.TryGetBarricade(out barricade))
                {
                    barricade.InternalData.barricade.askDamage((ushort)num.Internal);
                    entry.Good("Successfully damaged barricade " + TagParser.Escape(barricade.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                }
                ResourceTag resource;
                if (entity.TryGetResource(out resource))
                {
                    resource.Internal.askDamage((ushort)num.Internal);
                    entry.Good("Successfully damaged resource " + TagParser.Escape(resource.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                }
                StructureTag structure;
                if (entity.TryGetStructure(out structure))
                {
                    structure.InternalData.structure.askDamage((ushort)num.Internal);
                    entry.Good("Successfully damaged structure " + TagParser.Escape(structure.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                }
                VehicleTag vehicle;
                if (entity.TryGetVehicle(out vehicle))
                {
                    vehicle.Internal.askDamage((ushort)num.Internal, false);
                    entry.Good("Successfully damaged vehicle " + TagParser.Escape(vehicle.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
                }
                entry.Bad("That entity can't be damaged!");
            }
            catch (Exception ex)
            {
                entry.Bad("Failed to damage entity: " + ex.ToString());
            }
        }
    }
}
