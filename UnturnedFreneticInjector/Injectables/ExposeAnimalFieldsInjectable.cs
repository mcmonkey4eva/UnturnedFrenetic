using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace UnturnedFreneticInjector.Injectables
{
    public class ExposeAnimalFieldsInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // Expose the "health" field in Animal for easier use.
            TypeDefinition type = gamedef.GetType("SDG.Unturned.Animal");
            FieldDefinition field = GetField(type, "health");
            field.IsPrivate = false;
            field.IsPublic = true;
        }
    }
}
