using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript.TagHandlers;
using UnityEngine;
using SDG.Unturned;
using FreneticScript.TagHandlers.Objects;
using Steamworks;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class AnimateCommand : AbstractCommand
    {
        // <--[command]
        // @Name animate
        // @Arguments <entity> <animation>
        // @Short Causes an entity to play an animation.
        // @Updated 2016/04/30
        // @Authors mcmonkey
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // This causes an entity to play an animation.
        // TODO: Explain more!
        // @Example
        // // This causes the player to salute.
        // animate <{var[player]}> salute;
        // -->
        public AnimateCommand()
        {
            Name = "animate";
            Arguments = "<entity> <animation>";
            Description = "Causes an entity to play an animation.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                TextTag.For
            };
        }

        public override void Execute(CommandQueue queue, CommandEntry entry)
        {
            string animation = entry.GetArgument(queue, 1).ToUpperInvariant();
            EntityTag entity = EntityTag.For(entry.GetArgumentObject(queue, 0));
            if (entity == null)
            {
                queue.HandleError(entry, "Invalid entity!");
                return;
            }
            PlayerTag player;
            if (entity.TryGetPlayer(out player))
            {
                try
                {
                    player.Internal.player.animator.sendGesture((EPlayerGesture)Enum.Parse(typeof(EPlayerGesture), animation), true);
                    //player.Internal.player.animator.askGesture(player.Internal.playerID.steamID, (byte)((EPlayerGesture)Enum.Parse(typeof(EPlayerGesture), animation)));
                }
                catch (ArgumentException)
                {
                    queue.HandleError(entry, "Invalid animation specified!");
                    return;
                }
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Animated a player!");
                }
                return;
            }
            ZombieTag zombie;
            if (entity.TryGetZombie(out zombie))
            {
                int ind = animation.IndexOf('_');
                string after = animation.Substring(ind + 1);
                animation = animation.Substring(0, ind);
                if (animation == "STARTLE")
                {
                    ZombieManager.sendZombieStartle(zombie.Internal, (byte)IntegerTag.TryFor(after).Internal);
                }
                else if (animation == "STUN")
                {
                    ZombieManager.sendZombieStun(zombie.Internal, (byte)IntegerTag.TryFor(after).Internal);
                }
                else if (animation == "ATTACK")
                {
                    ZombieManager.sendZombieAttack(zombie.Internal, (byte)IntegerTag.TryFor(after).Internal);
                }
                else
                {
                    queue.HandleError(entry, "Invalid animation specified!");
                    return;
                }
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Animated an animal!");
                }
                return;
            }
            AnimalTag animal;
            if (entity.TryGetAnimal(out animal))
            {
                if (animation == "STARTLE")
                {
                    AnimalManager.sendAnimalStartle(animal.Internal);
                }
                else if (animation == "PANIC")
                {
                    AnimalManager.sendAnimalPanic(animal.Internal);
                }
                else if (animation == "ATTACK")
                {
                    AnimalManager.sendAnimalAttack(animal.Internal);
                }
                else
                {
                    queue.HandleError(entry, "Invalid animation specified!");
                    return;
                }
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Animated an animal!");
                }
                return;
            }
            queue.HandleError(entry, "That entity can't be animated!");
        }
    }
}
