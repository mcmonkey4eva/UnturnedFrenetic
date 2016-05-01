using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class ExposeAnimalManagerMethodsInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // Expose some things in AnimalManager for easier use.
            TypeDefinition type = gamedef.GetType("SDG.Unturned.AnimalManager");
            MakePublic(GetMethod(type, "addAnimal", 4));
            MakePublic(GetField(type, "manager"));
        }
    }
}
