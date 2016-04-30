using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class AnimalAIControlInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // Expose some fields in Animal for easier use. Then, add an AI disabled check to animal update methods.
            TypeDefinition type = gamedef.GetType("SDG.Unturned.Animal");
            FieldDefinition field = GetField(type, "health");
            field.IsPrivate = false;
            field.IsPublic = true;
            FieldDefinition fieldistick = GetField(type, "isTicking");
            fieldistick.IsPrivate = false;
            fieldistick.IsPublic = true;
            FieldDefinition fieldtarget = GetField(type, "target");
            fieldtarget.IsPrivate = false;
            fieldtarget.IsPublic = true;
            FieldDefinition aidisabledfield = new FieldDefinition("UFM_AIDisabled", FieldAttributes.Public, gamedef.TypeSystem.Boolean);
            type.Fields.Add(aidisabledfield);
            foreach (MethodDefinition tmethod in type.Methods)
            {
                switch (tmethod.Name)
                {
                    case "alert":
                    case "getFleeTarget":
                    case "getWanderTarget":
                        DisableAI(tmethod.Body, aidisabledfield);
                        break;
                    default:
                        break;
                }
            }
            // TODO: Disable things in the update methods too.
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
