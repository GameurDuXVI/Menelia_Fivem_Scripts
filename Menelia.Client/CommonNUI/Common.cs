using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using static CitizenFX.Core.Native.API;

namespace Menelia.Client.CommonNUI
{
    class Common : BaseScript
    {
        private float[] _speedBuffer = new float[2];
        private Vector3[] _velBuffer = new Vector3[2];
        private bool _beltOn;
        private bool _wasInCar;
        private bool _inVehicle;
        private bool _keyPressed;
        private bool _indicatorL;
        private bool _indicatorR;
        
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
                    _beltOn = !_beltOn;
                    Screen.ShowNotification(_beltOn? "~g~✅ Ceinture attachée" : "~r~❌ Ceinture enlevée");
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
                    _indicatorR = !_indicatorR;
                    _indicatorL = false;
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 1, _indicatorL);
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 0, _indicatorR);
                }
            }), false);
            RegisterCommand("left_indicator", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (IsInVehicle())
                {
                    _indicatorL = !_indicatorL;
                    _indicatorR = false;
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 1, _indicatorL);
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 0, _indicatorR);
                }
            }), false);
            RegisterCommand("up_indicator", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (IsInVehicle())
                {
                    if(_indicatorL && _indicatorR)
                    {
                        _indicatorL = false;
                        _indicatorR = false;
                    }
                    else
                    {
                        _indicatorL = true;
                        _indicatorR = true;
                    }
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 1, _indicatorL);
                    ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 0, _indicatorR);
                }
            }), false);
            ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 1, false);
            ClientUtils.SendToAllCLients("CommonNUI:ChangeVehicleIndicator", GetPlayerServerId(PlayerId()), 0, _indicatorR);


            RegisterKeyMapping("engine", "[Veh] Démarer/Éteindre le moteur", "keyboard", "i");
            RegisterKeyMapping("belt", "[Veh] Mettre/Enlever la ceinture", "keyboard", "b");
            RegisterKeyMapping("lock", "[Veh] Vérouiller/Devérouiller le vehicule", "keyboard", "l");
            RegisterKeyMapping("left_indicator", "[Veh] Clignotant Gauche", "keyboard", "left");
            RegisterKeyMapping("right_indicator", "[Veh] Clignotant Droit", "keyboard", "right");
            RegisterKeyMapping("up_indicator", "[Veh] Feux de détresse", "keyboard", "up");

            EventHandlers["CommonNUI:ChangeVehicleIndicator"] += new Action<List<object>>((args) =>
            {
                var veh = GetVehiclePedIsIn(GetPlayerPed(GetPlayerFromServerId((int)args[0])), false);
                SetVehicleIndicatorLights(veh, (int)args[1], (bool)args[2]);
            });
        }

        private async Task onTick500()
        {
            await Delay(150);
            if (IsInVehicle() != _inVehicle)
            {
                _inVehicle = IsInVehicle();
                SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"show\", \"data\": \"" + _inVehicle + "\"}");
            }

            if (Game.Player.IsDead)
            {
                SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"show\", \"data\": \"" + false + "\"}");
                SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"belt\", \"data\": \"" + _beltOn + "\"}");
            }

            if (_inVehicle)
            {
                var veh = Game.PlayerPed.CurrentVehicle;
                if (veh.IsEngineRunning)
                {
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"fuel\", \"data\": \"" + veh.FuelLevel + "\"}");
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"lights\", \"data\": \"" + veh.AreLightsOn + "\"}");
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"lock\", \"data\": \"" + (!(veh.LockStatus != VehicleLockStatus.Locked && veh.LockStatus != VehicleLockStatus.LockedForPlayer)) + "\"}");
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"brakes\", \"data\": \"" + veh.IsHandbrakeForcedOn + "\"}");
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"speed\", \"data\": \"" + veh.Speed + "\"}");
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"belt\", \"data\": \"" + _beltOn + "\"}");
                }
                else
                {
                    SendNuiMessage("{\"ui\":\"SpeedNUI\", \"action\": \"update\", \"type\": \"engine\", \"data\": \"" + veh.IsEngineRunning + "\"}");
                }
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

        private async Task BeltTick()
        {
            var vehicleToEnter = GetVehiclePedIsTryingToEnter(GetPlayerPed(PlayerId()));
            if (DoesEntityExist(vehicleToEnter))
            {
                var lockStatus = GetVehicleDoorLockStatus(vehicleToEnter);

                if(lockStatus == 4)
                {
                    await Delay(500);
                    ClearPedTasks(GetPlayerPed(PlayerId()));
                }
            }

            var ped = GetPlayerPed(-1);
            var vehicle = GetVehiclePedIsIn(ped, false);
            if (vehicle > 0 && (_wasInCar || IsCar(vehicle)))
            {
                var veh = Game.PlayerPed.CurrentVehicle;
                if (!_wasInCar)
                {
                    veh.IsEngineRunning = false;
                    veh.IsRadioEnabled = false;
                }
                _wasInCar = true;
                if (_beltOn)
                {
                    // Disables INPUT_VEH_EXIT // default: "F"
                    DisableControlAction(0, 75, true);
                    if (Game.IsControlPressed(0, Control.VehicleExit))
                    {
                        Screen.ShowNotification("~r~Enlevez votre ceinture.");
                    }
                }

                _speedBuffer[1] = _speedBuffer[0];
                _speedBuffer[0] = GetEntitySpeed(vehicle);

                if (_speedBuffer[1] > 0.0f &&
                    !_beltOn &&
                    GetEntitySpeedVector(vehicle, true).Y > 1.0f &&
                    _speedBuffer[0] > 15.0f &&               // m/s which comes out to approx 34mph
                    (_speedBuffer[1] - _speedBuffer[0]) > (_speedBuffer[0] * 0.255f))
                {
                    var coords = GetEntityCoords(ped, true);
                    var fw = ForwardVelocity(ped);
                    SetEntityCoords(ped, coords.X + fw[0], coords.Y + fw[1], coords.Z - 0.47f, true, true, true, true);
                    SetEntityVelocity(ped, _velBuffer[1].X, _velBuffer[1].Y, _velBuffer[1].Z);
                    await Delay(1);
                    SetPedToRagdoll(GetPlayerPed(-1), 3000, 3000, 0, false, false, false);
                }

                _velBuffer[1] = _velBuffer[0];
                _velBuffer[0] = GetEntityVelocity(vehicle);
            }
            else if (_wasInCar)
            {
                _wasInCar = false;
                _beltOn = false;
                _speedBuffer[0] = _speedBuffer[1] = 0.0f;
            }
            await Delay(0);
        }

        private float[] ForwardVelocity(int ent)
        {
            var hdg = GetEntityHeading(ent);
            if (hdg < 0.0) hdg += 360.0f;

            hdg *= 0.0174533f;

            var ret = new float[2];
            ret[0] = (float)Math.Cos(hdg) * 2.0f;
            ret[1] = (float)Math.Sin(hdg) * 2.0f;

            return ret;
        }
    }
}
