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
    // @Name AnimalDamagedEvent
    // @Fired When an animal is damaged.
    // @Updated 2016/04/27
    // @Authors Morphan1
    // @Group Entity
    // @Cancellable true
    // @Description
    // This event will fire when an animal takes any form of damage.
    // @Context animal AnimalTag returns the animal that is being damaged.
    // @Context amount TextTag returns the amount of damage being done. Editable.
    // -->

    public class AnimalDamagedScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the AnimalDamaged script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public AnimalDamagedScriptEvent(Commands system)
            : base(system, "animaldamagedevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnAnimalDamaged.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnAnimalDamaged.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnAnimalDamaged.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnAnimalDamaged.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, AnimalDamagedEventArgs oevt)
        {
            AnimalDamagedScriptEvent evt = (AnimalDamagedScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.Animal = oevt.Animal;
            evt.Amount = oevt.Amount;
            evt.Call(prio);
            oevt.Amount = evt.Amount;
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The animal that is being damaged.
        /// </summary>
        public AnimalTag Animal;

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
            vars.Add("animal", Animal);
            vars.Add("amount", Amount);
            return vars;
        }

        public override void UpdateVariables(Dictionary<string, TemplateObject> vars)
        {
            Amount = NumberTag.TryFor(vars["amount"]);
            base.UpdateVariables(vars);
        }
    }

    public class AnimalDamagedEventArgs : EventArgs
    {
        public AnimalTag Animal;
        public NumberTag Amount;

        public bool Cancelled = false;
    }
}
