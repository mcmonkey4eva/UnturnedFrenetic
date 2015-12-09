using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.CommandSystem;
using SDG.Unturned;
using UnturnedFrenetic.TagSystems.TagObjects;
using Frenetic.TagHandlers;
using System.Reflection;
using UnityEngine;
using Steamworks;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    class SpawnCommand : AbstractCommand
    {
        // TODO: Meta!
        public SpawnCommand()
        {
            Name = "spawn";
            Arguments = "<entity type> <location>"; // TODO: Direction!
            Description = "Spawns an entity at the location.";
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
                LocationTag loc = LocationTag.For(entry.GetArgument(1));
                if (loc == null)
                {
                    entry.Bad("Invalid location!");
                    return;
                }
                string targetAssetType = entry.GetArgument(0).ToLower();
                EntityType etype = EntityType.ValueOf(targetAssetType);
                if (etype == null)
                {
                    entry.Bad("Invalid entity type!");
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
                    entry.Good("Successfully spawned a zombie at " + TagParser.Escape(loc.ToString()) + "! (WARNING: IT WILL BE INVISIBLE CURRENTLY - SEE THE COMPLAINTS FILE)");
                }
                else if (etype.Type == EntityAssetType.ANIMAL)
                {
                    AnimalAssetTag asset = AnimalAssetTag.For(targetAssetType);
                    if (asset == null)
                    {
                        entry.Bad("Invalid animal type!");
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
                    entry.Good("Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "!");
                    return;
                }
                else if (etype.Type == EntityAssetType.VEHICLE)
                {
                    VehicleAssetTag asset = VehicleAssetTag.For(targetAssetType);
                    if (asset == null)
                    {
                        entry.Bad("Invalid vehicle type!");
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
                    entry.Good("Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "!");
                }
                else if (etype.Type == EntityAssetType.WORLD_OBJECT)
                {
                    WorldObjectAssetTag asset = WorldObjectAssetTag.For(targetAssetType);
                    if (asset == null)
                    {
                        entry.Bad("Invalid world object type!");
                        return;
                    }
                    LevelObjects.addObject(loc.ToVector3(), Quaternion.identity, asset.Internal.id);
                    // TODO: Network!
                    entry.Good("Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) +"! (WARNING: IT WILL BE INVISIBLE CURRENTLY - SEE THE COMPLAINTS FILE)");
                }
                else if (etype.Type == EntityAssetType.ITEM)
                {
                    ItemAssetTag asset = ItemAssetTag.For(targetAssetType);
                    if (asset == null)
                    {
                        entry.Bad("Invalid item type!");
                        return;
                    }
                    byte x;
                    byte y;
                    if (Regions.tryGetCoordinate(loc.ToVector3(), out x, out y))
                    {
                        Item item = new Item(asset.Internal.id, 1, asset.Internal.quality);
                        ItemManager.regions[x, y].items.Add(new ItemData(item, loc.ToVector3(), Dedicator.isDedicated));
                        ItemModelTracker.Track(item, loc.ToVector3());
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
                        entry.Good("Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "!");
                    }
                    else
                    {
                        entry.Bad("Trying to spawn item outside any valid item regions!");
                    }
                }
                else if (etype.Type == EntityAssetType.BARRICADE)
                {
                    ItemAssetTag asset = ItemAssetTag.For(targetAssetType.Substring("barricade_".Length));
                    if (asset == null)
                    {
                        entry.Bad("Invalid item barricade type!");
                        return;
                    }
                    BarricadeManager.dropBarricade(new Barricade(asset.Internal.id), null, loc.ToVector3(), 0f, 0f, 0f, CSteamID.Nil.m_SteamID, CSteamID.Nil.m_SteamID);
                    entry.Good("Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "!");
                }
                else
                {
                    entry.Bad("Invalid or unspawnable entity type!");
                }
            }
            catch (Exception ex)
            {
                entry.Bad("Failed to spawn entity: " + ex.ToString());
            }
            // TODO: Maybe add the spawned object as a var to the current commandqueue.
        }
    }
}
