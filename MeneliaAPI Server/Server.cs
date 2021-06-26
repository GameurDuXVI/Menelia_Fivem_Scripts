using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MeneliaAPI.Entities;

namespace MeneliaAPI.Server
{
    public class UpdateClientUtils : BaseScript
    {
        public UpdateClientUtils()
        {
            EventHandlers["MeneliaAPI:UpdateClient"] += new Action<String, Object[]>(returnToClient);
        }

        private void returnToClient(String channel, Object[] objects)
        {
            TriggerClientEvent(channel, objects);
        }
    }
    public class ServerInfoUtils
    {
        private static List<PlayerInfo> playerInfos = new List<PlayerInfo>();

        public static List<PlayerInfo> getPlayerInfos()
        {
            return playerInfos;
        }

        public static void save()
        {

        }

        public static bool hasPlayerInfo(String name)
        {
            foreach (PlayerInfo playerInfo in playerInfos)
            {
                if (playerInfo.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public static PlayerInfo getPlayerInfo(String name)
        {
            foreach (PlayerInfo playerInfo in playerInfos)
            {
                if (playerInfo.name == name)
                {
                    return playerInfo;
                }
            }
            return null;
        }

        public static bool hasPlayerInfoByIdentiefier(String identifier)
        {
            foreach (PlayerInfo playerInfo in playerInfos)
            {
                if (playerInfo.identifiers.Contains(identifier))
                {
                    return true;
                }
            }
            return false;
        }

        public static PlayerInfo getPlayerInfoByIdentiefier(String identifier)
        {
            foreach (PlayerInfo playerInfo in playerInfos)
            {
                if (playerInfo.identifiers.Contains(identifier))
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
    }

    public class ServerPlayersUtils
    {
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
        public static bool isOnline(String name)
        {
            foreach (Player p in pl)
            {
                if (p.Name.Equals(name))
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


