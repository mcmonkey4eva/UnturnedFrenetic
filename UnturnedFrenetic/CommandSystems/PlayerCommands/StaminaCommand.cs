using System;
using System.Collections.Generic;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using FreneticScript.TagHandlers.Objects;
using FreneticScript.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class StaminaCommand : AbstractCommand
    {
        // <--[command]
        // @Name stamina
        // @Arguments <player> <amount>
        // @Short Adds to or takes from a player's stamina level.
        // @Updated 2016/04/27
        // @Authors Morphan1
        // @Group Player
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Specify an amount to adjust the player's stamina level by.
        // TODO: Explain more!
        // @Example
        // // This increases the player's stamina level.
        // stamina <{var[player]}> 20;
        // @Example
        // // This takes stamina from the player.
        // stamina <{[context].[player]}> -35;
        // -->
        public StaminaCommand()
        {
            Name = "stamina";
            Arguments = "<player> <amount>";
            Description = "Adds to or takes from a player's stamina level.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                (input) => input,
                (input) =>
                {
                    return NumberTag.TryFor(input);
                }
            };
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            try
            {
                NumberTag num = NumberTag.TryFor(entry.GetArgumentObject(queue, 1));
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
                int amount = (int)num.Internal;
                PlayerLife life = player.Internal.player.life;
                if (amount >= 0)
                {
                    life.askRest((byte)amount);
                }
                else
                {
                    life.askTire((byte)-amount);
                }
                entry.Good(queue, "Successfully adjusted the stamina level of player " + TagParser.Escape(player.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
            }
            catch (Exception ex) // TODO: Necessity?
            {
                queue.HandleError(entry, "Failed to adjust player's stamina level: " + ex.ToString());
            }
        }
    }
}
