using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;

namespace UnturnedFreneticInjector
{
    public abstract class Injectable
    {
        public abstract void InjectInto(ModuleDefinition mdef);
    }
}
