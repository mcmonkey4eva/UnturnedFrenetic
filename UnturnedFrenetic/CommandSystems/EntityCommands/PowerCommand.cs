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
    class PowerCommand : AbstractCommand
    {
        // <--[command]
        // @Name power
        // @Arguments <entity> <boolean>
        // @Short Toggles a powerable entity on or off.
        // @Updated 2016/05/01
        // @Authors Morphan1
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Enables or disables a powerable barricade.
        // TODO: Explain more!
        // @Example
        // // This turns off the spotlight with ID 1.
        // power 1 false;
        // -->
        public PowerCommand()
        {
            Name = "power";
            Arguments = "<entity> <boolean>";
            Description = "Toggles a powerable entity on or off.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                BooleanTag.TryFor
            };
        }

        public override void Execute(FreneticScript.CommandSystem.CommandQueue queue, CommandEntry entry)
        {
            bool enable = BooleanTag.TryFor(entry.GetArgumentObject(queue, 1)).Internal;
            EntityTag entity = EntityTag.For(entry.GetArgumentObject(queue, 0));
            if (entity == null)
            {
                queue.HandleError(entry, "Invalid entity!");
                return;
            }
            BarricadeTag barricadeTag;
            byte x;
            byte y;
            ushort plant;
            ushort index;
            if (entity.TryGetBarricade(out barricadeTag, out x, out y, out plant, out index))
            {
                UnityEngine.GameObject obj = barricadeTag.Internal.gameObject;
                InteractableDoor door = obj.GetComponent<InteractableDoor>();
                if (door != null)
                {
                    SendMessage("tellToggleDoor", x, y, plant, index, !door.isOpen);
                    barricadeTag.InternalData.barricade.state[16] = (byte)(!door.isOpen ? 0 : 1);
                    return;
                }
                InteractableFire fire = obj.GetComponent<InteractableFire>();
                if (fire != null)
                {
                    SendMessage("tellToggleFire", x, y, plant, index, !fire.isLit);
                    barricadeTag.InternalData.barricade.state[0] = (byte)(!fire.isLit ? 0 : 1);
                    return;
                }
                InteractableGenerator generator = obj.GetComponent<InteractableGenerator>();
                if (generator != null)
                {
                    SendMessage("tellToggleGenerator", x, y, plant, index, !generator.isPowered);
                    barricadeTag.InternalData.barricade.state[0] = (byte)(!generator.isPowered ? 0 : 1);
                    EffectManager.sendEffect(8, EffectManager.SMALL, barricadeTag.Internal.position);
                    return;
                }
                InteractableSafezone safezone = obj.GetComponent<InteractableSafezone>();
                if (generator != null)
                {
                    SendMessage("tellToggleSafezone", x, y, plant, index, !safezone.isPowered);
                    barricadeTag.InternalData.barricade.state[0] = (byte)(!safezone.isPowered ? 0 : 1);
                    EffectManager.sendEffect(8, EffectManager.SMALL, barricadeTag.Internal.position);
                    return;
                }
                InteractableSpot spot = obj.GetComponent<InteractableSpot>();
                if (spot != null)
                {
                    SendMessage("tellToggleSpot", x, y, plant, index, !spot.isPowered);
                    barricadeTag.InternalData.barricade.state[0] = (byte)(!spot.isPowered ? 0 : 1);
                    EffectManager.sendEffect(8, EffectManager.SMALL, barricadeTag.Internal.position);
                    return;
                }
            }
            queue.HandleError(entry, "That entity isn't powerable!");
        }

        private static void SendMessage(string name, byte x, byte y, ushort plant, ushort index, bool powered)
        {
            BarricadeManager.manager.channel.send(name, ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
            {
                x,
                y,
                plant,
                index,
                powered
            });
        }
    }
}
