using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnturnedFrenetic.CommandSystems;

namespace UnturnedFrenetic
{
    public class UnturnedFreneticMod
    {
        public static UnturnedFreneticMod Instance;

        public static void Init()
        {
            Console.WriteLine("Unturned Frenetic mod loading...");
            SysConsole.Init();
            Instance = new UnturnedFreneticMod();
            Instance.Setup();
            SysConsole.Output(OutputType.INIT, "Unturned Frenetic mod loaded! Releasing back to main game...");
        }

        public static void RunCommands(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                Instance.CommandSystem.System.ExecuteCommands(input, null);
            }
        }

        public UnturnedFreneticCommands CommandSystem;

        public void Setup()
        {
            CommandSystem = new UnturnedFreneticCommands(this, new UnturnedFreneticOutputter() { TheMod = this });
        }
    }
}
