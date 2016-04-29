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
    // @Name VehicleDamagedEvent
    // @Fired When a vehicle is damaged.
    // @Updated 2016/04/29
    // @Authors Morphan1
    // @Group Entity
    // @Cancellable true
    // @Description
    // This event will fire when a vehicle takes any form of damage.
    // @Context vehicle VehicleTag returns the vehicle that is being damaged.
    // @Context amount TextTag returns the amount of damage being done. Editable.
    // @Context repairable BooleanTag returns whether the damage can be repaired. Editable.
    // -->

    public class VehicleDamagedScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the VehicleDamaged script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public VehicleDamagedScriptEvent(Commands system)
            : base(system, "vehicledamagedevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnVehicleDamaged.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnVehicleDamaged.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnVehicleDamaged.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnVehicleDamaged.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, VehicleDamagedEventArgs oevt)
        {
            VehicleDamagedScriptEvent evt = (VehicleDamagedScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.Vehicle = oevt.Vehicle;
            evt.Amount = oevt.Amount;
            evt.Call(prio);
            oevt.Amount = evt.Amount;
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The vehicle that is being damaged.
        /// </summary>
        public VehicleTag Vehicle;

        /// <summary>
        /// The amount of damage being done.
        /// </summary>
        public NumberTag Amount;

        /// <summary>
        /// Whether the damage can be repaired.
        /// </summary>
        public BooleanTag Repairable;

        /// <summary>
        /// Get all variables according the script event's current values.
        /// </summary>
        public override Dictionary<string, TemplateObject> GetVariables()
        {
            Dictionary<string, TemplateObject> vars = base.GetVariables();
            vars.Add("vehicle", Vehicle);
            vars.Add("amount", Amount);
            vars.Add("repairable", Repairable);
            return vars;
        }

        public override void UpdateVariables(Dictionary<string, TemplateObject> vars)
        {
            Amount = NumberTag.TryFor(vars["amount"]);
            Repairable = BooleanTag.TryFor(vars["repairable"]);
            base.UpdateVariables(vars);
        }
    }

    public class VehicleDamagedEventArgs : EventArgs
    {
        public VehicleTag Vehicle;
        public NumberTag Amount;
        public BooleanTag Repairable;

        public bool Cancelled = false;
    }
}
