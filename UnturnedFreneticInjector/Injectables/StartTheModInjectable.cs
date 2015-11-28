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
            MethodReference initmethod = gamedef.Import(GetMethod(modtype, "Init", 0));
            MethodReference setsetworldtimemethod = gamedef.Import(GetMethod(modtype, "SetSetWorldTime", 1));
            TypeDefinition providertype = gamedef.GetType("SDG.Unturned.Provider");
            TypeDefinition lightingmanagertype = gamedef.GetType("SDG.Unturned.LightingManager");
            MethodDefinition awakemethod = GetMethod(providertype, "Awake");
            MethodDefinition set_time = GetMethod(lightingmanagertype, "set_time", 1);
            MethodReference actionctr = gamedef.Import(typeof(Action<uint>).GetConstructors()[0]);
            MethodBody awakebody = awakemethod.Body;
            MethodDefinition md_setworldtime = new MethodDefinition("__FRENETIC_SetWorldTime", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static, gamedef.TypeSystem.Void);
            md_setworldtime.DeclaringType = providertype;
            md_setworldtime.Parameters.Add(new ParameterDefinition("time", ParameterAttributes.None, gamedef.TypeSystem.UInt32));
            md_setworldtime.Body = new MethodBody(md_setworldtime);
            // Push the first method argument onto the stack.
            md_setworldtime.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
            // Send the top of the stack to the lighting manager.
            md_setworldtime.Body.Instructions.Add(Instruction.Create(OpCodes.Call, set_time));
            // End the method.
            md_setworldtime.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            MethodDefinition md_init = new MethodDefinition("__FRENETIC_INITIALIZE", MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static, gamedef.TypeSystem.Void);
            md_init.DeclaringType = providertype;
            md_init.Body = new MethodBody(md_init);
            // Push a null reference onto the stack.
            md_init.Body.Instructions.Add(Instruction.Create(OpCodes.Ldnull));
            // Push a md_setworldtime reference onto the stack.
            md_init.Body.Instructions.Add(Instruction.Create(OpCodes.Ldftn, md_setworldtime));
            // Constructs an Action instance based on the null and md_setworldtime pushed above.
            md_init.Body.Instructions.Add(Instruction.Create(OpCodes.Newobj, actionctr));
            // Set the SetWorldTime action on the server.
            md_init.Body.Instructions.Add(Instruction.Create(OpCodes.Call, setsetworldtimemethod));
            // Call the mod initialization.
            md_init.Body.Instructions.Add(Instruction.Create(OpCodes.Call, initmethod));
            // End the method.
            md_init.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
            providertype.Methods.Add(md_setworldtime);
            providertype.Methods.Add(md_init);
            awakebody.Instructions.Insert(0, Instruction.Create(OpCodes.Call, md_init));
        }
    }
}
