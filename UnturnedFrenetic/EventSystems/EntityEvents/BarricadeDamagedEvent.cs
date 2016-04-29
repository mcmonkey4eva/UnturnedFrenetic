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
    // @Name BarricadeDamagedEvent
    // @Fired When a barricade is damaged.
    // @Updated 2016/04/29
    // @Authors Morphan1
    // @Group Entity
    // @Cancellable true
    // @Description
    // This event will fire when a barricade takes any form of damage.
    // @Context barricade BarricadeTag returns the barricade that is being damaged.
    // @Context amount TextTag returns the amount of damage being done. Editable.
    // -->

    public class BarricadeDamagedScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the BarricadeDamaged script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public BarricadeDamagedScriptEvent(Commands system)
            : base(system, "barricadedamagedevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnBarricadeDamaged.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnBarricadeDamaged.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnBarricadeDamaged.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnBarricadeDamaged.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, BarricadeDamagedEventArgs oevt)
        {
            BarricadeDamagedScriptEvent evt = (BarricadeDamagedScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.Barricade = oevt.Barricade;
            evt.Amount = oevt.Amount;
            evt.Call(prio);
            oevt.Amount = evt.Amount;
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The barricade that is being damaged.
        /// </summary>
        public BarricadeTag Barricade;

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
            vars.Add("barricade", Barricade);
            vars.Add("amount", Amount);
            return vars;
        }

        public override void UpdateVariables(Dictionary<string, TemplateObject> vars)
        {
            Amount = NumberTag.TryFor(vars["amount"]);
            base.UpdateVariables(vars);
        }
    }

    public class BarricadeDamagedEventArgs : EventArgs
    {
        public BarricadeTag Barricade;
        public NumberTag Amount;

        public bool Cancelled = false;
    }
}
