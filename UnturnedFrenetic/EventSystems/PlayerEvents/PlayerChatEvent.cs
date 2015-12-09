using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnturnedFrenetic.TagSystems.TagObjects;
using Frenetic;
using Frenetic.CommandSystem;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;

namespace UnturnedFrenetic.EventSystems.PlayerEvents
{
    // <--[event]
    // @Name PlayerChatEvent
    // @Fired When a player chats.
    // @Updated 2015/12/08
    // @Authors Morphan1
    // @Group Player
    // @Cancellable true
    // @Description
    // This event will fire when a player sends a chat message.
    // @Var player PlayerTag returns the player that is chatting.
    // @Var chat_mode TextTag returns the mode the player is chatting in.
    // @Var text TextTag returns the text contents of the chat.
    // -->
    /// <summary>
    /// PlayerChatScriptEvent, called by a player chatting.
    /// </summary>
    public class PlayerChatScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the PlayerChat script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public PlayerChatScriptEvent(Commands system)
            : base(system, "playerchatevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnPlayerChat.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnPlayerChat.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnPlayerChat.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnPlayerChat.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, PlayerChatEventArgs oevt)
        {
            PlayerChatScriptEvent evt = (PlayerChatScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.Player = oevt.Player;
            evt.ChatMode = oevt.ChatMode;
            evt.Text = oevt.Text;
            evt.Color = oevt.Color;
            evt.Call(prio);
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The player that is chatting.
        /// </summary>
        public PlayerTag Player;

        /// <summary>
        /// The chat mode the player is chatting in.
        /// </summary>
        public TextTag ChatMode;

        /// <summary>
        /// The text contents of the chat message.
        /// </summary>
        public TextTag Text;

        /// <summary>
        /// The color of the chat message.
        /// </summary>
        public ColorTag Color;

        /// <summary>
        /// Get all variables according the script event's current values.
        /// </summary>
        public override Dictionary<string, TemplateObject> GetVariables()
        {
            Dictionary<string, TemplateObject> vars = base.GetVariables();
            vars.Add("player", Player);
            vars.Add("chat_mode", ChatMode);
            vars.Add("text", Text);
            vars.Add("color", Color);
            return vars;
        }

        /// <summary>
        /// Applies a determination string to the event.
        /// </summary>
        /// <param name="determ">What was determined.</param>
        /// <param name="determLow">A lowercase copy of the determination.</param>
        /// <param name="mode">What debugmode to use.</param>
        public override void ApplyDetermination(string determ, string determLow, DebugMode mode)
        {
            // TODO: change chat mode, text, or color
            base.ApplyDetermination(determ, determLow, mode);
        }
    }

    public class PlayerChatEventArgs : EventArgs
    {
        public PlayerTag Player;
        public TextTag ChatMode;
        public TextTag Text;
        public ColorTag Color;

        public bool Cancelled = false;
    }
}
