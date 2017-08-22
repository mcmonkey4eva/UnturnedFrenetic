using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class TeleportCommand : AbstractCommand
    {
        // <--[command]
        // @Name teleport
        // @Arguments <entity> <location>
        // @Short Teleports an entity to the given location.
        // @Updated 2015/12/11
        // @Authors Morphan1
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // This teleports an entity from its current location to a new location.
        // TODO: Explain more!
        // @Example
        // // This teleports the entity with ID 1 at the location (50, 50, 50).
        // teleport 1 50,50,50;
        // -->
        public TeleportCommand()
        {
            Name = "teleport";
            Arguments = "<entity> <location>"; // TODO: Direction!
            Description = "Teleports the entity to the location.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                LocationTag.For
            };
        }

        public override void Execute(FreneticScript.CommandSystem.CommandQueue queue, CommandEntry entry)
        {
            try
            {
                LocationTag loc = LocationTag.For(entry.GetArgument(queue, 1));
                if (loc == null)
                {
                    queue.HandleError(entry, "Invalid location!");
                    return;
                }
                EntityTag entity = EntityTag.For(entry.GetArgumentObject(queue, 0));
                if (entity  == null)
                {
                    queue.HandleError(entry, "Invalid entity!");
                    return;
                }
                PlayerTag player;
                if (entity.TryGetPlayer(out player))
                {
                    player.Internal.player.sendTeleport(loc.ToVector3(), 0);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully teleported a player to " + TagParser.Escape(loc.ToString()) + "!");
                    }
                    return;
                }
                ZombieTag zombie;
                if (entity.TryGetZombie(out zombie))
                {
                    zombie.Internal.transform.position = loc.ToVector3();
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully teleported a zombie to " + TagParser.Escape(loc.ToString()) + "!");
                    }
                    return;
                }
                AnimalTag animal;
                if (entity.TryGetAnimal(out animal)) 
                {
                    animal.Internal.transform.position = loc.ToVector3();
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully teleported an animal to " + TagParser.Escape(loc.ToString()) + "!");
                    }
                    return;
                }
                ItemEntityTag item;
                if (entity.TryGetItem(out item))
                {
                    // TODO: Find some way to teleport items, barricades, etc without voiding the InstanceID?
                    /*
                    Transform transform = item.Internal.transform.parent;
                    byte x;
                    byte y;
                    if (Regions.tryGetCoordinate(transform.position, out x, out y))
                    {
                        ItemRegion region = ItemManager.regions[x, y];
                        for (byte i = 0; i < region.models.Count; i+)
                        {
                            if (region.models[i] == transform)
                            {
                                ItemManager.regions[x, y].items.RemoveAt(i);
                                ItemModelTracker.Untrack(x, y, i);
                                ItemManager.manager.channel.send("tellTakeItem", ESteamCall.CLIENTS, x, y, ItemManager.ITEM_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                                {
                                    x,
                                    y,
                                    loc.ToVector3()
                                });
                                // Then respawn item at new location
                                break;
                            }
                        }
                    }
                    */
                }
                queue.HandleError(entry, "That entity can't be teleported!");
            }
            catch (Exception ex) // TODO: Necessity of this?
            {
                queue.HandleError(entry, "Failed to teleport entity: " + ex.ToString());
            }
        }
    }
}
