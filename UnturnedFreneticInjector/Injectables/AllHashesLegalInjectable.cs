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
            // This code removes the check "if (!Hash.verifyHash(array3, Level.hash))" and "if (!ReadWrite.appIn(array4, (byte)objects[7]))"
            // from the provider's receiveServer method.
            // This allows the client to connect to the server even if the hash doesn't line up right.
            // Our own client cannot connect to our own server without this.
            TypeDefinition providertype = gamedef.GetType("SDG.Unturned.Provider");
            MethodDefinition receivemethod = GetMethod(providertype, "receiveServer", 5);
            MethodBody rmb = receivemethod.Body;
            for (int i = 0; i < rmb.Instructions.Count; i++)
            {
                if ((rmb.Instructions[i].Offset >= 0x0894 && rmb.Instructions[i].Offset <= 0x08a3)
                    || (rmb.Instructions[i].Offset >= 0x078c && rmb.Instructions[i].Offset <= 0x07ad))
                {
                    rmb.Instructions[i].OpCode = OpCodes.Nop;
                    rmb.Instructions[i].Operand = null;
                }
            }
        }
    }
}
