using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class ExposeZombieManagerMethodsInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // Expose the "addZombie" method in ZombieManager for easier use.
            TypeDefinition type = gamedef.GetType("SDG.Unturned.ZombieManager");
            MethodDefinition method = GetMethod(type, "addZombie", 12);
            method.IsPrivate = false;
            method.IsPublic = true;
            FieldDefinition field = GetField(type, "manager");
            field.IsPrivate = false;
            field.IsPublic = true;
            TypeDefinition zombie = gamedef.GetType("SDG.Unturned.Zombie");
            FieldDefinition health = GetField(zombie, "health");
            health.IsPrivate = false;
            health.IsPublic = true;
            FieldDefinition maxHealth = GetField(zombie, "maxHealth");
            maxHealth.IsPrivate = false;
            maxHealth.IsPublic = true;
        }
    }
}
