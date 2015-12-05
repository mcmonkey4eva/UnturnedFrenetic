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
            Commander.commands.Add(new UnturnedGenericCommand());
        }

        public static void RunCommands(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                Instance.CommandSystem.System.ExecuteCommands(input, null);
            }
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

        public static long cID = 1;
        
        public void Tick(float delta)
        {
            CommandSystem.System.Tick(delta);
        }
        
        public UnturnedFreneticCommands CommandSystem;

        public void EnableForLevel()
        {
            UnityEngine.GameObject game = new UnityEngine.GameObject("UnturnedFreneticGameObject");
            if (game.GetComponent<UnturnedFreneticTicker>() != null)
            {
                return;
            }
            game.AddComponent<UnturnedFreneticTicker>();
            ItemAssetTag.Init();
            AnimalAssetTag.Init();
            VehicleAssetTag.Init();
        }

        public void AutorunScripts()
        {
            string[] scripts = Directory.GetFiles(Environment.CurrentDirectory + "/scripts/", "*.cfg", SearchOption.AllDirectories);
            foreach (string script in scripts)
            {
                try
                {
                    string cmd = File.ReadAllText(script).Replace("\r", "\n").Replace("\0", "\\0");
                    if (cmd.StartsWith("/// AUTORUN\n"))
                    {
                        CommandSystem.System.ExecuteCommands(cmd, null);
                    }
                }
                catch (Exception ex)
                {
                    SysConsole.Output("Handling autorun script '" + script.Replace(Environment.CurrentDirectory, "") + "'", ex);
                }
            }
        }

        public void Setup()
        {
            CommandSystem = new UnturnedFreneticCommands(this, new UnturnedFreneticOutputter() { TheMod = this });
            UnturnedFreneticEvents.RegisterAll(CommandSystem.System);
            Provider.onEnemyConnected += (player) => UnturnedFreneticEvents.OnPlayerConnected.Fire(new PlayerConnectedEventArgs() { Player = new PlayerTag(player) });
            Level.onPostLevelLoaded += (o) => EnableForLevel();
            AutorunScripts();
        }
    }
}
