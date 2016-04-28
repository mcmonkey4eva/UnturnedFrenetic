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
    // @Name ZombieDamagedEvent
    // @Fired When a zombie is damaged.
    // @Updated 2016/04/27
    // @Authors Morphan1
    // @Group Entity
    // @Cancellable true
    // @Description
    // This event will fire when a zombie takes any form of damage.
    // @Context zombie ZombieTag returns the zombie that is being damaged.
    // @Context amount TextTag returns the amount of damage being done. Editable.
    // -->

    public class ZombieDamagedScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the ZombieDamaged script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public ZombieDamagedScriptEvent(Commands system)
            : base(system, "zombiedamagedevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnZombieDamaged.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnZombieDamaged.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnZombieDamaged.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnZombieDamaged.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, ZombieDamagedEventArgs oevt)
        {
            ZombieDamagedScriptEvent evt = (ZombieDamagedScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.Zombie = oevt.Zombie;
            evt.Amount = oevt.Amount;
            evt.Call(prio);
            oevt.Amount = evt.Amount;
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The zombie that is being damaged.
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

    public class ZombieDamagedEventArgs : EventArgs
    {
        public ZombieTag Zombie;
        public NumberTag Amount;

        public bool Cancelled = false;
    }
}
