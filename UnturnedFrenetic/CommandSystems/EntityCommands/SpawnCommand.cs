using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using SDG.Unturned;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript;
using FreneticScript.TagHandlers;
using UnityEngine;
using Steamworks;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class SpawnCommand : AbstractCommand
    {
        // <--[command]
        // @Name spawn
        // @Arguments <entity type> <location>
        // @Short Spawns an entity at the given location.
        // @Updated 2015/12/11
        // @Authors mcmonkey
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // This spawns an entity into the world at the given location.
        // TODO: Explain more!
        // @Example
        // // This spawns a deer at the location (50, 50, 50).
        // spawn deer 50,50,50;
        // -->
        public SpawnCommand()
        {
            Name = "spawn";
            Arguments = "<entity type> <location>"; // TODO: Direction!
            Description = "Spawns an entity at the location.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                (input) => input, // TODO: Maybe validate entity type?
                (input) =>
                {
                    return LocationTag.For(input);
                }
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
                string targetAssetType = entry.GetArgument(queue, 0).ToLowerFast();
                EntityType etype = EntityType.ValueOf(targetAssetType);
                if (etype == null)
                {
                    queue.HandleError(entry, "Invalid entity type!");
                    return;
                }
                if (etype.Type == EntityAssetType.ZOMBIE)
                {
                    Vector3 vec3 = loc.ToVector3();
                    byte reg = 0; // TODO: Optionally specifiable
                    float closest = float.MaxValue;
                    for (int r = 0; r < LevelZombies.zombies.Length; r++)
                    {
                        for (int i = 0; i < LevelZombies.zombies[r].Count; i++)
                        {
                            float dist = (LevelZombies.zombies[r][i].point - vec3).sqrMagnitude;
                            if (dist < closest)
                            {
                                closest = dist;
                                reg = (byte)r;
                            }
                        }
                    }
                    ZombieManager.manager.addZombie(reg, 0, 0, 0, 0, 0, 0, 0, 0, vec3, 0, false);
                    Zombie zombie = ZombieManager.regions[reg].zombies[ZombieManager.regions[reg].zombies.Count - 1];
                    // TODO: Make this actually work! (See complaints file!)
                    /*
                    foreach (SteamPlayer player in PlayerTool.getSteamPlayers())
                    {
                        ZombieManager.manager.channel.openWrite();
                        ZombieManager.manager.channel.write(reg);
                        ZombieManager.manager.channel.write((ushort)1);
                        ZombieManager.manager.channel.write(new object[]
                            {
                                zombie.type,
                                (byte)zombie.speciality,
                                zombie.shirt,
                                zombie.pants,
                                zombie.hat,
                                zombie.gear,
                                zombie.move,
                                zombie.idle,
                                zombie.transform.position,
                                MeasurementTool.angleToByte(zombie.transform.rotation.eulerAngles.y),
                                zombie.isDead
                            });
                        ZombieManager.manager.channel.closeWrite("tellZombies", player.playerID.steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
                    }
                    */
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully spawned a zombie at " + TagParser.Escape(loc.ToString()) + "! (WARNING: IT WILL BE INVISIBLE CURRENTLY - SEE THE COMPLAINTS FILE)");
                    }
                }
                else if (etype.Type == EntityAssetType.ANIMAL)
                {
                    AnimalAssetTag asset = AnimalAssetTag.For(targetAssetType);
                    if (asset == null)
                    {
                        queue.HandleError(entry, "Invalid animal type!");
                        return;
                    }
                    // TODO: Make this bit optional!
                    RaycastHit rch;
                    while (Physics.Raycast(loc.ToVector3(), new Vector3(0, 1, 0), out rch, 5))
                    {
                        loc.Y += 3;
                    }
                    // END TODO
                    AnimalManager.manager.addAnimal(asset.Internal.id, loc.ToVector3(), 0, false);
                    Animal animal = AnimalManager.animals[AnimalManager.animals.Count - 1];
                    foreach (SteamPlayer player in PlayerTool.getSteamPlayers())
                    {
                        AnimalManager.manager.channel.openWrite();
                        AnimalManager.manager.channel.write((ushort)1);
                        AnimalManager.manager.channel.write(new object[]
                        {
                            animal.id,
                            animal.transform.position,
                            MeasurementTool.angleToByte(animal.transform.rotation.eulerAngles.y),
                            animal.isDead
                        });
                        AnimalManager.manager.channel.closeWrite("tellAnimals", player.playerID.steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
                    }
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "! (" + animal.gameObject.GetInstanceID() + ")");
                    }
                    return;
                }
                else if (etype.Type == EntityAssetType.VEHICLE)
                {
                    VehicleAssetTag asset = VehicleAssetTag.For(targetAssetType);
                    if (asset == null)
                    {
                        queue.HandleError(entry, "Invalid vehicle type!");
                        return;
                    }
                    // TODO: Make this bit optional!
                    RaycastHit rch;
                    while (Physics.Raycast(loc.ToVector3(), new Vector3(0, 1, 0), out rch, 5))
                    {
                        loc.Y += 3;
                    }
                    // END TODO
                    VehicleManager.spawnVehicle(asset.Internal.id, loc.ToVector3(), Quaternion.identity);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "!");
                    }
                }
                else if (etype.Type == EntityAssetType.WORLD_OBJECT)
                {
                    WorldObjectAssetTag asset = WorldObjectAssetTag.For(targetAssetType);
                    if (asset == null)
                    {
                        queue.HandleError(entry, "Invalid world object type!");
                        return;
                    }
                    LevelObjects.addObject(loc.ToVector3(), Quaternion.identity, asset.Internal.id);
                    // TODO: Network!
                    entry.Good(queue, "Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) +"! (WARNING: IT WILL BE INVISIBLE CURRENTLY - SEE THE COMPLAINTS FILE)");
                }
                else if (etype.Type == EntityAssetType.ITEM)
                {
                    ItemAssetTag asset = ItemAssetTag.For(targetAssetType);
                    if (asset == null)
                    {
                        queue.HandleError(entry, "Invalid item type!");
                        return;
                    }
                    byte x;
                    byte y;
                    if (Regions.tryGetCoordinate(loc.ToVector3(), out x, out y))
                    {
                        Item item = new Item(asset.Internal.id, 1, asset.Internal.quality);
                        ItemData data = new ItemData(item, ++ItemManager.instanceCount, loc.ToVector3(), Dedicator.isDedicated);
                        ItemManager.regions[x, y].items.Add(data);
                        ItemModelTracker.Track(data, loc.ToVector3());
                        ItemManager.manager.channel.send("tellItem", ESteamCall.CLIENTS, x, y, ItemManager.ITEM_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                        {
                            x,
                            y,
                            item.id,
                            item.amount,
                            item.quality,
                            item.state,
                            loc.ToVector3()
                        });
                        if (entry.ShouldShowGood(queue))
                        {
                            entry.Good(queue, "Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "!");
                        }
                    }
                    else
                    {
                        queue.HandleError(entry, "Trying to spawn item outside any valid item regions!");
                    }
                }
                else if (etype.Type == EntityAssetType.BARRICADE)
                {
                    ItemAssetTag asset = ItemAssetTag.For(targetAssetType.Substring("barricade_".Length));
                    if (asset == null || !(asset.Internal is ItemBarricadeAsset))
                    {
                        queue.HandleError(entry, "Invalid item barricade type!");
                        return;
                    }
                    BarricadeManager.dropBarricade(new Barricade(asset.Internal.id), null, loc.ToVector3(), 0f, 0f, 0f, CSteamID.Nil.m_SteamID, CSteamID.Nil.m_SteamID);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "!");
                    }
                }
                else if (etype.Type == EntityAssetType.STRUCTURE)
                {
                    ItemAssetTag asset = ItemAssetTag.For(targetAssetType.Substring("structure_".Length));
                    if (asset == null || !(asset.Internal is ItemStructureAsset))
                    {
                        queue.HandleError(entry, "Invalid item structure type!");
                        return;
                    }
                    StructureManager.dropStructure(new Structure(asset.Internal.id), loc.ToVector3(), 0f, CSteamID.Nil.m_SteamID, CSteamID.Nil.m_SteamID);
                    if (entry.ShouldShowGood(queue))
                    {
                        entry.Good(queue, "Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "!");
                    }
                }
                else
                {
                    queue.HandleError(entry, "Invalid or unspawnable entity type!");
                }
                // TODO: Maybe add the spawned object as a var to the current commandqueue.
            }
            catch (Exception ex) // TODO: Necessity?
            {
                queue.HandleError(entry, "Failed to spawn entity: " + ex.ToString());
            }
        }
    }
}
