using System;
using System.IO;
using System.Collections.Generic;
using CitizenFX.Core;
using Menelia.Entities;

namespace Menelia.Server
{

    public class ServerUtils
    {
        private static PlayerList _pl = new PlayerList();
        
        public static List<PlayerInfo> getPlayerInfos()
        {
            return PlayerInfo.playerInfos;
        }

        public static string loadPlayersInfo(String file = "players.json")
        {
            if (File.Exists("data/" + file))
            {
                String json = File.ReadAllText("data/" + file);
                if (json.Length > 0)
                    PlayerInfo.playerInfos = PlayerInfo.listFromJson(json);
            }
            else
            {
                savePlayersInfo();
            }
            return file;
        }

        public static string savePlayersInfo(String file = "players.json")
        {
            File.WriteAllText("data/" + file, PlayerInfo.listToJson());
            return file;
        }

        public static bool hasPlayerInfo(string name)
        {
            foreach (var playerInfo in getPlayerInfos())
            {
                if (playerInfo.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public static PlayerInfo getPlayerInfo(string name)
        {
            foreach (var playerInfo in getPlayerInfos())
            {
                if (playerInfo.Name == name)
                {
                    return playerInfo;
                }
            }
            return null;
        }

        public static bool hasPlayerInfoByIdentiefier(string identifier)
        {
            foreach (var playerInfo in getPlayerInfos())
            {
                if (playerInfo.Identifiers.Contains(identifier))
                {
                    return true;
                }
            }
            return false;
        }

        public static PlayerInfo getPlayerInfoByIdentiefier(string identifier)
        {
            foreach (var playerInfo in getPlayerInfos())
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
            foreach (string identifier in identifiers)
            {
                if (hasPlayerInfoByIdentiefier(identifier))
                {
                    return getPlayerInfoByIdentiefier(identifier);
                }
            }
            return null;
        }

        public static bool updatePlayerInfoByIdentiefiers(List<string> identifiers, PlayerInfo pi)
        {
            try
            {
                PlayerInfo.playerInfos.Remove(getPlayerInfoByIdentiefiers(identifiers));
                PlayerInfo.playerInfos.Add(pi);
                return true;
            }
            catch(Exception e)
            {
                Log.error(e.Message);
                Log.error(e.StackTrace);
                return false;
            }
        }
        
        public static void sendChatMessage(string title, string message, int r, int g, int b)
        {
            var msg = new Dictionary<string, object>
            {
                ["color"] = new[] { r, g, b },
                ["args"] = new[] { title, message }
            };
            BaseScript.TriggerClientEvent("chat:addMessage", msg);
        }

        public static Player getPlayerByServerId(int serverId)
        {
            foreach (var p in _pl)
            {
                if (p.Handle.Equals(serverId.ToString()))
                {
                    return p;
                }
            }
            return null;
        }

        public static Player getPlayerByPlayerHandle(string handle)
        {
            foreach (var p in _pl)
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
            foreach (var p in _pl)
            {
                if (p.Handle.Equals(serverId.ToString()))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool isOnlineByHandle(string handle)
        {
            foreach (var p in _pl)
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
        public static void info(string message)
        {
            var t = DateTime.Now;
            Debug.WriteLine($"\u001b[32m[INFO]\u001b[0m {t.Hour}:{t.Minute} => {message}");
        }

        public static void warn(string message)
        {
            var t = DateTime.Now;
            Debug.WriteLine($"\u001b[33m[WARN]\u001b[0m {t.Hour}:{t.Minute} => {message}");
        }
        public static void error(string message)
        {
            var t = DateTime.Now;
            Debug.WriteLine($"\u001b[31m[ERROR]\u001b[0m {t.Hour}:{t.Minute} => {message}");
        }
    }
}
