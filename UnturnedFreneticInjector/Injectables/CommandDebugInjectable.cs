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
            // This injects a debug output message to the "debug" command, as well as allowing execution
            // of Frenetic command via the debug command.
            // To use, simple input: debug echo "Hello world!"
            // Replace the command after debug with any valid Frenetic-enabled command.
            TypeDefinition debugtype = gamedef.GetType("SDG.Unturned.CommandDebug");
            MethodDefinition method = GetMethod(debugtype, "execute");
            TypeDefinition modmaintype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodDefinition modruncommands = GetMethod(modmaintype, "RunCommands", 1);
            MethodReference game_modruncommands = gamedef.ImportReference(modruncommands);
            TypeDefinition windowtype = gamedef.GetType("SDG.Unturned.CommandWindow");
            MethodDefinition logmethod = GetMethod(windowtype, "Log", 1);
            MethodDefinition isnullorempty = GetMethod(gamedef.TypeSystem.String.Resolve(), "IsNullOrEmpty", 1);
            MethodReference game_isnullorempty = gamedef.ImportReference(isnullorempty);
            MethodBody body = method.Body;
            Instruction ldstr = Instruction.Create(OpCodes.Ldstr, "Frenetic loaded properly!");
            InjectInstructions(body, 3, new Instruction[]
            {
                // Load the parameter onto the stack.
                Instruction.Create(OpCodes.Ldarg_S, method.Parameters[1]),
                // Add whether the string 'is null or empty' to the stack.
                Instruction.Create(OpCodes.Call, game_isnullorempty),
                // Jump to the end if the top of the stack is false.
                Instruction.Create(OpCodes.Brtrue_S, ldstr), // NOTE: Instruction reference will move at the end.
                // Load the parameter onto the stack.
                Instruction.Create(OpCodes.Ldarg_S, method.Parameters[1]),
                // Run commands based on the string on top of the stack.
                Instruction.Create(OpCodes.Call, game_modruncommands),
                // Return so we don't get random debug spam.
                Instruction.Create(OpCodes.Ret),
                // Load the output string onto the stack.
                ldstr,
                // Log the string.
                Instruction.Create(OpCodes.Call, logmethod)
            });
            // Redirect the result of the IF to our new method.
            body.Instructions[1].Operand = body.Instructions[3];
        }
    }
}
