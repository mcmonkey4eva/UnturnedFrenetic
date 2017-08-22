using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace UnturnedFreneticInjector.Injectables
{
    public class ExposeBarricadeManagerMethodsInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // Expose the "manager" method in BarricadeManager for easier use.
            TypeDefinition type = gamedef.GetType("SDG.Unturned.BarricadeManager");
            FieldDefinition field = GetField(type, "manager");
            field.IsPrivate = false;
            field.IsPublic = true;
            //FieldDefinition regions = GetField(type, "regions");
            //regions.IsPrivate = false;
            //regions.IsPublic = true;
        }
    }
}
