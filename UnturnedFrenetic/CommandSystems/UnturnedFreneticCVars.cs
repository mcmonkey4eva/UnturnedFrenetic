using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript;
using FreneticScript.CommandSystem;

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
