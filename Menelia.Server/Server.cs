using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using MeneliaAPI.Entities;

namespace MeneliaAPI.Server
{
    public class UpdateClientUtils : BaseScript
    {
        public UpdateClientUtils()
        {
            ServerUtils.loadPlayersInfo();
            EventHandlers["MeneliaAPI:UpdateClient"] += new Action<String, Object[]>(ReturnToClient);
            /*EventHandlers["MeneliaAPI:GetPlayerInfo"] += new Action<String, int>((ScriptName, PlayerId) =>
            {
                Player p = ServerUtils.getPlayerByServerId(PlayerId);
                PlayerInfo pi = ServerUtils.getPlayerInfoByIdentiefiers(p.Identifiers.ToList());
                TriggerClientEvent(ScriptName + ":GetPlayerInfo", pi.ToJson()); 
            });
            EventHandlers["MeneliaAPI:GetPlayersInfo"] += new Action<String>((ScriptName) =>
            {
                TriggerClientEvent(ScriptName + ":GetPlayersInfo", PlayerInfo.ListToJson());
            });*/

            // GetPlayerInfo callback function to client response
            EventHandlers["MeneliaAPI:GetPlayerInfo"] += new Action<int, NetworkCallbackDelegate>((ServerId, Callback) =>
            {
                Callback.Invoke(ServerUtils.getPlayerInfoByIdentiefiers(ServerUtils.getPlayerByServerId(ServerId).Identifiers.ToList()).ToJson());
            });

            // HasPermission callback function to client response
            EventHandlers["MeneliaAPI:HasPermission"] += new Action<int, String, NetworkCallbackDelegate>((ServerId, Permission, Callback) =>
            {
                Callback.Invoke(API.IsPlayerAceAllowed(ServerUtils.getPlayerByServerId(ServerId).Handle, Permission));
            });

            // Save function
            Tick += onTick60000;
        }

        private void ReturnToClient(String channel, Object[] objects)
        {
            TriggerClientEvent(channel, objects);
        }

        public async Task onTick60000()
        {
            await Delay(60000);
            ServerUtils.SavePlayersInfo();
        }
    }
}


