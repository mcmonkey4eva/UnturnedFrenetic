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

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class MaxHealthCommand : AbstractCommand
    {
        // <--[command]
        // @Name maxhealth
        // @Arguments <entity> <amount>
        // @Short Sets an entity's max health to the specified amount.
        // @Updated 2016/04/29
        // @Authors mcmonkey
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Sets an entity's max health to the specified amount.
        // Currently only works for zombies.
        // TODO: Explain more!
        // @Example
        // // This sets the zombie with ID 1 to have a maxium health of 100 hp.
        // maxhealth 1 100;
        // -->
        public MaxHealthCommand()
        {
            Name = "maxhealth";
            Arguments = "<entity> <amount>";
            Description = "Sets an entity's max health to the specified amount.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                IntegerTag.TryFor
            };
        }

        public override void Execute(FreneticScript.CommandSystem.CommandQueue queue, CommandEntry entry)
        {
            IntegerTag num = IntegerTag.TryFor(entry.GetArgumentObject(queue, 1));
            if (num.Internal <= 0)
            {
                queue.HandleError(entry, "Must provide a number that is greater than 0!");
                return;
            }
            EntityTag entity = EntityTag.For(entry.GetArgumentObject(queue, 0));
            if (entity == null)
            {
                queue.HandleError(entry, "Invalid entity!");
                return;
            }
            ZombieTag zombie;
            if (entity.TryGetZombie(out zombie))
            {
                Zombie inZomb = zombie.Internal;
                inZomb.maxHealth = (ushort)num.Internal;
                if (inZomb.health > inZomb.maxHealth)
                {
                    inZomb.health = inZomb.maxHealth;
                }
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully set health of a zombie to " + inZomb.maxHealth + "!");
                }
                return;
            }
            PlayerTag player;
            if (entity.TryGetPlayer(out player))
            {
                GameObject playerObj = player.Internal.player.gameObject;
                UFMHealthController controller = playerObj.GetComponent<UFMHealthController>();
                if (controller == null)
                {
                    controller = playerObj.AddComponent<UFMHealthController>();
                }
                controller.maxHealth = (uint)num.Internal;
                PlayerLife life = player.Internal.player.life;
                byte curr = life.health;
                controller.health = curr >= controller.maxHealth ? controller.maxHealth : curr;
                life._health = controller.Translate();
                life.channel.send("tellHealth", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                    life.health
                });
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully set max health of a player to " + controller.maxHealth + "!");
                }
                return;
            }
            queue.HandleError(entry, "That entity can't be healed!");
        }
    }

    public class UFMHealthController : MonoBehaviour
    {
        public uint health;
        public uint maxHealth;

        public byte Translate()
        {
            return (byte)Math.Ceiling((health / (double)maxHealth) * 100.0);
        }

        public void Damage(uint amount)
        {
            if (amount >= health)
            {
                health = 0;
            }
            else
            {
                health -= amount;
            }
        }

        public void Heal(uint amount)
        {
            if (amount >= maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health += amount;
            }
        }
    }
}
