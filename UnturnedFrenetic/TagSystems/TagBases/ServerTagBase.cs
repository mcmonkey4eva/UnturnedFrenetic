using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class ServerTagBase : TemplateTagBase
    {
        // <--[tagbase]
        // @Base server
        // @Group Server Information
        // @ReturnType ServerTag
        // @Returns a server class full of specific helpful server tags,
        // such as <@link tag ServerTag.online_players><{server.online_players}><@/link>.
        // -->
        public ServerTagBase()
        {
            Name = "server";
        }

        public override TemplateObject Handle(TagData data)
        {
            data.Shrink();
            if (data.Remaining == 0)
            {
                return new TextTag(ToString());
            }
            switch (data[0])
            {
                // <--[tag]
                // @Name ServerTag.item_assets
                // @Group Server Information
                // @ReturnType ListTag
                // @Returns a list of all item assets in the game.
                // -->
                case "item_assets":
                    {
                        List<TemplateObject> items = new List<TemplateObject>();
                        foreach (ItemAsset it in ItemAssetTag.Items)
                        {
                            items.Add(new ItemAssetTag(it));
                        }
                        return new ListTag(items).Handle(data.Shrink());
                    }
                // <--[tag]
                // @Name ServerTag.online_players
                // @Group Server Information
                // @ReturnType ListTag
                // @Returns a list of all online players.
                // -->
                case "online_players":
                    {
                        List<TemplateObject> players = new List<TemplateObject>();
                        foreach (SteamPlayer player in Provider.clients)
                        {
                            players.Add(new PlayerTag(player));
                        }
                        return new ListTag(players).Handle(data.Shrink());
                    }
                // <--[tag]
                // @Name ServerTag.offline_players
                // @Group Server Information
                // @ReturnType ListTag
                // @Returns a list of all offline players.
                // @Other This specifically excludes any online players.
                // -->
                case "offline_players":
                    {
                        List<TemplateObject> players = new List<TemplateObject>();
                        foreach (string playerFolder in ReadWrite.getFolders(ServerSavedata.directory + "/" + Provider.serverID + "/Players"))
                        {
                            string[] split = playerFolder.Substring(playerFolder.LastIndexOf('\\') + 1).Split('_');
                            if (split.Length == 2)
                            {
                                ulong steamID = Utilities.StringToULong(split[0]);
                                bool online = false;
                                foreach (SteamPlayer onlinePlayer in Provider.clients)
                                {
                                    if (onlinePlayer.playerID.steamID.m_SteamID == steamID)
                                    {
                                        online = true;
                                        break;
                                    }
                                }
                                if (!online)
                                {
                                    OfflinePlayerTag player = OfflinePlayerTag.For(Utilities.StringToULong(split[0]), Utilities.StringToByte(split[1]));
                                    if (player != null)
                                    {
                                        players.Add(player);
                                    }
                                }
                            }
                        }
                        return new ListTag(players).Handle(data.Shrink());
                    }
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }
    }
}
