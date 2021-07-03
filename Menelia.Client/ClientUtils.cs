using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Menelia.Client.CallbackClasses;

namespace MeneliaAPI.Client
{
    public class ClientUtils : BaseScript
    {
        private static ClientUtils instance;

        public ClientUtils() {
            instance = this;
        }

        public static ClientUtils GetInstance()
        {
            if(instance == null)
            {
                instance = new ClientUtils();
            }
            return instance;
        }
        public static void SendToAllCLients(String channel, params Object[] objects)
        {
            TriggerServerEvent("MeneliaAPI:UpdateClient", channel, objects);
        }

        public static async Task<String> GetPlayerInfo()
        {
            PlayerInfoCallback p = new PlayerInfoCallback();
            BaseScript.TriggerServerEvent("MeneliaAPI:GetPlayerInfo", GetPlayerServerId(PlayerId()), new Action<String>((Json) =>
            {
                p.Json = Json;
                p.HasResponse = true;
            }));
            int i = 0;
            while (true)
            {
                i++;
                if (p.HasResponse || i > 1000)
                    break;
                await BaseScript.Delay(50);
            }
            return p.Json;
        }

        public static async Task<bool> HasPermission(String Permission)
        {
            PermissionCallback p = new PermissionCallback();
            BaseScript.TriggerServerEvent("MeneliaAPI:HasPermission", GetPlayerServerId(PlayerId()), Permission, new Action<bool>((HasPermission) =>
            {
                p.Permission = HasPermission;
                p.HasResponse = true;
            }));
            int i = 0;
            while (true)
            {
                i++;
                if (p.HasResponse || i > 1000)
                    break;
                await BaseScript.Delay(50);
            }
            return p.Permission;
        }
        public static void SendChatMessage(string title, string message, int r, int g, int b)
        {
            var msg = new Dictionary<string, object>
            {
                ["color"] = new[] { r, g, b },
                ["args"] = new[] { title, message }
            };
            TriggerEvent("chat:addMessage", msg);
        }
    }
}

