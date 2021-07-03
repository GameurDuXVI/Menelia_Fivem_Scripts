using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using MeneliaAPI.Entities;
using CitizenFX.Core.Native;

namespace MeneliaAPI.Server
{

    public class ServerUtils
    {
        public static List<PlayerInfo> GetPlayerInfos()
        {
            return PlayerInfo.playerInfos;
        }

        public static void loadPlayersInfo()
        {
            if (File.Exists("data.json"))
            {
                String json = File.ReadAllText("data.json");
                if (json.Length > 0)
                    PlayerInfo.playerInfos = PlayerInfo.ListFromJson(json);
            }
            else
            {
                SavePlayersInfo();
            }
        }

        public static void SavePlayersInfo()
        {
            File.WriteAllText("data.json", PlayerInfo.ListToJson());
        }

        public static bool hasPlayerInfo(String name)
        {
            foreach (PlayerInfo playerInfo in GetPlayerInfos())
            {
                if (playerInfo.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public static PlayerInfo getPlayerInfo(String name)
        {
            foreach (PlayerInfo playerInfo in GetPlayerInfos())
            {
                if (playerInfo.Name == name)
                {
                    return playerInfo;
                }
            }
            return null;
        }

        public static bool hasPlayerInfoByIdentiefier(String identifier)
        {
            foreach (PlayerInfo playerInfo in GetPlayerInfos())
            {
                if (playerInfo.Identifiers.Contains(identifier))
                {
                    return true;
                }
            }
            return false;
        }

        public static PlayerInfo getPlayerInfoByIdentiefier(String identifier)
        {
            foreach (PlayerInfo playerInfo in GetPlayerInfos())
            {
                if (playerInfo.Identifiers.Contains(identifier))
                {
                    return playerInfo;
                }
            }
            return null;
        }

        public static bool hasPlayerInfoByIdentiefiers(List<String> identifiers)
        {
            foreach (String identifier in identifiers)
            {
                if (hasPlayerInfoByIdentiefier(identifier))
                {
                    return true;
                }
            }
            return false;
        }

        public static PlayerInfo getPlayerInfoByIdentiefiers(List<String> identifiers)
        {
            foreach (String identifier in identifiers)
            {
                if (hasPlayerInfoByIdentiefier(identifier))
                {
                    return getPlayerInfoByIdentiefier(identifier);
                }
            }
            return null;
        }


        private static PlayerList pl = new PlayerList();
        public static void SendChatMessage(string title, string message, int r, int g, int b)
        {
            var msg = new Dictionary<string, object>
            {
                ["color"] = new[] { r, g, b },
                ["args"] = new[] { title, message }
            };
            BaseScript.TriggerEvent("chat:addMessage", msg);
        }

        public static Player getPlayerByServerId(int serverId)
        {
            foreach (Player p in pl)
            {
                if (p.Handle.Equals(serverId.ToString()))
                {
                    return p;
                }
            }
            return null;
        }

        public static Player getPlayerByPlayerHandle(String handle)
        {
            foreach (Player p in pl)
            {
                if (p.Handle.Equals(handle))
                {
                    return p;
                }
            }
            return null;
        }
        public static bool isOnline(int serverId)
        {
            foreach (Player p in pl)
            {
                if (p.Handle.Equals(serverId.ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool isOnlineByHandle(String handle)
        {
            foreach (Player p in pl)
            {
                if (p.Handle.Equals(handle))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class Log
    {
        public static void info(String message)
        {
            DateTime t = DateTime.Now;
            Debug.WriteLine($"\u001b[32m[INFO]\u001b[0m {t.Hour}:{t.Minute} => {message}");
        }

        public static void warn(String message)
        {
            DateTime t = DateTime.Now;
            Debug.WriteLine($"\u001b[33m[WARN]\u001b[0m {t.Hour}:{t.Minute} => {message}");
        }
        public static void error(String message)
        {
            DateTime t = DateTime.Now;
            Debug.WriteLine($"\u001b[31m[ERROR]\u001b[0m {t.Hour}:{t.Minute} => {message}");
        }
    }
}
