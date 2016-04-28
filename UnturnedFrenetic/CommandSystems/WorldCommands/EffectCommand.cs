using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using FreneticScript.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.WorldCommands
{
    public class EffectCommand : AbstractCommand
    {
        // <--[command]
        // @Name effect
        // @Arguments <effect> <location>
        // @Short Plays an effect at the specified location.
        // @Updated 2015/12/07
        // @Authors Morphan1
        // @Group World
        // @Minimum 2
        // @Maximum 2
        // @Description
        // This displays an effect at a location or plays an audio effect (or both) to players near a location in the world.
        // TODO: Explain more!
        // @Example
        // // This shows and plays the sound for a bomb explosion at "12,32,45".
        // effect bomb_0 12,32,45
        // -->

        public EffectCommand()
        {
            Name = "effect";
            Arguments = "<effect> <location>";
            Description = "Plays an effect at the specified location.";
            MinimumArguments = 2;
            MaximumArguments = 2;
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            LocationTag loc = LocationTag.For(entry.GetArgument(queue, 1));
            if (loc == null)
            {
                queue.HandleError(entry, "Invalid location!");
                return;
            }
            string targetAssetType = entry.GetArgument(queue, 0).ToLower();
            EffectAssetTag effectType = EffectAssetTag.For(targetAssetType);
            if (effectType == null)
            {
                queue.HandleError(entry, "Invalid effect type!");
                return;
            }
            EffectManager.sendEffect(effectType.Internal.id, EffectManager.INSANE, loc.ToVector3());
            // TODO: radius option instead of always 512 units (INSANE)?
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "Played effect " + TagParser.Escape(effectType.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "!");
            }
        }
    }
}
