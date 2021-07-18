using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using Menelia.Entities;

namespace Menelia.Server
{
    public class UpdateClientUtils : BaseScript
    {
        public UpdateClientUtils()
        {
            ServerUtils.loadPlayersInfo();
            EventHandlers["MeneliaAPI:UpdateClient"] += new Action<string, List<object>>((channel, objects) =>
            {
                TriggerClientEvent(channel, objects);
            });
            
            EventHandlers["MeneliaAPI:UpdateClientSpecific"] += new Action<string, int, List<object>>((channel, serverId, objects) =>
            {
                TriggerClientEvent(channel, serverId, objects);
            });

            try
            {
                // GetPlayerInfo callback function to client response
                EventHandlers["MeneliaAPI:UpdatePlayerInfo"] += new Action<String, NetworkCallbackDelegate>((json, callback) =>
                {
                    var pi = PlayerInfo.fromJson(json);
                    ServerUtils.updatePlayerInfoByIdentiefiers(pi.Identifiers, pi);
                    callback.Invoke(ServerUtils.getPlayerInfoByIdentiefiers(pi.Identifiers).toJson());
                });
            }
            catch (Exception e)
            {
                Log.error(e.Message);
                Log.error(e.StackTrace);
            }

            try
            {
                // GetPlayerInfo callback function to client response
                EventHandlers["MeneliaAPI:GetPlayerInfo"] += new Action<int, NetworkCallbackDelegate>((serverId, callback) =>
                {
                    callback.Invoke(ServerUtils.getPlayerInfoByIdentiefiers(ServerUtils.getPlayerByServerId(serverId).Identifiers.ToList()).toJson());
                });
            } catch(Exception e)
            {
                Log.error(e.Message);
                Log.error(e.StackTrace);
            }

            try
            {
                // GetPlayerInfos callback function to client response
                EventHandlers["MeneliaAPI:GetPlayerInfos"] += new Action<NetworkCallbackDelegate>((callback) =>
                {
                    callback.Invoke(PlayerInfo.listToJson());
                });
            }
            catch (Exception e)
            {
                Log.error(e.Message);
                Log.error(e.StackTrace);
            }

            try
            {
                // HasPermission callback function to client response
                EventHandlers["MeneliaAPI:HasPermission"] += new Action<int, string, NetworkCallbackDelegate>((serverId, permission, callback) =>
                {
                    callback.Invoke(API.IsPlayerAceAllowed(ServerUtils.getPlayerByServerId(serverId).Handle, permission));
                });
            }
            catch (Exception e)
            {
                Log.error(e.Message);
                Log.error(e.StackTrace);
            }

            RegisterCommand("save", new Action<int, List<object>, string>((source, args, raw) =>
            {
                // Check console
                if(source <= 0)
                {
                    try
                    {
                        string file;
                        if (args.Count > 0)
                        {
                            file = ServerUtils.savePlayersInfo((string)args[0]);
                        }
                        else
                        {
                            file = ServerUtils.savePlayersInfo();
                        }
                        Log.info($"Sauvegarde des données effectué dans {file} !");
                    }
                    catch(Exception e)
                    {
                        Log.error(e.Message);
                        Log.error(e.StackTrace);
                    }
                }
            }), true);
            
            RegisterCommand("load", new Action<int, List<object>, string>((source, args, raw) =>
            {
                // Check console
                if (source <= 0)
                {
                    try
                    {
                        string file;
                        if (args.Count > 0)
                        {
                            file = ServerUtils.loadPlayersInfo((string)args[0]);
                        }
                        else
                        {
                            file = ServerUtils.loadPlayersInfo();
                        }
                        Log.info($"Chargement des données effectué depuis {file} !");
                    }
                    catch (Exception e)
                    {
                        Log.error(e.Message);
                        Log.error(e.StackTrace);
                    }
                }
            }), true);

            // Save function
            Tick += onTick20000;
        }

        private static async Task onTick20000()
        {
            await Delay(20000);
            ServerUtils.savePlayersInfo();
        }
    }
}


