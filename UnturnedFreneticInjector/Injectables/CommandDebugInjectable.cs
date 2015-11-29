using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    class CommandDebugInjectable : Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            TypeDefinition debugtype = gamedef.GetType("SDG.Unturned.CommandDebug");
            MethodDefinition method = GetMethod(debugtype, "execute");
            ParameterDefinition paramstr = method.Parameters[1];
            TypeDefinition modmaintype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodDefinition modruncommands = GetMethod(modmaintype, "RunCommands", 1);
            MethodReference game_modruncommands = gamedef.Import(modruncommands);
            TypeDefinition windowtype = gamedef.GetType("SDG.Unturned.CommandWindow");
            MethodDefinition logmethod = GetMethod(windowtype, "Log", 1);
            MethodDefinition isnullorempty = GetMethod(gamedef.TypeSystem.String.Resolve(), "IsNullOrEmpty", 1);
            MethodReference game_isnullorempty = gamedef.Import(isnullorempty);
            MethodBody body = method.Body;
            // Load the parameter onto the stack.
            body.Instructions.Insert(3, Instruction.Create(OpCodes.Ldarg_S, paramstr));
            // Add whether the string 'is null or empty' to the stack.
            body.Instructions.Insert(4, Instruction.Create(OpCodes.Call, game_isnullorempty));
            // Jump to the end if the top of the stack is false.
            body.Instructions.Insert(5, Instruction.Create(OpCodes.Brtrue_S, body.Instructions[0])); // NOTE: Instruction reference will move at the end.
            // Load the parameter onto the stack.
            body.Instructions.Insert(6, Instruction.Create(OpCodes.Ldarg_S, paramstr));
            // Run commands based on the string on top of the stack.
            body.Instructions.Insert(7, Instruction.Create(OpCodes.Call, game_modruncommands));
            // Return so we don't get random debug spam.
            body.Instructions.Insert(8, Instruction.Create(OpCodes.Ret));
            // Load the output string onto the stack.
            body.Instructions.Insert(9, Instruction.Create(OpCodes.Ldstr, "Frenetic loaded properly!"));
            // Log the string.
            body.Instructions.Insert(10, Instruction.Create(OpCodes.Call, logmethod));
            // Redirect the result of the IF to our new method.
            body.Instructions[1].Operand = body.Instructions[3];
            // Redirect the result of the IF to the end of our first injection (The start of the second injection).
            body.Instructions[5].Operand = body.Instructions[9];
        }
    }
}
