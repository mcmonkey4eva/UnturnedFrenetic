using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.EventSystems;
using UnturnedFrenetic.CommandSystems.EntityCommands;
using UnturnedFrenetic.CommandSystems.PlayerCommands;
using UnturnedFrenetic.CommandSystems.WorldCommands;
using UnturnedFrenetic.TagSystems.TagBases;

namespace UnturnedFrenetic.CommandSystems
{
    public class UnturnedFreneticCommands
    {
        public Commands System;

        public UnturnedFreneticMod TheMod;

        public UnturnedFreneticCommands(UnturnedFreneticMod mod, UnturnedFreneticOutputter output)
        {
            try
            {
                TheMod = mod;
                System = new Commands();
                output.Syst = System;
                System.Output = output;
                System.Init();
                // Entity Commands
                System.RegisterCommand(new AICommand());
                System.RegisterCommand(new AnimateCommand());
                System.RegisterCommand(new DamageCommand());
                System.RegisterCommand(new HealCommand());
                System.RegisterCommand(new KillCommand());
                System.RegisterCommand(new LaunchCommand());
                System.RegisterCommand(new MaxHealthCommand());
                System.RegisterCommand(new RemoveCommand());
                System.RegisterCommand(new SpawnCommand());
                System.RegisterCommand(new TeleportCommand());
                System.RegisterCommand(new WalkCommand());
                // Player Commands
                System.RegisterCommand(new BleedingCommand());
                System.RegisterCommand(new BrokenCommand());
                System.RegisterCommand(new FoodCommand());
                System.RegisterCommand(new GiveCommand());
                System.RegisterCommand(new OxygenCommand());
                System.RegisterCommand(new StaminaCommand());
                System.RegisterCommand(new TakeCommand());
                System.RegisterCommand(new TellCommand());
                System.RegisterCommand(new UseItemCommand());
                System.RegisterCommand(new VirusCommand());
                System.RegisterCommand(new WarmthCommand());
                System.RegisterCommand(new WaterCommand());
                // World Commands
                System.RegisterCommand(new AnnounceCommand());
                System.RegisterCommand(new EffectCommand());
                System.RegisterCommand(new TimeCommand());
                // Tag Objects
                System.TagSystem.Register(new AnimalAssetTagBase());
                System.TagSystem.Register(new AnimalTagBase());
                System.TagSystem.Register(new BarricadeTagBase());
                System.TagSystem.Register(new ColorTagBase());
                System.TagSystem.Register(new EffectAssetTagBase());
                System.TagSystem.Register(new EntityTagBase());
                System.TagSystem.Register(new ItemAssetTagBase());
                System.TagSystem.Register(new ItemTagBase());
                System.TagSystem.Register(new LocationTagBase());
                System.TagSystem.Register(new OfflinePlayerTagBase());
                System.TagSystem.Register(new PlayerTagBase());
                System.TagSystem.Register(new ResourceAssetTagBase());
                System.TagSystem.Register(new ResourceTagBase());
                System.TagSystem.Register(new ServerTagBase());
                System.TagSystem.Register(new StructureTagBase());
                System.TagSystem.Register(new VehicleAssetTagBase());
                System.TagSystem.Register(new VehicleTagBase());
                System.TagSystem.Register(new WorldObjectAssetTagBase());
                System.TagSystem.Register(new WorldObjectTagBase());
                System.TagSystem.Register(new ZombieTagBase());
                // Events
                UnturnedFreneticEvents.RegisterAll(System);
                // Wrap up
                System.PostInit();
            }
            catch (Exception ex)
            {
                SysConsole.Output(OutputType.ERROR, "Error registering commands: " + ex.ToString());
            }
        }
    }
}
