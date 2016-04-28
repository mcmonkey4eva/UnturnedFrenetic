using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;

namespace UnturnedFrenetic.EventSystems.PlayerEvents
{
    // <--[event]
    // @Name PlayerDisconnectedEvent
    // @Fired When a player has disconnected.
    // @Updated 2015/12/06
    // @Authors mcmonkey
    // @Group Player
    // @Cancellable false
    // @Description
    // This event will fire after a player disconnects from the server.
    // @Context player PlayerTag returns the player that disconnected.
    // -->
    /// <summary>
    /// PlayerDisconnectedScriptEvent, called by player disconnection.
    /// </summary>
    public class PlayerDisconnectedScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the PlayerConnected script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public PlayerDisconnectedScriptEvent(Commands system)
            : base(system, "playerdisconnectedevent", false)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnPlayerDisconnected.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnPlayerDisconnected.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnPlayerDisconnected.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnPlayerDisconnected.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, PlayerDisconnectedEventArgs oevt)
        {
            PlayerDisconnectedScriptEvent evt = (PlayerDisconnectedScriptEvent)Duplicate();
            evt.Player = oevt.Player;
            evt.Call(prio);
        }

        /// <summary>
        /// The player that connected.
        /// </summary>
        public PlayerTag Player;

        /// <summary>
        /// Get all variables according the script event's current values.
        /// </summary>
        public override Dictionary<string, TemplateObject> GetVariables()
        {
            Dictionary<string, TemplateObject> vars = base.GetVariables();
            vars.Add("player", Player);
            return vars;
        }
    }

    public class PlayerDisconnectedEventArgs : EventArgs
    {
        public PlayerTag Player;
    }
}
