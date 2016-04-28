using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class ZombieDamagedEventInjectable : Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // This injects a call to the mod's static ZombieDamaged method for the ZombieDamagedScriptEvent
            TypeDefinition modtype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodReference eventmethod = gamedef.ImportReference(GetMethod(modtype, "ZombieDamaged", 3));
            TypeDefinition zombietype = gamedef.GetType("SDG.Unturned.Zombie");
            MethodDefinition damagemethod = GetMethod(zombietype, "askDamage", 3);
            MethodBody damagebody = damagemethod.Body;
            InjectInstructions(damagebody, 0, new Instruction[]
            {
                    // Load "this" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0),
                    // Load "amount" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[0]),
                    // Load "newRagdoll" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[1]),
                    // Call the ZombieDamaged method with the above parameters and return a bool.
                    Instruction.Create(OpCodes.Call, eventmethod),
                    // If the return is true, jump ahead to the original 0th instruction.
                    Instruction.Create(OpCodes.Brfalse, damagebody.Instructions[0]),
                    // Otherwise, return now.
                    Instruction.Create(OpCodes.Ret)
            });
        }
    }
}
