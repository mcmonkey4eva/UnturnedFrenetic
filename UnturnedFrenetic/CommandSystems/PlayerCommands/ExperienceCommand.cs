using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using FreneticScript.TagHandlers.Objects;
using FreneticScript.TagHandlers;
using FreneticScript;

namespace UnturnedFrenetic.CommandSystems.PlayerCommands
{
    public class ExperienceCommand : AbstractCommand
    {
        // <--[command]
        // @Name experience
        // @Arguments <player> 'award'/'take' <amount>
        // @Short Awards or takes a player's experience points.
        // @Updated 2016/05/02
        // @Authors mcmonkey
        // @Group Player
        // @Minimum 3
        // @Maximum 3
        // @Description
        // Awards or takes a player's experience points.
        // TODO: Explain more!
        // @Example
        // // This gives the player 50 xp.
        // experience award <{var[player]}> 50;
        // -->
        public ExperienceCommand()
        {
            Name = "experience";
            Arguments = "<player> <amount>";
            Description = "Gives a player experience points.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                verify,
                NumberTag.TryFor
            };
        }

        TemplateObject verify(TemplateObject input)
        {
            string low = input.ToString().ToLowerFast();
            if (low == "award" || low == "take")
            {
                return new TextTag(low);
            }
            return null;
        }

        public override void Execute(FreneticScript.CommandSystem.CommandQueue queue, CommandEntry entry)
        {
            IntegerTag num = IntegerTag.TryFor(entry.GetArgumentObject(queue, 2));
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
            bool award = entry.GetArgument(queue, 1) == "award";
            if (num.Internal > 0)
            {
                PlayerSkills skills = player.Internal.player.skills;
                if (award)
                {
                    skills._experience += (uint)num.Internal;
                }
                else
                {
                    skills._experience -= (uint)num.Internal;
                }
                skills.channel.send("tellExperience", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    skills.experience
                });
            }
            else
            {
                queue.HandleError(entry, "Amount must be positive!");
                return;
            }
            if (entry.ShouldShowGood(queue))
            {
                entry.Good(queue, "Successfully " + (award ? "awarded experience to" : "took experience from") + " a player!");
            }
        }
    }
}
