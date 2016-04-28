using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.EventSystems.EntityEvents
{
    // <--[event]
    // @Name ZombieDeathEvent
    // @Fired When a zombie dies.
    // @Updated 2016/04/27
    // @Authors Morphan1
    // @Group Player
    // @Cancellable true
    // @Description
    // This event will fire when a zombie dies for any reason.
    // @Context zombie ZombieTag returns the zombie that is dying.
    // @Context amount TextTag returns the amount of damage being done to kill the zombie. Editable.
    // -->

    public class ZombieDeathScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the ZombieDeath script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public ZombieDeathScriptEvent(Commands system)
            : base(system, "zombiedeathevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnZombieDeath.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnZombieDeath.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnZombieDeath.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnZombieDeath.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, ZombieDeathEventArgs oevt)
        {
            ZombieDeathScriptEvent evt = (ZombieDeathScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.Zombie = oevt.Zombie;
            evt.Amount = oevt.Amount;
            evt.Call(prio);
            oevt.Amount = evt.Amount;
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The zombie that is dying.
        /// </summary>
        public ZombieTag Zombie;

        /// <summary>
        /// The amount of damage being done.
        /// </summary>
        public NumberTag Amount;

        /// <summary>
        /// Get all variables according the script event's current values.
        /// </summary>
        public override Dictionary<string, TemplateObject> GetVariables()
        {
            Dictionary<string, TemplateObject> vars = base.GetVariables();
            vars.Add("zombie", Zombie);
            vars.Add("amount", Amount);
            return vars;
        }

        public override void UpdateVariables(Dictionary<string, TemplateObject> vars)
        {
            Amount = NumberTag.TryFor(vars["amount"]);
            base.UpdateVariables(vars);
        }
    }

    public class ZombieDeathEventArgs : EventArgs
    {
        public ZombieTag Zombie;
        public NumberTag Amount;

        public bool Cancelled = false;
    }
}
