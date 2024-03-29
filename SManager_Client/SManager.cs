﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using MeneliaAPI.Entities;

namespace SManager_CLient
{
    public class SManager : BaseScript
    {
        private static bool _spawnLock = false;

        public SManager()
        {
            EventHandlers.Add("SManager:SpawningAction", new Action<int, float, float, float, float>(spawn));
            TriggerServerEvent("SManager:SpawningReady", GetPlayerServerId(PlayerId()));

            Tick += onTick1000;
            Tick += onTick5000;
        }
        

        public static void FreezePlayer(int playerId, bool freeze)
        {
            var ped = GetPlayerPed(playerId);

            SetPlayerControl(playerId, !freeze, 0);

            if (!freeze)
            {
                if (!IsEntityVisible(ped))
                    SetEntityVisible(ped, true, false);

                if (!IsPedInAnyVehicle(ped, true))
                    SetEntityCollision(ped, true, true);

                FreezeEntityPosition(ped, false);
                //SetCharNeverTargetted(ped, false)
                SetPlayerInvincible(playerId, false);
            }
            else
            {
                if (IsEntityVisible(ped))
                    SetEntityVisible(ped, false, false);

                SetEntityCollision(ped, false, true);
                FreezeEntityPosition(ped, true);
                //SetCharNeverTargetted(ped, true)
                SetPlayerInvincible(playerId, true);

                if (IsPedFatallyInjured(ped))
                    ClearPedTasksImmediately(ped);
            }
        }

        public async Task SpawnPlayer(string skin, float x, float y, float z, float heading)
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

            FreezePlayer(PlayerId(), true);
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

            ShutdownLoadingScreen();
            DoScreenFadeIn(500);

            while (IsScreenFadingIn())
            {
                await Delay(1);
            }

            List<object> players = GetActivePlayers();

            FreezePlayer(PlayerId(), false);            

            _spawnLock = false;
        }
        public async void spawn(int serverId, float x, float y, float z, float heading)
        {
            if (GetPlayerServerId(PlayerId()) != serverId) return;
            await SpawnPlayer("mp_m_fibsec_01", x, y, z, heading);
            // 358 -589.5 28 250
        }

        public async Task onTick1000()
        {
            await Delay(1000);

            if (Game.Player.IsDead)
            {
                await Delay(3000);
                DoScreenFadeOut(1000);
                await Delay(1000);
                await SpawnPlayer("", 358, -589.5f, 28, 250);
            }
        }

        public async Task onTick5000()
        {
            await Delay(5000);

            Vector3 v = Game.Player.Character.Position;
            TriggerServerEvent("SManager:save", GetPlayerServerId(PlayerId()), v.X, v.Y, v.Z);;
        }
    }
}
