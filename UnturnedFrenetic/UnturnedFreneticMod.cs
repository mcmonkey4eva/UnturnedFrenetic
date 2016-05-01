using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnturnedFrenetic.CommandSystems;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using UnturnedFrenetic.EventSystems;
using UnturnedFrenetic.EventSystems.PlayerEvents;
using UnityEngine;
using FreneticScript.TagHandlers.Objects;
using FreneticScript.CommandSystem;
using Steamworks;
using FreneticScript.TagHandlers;
using UnturnedFrenetic.EventSystems.EntityEvents;
using System.Threading;
using System.Globalization;
using UnturnedFrenetic.CommandSystems.EntityCommands;
using UnturnedFrenetic.UnturnedCommands;

namespace UnturnedFrenetic
{
    public class UnturnedFreneticMod
    {
        public static UnturnedFreneticMod Instance;

        public static void Init()
        {
            Console.WriteLine("Unturned Frenetic mod loading...");
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            SysConsole.Init();
            Instance = new UnturnedFreneticMod();
            Instance.Setup();
            SysConsole.Output(OutputType.INIT, "Unturned Frenetic mod loaded! Releasing back to main game...");
        }

        public static void InitSecondary()
        {
            Commander.commands.Insert(0, new UnturnedPreCommand());
            Commander.commands.Add(new UnturnedReloadCommand());
            Commander.commands.Add(new UnturnedGenericCommand());
        }

        public static void RunCommands(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                Instance.CommandSystem.System.ExecuteCommands(input, null);
            }
        }

        public List<Command> PlayerCommands = new List<Command>();

        public static bool PlayerChat(SteamPlayer steamPlayer, ref byte modeByte, ref EChatMode mode, ref Color color, ref string text)
        {
            if (text.StartsWith("@") && steamPlayer.isAdmin)
            {
                // Send the admin command through.
                return false;
            }
            if (text.StartsWith("/"))
            {
                SysConsole.Output(OutputType.CLIENTINFO, "Player [" + steamPlayer.player.name + "] executing player command: " + text);
                string cmd = text.Substring(1);
                int splitPos = cmd.IndexOf(' ');
                string args = "";
                if (splitPos > 0)
                {
                    args = cmd.Substring(splitPos + 1);
                    cmd = cmd.Substring(0, splitPos);
                }
                for (int i = 0; i < Instance.PlayerCommands.Count; i++)
                {
                    if (Instance.PlayerCommands[i].check(steamPlayer.playerID.steamID, cmd, args))
                    {
                        return true;
                    }
                }
                // TODO: if (CVars.g_showinvalidplayercommand.ValueB)
                ChatManager.manager.channel.send("tellChat", steamPlayer.playerID.steamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, steamPlayer.playerID.steamID, (byte)0, Color.red, "Unknown command!");
                // Processed a local command.
                return true;
            }
            color = Color.white;
            if (steamPlayer.isAdmin)
            {
                color = Palette.ADMIN;
            }
            else if (steamPlayer.isPro)
            {
                color = Palette.PRO;
            }
            PlayerChatEventArgs evt = new PlayerChatEventArgs();
            evt.Player = new PlayerTag(steamPlayer);
            evt.ChatMode = new TextTag(mode.ToString());
            evt.Text = new TextTag(text);
            evt.Color = new ColorTag(color);
            UnturnedFreneticEvents.OnPlayerChat.Fire(evt);
            mode = (EChatMode)Enum.Parse(typeof(EChatMode), evt.ChatMode.ToString().ToUpper());
            modeByte = (byte)mode;
            text = evt.Text.ToString();
            color = evt.Color.Internal;
            return evt.Cancelled;
        }

        public static bool PlayerConnecting(SteamPending pending)
        {
            PlayerConnectingEventArgs evt = new PlayerConnectingEventArgs();
            evt.PlayerName = pending.playerID.playerName;
            UnturnedFreneticEvents.OnPlayerConnecting.Fire(evt);
            if (evt.Cancelled)
            {
                Provider.reject(pending.playerID.steamID, ESteamRejection.WHITELISTED); // TODO: Customizable rejection reason!
            }
            return evt.Cancelled;
        }

        public static bool PlayerHealed(Player player, ref byte amount, ref bool healBleeding, ref bool healBroken)
        {
            // TODO: Event!
            UFMHealthController healthController = player.gameObject.GetComponent<UFMHealthController>();
            if (healthController != null)
            {
                healthController.Heal(amount);
            }
            return false;
        }

        public static bool PlayerDamaged(Player player, ref byte amount, ref Vector3 ragdoll, ref EDeathCause deathCause, ref ELimb limb, CSteamID killer, object attacker)
        {
            uint uintAmount = amount;
            bool cancelled = PlayerDamaged(player, ref uintAmount, ref ragdoll, ref deathCause, ref limb, killer, attacker, false);
            amount = (byte)uintAmount;
            return cancelled;
        }

        public static bool PlayerDamaged(Player player, ref uint amount, ref Vector3 ragdoll, ref EDeathCause deathCause, ref ELimb limb, CSteamID killer, object attacker, bool fromUFM)
        {
            TemplateObject attackerTag = null;
            if (killer != null && killer != CSteamID.Nil && killer != Provider.server)
            {
                SteamPlayer steamkiller = PlayerTool.getSteamPlayer(killer);
                if (steamkiller != null)
                {
                    attackerTag = new PlayerTag(steamkiller);
                }
            }
            else if (attacker != null)
            {
                if (attacker is Animal)
                {
                    attackerTag = new AnimalTag((Animal)attacker);
                }
                else if (attacker is Zombie)
                {
                    attackerTag = new ZombieTag((Zombie)attacker);
                }
            }
            PlayerTag playerTag = new PlayerTag(player.channel.owner);
            UFMHealthController healthController = player.gameObject.GetComponent<UFMHealthController>();
            uint health = healthController != null ? healthController.health : player.life.health;
            if (amount >= health)
            {
                PlayerDeathEventArgs deathevt = new PlayerDeathEventArgs();
                deathevt.Player = playerTag;
                deathevt.Amount = new NumberTag(amount);
                deathevt.Cause = new TextTag(deathCause.ToString());
                deathevt.Limb = new TextTag(limb.ToString());
                deathevt.Killer = attackerTag;
                UnturnedFreneticEvents.OnPlayerDeath.Fire(deathevt);
                amount = (uint)deathevt.Amount.Internal;
                if (!deathevt.Cancelled && !EntityDeath(playerTag, ref amount) && healthController != null)
                {
                    healthController.Damage(amount);
                    player.life.ragdoll = ragdoll;
                    amount = (uint)(((double)amount / healthController.maxHealth) * 100.0);
                    return false;
                }
                return true;
            }
            PlayerDamagedEventArgs evt = new PlayerDamagedEventArgs();
            evt.Player = playerTag;
            evt.Amount = new NumberTag(amount);
            evt.Attacker = attackerTag;
            UnturnedFreneticEvents.OnPlayerDamaged.Fire(evt);
            amount = (uint)evt.Amount.Internal;
            if (!evt.Cancelled && !EntityDamaged(playerTag, ref amount) && healthController != null)
            {
                healthController.Damage(amount);
                player.life.ragdoll = ragdoll;
                amount = (uint)(((double)amount / healthController.maxHealth) * 100.0);
                return false;
            }
            return true;
        }

        public static bool PlayerShoot(Player player, UseableGun gun)
        {
            PlayerShootEventArgs evt = new PlayerShootEventArgs();
            evt.Player = new PlayerTag(player.channel.owner);
            // TODO: make GunTag/WeaponTag/EquipmentTag/WhateverTag to store this more accurately?
            evt.Gun = new ItemAssetTag(player.equipment.asset);
            UnturnedFreneticEvents.OnPlayerShoot.Fire(evt);
            return evt.Cancelled;
        }

        public static bool AnimalDamaged(Animal animal, ref byte amount, ref Vector3 ragdoll)
        {
            // TODO: causes?
            AnimalTag animalTag = new AnimalTag(animal);
            if (amount >= animal.health)
            {
                AnimalDeathEventArgs deathevt = new AnimalDeathEventArgs();
                deathevt.Animal = animalTag;
                deathevt.Amount = new NumberTag(amount);
                UnturnedFreneticEvents.OnAnimalDeath.Fire(deathevt);
                amount = (byte)deathevt.Amount.Internal;
                return deathevt.Cancelled || EntityDeath(animalTag, ref amount);
            }
            AnimalDamagedEventArgs evt = new AnimalDamagedEventArgs();
            evt.Animal = animalTag;
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnAnimalDamaged.Fire(evt);
            amount = (byte)evt.Amount.Internal;
            return evt.Cancelled || EntityDamaged(animalTag, ref amount);
        }

        public static bool ZombieDamaged(Zombie zombie, ref byte amount, ref Vector3 ragdoll)
        {
            // TODO: causes?
            ZombieTag zombieTag = new ZombieTag(zombie);
            if (amount >= zombie.health)
            {
                ZombieDeathEventArgs deathevt = new ZombieDeathEventArgs();
                deathevt.Zombie = zombieTag;
                deathevt.Amount = new NumberTag(amount);
                UnturnedFreneticEvents.OnZombieDeath.Fire(deathevt);
                amount = (byte)deathevt.Amount.Internal;
                return deathevt.Cancelled || EntityDeath(zombieTag, ref amount);
            }
            ZombieDamagedEventArgs evt = new ZombieDamagedEventArgs();
            evt.Zombie = zombieTag;
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnZombieDamaged.Fire(evt);
            amount = (byte)evt.Amount.Internal;
            return evt.Cancelled || EntityDamaged(zombieTag, ref amount);
        }

        public static bool ResourceDamaged(ResourceSpawnpoint resource, ref ushort amount)
        {
            // TODO: causes?
            ResourceTag resourceTag = new ResourceTag(resource);
            if (amount >= resource.health)
            {
                ResourceDestroyedEventArgs deathevt = new ResourceDestroyedEventArgs();
                deathevt.Resource = resourceTag;
                deathevt.Amount = new NumberTag(amount);
                UnturnedFreneticEvents.OnResourceDestroyed.Fire(deathevt);
                amount = (ushort)deathevt.Amount.Internal;
                return deathevt.Cancelled || EntityDestroyed(resourceTag, ref amount);
            }
            ResourceDamagedEventArgs evt = new ResourceDamagedEventArgs();
            evt.Resource = resourceTag;
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnResourceDamaged.Fire(evt);
            amount = (ushort)evt.Amount.Internal;
            return evt.Cancelled || EntityDamaged(resourceTag, ref amount);
        }

        public static bool BarricadeDamaged(Barricade barricade, ref ushort amount)
        {
            // TODO: causes?
            BarricadeTag barricadeTag = new BarricadeTag(barricade);
            if (amount >= barricade.health)
            {
                BarricadeDestroyedEventArgs deathevt = new BarricadeDestroyedEventArgs();
                deathevt.Barricade = barricadeTag;
                deathevt.Amount = new NumberTag(amount);
                UnturnedFreneticEvents.OnBarricadeDestroyed.Fire(deathevt);
                amount = (ushort)deathevt.Amount.Internal;
                return deathevt.Cancelled || EntityDestroyed(barricadeTag, ref amount);
            }
            BarricadeDamagedEventArgs evt = new BarricadeDamagedEventArgs();
            evt.Barricade = barricadeTag;
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnBarricadeDamaged.Fire(evt);
            amount = (ushort)evt.Amount.Internal;
            return evt.Cancelled || EntityDamaged(barricadeTag, ref amount);
        }

        public static bool StructureDamaged(Structure structure, ref ushort amount)
        {
            // TODO: causes?
            StructureTag structureTag = new StructureTag(structure);
            if (amount >= structure.health)
            {
                StructureDestroyedEventArgs deathevt = new StructureDestroyedEventArgs();
                deathevt.Structure = structureTag;
                deathevt.Amount = new NumberTag(amount);
                UnturnedFreneticEvents.OnStructureDestroyed.Fire(deathevt);
                amount = (ushort)deathevt.Amount.Internal;
                return deathevt.Cancelled || EntityDestroyed(structureTag, ref amount);
            }
            StructureDamagedEventArgs evt = new StructureDamagedEventArgs();
            evt.Structure = structureTag;
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnStructureDamaged.Fire(evt);
            amount = (ushort)evt.Amount.Internal;
            return evt.Cancelled || EntityDamaged(structureTag, ref amount);
        }

        public static bool VehicleDamaged(InteractableVehicle vehicle, ref ushort amount, ref bool repairable)
        {
            // TODO: causes?
            VehicleTag vehicleTag = new VehicleTag(vehicle);
            if (!repairable && (vehicle.isDead || amount >= vehicle.health))
            {
                VehicleDestroyedEventArgs explodeevt = new VehicleDestroyedEventArgs();
                explodeevt.Vehicle = vehicleTag;
                explodeevt.Amount = new NumberTag(amount);
                explodeevt.Repairable = new BooleanTag(repairable);
                UnturnedFreneticEvents.OnVehicleDestroyed.Fire(explodeevt);
                amount = (ushort)explodeevt.Amount.Internal;
                repairable = explodeevt.Repairable.Internal;
                return explodeevt.Cancelled || EntityDestroyed(vehicleTag, ref amount);
            }
            VehicleDamagedEventArgs evt = new VehicleDamagedEventArgs();
            evt.Vehicle = vehicleTag;
            evt.Amount = new NumberTag(amount);
            evt.Repairable = new BooleanTag(repairable);
            UnturnedFreneticEvents.OnVehicleDamaged.Fire(evt);
            amount = (ushort)evt.Amount.Internal;
            return evt.Cancelled || EntityDamaged(vehicleTag, ref amount);
        }

        public static bool EntityDamaged(TemplateObject entity, ref byte amount)
        {
            // TODO: causes?
            EntityDamagedEventArgs evt = new EntityDamagedEventArgs();
            evt.Entity = entity;
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnEntityDamaged.Fire(evt);
            amount = (byte)evt.Amount.Internal;
            return evt.Cancelled;
        }

        public static bool EntityDamaged(TemplateObject entity, ref ushort amount)
        {
            // TODO: causes?
            EntityDamagedEventArgs evt = new EntityDamagedEventArgs();
            evt.Entity = entity;
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnEntityDamaged.Fire(evt);
            amount = (ushort)evt.Amount.Internal;
            return evt.Cancelled;
        }

        public static bool EntityDamaged(TemplateObject entity, ref uint amount)
        {
            // TODO: causes?
            EntityDamagedEventArgs evt = new EntityDamagedEventArgs();
            evt.Entity = entity;
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnEntityDamaged.Fire(evt);
            amount = (uint)evt.Amount.Internal;
            return evt.Cancelled;
        }

        public static bool EntityDeath(TemplateObject entity, ref uint amount)
        {
            // TODO: causes?
            EntityDeathEventArgs evt = new EntityDeathEventArgs();
            evt.Entity = entity;
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnEntityDeath.Fire(evt);
            amount = (uint)evt.Amount.Internal;
            return evt.Cancelled;
        }

        public static bool EntityDeath(TemplateObject entity, ref byte amount)
        {
            // TODO: causes?
            EntityDeathEventArgs evt = new EntityDeathEventArgs();
            evt.Entity = entity;
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnEntityDeath.Fire(evt);
            amount = (byte)evt.Amount.Internal;
            return evt.Cancelled;
        }

        public static bool EntityDestroyed(TemplateObject entity, ref ushort amount)
        {
            // TODO: causes?
            EntityDestroyedEventArgs evt = new EntityDestroyedEventArgs();
            evt.Entity = entity;
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnEntityDestroyed.Fire(evt);
            amount = (ushort)evt.Amount.Internal;
            return evt.Cancelled;
        }

        public static long cID = 1; // TODO: What is this?
        
        public void Tick(float delta)
        {
            CommandSystem.System.Tick(delta);
        }
        
        public UnturnedFreneticCommands CommandSystem;

        public void EnableForLevel()
        {
            GameObject game = new GameObject("UnturnedFreneticGameObject");
            if (game.GetComponent<UnturnedFreneticTicker>() != null)
            {
                return;
            }
            game.AddComponent<UnturnedFreneticTicker>();
            ItemAssetTag.Init();
            AnimalAssetTag.Init();
            VehicleAssetTag.Init();
            WorldObjectAssetTag.Init();
            ResourceAssetTag.Init();
            BarricadeTag.Init();
            EffectAssetTag.Init();
        }

        public void AutorunScripts()
        {
            string[] files = Directory.GetFiles(Environment.CurrentDirectory + "/frenetic/scripts/", "*.cfg", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                string cmd = File.ReadAllText(file).Replace("\r", "").Replace("\0", "\\0");
                CommandSystem.System.PrecalcScript(file.Replace(Environment.CurrentDirectory, ""), cmd);
            }
            CommandSystem.System.RunPrecalculated();
        }

        public void Setup()
        {
            CommandSystem = new UnturnedFreneticCommands(this, new UnturnedFreneticOutputter() { TheMod = this });
            Provider.onEnemyConnected += (player) => UnturnedFreneticEvents.OnPlayerConnected.Fire(new PlayerConnectedEventArgs() { Player = new PlayerTag(player) });
            Provider.onEnemyDisconnected += (player) => UnturnedFreneticEvents.OnPlayerDisconnected.Fire(new PlayerDisconnectedEventArgs() { Player = new PlayerTag(player) });
            Level.onPostLevelLoaded += (o) => EnableForLevel();
            AutorunScripts();
        }
    }
}
