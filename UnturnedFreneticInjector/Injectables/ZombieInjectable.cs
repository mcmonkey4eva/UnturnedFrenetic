using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class ZombieInjectable : Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // This injects a call to the mod's static ZombieDamaged method for the ZombieDamagedScriptEvent. Also exposes some zombie variables. Also, disable its AI optionally.
            TypeDefinition zombietype = gamedef.GetType("SDG.Unturned.Zombie");
            FieldDefinition field = GetField(zombietype, "target");
            field.IsPrivate = false;
            field.IsPublic = true;
            FieldDefinition fieldistick = GetField(zombietype, "isTicking");
            fieldistick.IsPrivate = false;
            fieldistick.IsPublic = true;
            FieldDefinition fieldseeker = GetField(zombietype, "seeker");
            fieldseeker.IsPrivate = false;
            fieldseeker.IsPublic = true;
            FieldDefinition fieldpath = GetField(zombietype, "path");
            fieldpath.IsPrivate = false;
            fieldpath.IsPublic = true;
            FieldDefinition aidisabledfield = new FieldDefinition("UFM_AIDisabled", FieldAttributes.Public, gamedef.TypeSystem.Boolean);
            zombietype.Fields.Add(aidisabledfield);
            foreach (MethodDefinition tmethod in zombietype.Methods)
            {
                switch (tmethod.Name)
                {
                    case "alert":
                        DisableAI(tmethod.Body, aidisabledfield);
                        break;
                    default:
                        break;
                }
            }
            // TODO: Disable things in the update methods too.
            TypeDefinition modtype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodReference eventmethod = gamedef.ImportReference(GetMethod(modtype, "ZombieDamaged", 4));
            MethodDefinition damagemethod = GetMethod(zombietype, "askDamage", 6);
            MethodBody damagebody = damagemethod.Body;
            InjectInstructions(damagebody, 0, new Instruction[]
            {
                    // Load "this" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0),
                    // Load "amount" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[0]),
                    // Load "newRagdoll" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[1]),
                    // Load "xp" onto the stack
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[3]),
                    // Call the ZombieDamaged method with the above parameters and return a bool.
                    Instruction.Create(OpCodes.Call, eventmethod),
                    // If the return is false, jump ahead to the original 0th instruction.
                    Instruction.Create(OpCodes.Brfalse, damagebody.Instructions[0]),
                    // Otherwise, return now.
                    Instruction.Create(OpCodes.Ret)
            });
        }

        void DisableAI(MethodBody body, FieldDefinition aidisabledfield)
        {
            InjectInstructions(body, 0, new Instruction[]
            {
                // Load 'this' to the stack.
                Instruction.Create(OpCodes.Ldarg_0),
                // Load the 'UFM_AIDisabled' field to the stack.
                Instruction.Create(OpCodes.Ldfld, aidisabledfield),
                // If it is false, jump straight to the original first instruction.
                Instruction.Create(OpCodes.Brfalse, body.Instructions[0]),
                // Otherwise, return.
                Instruction.Create(OpCodes.Ret)
            });
        }
    }
}
