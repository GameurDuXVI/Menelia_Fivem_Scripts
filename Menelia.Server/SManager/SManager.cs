using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using MeneliaAPI.Entities;
using MeneliaAPI.Server;
using static CitizenFX.Core.Native.API;

namespace SManager.Server
{
    public class SManager : BaseScript
    {
        public SManager()
        {
            EventHandlers["SManager:SpawningReady"] += new Action<int>(playerJoining);
            EventHandlers["SManager:save"] += new Action<int, float, float, float>(playerSavePosition);
        }

        public void playerJoining(int ServerId)
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

            Player player = ServerUtils.getPlayerByServerId(ServerId);

            if (ServerUtils.hasPlayerInfoByIdentiefiers(player.Identifiers.ToList()))
            {
                data = ServerUtils.getPlayerInfoByIdentiefiers(player.Identifiers.ToList());
                Log.info("Data found, sending from existing data...");
            }
            else
            {
                data = new PlayerInfo(player.Identifiers.ToList() , player.Name, -1045f, -2751.5f, 21f, 325);
                ServerUtils.GetPlayerInfos().Add(data);
                ServerUtils.SavePlayersInfo();
                Log.info("Data not found, sending new requested data...");
                
            }
            TriggerClientEvent("SManager:SpawningAction", ServerId, data.Position.X, data.Position.Y, data.Position.Z, data.Position.Heading);
        }
        public static void playerSavePosition(int ServerId, float X, float Y, float Z)
        {
            PlayerList pl = new PlayerList();

            if (!ServerUtils.hasPlayerInfoByIdentiefiers(ServerUtils.getPlayerByServerId(ServerId).Identifiers.ToList()))
                return;

            foreach(Player p in pl)
            {
                if(ServerUtils.hasPlayerInfoByIdentiefiers(p.Identifiers.ToList()))
                {
                    PlayerInfo data = ServerUtils.getPlayerInfoByIdentiefiers(p.Identifiers.ToList());
                    data.Position.X = X;
                    data.Position.Y = Y + 1;
                    data.Position.Z = Z;
                }
            }
        }
    }    
}
