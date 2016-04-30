using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnturnedFreneticInjector.Injectables
{
    public class PlayerShootEventInjectable : Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // This injects a call to the mod's static PlayerShoot method for the PlayerShootScriptEvent
            TypeDefinition modtype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodReference eventmethod = gamedef.ImportReference(GetMethod(modtype, "PlayerShoot", 2));
            TypeDefinition guntype = gamedef.GetType("SDG.Unturned.UseableGun");
            MethodDefinition firemethod = GetMethod(guntype, "fire", 0);
            MethodBody firebody = firemethod.Body;
            InjectInstructions(firebody, 0, new Instruction[]
                {
                    // Load "base.player" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0),
                    Instruction.Create(OpCodes.Call, GetMethod(gamedef.GetType("SDG.Unturned.PlayerCaller"), "get_player", 0)),
                    // Load "this" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0),
                    // Call the PlayerShoot method with the above parameters and return a bool.
                    Instruction.Create(OpCodes.Call, eventmethod),
                    // If the return is false, jump ahead to the original 0th instruction.
                    Instruction.Create(OpCodes.Brfalse, firebody.Instructions[0]),
                    // Otherwise, return now.
                    Instruction.Create(OpCodes.Ret)
                });
        }
    }
}
