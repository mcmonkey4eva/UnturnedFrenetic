using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    class PlayerTag : TemplateObject
    {
        public SteamPlayer Internal;

        public string Name;
        
        public PlayerTag(SteamPlayer p, string name)
        {
            Internal = p;
            Name = name;
        }

        public static PlayerTag For(string name)
        {
            SteamPlayer p = PlayerTool.getSteamPlayer(name);
            if (p == null)
            {
                return null;
            }
            return new PlayerTag(p, name);
        }
        
        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                // <--[tag]
                // @Name PlayerTag.name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the name of the player.
                // @Example "bob" .name returns "bob".
                // -->
                case "name":
                    return new TextTag(Internal.playerID.playerName).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.steam_id
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the Steam ID number of the player.
                // @Example "bob" .steam_id returns "1000".
                // -->
                case "steam_id":
                    return new TextTag(Internal.playerID.steamID.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.is_pro
                // @Group General Information
                // @ReturnType TextTag
                // @Returns whether the player is considered pro.
                // @Example "bob" .is_pro returns "true".
                // -->
                case "is_pro":
                    return new TextTag(Internal.isPro).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.agro
                // @Group Status
                // @ReturnType TextTag
                // @Returns the number of zombies agro'd by this player currently.
                // @Example "bob" .agro returns "5".
                // -->
                case "agro":
                    return new TextTag(Internal.player.agro).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.health
                // @Group Status
                // @ReturnType TextTag
                // @Returns the player's current health level. Maximum health level is 100.
                // @Example "bob" .health returns "56".
                // -->
                case "health":
                    return new TextTag(Internal.player.life.health).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.food
                // @Group Status
                // @ReturnType TextTag
                // @Returns the player's current food level. Maximum food level is 100.
                // @Example "bob" .food returns "89".
                // -->
                case "food":
                    return new TextTag(Internal.player.life.food).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.water
                // @Group Status
                // @ReturnType TextTag
                // @Returns the player's current water level. Maximum water level is 100.
                // @Example "bob" .water returns "74".
                // -->
                case "water":
                    return new TextTag(Internal.player.life.water).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.virus
                // @Group Status
                // @ReturnType TextTag
                // @Returns the player's current virus level. Maximum virus level is 100.
                // @Example "bob" .virus returns "37".
                // -->
                case "virus":
                    return new TextTag(Internal.player.life.virus).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.stamina
                // @Group Status
                // @ReturnType TextTag
                // @Returns the player's current stamina level. Maximum stamina level is 100.
                // @Example "bob" .stamina returns "13".
                // -->
                case "stamina":
                    return new TextTag(Internal.player.life.stamina).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.oxygen
                // @Group Status
                // @ReturnType TextTag
                // @Returns the player's current oxygen level. Maximum oxygen level is 100.
                // @Example "bob" .oxygen returns "42".
                // -->
                case "oxygen":
                    return new TextTag(Internal.player.life.oxygen).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.vision
                // @Group Status
                // @ReturnType TextTag
                // @Returns the player's current vision.
                // @Example "bob" .vision returns "124".
                // -->
                case "vision": // TODO: find purpose, it seems to be more of a "blindness" level, as it starts from 0
                    return new TextTag(Internal.player.life.vision).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.is_bleeding
                // @Group Status
                // @ReturnType TextTag
                // @Returns whether the player is bleeding.
                // @Example "bob" .is_bleeding returns "false".
                // -->
                case "is_bleeding":
                    return new TextTag(Internal.player.life.isBleeding).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.is_broken
                // @Group Status
                // @ReturnType TextTag
                // @Returns whether the player has a broken limb.
                // @Example "bob" .is_broken returns "true".
                // -->
                case "is_broken":
                    return new TextTag(Internal.player.life.isBroken).Handle(data.Shrink());
                // <--[tag]
                // @Name PlayerTag.is_freezing
                // @Group Status
                // @ReturnType TextTag
                // @Returns whether the player is currently freezing.
                // @Example "bob" .is_freezing returns "false".
                // -->
                case "is_freezing":
                    return new TextTag(Internal.player.life.isFreezing).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.playerID.steamID.ToString();
        }
    }
}
