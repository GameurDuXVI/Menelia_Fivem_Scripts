using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using Menelia.Entities;

namespace Menelia.Client.SpawnManager
{
    public class SManager : BaseScript
    {
        private static bool _spawnLock;

        public SManager()
        {
            BaseScript.TriggerServerEvent("MeneliaAPI:SpawningReady", GetPlayerServerId(PlayerId()), new Action<string>(async (json) =>
            {
                try
                {
                    var pi = PlayerInfo.fromJson(json);
                    await spawnPlayer("mp_m_fibsec_01", pi.Position.X, pi.Position.Y, pi.Position.Z, pi.Position.Heading);
                }
                catch (Exception e)
                {
                    Screen.ShowNotification("Un erreur est survenue !");
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine(e.StackTrace);
                }
            }));

            Tick += onTick1000;
            Tick += onTick30000;
        }
        

        public static void freezePlayer(int playerId, bool freeze)
        {
            var ped = GetPlayerPed(playerId);

            SetPlayerControl(playerId, !freeze, 0);

            if (!freeze)
            {
                if (!IsEntityVisible(ped))
                    SetEntityVisible(ped, true, false);

                if (!IsPedInAnyVehicle(ped, true))
                    SetEntityCollision(ped, true, true);

                SetCanAttackFriendly(PlayerPedId(), true, false);
                NetworkSetFriendlyFireOption(true);
                FreezeEntityPosition(ped, false);
                //SetCharNeverTargetted(ped, false)
                SetPlayerInvincible(playerId, false);
                SetEntityInvincible(ped, false);
            }
            else
            {
                if (IsEntityVisible(ped))
                    SetEntityVisible(ped, false, false);

                SetCanAttackFriendly(PlayerPedId(), false, false);
                NetworkSetFriendlyFireOption(false);
                SetEntityCollision(ped, false, true);
                FreezeEntityPosition(ped, true);
                //SetCharNeverTargetted(ped, true)
                SetPlayerInvincible(playerId, true);
                SetEntityInvincible(ped, true);

                if (IsPedFatallyInjured(ped))
                    ClearPedTasksImmediately(ped);
            }
        }

        private static async Task spawnPlayer(string skin, float x, float y, float z, float heading)
        {
            await Delay(0);
            if (_spawnLock)
                return;

            _spawnLock = true;

            DoScreenFadeOut(500);

            while (IsScreenFadingOut())
            {
                await Delay(1);
            }

            freezePlayer(PlayerId(), true);
            //await Game.Player.ChangeModel(GetHashKey(skin));
            SetPedDefaultComponentVariation(GetPlayerPed(-1));
            RequestCollisionAtCoord(x, y, z);

            var ped = GetPlayerPed(-1);

            SetEntityCoordsNoOffset(ped, x, y, z, false, false, false);
            NetworkResurrectLocalPlayer(x, y, z, heading, true, true);
            ClearPedTasksImmediately(ped);
            ClearPlayerWantedLevel(PlayerId());

            while (!HasCollisionLoadedAroundEntity(ped))
            {
                await Delay(1);
            }

            DoScreenFadeIn(500);

            while (IsScreenFadingIn())
            {
                await Delay(1);
            }

            freezePlayer(PlayerId(), false);

            await Delay(500);

            ShutdownLoadingScreen();

            _spawnLock = false;
        }

        private async Task onTick1000()
        {
            await Delay(1000);

            if (Game.Player.IsDead)
            {
                await Delay(3000);
                DoScreenFadeOut(1000);
                await Delay(1000);
                await spawnPlayer("", 358, -589.5f, 28, 250);
            }
        }

        private async Task onTick30000()
        {
            await Delay(30000);

            var v = Game.Player.Character.Position;
            TriggerServerEvent("SManager:save", GetPlayerServerId(PlayerId()), v.X, v.Y, v.Z);
        }
    }
}
