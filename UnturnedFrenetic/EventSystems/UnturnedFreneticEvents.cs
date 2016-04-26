using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript;
using UnturnedFrenetic.EventSystems.PlayerEvents;
using FreneticScript.CommandSystem;

namespace UnturnedFrenetic.EventSystems
{
    public class UnturnedFreneticEvents
    {
        public static void RegisterAll(Commands system)
        {
            system.RegisterEvent(new PlayerChatScriptEvent(system));
            system.RegisterEvent(new PlayerConnectingScriptEvent(system));
            system.RegisterEvent(new PlayerConnectedScriptEvent(system));
            system.RegisterEvent(new PlayerDamagedScriptEvent(system));
            system.RegisterEvent(new PlayerDeathScriptEvent(system));
            system.RegisterEvent(new PlayerDisconnectedScriptEvent(system));
            system.RegisterEvent(new PlayerShootScriptEvent(system));
        }

        public static FreneticScriptEventHandler<PlayerChatEventArgs> OnPlayerChat = new FreneticScriptEventHandler<PlayerChatEventArgs>();

        public static FreneticScriptEventHandler<PlayerConnectingEventArgs> OnPlayerConnecting = new FreneticScriptEventHandler<PlayerConnectingEventArgs>();

        public static FreneticScriptEventHandler<PlayerConnectedEventArgs> OnPlayerConnected = new FreneticScriptEventHandler<PlayerConnectedEventArgs>();

        public static FreneticScriptEventHandler<PlayerDamagedEventArgs> OnPlayerDamaged = new FreneticScriptEventHandler<PlayerDamagedEventArgs>();

        public static FreneticScriptEventHandler<PlayerDeathEventArgs> OnPlayerDeath = new FreneticScriptEventHandler<PlayerDeathEventArgs>();

        public static FreneticScriptEventHandler<PlayerDisconnectedEventArgs> OnPlayerDisconnected = new FreneticScriptEventHandler<PlayerDisconnectedEventArgs>();

        public static FreneticScriptEventHandler<PlayerShootEventArgs> OnPlayerShoot = new FreneticScriptEventHandler<PlayerShootEventArgs>();
    }
}
