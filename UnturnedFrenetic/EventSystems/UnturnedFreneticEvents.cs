﻿using System;
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
            system.RegisterEvent(new PlayerConnectingScriptEvent(system));
            system.RegisterEvent(new PlayerConnectedScriptEvent(system));
        }

        public static FreneticEventHandler<PlayerConnectingEventArgs> OnPlayerConnecting = new FreneticEventHandler<PlayerConnectingEventArgs>();

        public static FreneticEventHandler<PlayerConnectedEventArgs> OnPlayerConnected = new FreneticEventHandler<PlayerConnectedEventArgs>();
    }
}