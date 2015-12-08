using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic;
using Frenetic.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using Frenetic.TagHandlers;

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
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 2)
            {
                ShowUsage(entry);
                return;
            }
            LocationTag loc = LocationTag.For(entry.GetArgument(1));
            if (loc == null)
            {
                entry.Bad("Invalid location!");
                return;
            }
            string targetAssetType = entry.GetArgument(0).ToLower();
            EffectAssetTag effectType = EffectAssetTag.For(targetAssetType);
            if (effectType == null)
            {
                entry.Bad("Invalid effect type!");
                return;
            }
            EffectManager.sendEffect(effectType.Internal.id, EffectManager.INSANE, loc.ToVector3());
            // TODO: radius option instead of always 512 units (INSANE)?
            entry.Good("Played effect " + TagParser.Escape(effectType.ToString()) + " at " + TagParser.Escape(loc.ToString()) +  "!");
        }
    }
}
