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
            // This injects a call to the mod's static PlayerDamaged method for the PlayerDamagedScriptEvent, and exposes relevant fields. It also adds a new parameter to 'PlayerLife.askDamage'.
            TypeDefinition lifetype = gamedef.GetType("SDG.Unturned.PlayerLife");
            MakePublic(GetField(lifetype, "_health"));
            MakePublic(GetField(lifetype, "_isBleeding"));
            MakePublic(GetField(lifetype, "lastBleeding"));
            MakePublic(GetField(lifetype, "lastBleed"));
            MakePublic(GetField(lifetype, "_isBroken"));
            MakePublic(GetField(lifetype, "ragdoll"));

            TypeDefinition skillstype = gamedef.GetType("SDG.Unturned.PlayerSkills");
            MakePublic(GetField(skillstype, "_experience"));

            MethodDefinition damagemethod = GetMethod(lifetype, "askDamage", 6);
            ParameterDefinition objectParam = new ParameterDefinition("obj", ParameterAttributes.None, gamedef.ImportReference(typeof(object)));
            damagemethod.Parameters.Add(objectParam);

            MethodDefinition dodamagemethod = GetMethod(lifetype, "doDamage", 6);
            ParameterDefinition doobjectParam = new ParameterDefinition("obj", ParameterAttributes.None, gamedef.ImportReference(typeof(object)));
            dodamagemethod.Parameters.Add(doobjectParam);

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
            InjectInstructions(damagebody, 22, Instruction.Create(OpCodes.Ldarg, objectParam));
            MethodBody dodamagebody = dodamagemethod.Body;
            InjectInstructions(dodamagebody, 0, new Instruction[]
                {
                    // Load "base.player" onto the stack.
                    Instruction.Create(OpCodes.Ldarg_0),
                    Instruction.Create(OpCodes.Call, GetMethod(gamedef.GetType("SDG.Unturned.PlayerCaller"), "get_player", 0)),
                    // Load "amount" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, dodamagemethod.Parameters[0]),
                    // Load "newRagdoll" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, dodamagemethod.Parameters[1]),
                    // Load "newCause" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, dodamagemethod.Parameters[2]),
                    // Load "newLimb" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, dodamagemethod.Parameters[3]),
                    // Load "newKiller" onto the stack.
                    Instruction.Create(OpCodes.Ldarg, dodamagemethod.Parameters[4]),
                    // Load our custom "obj" onto the stack
                    Instruction.Create(OpCodes.Ldarg, objectParam),
                    // Call the PlayerDamaged method with the above parameters and return a bool.
                    Instruction.Create(OpCodes.Call, eventmethod),
                    // If the return is false, jump ahead to the original 0th instruction.
                    Instruction.Create(OpCodes.Brfalse, dodamagebody.Instructions[0]),
                    // Otherwise,return now.
                    Instruction.Create(OpCodes.Ret)
                });

            MethodDefinition suicidemethod = GetMethod(lifetype, "askSuicide", 1);
            MethodBody suicidebody = suicidemethod.Body;
           // Load "null" onto the stack.
            InjectInstructions(suicidebody, 35, Instruction.Create(OpCodes.Ldnull));

            MethodDefinition landedmethod = GetMethod(lifetype, "onLanded", 1);
            MethodBody landedbody = landedmethod.Body;
            // Load "null" onto the stack.
            InjectInstructions(landedbody, 38, Instruction.Create(OpCodes.Ldnull));

            MethodDefinition simulatemethod = GetMethod(lifetype, "simulate", 1);
            MethodBody simulatebody = simulatemethod.Body;
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 1170, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 992, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 900, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 875, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 802, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 721, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 646, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 193, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 150, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 128, Instruction.Create(OpCodes.Ldnull));
            // Load "null" onto the stack.
            InjectInstructions(simulatebody, 75, Instruction.Create(OpCodes.Ldnull));
            
            TypeDefinition barriertype = gamedef.GetType("SDG.Unturned.Barrier");
            MethodDefinition barriercollide = GetMethod(barriertype, "OnTriggerEnter", 1);
            MethodBody barriercollidebody = barriercollide.Body;
            // Load "this" onto the stack.
            InjectInstructions(barriercollidebody, 25, Instruction.Create(OpCodes.Ldarg_0));

            TypeDefinition slaytype = gamedef.GetType("SDG.Unturned.CommandSlay");
            MethodDefinition slayexecute = GetMethod(slaytype, "execute", 2);
            MethodBody slayexecutebody = slayexecute.Body;
            // Load "this" onto the stack.
            InjectInstructions(slayexecutebody, 111, Instruction.Create(OpCodes.Ldarg_0));

            TypeDefinition killtype = gamedef.GetType("SDG.Unturned.CommandKill");
            MethodDefinition killexecute = GetMethod(killtype, "execute", 2);
            MethodBody killexecutebody = killexecute.Body;
            // Load "this" onto the stack.
            InjectInstructions(killexecutebody, 40, Instruction.Create(OpCodes.Ldarg_0));

            TypeDefinition damagetooltype = gamedef.GetType("SDG.Unturned.DamageTool");
            MethodDefinition damagetooldamage = GetMethod(damagetooltype, "damage", 8);
            MethodBody damagetooldamagebody = damagetooldamage.Body;
            // Load "null" onto the stack.
            InjectInstructions(damagetooldamagebody, 30, Instruction.Create(OpCodes.Ldnull));

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
            InjectInstructions(animalupdatebody, 167, Instruction.Create(OpCodes.Ldarg_0));
            TypeDefinition zombietype = gamedef.GetType("SDG.Unturned.Zombie");
            MethodDefinition zombieupdate = GetMethod(zombietype, "tick", 0);
            MethodBody zombieupdatebody = zombieupdate.Body;
            // Load "this" onto the stack.
            InjectInstructions(zombieupdatebody, 2116, Instruction.Create(OpCodes.Ldarg_0));
        }
    }
}
