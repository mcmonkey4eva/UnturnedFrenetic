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
    // @Name ResourceDamagedEvent
    // @Fired When a resource is damaged.
    // @Updated 2016/04/27
    // @Authors Morphan1
    // @Group Entity
    // @Cancellable true
    // @Description
    // This event will fire when a resource takes any form of damage.
    // @Context resource ResourceTag returns the resource that is being damaged.
    // @Context amount TextTag returns the amount of damage being done. Editable.
    // -->

    public class ResourceDamagedScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the ResourceDamaged script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public ResourceDamagedScriptEvent(Commands system)
            : base(system, "resourcedamagedevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnResourceDamaged.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnResourceDamaged.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnResourceDamaged.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnResourceDamaged.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, ResourceDamagedEventArgs oevt)
        {
            ResourceDamagedScriptEvent evt = (ResourceDamagedScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.Resource = oevt.Resource;
            evt.Amount = oevt.Amount;
            evt.Call(prio);
            oevt.Amount = evt.Amount;
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The resource that is being damaged.
        /// </summary>
        public ResourceTag Resource;

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
            vars.Add("resource", Resource);
            vars.Add("amount", Amount);
            return vars;
        }

        public override void UpdateVariables(Dictionary<string, TemplateObject> vars)
        {
            Amount = NumberTag.TryFor(vars["amount"]);
            base.UpdateVariables(vars);
        }
    }

    public class ResourceDamagedEventArgs : EventArgs
    {
        public ResourceTag Resource;
        public NumberTag Amount;

        public bool Cancelled = false;
    }
}
