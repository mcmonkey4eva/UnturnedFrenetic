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
            MethodBody body = method.Body;
            // Load the output string onto the stack
            body.Instructions.Insert(3, Instruction.Create(OpCodes.Ldstr, "Hello World! This is the first working Unturned Frenetic Injector :)"));
            TypeDefinition windowtype = gamedef.GetType("SDG.Unturned.CommandWindow");
            MethodDefinition logmethod = GetMethod(windowtype, "Log", 1);
            // Log the string
            body.Instructions.Insert(4, Instruction.Create(OpCodes.Call, logmethod.GetElementMethod()));
            // Redirect the result of the IF to our new method
            body.Instructions[1].Operand = body.Instructions[3];
        }
    }
}
