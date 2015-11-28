using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace UnturnedFreneticInjector
{
    public abstract class Injectable
    {
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

        public abstract void InjectInto(ModuleDefinition mdef);
    }
}
