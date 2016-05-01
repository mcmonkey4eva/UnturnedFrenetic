using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class ExposeObjectManagerMethodsInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // Expose the "manager" method in ObjectManager for easier use.
            TypeDefinition type = gamedef.GetType("SDG.Unturned.ObjectManager");
            MakePublic(GetField(type, "manager"));
        }
    }
}
