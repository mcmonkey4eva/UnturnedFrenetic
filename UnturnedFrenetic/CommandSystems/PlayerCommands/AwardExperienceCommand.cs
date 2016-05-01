using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using FreneticScript.TagHandlers.Objects;
using FreneticScript.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.PlayerCommands
{
    public class AwardExperienceCommand : AbstractCommand
    {
        // <--[command]
        // @Name awardexperience
        // @Arguments <player> <amount>
        // @Short Gives a player experience points.
        // @Updated 2016/04/30
        // @Authors mcmonkey
        // @Group Player
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Gives a player experience points.
        // TODO: Explain more!
        // @Example
        // // This gives the player 50 xp.
        // water <{var[player]}> 50;
        // -->
        public AwardExperienceCommand()
        {
            Name = "awardexperience";
            Arguments = "<player> <amount>";
            Description = "Gives a player experience points.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                NumberTag.TryFor
            };
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            IntegerTag num = IntegerTag.TryFor(entry.GetArgumentObject(queue, 1));
            if (num == null)
            {
                queue.HandleError(entry, "Invalid amount number!");
                return;
            }
            PlayerTag player = PlayerTag.For(entry.GetArgument(queue, 0));
            if (player == null)
            {
                queue.HandleError(entry, "Invalid player!");
                return;
            }
            if (num.Internal > 0)
            {
                player.Internal.player.skills.askAward((uint)num.Internal);
            }
            else
            {
                queue.HandleError(entry, "Amount must be positive!");
                return;
            }
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "Successfully gave experience to a player!");
            }
        }
    }
}
