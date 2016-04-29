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
    // @Name EntityDeathEvent
    // @Fired When an entity dies.
    // @Updated 2016/04/29
    // @Authors Morphan1
    // @Group Player
    // @Cancellable true
    // @Description
    // This event will fire when an entity dies for any reason. (Animals, Players, and Zombies)
    // @Context entity EntityTag returns the entity that is dying.
    // @Context amount TextTag returns the amount of damage being done to kill the entity. Editable.
    // -->

    public class EntityDeathScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the EntityDeath script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public EntityDeathScriptEvent(Commands system)
            : base(system, "entitydeathevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnEntityDeath.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnEntityDeath.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnEntityDeath.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnEntityDeath.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, EntityDeathEventArgs oevt)
        {
            EntityDeathScriptEvent evt = (EntityDeathScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.Entity = oevt.Entity;
            evt.Amount = oevt.Amount;
            evt.Call(prio);
            oevt.Amount = evt.Amount;
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The entity that is dying.
        /// </summary>
        public TemplateObject Entity;

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
            vars.Add("entity", Entity);
            vars.Add("amount", Amount);
            return vars;
        }

        public override void UpdateVariables(Dictionary<string, TemplateObject> vars)
        {
            Amount = NumberTag.TryFor(vars["amount"]);
            base.UpdateVariables(vars);
        }
    }

    public class EntityDeathEventArgs : EventArgs
    {
        public TemplateObject Entity;
        public NumberTag Amount;

        public bool Cancelled = false;
    }
}
