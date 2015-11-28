using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace UnturnedFreneticInjector.Injectables
{
    class CommandDebugInjectable : Injectable
    {
        public override void InjectInto(ModuleDefinition mdef)
        {
            TypeDefinition debugCMD = mdef.GetType("SDG.Unturned.CommandDebug");
            foreach (MethodDefinition method in debugCMD.Methods)
            {
                Console.WriteLine("Found Debug method: " + method.Name);
            }
        }
    }
}
