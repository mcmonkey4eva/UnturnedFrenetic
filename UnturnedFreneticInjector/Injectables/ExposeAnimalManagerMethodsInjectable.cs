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
            // Expose the "addAnimal" method in AnimalManager for easier use.
            TypeDefinition type = gamedef.GetType("SDG.Unturned.AnimalManager");
            MethodDefinition method = GetMethod(type, "addAnimal", 4);
            method.IsPrivate = false;
            method.IsPublic = true;
            FieldDefinition field = GetField(type, "manager");
            field.IsPrivate = false;
            field.IsPublic = true;
        }
    }
}
