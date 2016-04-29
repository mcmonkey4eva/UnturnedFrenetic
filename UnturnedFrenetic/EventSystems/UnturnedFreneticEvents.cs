using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript;
using UnturnedFrenetic.EventSystems.PlayerEvents;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.EventSystems.EntityEvents;

namespace UnturnedFrenetic.EventSystems
{
    public class UnturnedFreneticEvents
    {
        public static void RegisterAll(Commands system)
        {
            // Entity Events
            system.RegisterEvent(new AnimalDamagedScriptEvent(system));
            system.RegisterEvent(new AnimalDeathScriptEvent(system));
            system.RegisterEvent(new BarricadeDamagedScriptEvent(system));
            system.RegisterEvent(new BarricadeDeathScriptEvent(system));
            system.RegisterEvent(new ResourceDamagedScriptEvent(system));
            system.RegisterEvent(new ResourceDeathScriptEvent(system));
            system.RegisterEvent(new StructureDamagedScriptEvent(system));
            system.RegisterEvent(new StructureDeathScriptEvent(system));
            system.RegisterEvent(new ZombieDamagedScriptEvent(system));
            system.RegisterEvent(new ZombieDeathScriptEvent(system));

            // Player Events
            system.RegisterEvent(new PlayerChatScriptEvent(system));
            system.RegisterEvent(new PlayerConnectingScriptEvent(system));
            system.RegisterEvent(new PlayerConnectedScriptEvent(system));
            system.RegisterEvent(new PlayerDamagedScriptEvent(system));
            system.RegisterEvent(new PlayerDeathScriptEvent(system));
            system.RegisterEvent(new PlayerDisconnectedScriptEvent(system));
            system.RegisterEvent(new PlayerShootScriptEvent(system));
        }

        public static FreneticScriptEventHandler<AnimalDamagedEventArgs> OnAnimalDamaged = new FreneticScriptEventHandler<AnimalDamagedEventArgs>();

        public static FreneticScriptEventHandler<AnimalDeathEventArgs> OnAnimalDeath = new FreneticScriptEventHandler<AnimalDeathEventArgs>();

        public static FreneticScriptEventHandler<BarricadeDamagedEventArgs> OnBarricadeDamaged = new FreneticScriptEventHandler<BarricadeDamagedEventArgs>();

        public static FreneticScriptEventHandler<BarricadeDeathEventArgs> OnBarricadeDeath = new FreneticScriptEventHandler<BarricadeDeathEventArgs>();

        public static FreneticScriptEventHandler<PlayerChatEventArgs> OnPlayerChat = new FreneticScriptEventHandler<PlayerChatEventArgs>();

        public static FreneticScriptEventHandler<PlayerConnectingEventArgs> OnPlayerConnecting = new FreneticScriptEventHandler<PlayerConnectingEventArgs>();

        public static FreneticScriptEventHandler<PlayerConnectedEventArgs> OnPlayerConnected = new FreneticScriptEventHandler<PlayerConnectedEventArgs>();

        public static FreneticScriptEventHandler<PlayerDamagedEventArgs> OnPlayerDamaged = new FreneticScriptEventHandler<PlayerDamagedEventArgs>();

        public static FreneticScriptEventHandler<PlayerDeathEventArgs> OnPlayerDeath = new FreneticScriptEventHandler<PlayerDeathEventArgs>();

        public static FreneticScriptEventHandler<PlayerDisconnectedEventArgs> OnPlayerDisconnected = new FreneticScriptEventHandler<PlayerDisconnectedEventArgs>();

        public static FreneticScriptEventHandler<PlayerShootEventArgs> OnPlayerShoot = new FreneticScriptEventHandler<PlayerShootEventArgs>();

        public static FreneticScriptEventHandler<ResourceDamagedEventArgs> OnResourceDamaged = new FreneticScriptEventHandler<ResourceDamagedEventArgs>();

        public static FreneticScriptEventHandler<ResourceDeathEventArgs> OnResourceDeath = new FreneticScriptEventHandler<ResourceDeathEventArgs>();

        public static FreneticScriptEventHandler<StructureDamagedEventArgs> OnStructureDamaged = new FreneticScriptEventHandler<StructureDamagedEventArgs>();

        public static FreneticScriptEventHandler<StructureDeathEventArgs> OnStructureDeath = new FreneticScriptEventHandler<StructureDeathEventArgs>();

        public static FreneticScriptEventHandler<ZombieDamagedEventArgs> OnZombieDamaged = new FreneticScriptEventHandler<ZombieDamagedEventArgs>();

        public static FreneticScriptEventHandler<ZombieDeathEventArgs> OnZombieDeath = new FreneticScriptEventHandler<ZombieDeathEventArgs>();
    }
}
