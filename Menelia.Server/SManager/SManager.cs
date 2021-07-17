using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Menelia.Entities;
using Menelia.Server;

namespace SManager.Server
{
    public class SManager : BaseScript
    {
        public SManager()
        {
            //EventHandlers["SManager:SpawningReady"] += new Action<int>(playerJoining);
            EventHandlers["SManager:save"] += new Action<int, float, float, float>(playerSavePosition);

            try
            {
                EventHandlers["MeneliaAPI:SpawningReady"] += new Action<int, NetworkCallbackDelegate>((ServerId, Callback) =>
                {
                    PlayerInfo data;

                    Player player = ServerUtils.getPlayerByServerId(ServerId);
                    Log.info(player.Name);

                    foreach (String identiefier in player.Identifiers.ToList())
                    {
                        Log.info("   " + identiefier);
                    }

                    if (ServerUtils.hasPlayerInfoByIdentiefiers(player.Identifiers.ToList()))
                    {
                        data = ServerUtils.getPlayerInfoByIdentiefiers(player.Identifiers.ToList());

                        List<String> Identiefiers = new List<string>();
                        foreach (String Identifier in player.Identifiers.ToList())
                            if (!Identifier.Contains("ip:"))
                                Identiefiers.Add(Identifier);

                        data.Identifiers = Identiefiers;
                        Log.info("Name: " + data.Name);
                        Log.info("Data found, sending from existing data...");
                        Log.info(data.Position.X + " " + data.Position.Y + " " + data.Position.Z + " " + data.Position.Heading);
                    }
                    else
                    {
                        data = new PlayerInfo(player.Identifiers.ToList(), player.Name, -1045f, -2751.5f, 21f, 325);
                        ServerUtils.GetPlayerInfos().Add(data);
                        ServerUtils.SavePlayersInfo();
                        Log.info("Data not found, sending new requested data...");
                    }
                    //TriggerClientEvent("SManager:SpawningAction", ServerId, data.Position.X, data.Position.Y, data.Position.Z, data.Position.Heading);
                    Callback.Invoke(data.toJson());
                });
            }
            catch (Exception e)
            {
                Log.error(e.StackTrace);
            }
        }

        public void playerJoining(int ServerId)
        {
            PlayerInfo data;

            PlayerList pl = new PlayerList();

            Player player = ServerUtils.getPlayerByServerId(ServerId);

            foreach (String identiefier in player.Identifiers.ToList())
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
                ServerUtils.GetPlayerInfos().Add(data);
                ServerUtils.SavePlayersInfo();
                Log.info("Data not found, sending new requested data...");
                
            }
            TriggerClientEvent("SManager:SpawningAction", ServerId, data.Position.X, data.Position.Y, data.Position.Z, data.Position.Heading);
        }
        public static void playerSavePosition(int ServerId, float X, float Y, float Z)
        {
            if (!ServerUtils.hasPlayerInfoByIdentiefiers(ServerUtils.getPlayerByServerId(ServerId).Identifiers.ToList()))
                return;

            Player p = ServerUtils.getPlayerByServerId(ServerId);
            /*Log.info("Save: " + p.Name);
            foreach (String iden in p.Identifiers.ToList())
            {
                Log.info("   " + iden);
            }*/
            if (ServerUtils.hasPlayerInfoByIdentiefiers(p.Identifiers.ToList()))
            {
                PlayerInfo data = ServerUtils.getPlayerInfoByIdentiefiers(p.Identifiers.ToList());
                //Log.info("   " + data.Name);
                //Log.info("   " + data.Position.X + " " + data.Position.Y + " " + data.Position.Z);
                data.Position.X = X;
                data.Position.Y = Y + 1;
                data.Position.Z = Z;
            }
        }
    }    
}
