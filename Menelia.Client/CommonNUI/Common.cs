using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;
using MeneliaAPI.Client;

namespace Menelia.Client.CommonNUI
{
    class Common : BaseScript
    {
        bool InVehicle = false;
        public Common()
        {
            Tick += onTick500;
            Tick += BeltTick;

            RegisterCommand("engine", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (IsInVehicle())
                {
                    var veh = Game.PlayerPed.CurrentVehicle;
                    if (veh.IsEngineRunning && veh.Speed < 5.0f)
                    {
                        veh.IsEngineRunning = false;
                        veh.IsEngineStarting = false;
                        Screen.ShowNotification("~r~❌ Moteur éteint");
                    }
                    else if (!veh.IsEngineRunning)
                    {
                        veh.IsEngineStarting = true;
                        Screen.ShowNotification("~g~🔑 Moteur allumé");
                    }
                }
            }), false);
            RegisterCommand("belt", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (IsInVehicle())
                {
                    beltOn = !beltOn;
                    if (beltOn)
                        Screen.ShowNotification("~g~✅ Ceinture attachée");
                    else
                        Screen.ShowNotification("~r~❌ Ceinture enlevée");
                }
            }), false);
            RegisterCommand("lock", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (IsInVehicle())
                {
                    var veh = Game.PlayerPed.CurrentVehicle;
                    if (veh.LockStatus != VehicleLockStatus.Locked && veh.LockStatus != VehicleLockStatus.LockedForPlayer && veh.LockStatus != VehicleLockStatus.StickPlayerInside)
                    {
                        veh.LockStatus = VehicleLockStatus.StickPlayerInside;
                        Screen.ShowNotification("~g~🔐 Portières bloquées");
                    }
                    else
                    {
                        veh.LockStatus = VehicleLockStatus.Unlocked;
                        Screen.ShowNotification("~r~🔓 Portière ouvertes");
                    }
                }
            }), false);
            RegisterCommand("right_indicator", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (IsInVehicle())
                {
                    var veh = Game.PlayerPed.CurrentVehicle;
                    IndicatorR = !IndicatorR;
                    IndicatorL = false;
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 1, IndicatorL);
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 0, IndicatorR);
                }
            }), false);
            RegisterCommand("left_indicator", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (IsInVehicle())
                {
                    var veh = Game.PlayerPed.CurrentVehicle;
                    IndicatorL = !IndicatorL;
                    IndicatorR = false;
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 1, IndicatorL);
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 0, IndicatorR);
                }
            }), false);
            RegisterCommand("up_indicator", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (IsInVehicle())
                {
                    var veh = Game.PlayerPed.CurrentVehicle;
                    if(IndicatorL && IndicatorR)
                    {
                        IndicatorL = false;
                        IndicatorR = false;
                    }
                    else
                    {
                        IndicatorL = true;
                        IndicatorR = true;
                    }
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 1, IndicatorL);
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 0, IndicatorR);
                }
            }), false);
            ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 1, false);
            ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 0, IndicatorR);


            RegisterKeyMapping("engine", "[Veh] Démarer/Éteindre le moteur", "keyboard", "i");
            RegisterKeyMapping("belt", "[Veh] Mettre/Enlever la ceinture", "keyboard", "b");
            RegisterKeyMapping("lock", "[Veh] Vérouiller/Devérouiller le vehicule", "keyboard", "l");
            RegisterKeyMapping("left_indicator", "[Veh] Clignotant Gauche", "keyboard", "left");
            RegisterKeyMapping("right_indicator", "[Veh] Clignotant Droit", "keyboard", "right");
            RegisterKeyMapping("up_indicator", "[Veh] Feux de détresse", "keyboard", "up");

            EventHandlers["CommonNUI:ChangeVehicleIndicator"] += new Action<List<object>>((args) =>
            {
                int Veh = GetVehiclePedIsIn(GetPlayerPed(GetPlayerFromServerId((int)args[0])), false);
                SetVehicleIndicatorLights(Veh, (int)args[1], (bool)args[2]);
            });
        }

        public async Task onTick500()
        {
            await Delay(150);
            if (IsInVehicle() != InVehicle)
            {
                InVehicle = IsInVehicle();
                SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"show\", \"data\": \"" + InVehicle + "\"}");
            }

            if (Game.Player.IsDead)
            {
                SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"show\", \"data\": \"" + false + "\"}");
                SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"belt\", \"data\": \"" + beltOn + "\"}");
                //beltOn = false;
            }

            if (InVehicle)
            {
                var veh = Game.PlayerPed.CurrentVehicle;
                /*if (beltOn)
                {
                    // Disables INPUT_VEH_EXIT // default: "F"
                    DisableControlAction(0, 75, true);
                }*/
                //SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"lights\", \"data\": \"" + veh.at + "\"}");
                
                if (veh.IsEngineRunning)
                {
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"fuel\", \"data\": \"" + veh.FuelLevel + "\"}");
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"lights\", \"data\": \"" + veh.AreLightsOn + "\"}");
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"lock\", \"data\": \"" + (!(veh.LockStatus != VehicleLockStatus.Locked && veh.LockStatus != VehicleLockStatus.LockedForPlayer)) + "\"}");
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"brakes\", \"data\": \"" + veh.IsHandbrakeForcedOn + "\"}");
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"speed\", \"data\": \"" + veh.Speed + "\"}");
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"belt\", \"data\": \"" + beltOn + "\"}");
                }
                else
                {
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"engine\", \"data\": \"" + veh.IsEngineRunning + "\"}");
                }

                /*int _ped = GetPlayerPed(-1);
                int _vehicle = GetVehiclePedIsIn(_ped, false);

                speedBuffer[1] = speedBuffer[0];
                speedBuffer[0] = GetEntitySpeed(_vehicle);

                if (speedBuffer[1] > 0.0f &&
                    !beltOn &&
                    GetEntitySpeedVector(_vehicle, true).Y > 1.0f &&
                    speedBuffer[0] > 15.0f &&               // m/s which comes out to approx 34mph
                    (speedBuffer[1] - speedBuffer[0]) > (speedBuffer[0] * 0.255f))
                {
                    var coords = GetEntityCoords(_ped, true);
                    var fw = ForwardVelocity(_ped);
                    SetEntityCoords(_ped, coords.X + fw[0], coords.Y + fw[1], coords.Z - 0.47f, true, true, true, true);
                    SetEntityVelocity(_ped, velBuffer[1].X, velBuffer[1].Y, velBuffer[1].Z);
                    await Delay(1);
                    SetPedToRagdoll(GetPlayerPed(-1), 3000, 3000, 0, false, false, false);
                }

                velBuffer[1] = velBuffer[0];
                velBuffer[0] = GetEntityVelocity(_vehicle);

                // Key is "Ctrl+Z"
                if (IsControlJustPressed(36, 20))
                {
                    beltOn = !beltOn;
                    if (beltOn)
                        Screen.ShowNotification("Ceinture On");
                    else 
                        Screen.ShowNotification("Ceinture Off");
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"belt\", \"data\": \"" + beltOn + "\"}");
                }*/
            }
            
        }
        private bool IsInVehicle()
        {
            var veh = Game.PlayerPed.CurrentVehicle;
            return veh != null;
        }



        private bool IsCar(int vehicle)
        {
            var vc = GetVehicleClass(vehicle);

            return (vc >= 0 && vc <= 7) || (vc >= 9 && vc <= 12) || (vc >= 17 && vc <= 20);
        }

        private float[] speedBuffer = new float[2];
        private Vector3[] velBuffer = new Vector3[2];
        bool beltOn = false;
        private bool wasInCar = false;
        private bool keyPressed = false;
        private bool IndicatorL = false;
        private bool IndicatorR = false;
        private async Task BeltTick()
        {
            int _vehicleToEnter = GetVehiclePedIsTryingToEnter(GetPlayerPed(PlayerId()));
            if (DoesEntityExist(_vehicleToEnter))
            {
                int lockStatus = GetVehicleDoorLockStatus(_vehicleToEnter);

                if(lockStatus == 4)
                {
                    await Delay(500);
                    ClearPedTasks(GetPlayerPed(PlayerId()));
                }
            }

            int _ped = GetPlayerPed(-1);
            int _vehicle = GetVehiclePedIsIn(_ped, false);
            if (_vehicle > 0 && (wasInCar || IsCar(_vehicle)))
            {
                var veh = Game.PlayerPed.CurrentVehicle;
                if (!wasInCar)
                {
                    veh.IsEngineRunning = false;
                    veh.IsRadioEnabled = false;
                }
                wasInCar = true;

                // If seat belt is on, keep them from exiting vehicle
                if (beltOn)
                {
                    // Disables INPUT_VEH_EXIT // default: "F"
                    DisableControlAction(0, 75, true);
                    if (Game.IsControlPressed(0, Control.VehicleExit))
                    {
                        Screen.ShowNotification("~r~Enlevez votre ceinture.");
                    }
                }

                speedBuffer[1] = speedBuffer[0];
                speedBuffer[0] = GetEntitySpeed(_vehicle);

                if (speedBuffer[1] > 0.0f &&
                    !beltOn &&
                    GetEntitySpeedVector(_vehicle, true).Y > 1.0f &&
                    speedBuffer[0] > 15.0f &&               // m/s which comes out to approx 34mph
                    (speedBuffer[1] - speedBuffer[0]) > (speedBuffer[0] * 0.255f))
                {
                    var coords = GetEntityCoords(_ped, true);
                    var fw = ForwardVelocity(_ped);
                    SetEntityCoords(_ped, coords.X + fw[0], coords.Y + fw[1], coords.Z - 0.47f, true, true, true, true);
                    SetEntityVelocity(_ped, velBuffer[1].X, velBuffer[1].Y, velBuffer[1].Z);
                    await Delay(1);
                    SetPedToRagdoll(GetPlayerPed(-1), 3000, 3000, 0, false, false, false);
                }

                velBuffer[1] = velBuffer[0];
                velBuffer[0] = GetEntityVelocity(_vehicle);

                // Key is "Ctrl + B"
                /*if (Game.IsControlPressed(0, Control.Duck) && Game.IsControlPressed(0, Control.SpecialAbilitySecondary) && !keyPressed)//IsControlJustPressed(36, 29)
                {
                    keyPressed = true;
                    beltOn = !beltOn;
                    if (beltOn)
                        Screen.ShowNotification("~g~✅ Ceinture attachée");
                    else
                        Screen.ShowNotification("~r~❌ Ceinture enlevée");
                    await Delay(700);
                    keyPressed = false;
                }*/
                // Key is "Ctrl + L"
                /*else if (Game.IsControlPressed(0, Control.Duck) && Game.IsControlPressed(0, Control.PhoneCameraFocusLock) && Game.PlayerPed.Handle == veh.Driver.Handle)
                {
                    keyPressed = true;
                    if (veh.LockStatus != VehicleLockStatus.Locked && veh.LockStatus != VehicleLockStatus.LockedForPlayer && veh.LockStatus != VehicleLockStatus.StickPlayerInside)
                    {
                        veh.LockStatus = VehicleLockStatus.StickPlayerInside;
                        Screen.ShowNotification("~g~🔐 Portières bloquées");
                    }
                    else
                    {
                        veh.LockStatus = VehicleLockStatus.Unlocked;
                        Screen.ShowNotification("~r~🔓 Portière ouvertes");
                    }
                    // Disables // default: "L"
                    DisableControlAction(0, 182, true);
                    await Delay(700);
                    keyPressed = false;
                }*/
                // Ctrl + D
                /*else if (Game.IsControlPressed(0, Control.Duck) && Game.IsControlPressed(0, Control.VehicleMoveRightOnly) && !keyPressed && Game.PlayerPed.Handle == veh.Driver.Handle)
                {
                    keyPressed = true;
                    DisableControlAction(0, 64, true);
                    DisableControlAction(0, 279, true);
                    IndicatorR = !IndicatorR;
                    IndicatorL = false;
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 1, false);
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 0, IndicatorR);
                    await Delay(700);
                    keyPressed = false;
                }
                // Ctrl + Q
                else if (Game.IsControlPressed(0, Control.Duck) && Game.IsControlPressed(0, Control.VehicleMoveLeftOnly) && !keyPressed && Game.PlayerPed.Handle == veh.Driver.Handle)
                {
                    keyPressed = true;
                    DisableControlAction(0, 63, true);
                    DisableControlAction(0, 278, true);
                    IndicatorL = !IndicatorL;
                    IndicatorR = false;
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 1, IndicatorL);
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 0, false);
                    await Delay(700);
                    keyPressed = false;
                }*/
                // Key is "Ctrl + Z"
                /*else if (Game.IsControlPressed(0, Control.Duck) && Game.IsControlPressed(0, Control.VehicleAccelerate) && !keyPressed && Game.PlayerPed.Handle == veh.Driver.Handle)//IsControlJustPressed(36, 20)
                {
                    if (veh.IsEngineRunning && GetEntitySpeed(_vehicle) < 5.0f)
                    {
                        keyPressed = true;
                        veh.IsEngineRunning = false;
                        veh.IsEngineStarting = false;
                        //veh.IsHandbrakeForcedOn = true;
                        Screen.ShowNotification("~r~❌ Moteur éteint");
                        await Delay(700);
                        keyPressed = false;
                    }
                    else if(!veh.IsEngineRunning)
                    {
                        keyPressed = true;
                        //veh.IsHandbrakeForcedOn = false;
                        veh.IsEngineStarting = true;
                        Screen.ShowNotification("~g~🔑 Moteur allumé");
                        await Delay(700);
                        keyPressed = false;
                    }
                }*/
            }
            else if (wasInCar)
            {
                wasInCar = false;
                beltOn = false;
                speedBuffer[0] = speedBuffer[1] = 0.0f;
            }

            await Delay(0);
        }

        private float[] ForwardVelocity(int ent)
        {
            float hdg = GetEntityHeading(ent);
            if (hdg < 0.0) hdg += 360.0f;

            hdg = hdg * 0.0174533f;

            float[] ret = new float[2];
            ret[0] = (float)Math.Cos(hdg) * 2.0f;
            ret[1] = (float)Math.Sin(hdg) * 2.0f;

            return ret;
        }
    }
}
