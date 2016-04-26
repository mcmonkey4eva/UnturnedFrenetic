using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;

namespace UnturnedFrenetic.EventSystems.PlayerEvents
{
    // <--[event]
    // @Name PlayerConnectingEvent
    // @Fired When a player is connected.
    // @Updated 2015/12/04
    // @Authors mcmonkey
    // @Group Player
    // @Cancellable true
    // @Description
    // This event will fire before a player connects to the server.
    // @Var player_name TextTag returns the player that is connecting. TODO: SteamID, etc.
    // -->
    /// <summary>
    /// PlayerConnectingScriptEvent, called by player connection.
    /// </summary>
    public class PlayerConnectingScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the PlayerConnected script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public PlayerConnectingScriptEvent(Commands system)
            : base(system, "playerconnectingevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnPlayerConnecting.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnPlayerConnecting.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnPlayerConnecting.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnPlayerConnecting.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, PlayerConnectingEventArgs oevt)
        {
            PlayerConnectingScriptEvent evt = (PlayerConnectingScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.PlayerName = new TextTag(oevt.PlayerName);
            evt.Call(prio);
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The name of the player that is connecting.
        /// </summary>
        public TextTag PlayerName;

        /// <summary>
        /// Get all variables according the script event's current values.
        /// </summary>
        public override Dictionary<string, TemplateObject> GetVariables()
        {
            Dictionary<string, TemplateObject> vars = base.GetVariables();
            vars.Add("player_name", PlayerName);
            return vars;
        }

        /// <summary>
        /// Applies a determination string to the event.
        /// </summary>
        /// <param name="determ">What was determined.</param>
        /// <param name="mode">What debugmode to use.</param>
        public override void ApplyDetermination(TemplateObject determ, DebugMode mode)
        {
            base.ApplyDetermination(determ, mode);
        }
    }

    public class PlayerConnectingEventArgs : EventArgs
    {
        public string PlayerName;

        public bool Cancelled = false;
    }
}
