using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class PlayerDamagedEventInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // This injects a call to the mod's static PlayerDamaged method for the PlayerDamagedScriptEvent
            TypeDefinition modtype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodReference eventmethod = gamedef.ImportReference(GetMethod(modtype, "PlayerDamaged", 6));
            TypeDefinition lifetype = gamedef.GetType("SDG.Unturned.PlayerLife");
            MethodDefinition damagemethod = GetMethod(lifetype, "askDamage", 6);
            MethodBody damagebody = damagemethod.Body;
            InjectInstructions(damagebody, 0, new Instruction[]
                {
                    // Load "base.player" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0),
                    Instruction.Create(OpCodes.Call, GetMethod(gamedef.GetType("SDG.Unturned.PlayerCaller"), "get_player", 0)),
                    // Load "amount" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[0]),
                    // Load "newRagdoll" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[1]),
                    // Load "newCause" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[2]),
                    // Load "newLimb" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[3]),
                    // Load "newKiller" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[4]),
                    // Call the PlayerDamaged method with the above parameters and return a bool.
                    Instruction.Create(OpCodes.Call, eventmethod),
                    // If the return is true, jump ahead to the original 0th instruction.
                    Instruction.Create(OpCodes.Brfalse, damagebody.Instructions[0]),
                    // Otherwise,return now.
                    Instruction.Create(OpCodes.Ret)
                });
        }
    }
}
