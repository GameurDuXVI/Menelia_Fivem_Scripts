using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Menelia.Client.CallbackClasses;
using Menelia.Entities;
using Menelia.Client.SpawnManager;

namespace Menelia.Client
{
    public class ClientUtils : BaseScript
    {
        private static ClientUtils _instance;

        private ClientUtils() {
            _instance = this;
        }

        public static ClientUtils getInstance()
        {
            if(_instance == null)
            {
                _instance = new ClientUtils();
            }
            return _instance;
        }
        public static void sendToAllCLients(string channel, params object[] objects)
        {
            TriggerServerEvent("MeneliaAPI:UpdateClient", channel, objects.ToList());
        }
        
        public static void sendToSecificPlayer(string channel, int serverId, params object[] objects)
        {
            TriggerServerEvent("MeneliaAPI:UpdateClientSpecific", channel, serverId, objects.ToList());
        }

        public static async Task<string> updatePlayerInfo(PlayerInfo pi)
        {
            var p = new PlayerInfoCallback();
            TriggerServerEvent("MeneliaAPI:UpdatePlayerInfo", pi.toJson(), new Action<string>((json) =>
            {
                p.Json = json;
                p.HasResponse = true;
            }));
            var i = 0;
            while (true)
            {
                i++;
                if (p.HasResponse || i > 1000)
                    break;
                await Delay(50);
            }
            return p.Json;
        }
        public static async Task<string> getPlayerInfo()
        {
            var p = new PlayerInfoCallback();
            TriggerServerEvent("MeneliaAPI:GetPlayerInfo", GetPlayerServerId(PlayerId()), new Action<string>((json) =>
            {
                p.Json = json;
                p.HasResponse = true;
            }));
            var i = 0;
            while (true)
            {
                i++;
                if (p.HasResponse || i > 1000)
                    break;
                await Delay(50);
            }
            return p.Json;
        }

        public static async Task<string> getPlayerInfos()
        {
            var p = new PlayerInfoCallback();
            TriggerServerEvent("MeneliaAPI:GetPlayerInfos", new Action<string>((json) =>
            {
                p.Json = json;
                p.HasResponse = true;
            }));
            var i = 0;
            while (true)
            {
                i++;
                if (p.HasResponse || i > 1000)
                    break;
                await Delay(50);
            }
            return p.Json;
        }

        public static async Task<bool> hasPermission(string permission)
        {
            var p = new PermissionCallback();
            TriggerServerEvent("MeneliaAPI:HasPermission", GetPlayerServerId(PlayerId()), permission, new Action<bool>((hasPermission) =>
            {
                p.Permission = hasPermission;
                p.HasResponse = true;
            }));
            var i = 0;
            while (true)
            {
                i++;
                if (p.HasResponse || i > 1000)
                    break;
                await Delay(50);
            }
            return p.Permission;
        }
        public static void sendChatMessage(string title, string message, int r, int g, int b)
        {
            var msg = new Dictionary<string, object>
            {
                ["color"] = new[] { r, g, b },
                ["args"] = new[] { title, message }
            };
            TriggerEvent("chat:addMessage", msg);
        }

        public static async void teleport(float x, float y, float z, float heading)
        {
            DoScreenFadeOut(500);

            while (IsScreenFadingOut())
            {
                await BaseScript.Delay(1);
            }

            SManager.freezePlayer(PlayerId(), true);
            SetPedDefaultComponentVariation(GetPlayerPed(-1));
            RequestCollisionAtCoord(x, y, z);

            var ped = GetPlayerPed(-1);

            SetEntityCoordsNoOffset(ped, x, y, z, false, false, false);
            NetworkResurrectLocalPlayer(x, y, z, heading, true, true);
            ClearPedTasksImmediately(ped);
            ClearPlayerWantedLevel(PlayerId());

            while (!HasCollisionLoadedAroundEntity(ped))
            {
                await BaseScript.Delay(1);
            }

            DoScreenFadeIn(500);

            while (IsScreenFadingIn())
            {
                await BaseScript.Delay(1);
            }

            SManager.freezePlayer(PlayerId(), false);
        }
    }
}

