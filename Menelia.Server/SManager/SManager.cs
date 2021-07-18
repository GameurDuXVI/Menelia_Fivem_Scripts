using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Menelia.Entities;

namespace Menelia.Server.SManager
{
    public class SManager : BaseScript
    {
        public SManager()
        {
            //EventHandlers["SManager:SpawningReady"] += new Action<int>(playerJoining);
            EventHandlers["SManager:save"] += new Action<int, float, float, float>(playerSavePosition);

            try
            {
                EventHandlers["MeneliaAPI:SpawningReady"] += new Action<int, NetworkCallbackDelegate>((serverId, callback) =>
                {
                    PlayerInfo data;

                    var player = ServerUtils.getPlayerByServerId(serverId);
                    Log.info(player.Name);

                    foreach (String identiefier in player.Identifiers.ToList())
                    {
                        Log.info("   " + identiefier);
                    }

                    if (ServerUtils.hasPlayerInfoByIdentiefiers(player.Identifiers.ToList()))
                    {
                        data = ServerUtils.getPlayerInfoByIdentiefiers(player.Identifiers.ToList());

                        var identiefiers = new List<string>();
                        foreach (var identifier in player.Identifiers.ToList())
                            if (!identifier.Contains("ip:"))
                                identiefiers.Add(identifier);

                        data.Identifiers = identiefiers;
                        Log.info("Name: " + data.Name);
                        Log.info("Data found, sending from existing data...");
                        Log.info(data.Position.X + " " + data.Position.Y + " " + data.Position.Z + " " + data.Position.Heading);
                    }
                    else
                    {
                        data = new PlayerInfo(player.Identifiers.ToList(), player.Name, -1045f, -2751.5f, 21f, 325);
                        ServerUtils.getPlayerInfos().Add(data);
                        ServerUtils.savePlayersInfo();
                        Log.info("Data not found, sending new requested data...");
                    }
                    callback.Invoke(data.toJson());
                });
            }
            catch (Exception e)
            {
                Log.error(e.StackTrace);
            }
        }

        public void playerJoining(int serverId)
        {
            PlayerInfo data;

            var player = ServerUtils.getPlayerByServerId(serverId);

            foreach (var identiefier in player.Identifiers.ToList())
            {
                Log.info("   " + identiefier);
            }

            if (ServerUtils.hasPlayerInfoByIdentiefiers(player.Identifiers.ToList()))
            {
                data = ServerUtils.getPlayerInfoByIdentiefiers(player.Identifiers.ToList());
                data.Name = player.Name;
                data.Identifiers = player.Identifiers.ToList();
                Log.info("Data found, sending from existing data...");
                Log.info(data.Position.X + " " + data.Position.Y + " " + data.Position.Z + " " + data.Position.Heading);
            }
            else
            {
                data = new PlayerInfo(player.Identifiers.ToList() , player.Name, -1045f, -2751.5f, 21f, 325);
                ServerUtils.getPlayerInfos().Add(data);
                ServerUtils.savePlayersInfo();
                Log.info("Data not found, sending new requested data...");
                
            }
            TriggerClientEvent("SManager:SpawningAction", serverId, data.Position.X, data.Position.Y, data.Position.Z, data.Position.Heading);
        }
        public static void playerSavePosition(int serverId, float x, float y, float z)
        {
            if (!ServerUtils.hasPlayerInfoByIdentiefiers(ServerUtils.getPlayerByServerId(serverId).Identifiers.ToList()))
                return;

            var p = ServerUtils.getPlayerByServerId(serverId);
            if (ServerUtils.hasPlayerInfoByIdentiefiers(p.Identifiers.ToList()))
            {
                var data = ServerUtils.getPlayerInfoByIdentiefiers(p.Identifiers.ToList());
                data.Position.X = x;
                data.Position.Y = y + 1;
                data.Position.Z = z;
            }
        }
    }    
}
