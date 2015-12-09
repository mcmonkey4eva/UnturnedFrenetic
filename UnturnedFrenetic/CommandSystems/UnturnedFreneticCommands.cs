using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic;
using Frenetic.CommandSystem;
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

        public UnturnedFreneticCommands(UnturnedFreneticMod mod, Outputter output)
        {
            try
            {
                TheMod = mod;
                System = new Commands();
                System.Output = output;
                System.Init();
                // Entity Commands
                System.RegisterCommand(new SpawnCommand());
                // Player Commands
                System.RegisterCommand(new GiveCommand());
                System.RegisterCommand(new TakeCommand());
                // World Commands
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
                System.TagSystem.Register(new PlayerTagBase());
                System.TagSystem.Register(new ResourceAssetTagBase());
                System.TagSystem.Register(new ResourceTagBase());
                System.TagSystem.Register(new StructureTagBase());
                System.TagSystem.Register(new VehicleAssetTagBase());
                System.TagSystem.Register(new VehicleTagBase());
                System.TagSystem.Register(new WorldObjectAssetTagBase());
                System.TagSystem.Register(new WorldObjectTagBase());
                System.TagSystem.Register(new ZombieTagBase());
            }
            catch (Exception ex)
            {
                SysConsole.Output(OutputType.ERROR, "Error registering commands: " + ex.ToString());
            }
        }
    }
}
