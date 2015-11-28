using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "decompile")
            {
                Decompile();
                return;
            }
            Console.WriteLine("Reading game file and loading game assembly...");
            AssemblyDefinition assemblyCSharpdll = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
            ModuleDefinition gamedef = assemblyCSharpdll.MainModule;
            Console.WriteLine("Reading mod file and loading mod assembly...");
            AssemblyDefinition unturnedFrenetic = AssemblyDefinition.ReadAssembly("UnturnedFrenetic.dll");
            ModuleDefinition moddef = unturnedFrenetic.MainModule;
            Console.WriteLine("Loaded. Running all injectables...");
            Assembly thisasm = Assembly.GetCallingAssembly();
            gamedef.AssemblyReferences.Add(new AssemblyNameReference("UnturnedFrenetic", new Version(1, 0, 0, 0)));
            foreach (Type type in thisasm.GetTypes())
            {
                try
                {
                    if (!type.IsAbstract && type.BaseType == typeof(Injectable))
                    {
                        Console.WriteLine("Running: " + type.Name);
                        Injectable obj = (Injectable)Activator.CreateInstance(type);
                        obj.InjectInto(gamedef, moddef);
                        Console.WriteLine("Ran: " + type.Name);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to handle " + type.Name + " -> " + ex.ToString());
                }
            }
            Console.WriteLine("Ran all injectables. Saving...");
            assemblyCSharpdll.Write("Assembly-CSharp.Patched.dll");
            Console.WriteLine("Saved. Completing...");
            Console.WriteLine("Completed.");
        }

        static void Decompile()
        {
            Console.WriteLine("Reading file and loading assembly...");
            AssemblyDefinition assemblyCSharpdll = AssemblyDefinition.ReadAssembly("Assembly-CSharp.dll");
            ModuleDefinition mod = assemblyCSharpdll.MainModule;
            Console.WriteLine("Loaded. Dumping to file...");
            Directory.CreateDirectory(Environment.CurrentDirectory + "/ildump");
            foreach (TypeDefinition type in mod.GetTypes())
            {
                string safeFName = Uri.EscapeUriString(type.FullName.Replace('\\', '/').Replace('/', '.'));
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeFName = safeFName.Replace(c, '_');
                }
                Console.WriteLine("Dump: " + type.FullName + " (" + safeFName + ".il)");
                StringBuilder data = new StringBuilder(1000);
                data.Append("FULLNAME: " + type.FullName).Append("\n");
                data.Append("Fields:\n");
                if (type.HasFields)
                {
                    foreach (FieldDefinition field in type.Fields)
                    {
                        data.Append("\t").Append(field.FieldType.FullName + " ").Append(field.Name).Append(": ").Append(BytesToStr(field.InitialValue));
                        data.Append("\n");
                    }
                }
                data.Append("\nMethods:\n");
                if (type.HasMethods)
                {
                    foreach (MethodDefinition method in type.Methods)
                    {
                        data.Append("\t").Append(method.ReturnType.FullName).Append(" ").Append(method.Name).Append("(");
                        if (method.HasParameters)
                        {
                            for (int i = 0; i < method.Parameters.Count; i++)
                            {
                                data.Append(method.Parameters[i].ParameterType.FullName);
                                if (i + 1 < method.Parameters.Count)
                                {
                                    data.Append(", ");
                                }
                            }
                        }
                        data.Append(")\n\t{\n");
                        if (method.HasBody)
                        {
                            foreach (Instruction inst in method.Body.Instructions)
                            {
                                data.Append("\t\t").Append(inst.ToString()).Append("\n");
                            }
                        }
                        data.Append("\t}\n\n");
                    }
                }
                File.WriteAllText(Environment.CurrentDirectory + "/ildump/" + safeFName + ".il", data.ToString());
            }
        }

        static string BytesToStr(byte[] bits)
        {
            StringBuilder sb = new StringBuilder(bits.Length * 5);
            sb.Append("{ ");
            for (int i = 0; i < bits.Length; i++)
            {
                sb.Append((int)bits[i]);
                if (i + 1 < bits.Length)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(" }");
            return sb.ToString();
        }
    }
}
