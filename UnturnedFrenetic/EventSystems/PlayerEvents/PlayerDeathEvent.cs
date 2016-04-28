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
    // @Name PlayerDeathEvent
    // @Fired When a player dies.
    // @Updated 2015/12/31
    // @Authors Morphan1
    // @Group Player
    // @Cancellable true
    // @Description
    // This event will fire when a player dies for any reason.
    // @Var player PlayerTag returns the player that is dying.
    // @Var amount TextTag returns the amount of damage being done to kill the player.
    // @Var cause TextTag returns the reason the player is dying. Can be: BLEEDING, BONES, FREEZING, BURNING, FOOD, WATER, GUN, MELEE, ZOMBIE, ANIMAL, SUICIDE, KILL, INFECTION, PUNCH, BREATH, ROADKILL, VEHICLE, GRENADE, SHRED, LANDMINE.
    // @Var limb TextTag returns the specific limb that was damaged to kill the player. Can be: LEFT_FOOT, LEFT_LEG, RIGHT_FOOT, RIGHT_LEG, LEFT_HAND, LEFT_ARM, RIGHT_HAND, RIGHT_ARM, LEFT_BACK, RIGHT_BACK, LEFT_FRONT, RIGHT_FRONT, SPINE, SKULL.
    // @Var killer PlayerTag returns the player that killed this player, if any.
    // @Determination amount:<TextTag> sets the amount of damage to be done (possibly instead of dying).
    // -->

    /// <summary>
    /// PlayerDeathScriptEvent, called by a player being damaaged.
    /// </summary>
    public class PlayerDeathScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the PlayerDeath script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public PlayerDeathScriptEvent(Commands system)
            : base(system, "playerdeathevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnPlayerDeath.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnPlayerDeath.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnPlayerDeath.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnPlayerDeath.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, PlayerDeathEventArgs oevt)
        {
            PlayerDeathScriptEvent evt = (PlayerDeathScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.Player = oevt.Player;
            evt.Amount = oevt.Amount;
            evt.Cause = oevt.Cause;
            evt.Killer = oevt.Killer;
            evt.Call(prio);
            oevt.Amount = evt.Amount;
            oevt.Cause = evt.Cause;
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The player that is dying.
        /// </summary>
        public PlayerTag Player;

        /// <summary>
        /// The amount of damage being done.
        /// </summary>
        public NumberTag Amount;

        /// <summary>
        /// The reason the player is dying.
        /// </summary>
        public TextTag Cause;

        /// <summary>
        /// The entity killing this player, if any.
        /// </summary>
        public TemplateObject Killer;

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
            if (Killer != null)
            {
                vars.Add("killer", Killer);
            }
            return vars;
        }

        public override void UpdateVariables(Dictionary<string, TemplateObject> vars)
        {
            Amount = NumberTag.TryFor(vars["amount"]);
            // TODO: Verify amount is valid?
            base.UpdateVariables(vars);
        }
    }

    public class PlayerDeathEventArgs : EventArgs
    {
        public PlayerTag Player;
        public NumberTag Amount;
        public TextTag Cause;
        public TextTag Limb;
        public TemplateObject Killer;

        public bool Cancelled = false;
    }
}
