using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class ExposeItemManagerMethodsInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // Expose the "spawnItem" method in ItemManager for easier use.
            TypeDefinition type = gamedef.GetType("SDG.Unturned.ItemManager");
            MethodDefinition method = GetMethod(type, "spawnItem", 7);
            method.IsPrivate = false;
            method.IsPublic = true;
            FieldDefinition field = GetField(type, "manager");
            field.IsPrivate = false;
            field.IsPublic = true;
        }
    }
}
