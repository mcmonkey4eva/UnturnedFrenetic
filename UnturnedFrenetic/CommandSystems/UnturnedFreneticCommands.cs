using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic;
using Frenetic.CommandSystem;
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
            TheMod = mod;
            System = new Commands();
            System.Output = output;
            System.Init();
            System.RegisterCommand(new TimeCommand());
            System.TagSystem.Register(new PlayerTagBase());
        }
    }
}
