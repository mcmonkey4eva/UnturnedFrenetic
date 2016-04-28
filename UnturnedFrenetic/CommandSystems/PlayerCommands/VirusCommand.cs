using System;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using FreneticScript.TagHandlers.Objects;
using FreneticScript.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class VirusCommand : AbstractCommand
    {
        // <--[command]
        // @Name virus
        // @Arguments <player> <amount>
        // @Short Adds to or takes from a player's virus level.
        // @Updated 2016/04/27
        // @Authors Morphan1
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Specify an amount to adjust the player's virus level by.
        // TODO: Explain more!
        // @Example
        // // This disinfects the player completely.
        // virus <{var[player]}> -100
        // @Example
        // // This infects the player greatly.
        // virus <{[context].[player]}> 50
        // -->
        public VirusCommand()
        {
            Name = "virus";
            Arguments = "<player> <amount>";
            Description = "Adds to or takes from a player's virus level.";
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
                    life.askInfect((byte)amount);
                }
                else
                {
                    life.askDisinfect((byte)-amount);
                }
                entry.Good(queue, "Successfully adjusted the virus level of player " + TagParser.Escape(player.ToString()) + " by " + TagParser.Escape(num.ToString()) + "!");
            }
            catch (Exception ex)
            {
                queue.HandleError(entry, "Failed to adjust player's virus level: " + ex.ToString());
            }
        }
    }
}
