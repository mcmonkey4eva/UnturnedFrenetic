using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic;
using Frenetic.CommandSystem;

namespace UnturnedFrenetic.CommandSystems
{
    public class UnturnedFreneticCVars
    {
        public CVarSystem System;

        public UnturnedFreneticCVars(UnturnedFreneticOutputter output)
        {
            System = new CVarSystem(output);
        }
    }
}
