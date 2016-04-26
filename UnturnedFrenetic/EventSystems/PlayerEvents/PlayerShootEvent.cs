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
    // @Name PlayerShootEvent
    // @Fired When a player shoots with a gun.
    // @Updated 2016/04/26
    // @Authors Morphan1
    // @Group Player
    // @Cancellable true
    // @Description
    // This event will fire when a player shoots with a gun.
    // @Var player PlayerTag returns the player that is shooting the gun.
    // @Var gun ItemAssetTag returns the item of the gun.
    // -->

    /// <summary>
    /// PlayerDamagedScriptEvent, called by a player being damaaged.
    /// </summary>
    public class PlayerShootScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the PlayerShoot script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public PlayerShootScriptEvent(Commands system)
            : base(system, "playershootevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnPlayerShoot.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnPlayerShoot.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnPlayerShoot.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnPlayerShoot.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, PlayerShootEventArgs oevt)
        {
            PlayerShootScriptEvent evt = (PlayerShootScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.Player = oevt.Player;
            evt.Gun = oevt.Gun;
            evt.Call(prio);
            oevt.Gun = evt.Gun;
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The player that is being damaged.
        /// </summary>
        public PlayerTag Player;

        /// <summary>
        /// The type of gun being shot with.
        /// </summary>
        public ItemAssetTag Gun;

        /// <summary>
        /// Get all variables according the script event's current values.
        /// </summary>
        public override Dictionary<string, TemplateObject> GetVariables()
        {
            Dictionary<string, TemplateObject> vars = base.GetVariables();
            vars.Add("player", Player);
            vars.Add("gun", Gun);
            return vars;
        }

        /// <summary>
        /// Applies a determination string to the event.
        /// </summary>
        /// <param name="determ">What was determined.</param>
        /// <param name="mode">What debugmode to use.</param>
        public override void ApplyDetermination(TemplateObject determ, DebugMode mode)
        {
            base.ApplyDetermination(determ, mode);
        }
    }

    public class PlayerShootEventArgs : EventArgs
    {
        public PlayerTag Player;

        public ItemAssetTag Gun;

        public bool Cancelled = false;
    }
}
