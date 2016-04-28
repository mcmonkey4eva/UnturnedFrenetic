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
            TypeDefinition modtype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodReference eventmethod = gamedef.ImportReference(GetMethod(modtype, "PlayerDamaged", 7));
            MethodDefinition damagemethod = GetMethod(lifetype, "askDamage", 6);
            ParameterDefinition objectParam = new ParameterDefinition("obj", ParameterAttributes.Optional, gamedef.ImportReference(typeof(object)));
            damagemethod.Parameters.Add(objectParam);
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
                    // If the return is true, jump ahead to the original 0th instruction.
                    Instruction.Create(OpCodes.Brfalse, damagebody.Instructions[0]),
                    // Otherwise,return now.
                    Instruction.Create(OpCodes.Ret)
                });

            MethodDefinition suicidemethod = GetMethod(lifetype, "askSuicide", 1);
            MethodBody suicidebody = suicidemethod.Body;
            InjectInstructions(suicidebody, 19, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });

            MethodDefinition landedmethod = GetMethod(lifetype, "onLanded", 1);
            MethodBody landedbody = landedmethod.Body;
            InjectInstructions(landedbody, 34, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });

            MethodDefinition simulatemethod = GetMethod(lifetype, "simulate", 1);
            MethodBody simulatebody = simulatemethod.Body;
            InjectInstructions(simulatebody, 986, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });
            InjectInstructions(simulatebody, 814, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });
            InjectInstructions(simulatebody, 714, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });
            InjectInstructions(simulatebody, 626, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });
            InjectInstructions(simulatebody, 340, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });
            InjectInstructions(simulatebody, 201, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });
            InjectInstructions(simulatebody, 158, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });
            InjectInstructions(simulatebody, 136, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });
            InjectInstructions(simulatebody, 83, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });

            TypeDefinition animaltype = gamedef.GetType("SDG.Unturned.Animal");
            MethodDefinition animalupdate = GetMethod(animaltype, "FixedUpdate", 0);
            MethodBody animalupdatebody = animalupdate.Body;
            InjectInstructions(animalupdatebody, 338, new Instruction[]
                {
                    // Load "this" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0)
                });

            TypeDefinition barriertype = gamedef.GetType("SDG.Unturned.Barrier");
            MethodDefinition barriercollide = GetMethod(barriertype, "OnTriggerEnter", 1);
            MethodBody barriercollidebody = barriercollide.Body;
            InjectInstructions(barriercollidebody, 26, new Instruction[]
                {
                    // Load "this" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0)
                });

            TypeDefinition slaytype = gamedef.GetType("SDG.Unturned.CommandSlay");
            MethodDefinition slayexecute = GetMethod(slaytype, "execute", 2);
            MethodBody slayexecutebody = slayexecute.Body;
            InjectInstructions(slayexecutebody, 67, new Instruction[]
                {
                    // Load "this" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0)
                });

            TypeDefinition damagetooltype = gamedef.GetType("SDG.Unturned.DamageTool");
            MethodDefinition damagetooldamage = GetMethod(damagetooltype, "damage", 8);
            MethodBody damagetooldamagebody = damagetooldamage.Body;
            InjectInstructions(damagetooldamagebody, 24, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });

            TypeDefinition levelmanagertype = gamedef.GetType("SDG.Unturned.LevelManager");
            MethodDefinition levelmanagerplay = GetMethod(levelmanagertype, "arenaPlay", 0);
            MethodBody levelmanagerplaybody = levelmanagerplay.Body;
            InjectInstructions(levelmanagerplaybody, 217, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });

            MethodDefinition levelmanagerrestart = GetMethod(levelmanagertype, "arenaRestart", 0);
            MethodBody levelmanagerrestartbody = levelmanagerrestart.Body;
            InjectInstructions(levelmanagerrestartbody, 82, new Instruction[]
                {
                    // Load "null" onto the stack.
                    Instruction.Create(OpCodes.Ldnull)
                });

            TypeDefinition zombietype = gamedef.GetType("SDG.Unturned.Zombie");
            MethodDefinition zombieupdate = GetMethod(zombietype, "FixedUpdate", 0);
            MethodBody zombieupdatebody = zombieupdate.Body;
            InjectInstructions(zombieupdatebody, 1709, new Instruction[]
                {
                    // Load "this" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0)
                });
        }
    }
}
