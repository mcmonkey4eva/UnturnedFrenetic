using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript;
using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
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
            evt.Cause = oevt.Cause;
            evt.Attacker = oevt.Attacker;
            evt.Limb = oevt.Limb;
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
        public NumberTag Amount;

        /// <summary>
        /// The reason the player is being damaged.
        /// </summary>
        public TextTag Cause;

        /// <summary>
        /// The entity attacking this player, if any.
        /// </summary>
        public TemplateObject Attacker;

        /// <summary>
        /// The specific limb that was damaged.
        /// </summary>
        public TextTag Limb;

        /// <summary>
        /// Get all variables according the script event's current values.
        /// </summary>
        public override Dictionary<string, TemplateObject> GetVariables()
        {
            Dictionary<string, TemplateObject> vars = base.GetVariables();
            vars.Add("player", Player);
            vars.Add("amount", Amount);
            vars.Add("cause", Cause);
            vars.Add("limb", Limb);
            vars.Add("attacker", Attacker);
            return vars;
        }

        public override void UpdateVariables(Dictionary<string, TemplateObject> vars)
        {
            Amount = NumberTag.TryFor(vars["amount"]);
            base.UpdateVariables(vars);
        }
    }

    public class PlayerDamagedEventArgs : EventArgs
    {
        public PlayerTag Player;
        public NumberTag Amount;
        public TextTag Cause;
        public TextTag Limb;
        public TemplateObject Attacker;

        public bool Cancelled = false;
    }
}
