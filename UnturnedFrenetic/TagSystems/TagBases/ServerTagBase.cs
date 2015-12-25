using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class ServerTagBase : TemplateTags
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

        public override string Handle(TagData data)
        {
            data.Shrink();
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
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
