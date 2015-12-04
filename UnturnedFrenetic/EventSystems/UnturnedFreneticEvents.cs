using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic;
using UnturnedFrenetic.EventSystems.PlayerEvents;
using Frenetic.CommandSystem;

namespace UnturnedFrenetic.EventSystems
{
    public class UnturnedFreneticEvents
    {
        public static void RegisterAll(Commands system)
        {
            system.RegisterEvent(new PlayerConnectedScriptEvent(system));
        }

        /// <summary>
        /// Fires when a new player connects to the server.
        /// </summary>
        public static FreneticEventHandler<PlayerConnectedEventArgs> OnPlayerConnected = new FreneticEventHandler<PlayerConnectedEventArgs>();
    }
}
