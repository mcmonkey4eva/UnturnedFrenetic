using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class PlayerDamagedEventInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // This injects a call to the mod's static PlayerDamaged method for the PlayerDamagedScriptEvent, and exposes relevant fields.
            TypeDefinition lifetype = gamedef.GetType("SDG.Unturned.PlayerLife");
            FieldDefinition healthField = GetField(lifetype, "_health");
            healthField.IsPrivate = false;
            healthField.IsPublic = true;
            FieldDefinition bleedingField = GetField(lifetype, "_isBleeding");
            bleedingField.IsPrivate = false;
            bleedingField.IsPublic = true;
            FieldDefinition lastBleeding = GetField(lifetype, "lastBleeding");
            lastBleeding.IsPrivate = false;
            lastBleeding.IsPublic = true;
            FieldDefinition lastBleed = GetField(lifetype, "lastBleed");
            lastBleed.IsPrivate = false;
            lastBleed.IsPublic = true;
            FieldDefinition brokenField = GetField(lifetype, "_isBroken");
            brokenField.IsPrivate = false;
            brokenField.IsPublic = true;
            FieldDefinition ragdollField = GetField(lifetype, "ragdoll");
            ragdollField.IsPrivate = false;
            ragdollField.IsPublic = true;

            MethodDefinition damagemethod = GetMethod(lifetype, "askDamage", 6);
            ParameterDefinition objectParam = new ParameterDefinition("obj", ParameterAttributes.None, gamedef.ImportReference(typeof(object)));
            damagemethod.Parameters.Add(objectParam);

            TypeDefinition modtype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodReference eventhealmethod = gamedef.ImportReference(GetMethod(modtype, "PlayerHealed", 4));
            MethodDefinition healmethod = GetMethod(lifetype, "askHeal", 3);
            MethodBody healbody = healmethod.Body;
            InjectInstructions(healbody, 0, new Instruction[]
                {
                    // Load "base.player" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0),
                    Instruction.Create(OpCodes.Call, GetMethod(gamedef.GetType("SDG.Unturned.PlayerCaller"), "get_player", 0)),
                    // Load "amount" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, healmethod.Parameters[0]),
                    // Load "healBleeding" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, healmethod.Parameters[1]),
                    // Load "healBroken" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, healmethod.Parameters[2]),
                    // Call the PlayerHealed method with the above parameters and return a bool.
                    Instruction.Create(OpCodes.Call, eventhealmethod),
                    // If the return is false, jump ahead to the original 0th instruction.
                    Instruction.Create(OpCodes.Brfalse, healbody.Instructions[0]),
                    // Otherwise,return now.
                    Instruction.Create(OpCodes.Ret)
                });

            MethodReference eventmethod = gamedef.ImportReference(GetMethod(modtype, "PlayerDamaged", 7));
            MethodBody damagebody = damagemethod.Body;
            InjectInstructions(damagebody, 0, new Instruction[]
                {
                    // Load "base.player" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0),
                    Instruction.Create(OpCodes.Call, GetMethod(gamedef.GetType("SDG.Unturned.PlayerCaller"), "get_player", 0)),
                    // Load "amount" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[0]),
                    // Load "newRagdoll" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[1]),
                    // Load "newCause" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[2]),
                    // Load "newLimb" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, damagemethod.Parameters[3]),
                    // Load "newKiller" onto the stack.
                    Instruction.Create(OpCodes.Ldarg, damagemethod.Parameters[4]),
                    // Load our custom "obj" onto the stack
                    Instruction.Create(OpCodes.Ldarg, objectParam),
                    // Call the PlayerDamaged method with the above parameters and return a bool.
                    Instruction.Create(OpCodes.Call, eventmethod),
                    // If the return is false, jump ahead to the original 0th instruction.
                    Instruction.Create(OpCodes.Brfalse, damagebody.Instructions[0]),
                    // Otherwise,return now.
                    Instruction.Create(OpCodes.Ret)
                });

            MethodDefinition suicidemethod = GetMethod(lifetype, "askSuicide", 1);
            MethodBody suicidebody = suicidemethod.Body;
           // Load "null" onto the stack.
            InjectInstructions(suicidebody, 19, Instruction.Create(OpCodes.Ldnull));

            MethodDefinition landedmethod = GetMethod(lifetype, "onLanded", 1);
            MethodBody landedbody = landedmethod.Body;
            // Load "null" onto the stack.
            InjectInstructions(landedbody, 34, Instruction.Create(OpCodes.Ldnull));

            MethodDefinition simulatemethod = GetMethod(lifetype, "simulate", 1);
            MethodBody simulatebody = simulatemethod.Body;
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 986, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 814, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 714, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 626, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 340, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 201, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 158, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 136, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 83, Instruction.Create(OpCodes.Ldnull));
            
            TypeDefinition barriertype = gamedef.GetType("SDG.Unturned.Barrier");
            MethodDefinition barriercollide = GetMethod(barriertype, "OnTriggerEnter", 1);
            MethodBody barriercollidebody = barriercollide.Body;
            // Load "this" onto the stack.
            InjectInstructions(barriercollidebody, 25, Instruction.Create(OpCodes.Ldarg_0));

            TypeDefinition slaytype = gamedef.GetType("SDG.Unturned.CommandSlay");
            MethodDefinition slayexecute = GetMethod(slaytype, "execute", 2);
            MethodBody slayexecutebody = slayexecute.Body;
            // Load "this" onto the stack.
            InjectInstructions(slayexecutebody, 67, Instruction.Create(OpCodes.Ldarg_0));

            TypeDefinition damagetooltype = gamedef.GetType("SDG.Unturned.DamageTool");
            MethodDefinition damagetooldamage = GetMethod(damagetooltype, "damage", 8);
            MethodBody damagetooldamagebody = damagetooldamage.Body;
            // Load "null" onto the stack.
            InjectInstructions(damagetooldamagebody, 24, Instruction.Create(OpCodes.Ldnull));

            TypeDefinition levelmanagertype = gamedef.GetType("SDG.Unturned.LevelManager");
            MethodDefinition levelmanagerplay = GetMethod(levelmanagertype, "arenaPlay", 0);
            MethodBody levelmanagerplaybody = levelmanagerplay.Body;
            // Load "null" onto the stack.
            InjectInstructions(levelmanagerplaybody, 217, Instruction.Create(OpCodes.Ldnull));

            MethodDefinition levelmanagerrestart = GetMethod(levelmanagertype, "arenaRestart", 0);
            MethodBody levelmanagerrestartbody = levelmanagerrestart.Body;
            // Load "null" onto the stack.
            InjectInstructions(levelmanagerrestartbody, 82, Instruction.Create(OpCodes.Ldnull));
            
            TypeDefinition animaltype = gamedef.GetType("SDG.Unturned.Animal");
            MethodDefinition animalupdate = GetMethod(animaltype, "tick", 0);
            MethodBody animalupdatebody = animalupdate.Body;
            // Load "this" onto the stack.
            InjectInstructions(animalupdatebody, 154, Instruction.Create(OpCodes.Ldarg_0));
            TypeDefinition zombietype = gamedef.GetType("SDG.Unturned.Zombie");
            MethodDefinition zombieupdate = GetMethod(zombietype, "tick", 0);
            MethodBody zombieupdatebody = zombieupdate.Body;
            // Load "this" onto the stack.
            InjectInstructions(zombieupdatebody, 1450, Instruction.Create(OpCodes.Ldarg_0));
        }
    }
}
