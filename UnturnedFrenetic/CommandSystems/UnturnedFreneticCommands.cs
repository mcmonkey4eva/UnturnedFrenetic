using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic;
using Frenetic.CommandSystem;
using UnturnedFrenetic.CommandSystems.WorldCommands;
using UnturnedFrenetic.TagSystems.TagBases;
using UnturnedFrenetic.CommandSystems.PlayerCommands;

namespace UnturnedFrenetic.CommandSystems
{
    public class UnturnedFreneticCommands
    {
        public Commands System;

        public UnturnedFreneticMod TheMod;

        public UnturnedFreneticCommands(UnturnedFreneticMod mod, Outputter output)
        {
            TheMod = mod;
            System = new Commands();
            System.Output = output;
            System.Init();
            // Player Commands
            System.RegisterCommand(new GiveCommand());
            // World Commands
            System.RegisterCommand(new TimeCommand());
            // Tag Objects
            System.TagSystem.Register(new PlayerTagBase());
        }
    }
}
