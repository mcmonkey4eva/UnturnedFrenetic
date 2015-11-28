using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class StartTheModInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            TypeDefinition modtype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodReference initmethod = GetMethod(modtype, "Init", 0);
            TypeDefinition providertype = gamedef.GetType("SDG.Unturned.Provider");
            MethodDefinition awakemethod = GetMethod(providertype, "Awake");
            MethodReference tref = gamedef.Import(initmethod);
            MethodBody awakebody = awakemethod.Body;
            // Insert the mod init call
            awakebody.Instructions.Insert(0, Instruction.Create(OpCodes.Call, tref));
        }
    }
}
