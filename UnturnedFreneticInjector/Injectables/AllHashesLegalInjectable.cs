using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    class AllHashesLegalInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            TypeDefinition providertype = gamedef.GetType("SDG.Unturned.Provider");
            MethodDefinition receivemethod = GetMethod(providertype, "receiveServer", 5);
            MethodBody rmb = receivemethod.Body;
            for (int i = 0; i < rmb.Instructions.Count; i++)
            {
                if ((rmb.Instructions[i].Offset >= 0x0853 && rmb.Instructions[i].Offset <= 0x0862)
                    || (rmb.Instructions[i].Offset >= 0x074b && rmb.Instructions[i].Offset <= 0x076c))
                {
                    rmb.Instructions[i].OpCode = OpCodes.Nop;
                    rmb.Instructions[i].Operand = null;
                }
            }
        }
    }
}
