using System;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using FreneticScript.TagHandlers.Objects;
using FreneticScript.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class OxygenCommand : AbstractCommand
    {
        // <--[command]
        // @Name oxygen
        // @Arguments <player> <amount>
        // @Short Adds to or takes from a player's oxygen level.
        // @Updated 2016/04/27
        // @Authors Morphan1
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Specify an amount to adjust the player's oxygen level by.
        // TODO: Explain more!
        // @Example
        // // This increases the player's oxygen level.
        // oxygen <{var[player]}> 20
        // @Example
        // // This takes oxygen from the player.
        // oxygen <{[context].[player]}> -35
        // -->
        public OxygenCommand()
        {
            Name = "oxygen";
            Arguments = "<player> <amount>";
            Description = "Adds to or takes from a player's oxygen level.";
            MinimumArguments = 2;
            MaximumArguments = 2;
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
                    life.askBreath((byte)amount);
                }
                else
                {
                    life.askSuffocate((byte)-amount);
                }
                entry.Good(queue, "Successfully adjusted the oxygen level of player " + TagParser.Escape(player.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
            }
            catch (Exception ex)
            {
                queue.HandleError(entry, "Failed to adjust player's oxygen level: " + ex.ToString());
            }
        }
    }
}
