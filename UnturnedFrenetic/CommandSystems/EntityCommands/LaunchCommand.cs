using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript.TagHandlers;
using UnityEngine;
using SDG.Unturned;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class LaunchCommand : AbstractCommand
    {
        // <--[command]
        // @Name launch
        // @Arguments <entity> <location>
        // @Short Launches an entity based on a vector.
        // @Updated 2015/12/22
        // @Authors Morphan1
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // This launches an entity in a direction, using the coordinates of the location
        // as a 3D vector for direction.
        // TODO: Explain more!
        // @Example
        // // This launches the entity with ID 1 at the direction (50, 50, 50).
        // launch 1 50,50,50;
        // -->
        public LaunchCommand()
        {
            Name = "launch";
            Arguments = "<entity> <location>";
            Description = "Launches an entity based on a vector.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                LocationTag.For
            };
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            try
            {
                LocationTag loc = LocationTag.For(entry.GetArgument(queue, 1));
                if (loc == null)
                {
                    queue.HandleError(entry, "Invalid location!");
                    return;
                }
                EntityTag entity = EntityTag.For(Utilities.StringToInt(entry.GetArgument(queue, 0)));
                if (entity  == null)
                {
                    queue.HandleError(entry, "Invalid entity!");
                    return;
                }
                PlayerTag player;
                if (entity.TryGetPlayer(out player))
                {
                    player.Internal.player.gameObject.AddComponent<LaunchComponent>().LaunchPlayer(loc.ToVector3());
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully launched player " + TagParser.Escape(player.ToString()) + " to " + TagParser.Escape(loc.ToString()) + "!");
                    }
                    return;
                }
                ZombieTag zombie;
                if (entity.TryGetZombie(out zombie))
                {
                    zombie.Internal.gameObject.AddComponent<LaunchComponent>().Launch(loc.ToVector3());
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully launched zombie " + TagParser.Escape(zombie.ToString()) + " to " + TagParser.Escape(loc.ToString()) + "!");
                    }
                    return;
                }
                AnimalTag animal;
                if (entity.TryGetAnimal(out animal))
                {
                    animal.Internal.gameObject.AddComponent<LaunchComponent>().Launch(loc.ToVector3());
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully launched animal " + TagParser.Escape(animal.ToString()) + " to " + TagParser.Escape(loc.ToString()) + "!");
                    }
                    return;
                }
                ItemTag item;
                if (entity.TryGetItem(out item))
                {
                    // TODO: Find some way to teleport items, barricades, etc without voiding the InstanceID?
                }
                queue.HandleError(entry, "That entity can't be launched!");
            }
            catch (Exception ex) // TODO: Necessity?
            {
                queue.HandleError(entry, "Failed to launch entity: " + ex.ToString());
            }
        }

        public class LaunchComponent : MonoBehaviour
        {
            public Vector3 Deceleration;
            public Vector3 Velocity;
            public Player Player;

            public void Launch(Vector3 velocity)
            {
                Deceleration = velocity * 0.02f / 3f;
                Velocity = velocity;
            }

            public void LaunchPlayer(Vector3 velocity)
            {
                Player = gameObject.GetComponent<Player>();
                Launch(velocity);
            }

            public void FixedUpdate()
            {
                gameObject.transform.position += Velocity * Time.deltaTime;
                if (Player != null)
                {
                    Player.sendTeleport(gameObject.transform.position, MeasurementTool.angleToByte(gameObject.transform.eulerAngles.y));
                }
                Velocity -= Deceleration;
                if (Velocity.sqrMagnitude < 1)
                {
                    Destroy(this);
                }
            }
        }
    }
}
