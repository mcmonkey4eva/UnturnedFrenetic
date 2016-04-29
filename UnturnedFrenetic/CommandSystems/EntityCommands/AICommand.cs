using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using SDG.Unturned;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    class AICommand : AbstractCommand
    {
        // <--[command]
        // @Name ai
        // @Arguments <entity> <boolean>
        // @Short Enables or disables AI for an entity.
        // @Updated 2016/04/28
        // @Authors mcmonkey
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Enables or disables AI for an entity.
        // TODO: Explain more!
        // @Example
        // // This makes the animal with ID 1 no longer have AI.
        // ai 1 false;
        // -->
        public AICommand()
        {
            Name = "ai";
            Arguments = "<entity> <boolean>";
            Description = "Enables or disables AI for an entity.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                (input) => input,
                (input) =>
                {
                    return BooleanTag.TryFor(input);
                }
            };
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            bool enable = BooleanTag.TryFor(entry.GetArgumentObject(queue, 1)).Internal;
            EntityTag entity = EntityTag.For(Utilities.StringToInt(entry.GetArgument(queue, 0)));
            if (entity == null)
            {
                queue.HandleError(entry, "Invalid entity!");
                return;
            }
            ZombieTag zombie;
            if (entity.TryGetZombie(out zombie))
            {
                zombie.Internal.UFM_AIDisabled = !enable;
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "AI for a zombie " + (enable ? "enabled!" : "disabled!"));
                }
                return;
            }
            AnimalTag animal;
            if (entity.TryGetAnimal(out animal))
            {
                animal.Internal.UFM_AIDisabled = !enable;
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "AI for an animal " + (enable ? "enabled!" : "disabled!"));
                }
                return;
            }
            queue.HandleError(entry, "That entity doesn't have AI!");
        }
    }
}
