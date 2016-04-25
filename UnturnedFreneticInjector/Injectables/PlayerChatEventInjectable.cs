using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class PlayerChatEventInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // This injects a call to the mod's static PlayerChat method for the PlayerChatScriptEvent
            TypeDefinition managertype = gamedef.GetType("SDG.Unturned.ChatManager");
            FieldDefinition managerfield = GetField(managertype, "manager");
            managerfield.IsPrivate = false;
            managerfield.IsPublic = true;
            TypeDefinition modtype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodReference eventmethod = gamedef.ImportReference(GetMethod(modtype, "PlayerChat", 5));
            MethodDefinition chatmethod = GetMethod(managertype, "askChat", 3);
            MethodBody chatbody = chatmethod.Body;
            // Remove old color handling
            chatbody.Instructions[53].Operand = chatbody.Instructions[124];
            chatbody.Instructions[81].Operand = chatbody.Instructions[124];
            chatbody.Instructions[109].Operand = chatbody.Instructions[124];
            for (int i = 111; i <= 123; i++)
            {
                chatbody.Instructions.RemoveAt(111);
            }
            InjectInstructions(chatbody, 27, new Instruction[]
                {
                    // Load "steamPlayer" onto the stack.
                    Instruction.Create(OpCodes.Ldloc_0),
                    // Load "mode" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, chatmethod.Parameters[1]),
                    // Load "eChatMode" onto the stack.
                    Instruction.Create(OpCodes.Ldloca_S, chatbody.Variables[1]),
                    // Load "color" onto the stack.
                    Instruction.Create(OpCodes.Ldloca_S, chatbody.Variables[2]),
                    // Load "text" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, chatmethod.Parameters[2]),
                    // Call the PlayerChat method with the above parameters and return a bool.
                    Instruction.Create(OpCodes.Call, eventmethod),
                    // If the return is true, jump ahead to the original 27th instruction.
                    Instruction.Create(OpCodes.Brfalse, chatbody.Instructions[27]),
                    // Otherwise, return now.
                    Instruction.Create(OpCodes.Ret)
                });
        }
    }
}
