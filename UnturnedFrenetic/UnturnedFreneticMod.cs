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

namespace UnturnedFrenetic
{
    public class UnturnedFreneticMod
    {
        public static UnturnedFreneticMod Instance;

        public static void Init()
        {
            Console.WriteLine("Unturned Frenetic mod loading...");
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

        public static bool PlayerChat(SteamPlayer steamPlayer, ref byte modeByte, ref EChatMode mode, ref Color color, ref string text)
        {
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

        public static bool PlayerDamaged(Player player, ref byte amount, ref Vector3 ragdoll, ref EDeathCause deathCause, ref ELimb limb, CSteamID killer, object attacker)
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
            if (amount >= player.life.health)
            {
                PlayerDeathEventArgs deathevt = new PlayerDeathEventArgs();
                deathevt.Player = new PlayerTag(player.channel.owner);
                deathevt.Amount = new NumberTag(amount);
                deathevt.Cause = new TextTag(deathCause.ToString());
                deathevt.Limb = new TextTag(limb.ToString());
                deathevt.Killer = attackerTag;
                UnturnedFreneticEvents.OnPlayerDeath.Fire(deathevt);
                amount = (byte)deathevt.Amount.Internal;
                return deathevt.Cancelled;
            }
            PlayerDamagedEventArgs evt = new PlayerDamagedEventArgs();
            evt.Player = new PlayerTag(player.channel.owner);
            evt.Amount = new NumberTag(amount);
            evt.Attacker = attackerTag;
            UnturnedFreneticEvents.OnPlayerDamaged.Fire(evt);
            amount = (byte)evt.Amount.Internal;
            return evt.Cancelled;
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
            if (amount >= animal.health)
            {
                AnimalDeathEventArgs deathevt = new AnimalDeathEventArgs();
                deathevt.Animal = new AnimalTag(animal);
                deathevt.Amount = new NumberTag(amount);
                UnturnedFreneticEvents.OnAnimalDeath.Fire(deathevt);
                amount = (byte)deathevt.Amount.Internal;
                return deathevt.Cancelled;
            }
            AnimalDamagedEventArgs evt = new AnimalDamagedEventArgs();
            evt.Animal = new AnimalTag(animal);
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnAnimalDamaged.Fire(evt);
            amount = (byte)evt.Amount.Internal;
            return evt.Cancelled;
        }

        public static bool ZombieDamaged(Zombie zombie, ref byte amount, ref Vector3 ragdoll)
        {
            // TODO: causes?
            if (amount >= zombie.health)
            {
                ZombieDeathEventArgs deathevt = new ZombieDeathEventArgs();
                deathevt.Zombie = new ZombieTag(zombie);
                deathevt.Amount = new NumberTag(amount);
                UnturnedFreneticEvents.OnZombieDeath.Fire(deathevt);
                amount = (byte)deathevt.Amount.Internal;
                return deathevt.Cancelled;
            }
            ZombieDamagedEventArgs evt = new ZombieDamagedEventArgs();
            evt.Zombie = new ZombieTag(zombie);
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnZombieDamaged.Fire(evt);
            amount = (byte)evt.Amount.Internal;
            return evt.Cancelled;
        }

        public static bool ResourceDamaged(ResourceSpawnpoint resource, ref ushort amount)
        {
            // TODO: causes?
            if (amount >= resource.health)
            {
                ResourceDeathEventArgs deathevt = new ResourceDeathEventArgs();
                deathevt.Resource = new ResourceTag(resource);
                deathevt.Amount = new NumberTag(amount);
                UnturnedFreneticEvents.OnResourceDeath.Fire(deathevt);
                amount = (ushort)deathevt.Amount.Internal;
                return deathevt.Cancelled;
            }
            ResourceDamagedEventArgs evt = new ResourceDamagedEventArgs();
            evt.Resource = new ResourceTag(resource);
            evt.Amount = new NumberTag(amount);
            UnturnedFreneticEvents.OnResourceDamaged.Fire(evt);
            amount = (ushort)evt.Amount.Internal;
            return evt.Cancelled;
        }

        public static long cID = 1;
        
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
            string[] files = Directory.GetFiles(Environment.CurrentDirectory + "/scripts/", "*.cfg", SearchOption.AllDirectories);
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
