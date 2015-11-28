using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Mono.Cecil;

namespace UnturnedFreneticInjector
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Reading file and loading assembly...");
            AssemblyDefinition assemblyCSharpdll = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
            ModuleDefinition mod = assemblyCSharpdll.MainModule;
            Console.WriteLine("Loaded. Running all injectables...");
            Assembly thisasm = Assembly.GetCallingAssembly();
            foreach (Type type in thisasm.GetTypes())
            {
                if (!type.IsAbstract && type.BaseType == typeof(Injectable))
                {
                    Console.WriteLine("Running: " + type.Name);
                    Injectable obj = (Injectable)Activator.CreateInstance(type);
                    obj.InjectInto(mod);
                    Console.WriteLine("Ran: " + type.Name);
                }
            }
            Console.WriteLine("Ran all injectables. Saving...");
            assemblyCSharpdll.Write("Assembly-CSharp.Patched.dll");
            Console.WriteLine("Saved. Completing...");
            Console.WriteLine("Completed.");
        }
    }
}
