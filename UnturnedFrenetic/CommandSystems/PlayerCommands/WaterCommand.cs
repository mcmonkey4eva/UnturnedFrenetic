using System;
using System.Collections.Generic;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using FreneticScript.TagHandlers.Objects;
using FreneticScript.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class WaterCommand : AbstractCommand
    {
        // <--[command]
        // @Name water
        // @Arguments <player> <amount>
        // @Short Adds to or takes from a player's water level.
        // @Updated 2016/04/27
        // @Authors Morphan1
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Specify an amount to adjust the player's water level by.
        // TODO: Explain more!
        // @Example
        // // This makes the player very thirsty.
        // water <{var[player]}> -50
        // @Example
        // // This quenches some of the player's thirst.
        // water <{[context].[player]}> 25
        // -->
        public WaterCommand()
        {
            Name = "water";
            Arguments = "<player> <amount>";
            Description = "Adds to or takes from a player's water level.";
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
                    life.askDrink((byte)amount);
                }
                else
                {
                    life.askDehydrate((byte)-amount);
                }
                entry.Good(queue, "Successfully adjusted the water level of player " + TagParser.Escape(player.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
            }
            catch (Exception ex)
            {
                queue.HandleError(entry, "Failed to adjust player's water level: " + ex.ToString());
            }
        }
    }
}
