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
                // @Returns the name of the player.
                // @Example "bob" .steam_id returns "1000".
                // -->
                case "steam_id":
                    return new TextTag(Internal.playerID.steamID.ToString()).Handle(data.Shrink());
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
