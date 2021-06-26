using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using MeneliaAPI.Entities;
using MeneliaAPI.Server;
using static CitizenFX.Core.Native.API;

namespace SManager_Server
{
    public class SManager : BaseScript
    {
        public SManager()
        {
            EventHandlers["SManager:SpawningReady"] += new Action<int>(playerJoining);
            EventHandlers["SManager:save"] += new Action<int, float, float, float>(playerSavePosition);
        }

        public void playerJoining(int serverId)
        {
            PlayerInfo data;

            PlayerList pl = new PlayerList();
            Log.info("Players: ");
            foreach (Player p in pl)
            {
                Log.info(" - " + p.Handle + " " + p.Name);
                foreach(String identiefier in p.Identifiers.ToArray())
                {
                    Log.info("   " + identiefier);
                }
            }

            Player player = ServerPlayersUtils.getPlayerByServerId(serverId);

            if (ServerInfoUtils.hasPlayerInfoByIdentiefiers(player.Identifiers.ToList()))
            {
                data = ServerInfoUtils.getPlayerInfoByIdentiefiers(player.Identifiers.ToList());
                Log.info("Data found, sending from existing data...");
            }
            else
            {
                data = new PlayerInfo(player.Identifiers.ToList() , player.Name, -1045f, -2751.5f, 21f, 325);
                ServerInfoUtils.getPlayerInfos().Add(data);
                Log.info("Data not found, sending new requested data...");
                
            }
            TriggerClientEvent("SManager:SpawningAction", serverId, data.x, data.y, data.z, data.heading);
        }
        public static void playerSavePosition(int serverId, float x, float y, float z)
        {
            PlayerList pl = new PlayerList();

            foreach(Player p in pl)
            {
                if(ServerInfoUtils.hasPlayerInfoByIdentiefiers(p.Identifiers.ToList()))
                {
                    PlayerInfo data = ServerInfoUtils.getPlayerInfoByIdentiefiers(p.Identifiers.ToList());
                    data.x = x;
                    data.y = y;
                    data.z = z;
                }
            }
        }
    }    
}
