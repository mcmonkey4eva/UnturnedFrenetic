using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnturnedFrenetic.TagSystems.TagObjects;
using Frenetic;
using Frenetic.CommandSystem;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;

namespace UnturnedFrenetic.EventSystems.PlayerEvents
{
    // <--[event]
    // @Name PlayerDamagedEvent
    // @Fired When a player is damaged.
    // @Updated 2015/12/12
    // @Authors Morphan1
    // @Group Player
    // @Cancellable true
    // @Description
    // This event will fire when a player takes any form of damage.
    // @Var player PlayerTag returns the player that is being damaged.
    // @Var amount TextTag returns the amount of damage being done.
    // @Determination amount:<TextTag> sets the amount of damage to be done.
    // -->

    /// <summary>
    /// PlayerDamagedScriptEvent, called by a player being damaaged.
    /// </summary>
    public class PlayerDamagedScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the PlayerDamaged script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public PlayerDamagedScriptEvent(Commands system)
            : base(system, "playerdamagedevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnPlayerDamaged.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnPlayerDamaged.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnPlayerDamaged.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnPlayerDamaged.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, PlayerDamagedEventArgs oevt)
        {
            PlayerDamagedScriptEvent evt = (PlayerDamagedScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.Player = oevt.Player;
            evt.Amount = oevt.Amount;
            evt.Call(prio);
            oevt.Amount = evt.Amount;
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The player that is being damaged.
        /// </summary>
        public PlayerTag Player;

        /// <summary>
        /// The amount of damage being done.
        /// </summary>
        public TextTag Amount;

        /// <summary>
        /// Get all variables according the script event's current values.
        /// </summary>
        public override Dictionary<string, TemplateObject> GetVariables()
        {
            Dictionary<string, TemplateObject> vars = base.GetVariables();
            vars.Add("player", Player);
            vars.Add("amount", Amount);
            return vars;
        }

        /// <summary>
        /// Applies a determination string to the event.
        /// </summary>
        /// <param name="determ">What was determined.</param>
        /// <param name="determLow">A lowercase copy of the determination.</param>
        /// <param name="mode">What debugmode to use.</param>
        public override void ApplyDetermination(string determ, string determLow, DebugMode mode)
        {
            if (determLow.StartsWith("amount:"))
            {
                Amount = new TextTag(determ.Substring("amount:".Length));
            }
            else
            {
                base.ApplyDetermination(determ, determLow, mode);
            }
        }
    }

    public class PlayerDamagedEventArgs : EventArgs
    {
        public PlayerTag Player;
        public TextTag Amount;

        public bool Cancelled = false;
    }
}
