using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector
{
    public abstract class Injectable
    {
        public void MakePublic(FieldDefinition field)
        {
            field.IsPrivate = false;
            field.IsPublic = true;
        }

        public void MakePublic(MethodDefinition method)
        {
            method.IsPrivate = false;
            method.IsPublic = true;
        }

        public MethodDefinition GetMethod(TypeDefinition type, string name, int paramscount = -1)
        {
            foreach (MethodDefinition method in type.Methods)
            {
                if (method.Name == name && (paramscount == -1 || method.Parameters.Count == paramscount))
                {
                    return method;
                }
            }
            return null;
        }

        public FieldDefinition GetField(TypeDefinition type, string name)
        {
            foreach (FieldDefinition method in type.Fields)
            {
                if (method.Name == name)
                {
                    return method;
                }
            }
            return null;
        }

        public void InjectInstructions(MethodBody body, int line, Instruction[] instructions)
        {
            foreach (Instruction instr in instructions)
            {
                body.Instructions.Insert(line, instr);
                line++;
            }
        }

        public abstract void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef);
    }
}
