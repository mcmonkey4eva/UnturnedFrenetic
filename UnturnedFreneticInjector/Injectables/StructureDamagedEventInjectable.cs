using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class StructureDamagedEventInjectable : Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // This injects a call to the mod's static AnimalDamaged method for the AnimalDamagedScriptEvent
            TypeDefinition modtype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodReference eventmethod = gamedef.ImportReference(GetMethod(modtype, "StructureDamaged", 2));
            TypeDefinition resourcetype = gamedef.GetType("SDG.Unturned.Structure");
            MethodDefinition damagemethod = GetMethod(resourcetype, "askDamage", 1);
            MethodBody damagebody = damagemethod.Body;
            InjectInstructions(damagebody, 0, new Instruction[]
            {
                    // Load "this" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0),
                    // Load "amount" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[0]),
                    // Call the StructureDamaged method with the above parameters and return a bool.
                    Instruction.Create(OpCodes.Call, eventmethod),
                    // If the return is true, jump ahead to the original 0th instruction.
                    Instruction.Create(OpCodes.Brfalse, damagebody.Instructions[0]),
                    // Otherwise, return now.
                    Instruction.Create(OpCodes.Ret)
            });
        }
    }
}
