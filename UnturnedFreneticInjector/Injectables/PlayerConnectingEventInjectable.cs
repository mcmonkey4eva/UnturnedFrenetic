using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class PlayerConnectingEventInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // This injects a call to the mod's static Init() method into the top of the game's central Provider Awake method.
            // Our mod init will run before anything else in the assembly due to this.
            TypeDefinition modtype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodReference connectingmethod = gamedef.ImportReference(GetMethod(modtype, "PlayerConnecting", 1));
            TypeDefinition providertype = gamedef.GetType("SDG.Unturned.Provider");
            MethodDefinition validatemethod = GetMethod(providertype, "handleValidateAuthTicketResponse", 1);
            MethodBody validatebody = validatemethod.Body;
            // Call: the mod's wrapper method.
            InjectInstructions(validatebody, 78, new Instruction[]
                {
                    // Load "steamPending" onto the stack.
                    Instruction.Create(OpCodes.Ldloc_1),
                    // "Call the connect method with parameter 'steamPending' and returning a bool.
                    Instruction.Create(OpCodes.Call, connectingmethod),
                    // If the return is false, jump ahead to the original 78th instruction.
                    Instruction.Create(OpCodes.Brfalse, validatebody.Instructions[78]),
                    // Otherwise,return now.
                    Instruction.Create(OpCodes.Ret)
                });
        }
    }
}
