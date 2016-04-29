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
            system.RegisterEvent(new BarricadeDestroyedScriptEvent(system));
            system.RegisterEvent(new ResourceDamagedScriptEvent(system));
            system.RegisterEvent(new ResourceDestroyedScriptEvent(system));
            system.RegisterEvent(new StructureDamagedScriptEvent(system));
            system.RegisterEvent(new StructureDestroyedScriptEvent(system));
            system.RegisterEvent(new VehicleDamagedScriptEvent(system));
            system.RegisterEvent(new VehicleDestroyedScriptEvent(system));
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

        public static FreneticScriptEventHandler<BarricadeDestroyedEventArgs> OnBarricadeDestroyed = new FreneticScriptEventHandler<BarricadeDestroyedEventArgs>();

        public static FreneticScriptEventHandler<PlayerChatEventArgs> OnPlayerChat = new FreneticScriptEventHandler<PlayerChatEventArgs>();

        public static FreneticScriptEventHandler<PlayerConnectingEventArgs> OnPlayerConnecting = new FreneticScriptEventHandler<PlayerConnectingEventArgs>();

        public static FreneticScriptEventHandler<PlayerConnectedEventArgs> OnPlayerConnected = new FreneticScriptEventHandler<PlayerConnectedEventArgs>();

        public static FreneticScriptEventHandler<PlayerDamagedEventArgs> OnPlayerDamaged = new FreneticScriptEventHandler<PlayerDamagedEventArgs>();

        public static FreneticScriptEventHandler<PlayerDeathEventArgs> OnPlayerDeath = new FreneticScriptEventHandler<PlayerDeathEventArgs>();

        public static FreneticScriptEventHandler<PlayerDisconnectedEventArgs> OnPlayerDisconnected = new FreneticScriptEventHandler<PlayerDisconnectedEventArgs>();

        public static FreneticScriptEventHandler<PlayerShootEventArgs> OnPlayerShoot = new FreneticScriptEventHandler<PlayerShootEventArgs>();

        public static FreneticScriptEventHandler<ResourceDamagedEventArgs> OnResourceDamaged = new FreneticScriptEventHandler<ResourceDamagedEventArgs>();

        public static FreneticScriptEventHandler<ResourceDestroyedEventArgs> OnResourceDestroyed = new FreneticScriptEventHandler<ResourceDestroyedEventArgs>();

        public static FreneticScriptEventHandler<StructureDamagedEventArgs> OnStructureDamaged = new FreneticScriptEventHandler<StructureDamagedEventArgs>();

        public static FreneticScriptEventHandler<StructureDestroyedEventArgs> OnStructureDestroyed = new FreneticScriptEventHandler<StructureDestroyedEventArgs>();

        public static FreneticScriptEventHandler<VehicleDamagedEventArgs> OnVehicleDamaged = new FreneticScriptEventHandler<VehicleDamagedEventArgs>();

        public static FreneticScriptEventHandler<VehicleDestroyedEventArgs> OnVehicleDestroyed = new FreneticScriptEventHandler<VehicleDestroyedEventArgs>();

        public static FreneticScriptEventHandler<ZombieDamagedEventArgs> OnZombieDamaged = new FreneticScriptEventHandler<ZombieDamagedEventArgs>();

        public static FreneticScriptEventHandler<ZombieDeathEventArgs> OnZombieDeath = new FreneticScriptEventHandler<ZombieDeathEventArgs>();
    }
}
