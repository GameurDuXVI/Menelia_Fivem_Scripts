using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using static CitizenFX.Core.Native.API;
using MeneliaAPI.Entities;

namespace MeneliaAPI.Server
{
    public class UpdateClientUtils : BaseScript
    {
        public UpdateClientUtils()
        {
            ServerUtils.loadPlayersInfo();
            EventHandlers["MeneliaAPI:UpdateClient"] += new Action<String, List<Object>>(ReturnToClient);

            try
            {
                // GetPlayerInfo callback function to client response
                EventHandlers["MeneliaAPI:UpdatePlayerInfo"] += new Action<String, NetworkCallbackDelegate>((Json, Callback) =>
                {
                    PlayerInfo pi = PlayerInfo.FromJson(Json);
                    ServerUtils.UpdatePlayerInfoByIdentiefiers(pi.Identifiers, pi);
                    Callback.Invoke(ServerUtils.getPlayerInfoByIdentiefiers(pi.Identifiers).ToJson());
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
                EventHandlers["MeneliaAPI:GetPlayerInfo"] += new Action<int, NetworkCallbackDelegate>((ServerId, Callback) =>
                {
                    Callback.Invoke(ServerUtils.getPlayerInfoByIdentiefiers(ServerUtils.getPlayerByServerId(ServerId).Identifiers.ToList()).ToJson());
                });
            } catch(Exception e)
            {
                Log.error(e.Message);
                Log.error(e.StackTrace);
            }

            try
            {
                // GetPlayerInfos callback function to client response
                EventHandlers["MeneliaAPI:GetPlayerInfos"] += new Action<NetworkCallbackDelegate>((Callback) =>
                {
                    Callback.Invoke(PlayerInfo.ListToJson());
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
                EventHandlers["MeneliaAPI:HasPermission"] += new Action<int, String, NetworkCallbackDelegate>((ServerId, Permission, Callback) =>
                {
                    Callback.Invoke(API.IsPlayerAceAllowed(ServerUtils.getPlayerByServerId(ServerId).Handle, Permission));
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
                        String file;
                        if (args.Count > 0)
                        {
                            file = ServerUtils.SavePlayersInfo((String)args[0]);
                        }
                        else
                        {
                            file = ServerUtils.SavePlayersInfo();
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
                        String file;
                        if (args.Count > 0)
                        {
                            file = ServerUtils.loadPlayersInfo((String)args[0]);
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

        private void ReturnToClient(String channel, List<Object> objects)
        {
            TriggerClientEvent(channel, objects);
        }

        public async Task onTick20000()
        {
            await Delay(20000);
            ServerUtils.SavePlayersInfo();
        }
    }
}


