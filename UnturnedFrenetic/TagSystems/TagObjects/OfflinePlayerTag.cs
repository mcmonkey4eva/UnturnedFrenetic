using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;
using Steamworks;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class OfflinePlayerTag : TemplateObject
    {
        // <--[object]
        // @Type OfflinePlayerTag
        // @Group Server Information
        // @Description Represents a player that is currently offline.
        // -->

        public SteamPlayerID Internal;

        public OfflinePlayerTag(SteamPlayerID steamID)
        {
            Internal = steamID;
        }
        
        public static OfflinePlayerTag For(ulong steamID)
        {
            if (steamID == 0)
            {
                return null;
            }
            byte characterID = 255;
            // TODO: make more efficient? maybe cache on startup?
            foreach (string playerFolder in ReadWrite.getFolders(ServerSavedata.directory + "/Players"))
            {
                string[] split = playerFolder.Split('_');
                if (split[0] == steamID.ToString())
                {
                    characterID = Utilities.StringToByte(split[1]);
                    break;
                }

            }
            if (characterID == 255)
            {
                return null;
            }
            return For(steamID, characterID);
        }

        public static OfflinePlayerTag For(ulong steamID, byte characterID)
        {
            if (steamID == 0)
            {
                return null;
            }
            return new OfflinePlayerTag(new SteamPlayerID(new CSteamID(steamID), characterID, "&{NULL}", "&{NULL}", "&{NULL}", CSteamID.Nil));
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
                // @Name OfflinePlayerTag.steam_id
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the Steam ID number of the player.
                // @Example "1000" .steam_id returns "1000".
                // -->
                case "steam_id":
                    return new TextTag(Internal.steamID.ToString()).Handle(data.Shrink());
                // <--[tag]
                // @Name OfflinePlayerTag.health
                // @Group Status
                // @ReturnType TextTag
                // @Returns the offline player's current health level. Maximum health level is 100.
                // @Example "1000" .health returns "56".
                // -->
                case "health":
                    // TODO: make more efficient? maybe cache?
                    if (PlayerSavedata.fileExists(Internal, "/Player/Life.dat") && Level.info.type == ELevelType.SURVIVAL)
                    {
                        Block block = PlayerSavedata.readBlock(Internal, "/Player/Life.dat", 0);
                        if (block.readByte() > 1)
                        {
                            // block data:
                            // 1) byte - health
                            // 2) byte - food
                            // 3) byte - water
                            // 4) byte - virus
                            // 5) boolean - isBleeding
                            // 6) boolean - isBroken
                            return new TextTag(block.readByte()).Handle(data.Shrink());
                        }
                    }
                    return new TextTag(100).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.ToString();
        }
    }
}
