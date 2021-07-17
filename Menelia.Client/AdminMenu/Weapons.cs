using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menelia.Client.AdminMenu
{
    public class Weapons
    {
        public enum Melee : uint
        {
            Knife = 2578778090,
            Nightstick = 1737195953,
            Hammer = 1317494643,
            Bat = 2508868239,
            Crowbar = 2227010557,
            GolfClub = 1141786504,
            Bottle = 4192643659,
            Dagger = 2460120199,
            Hatchet = 4191993645,
            KnuckleDuster = 3638508604,
            Machete = 3713923289,
            Flashlight = 2343591895,
            SwitchBlade = 3756226112,
            PoolCue = 2484171525,
            Wrench = 419712736,
            BattleAxe = 3441901897,
            StoneHatchet = 940833800,
        }

        public enum HandGuns : uint
        {
            //Unarmed = 2725352035,
            Pistol = 453432689,
            PistolMk2 = 3219281620,
            CombatPistol = 1593441988,
            Pistol50 = 2578377531,
            SNSPistol = 3218215474,
            SNSPistolMk2 = 2285322324,
            HeavyPistol = 3523564046,
            VintagePistol = 137902532,
            MarksmanPistol = 3696079510,
            Revolver = 3249783761,
            RevolverMk2 = 3415619887,
            APPistol = 584646201,
            StunGun = 911657153,
            FlareGun = 1198879012,
            RayPistol = 2939590305,
        }

        public enum Machine_Guns : uint
        {
            MicroSMG = 324215364,
            MachinePistol = 3675956304,
            SMG = 736523883,
            SMGMk2 = 2024373456,
            AssaultSMG = 4024951519,
            CombatPDW = 171789620,
            MG = 2634544996,
            CombatMG = 2144741730,
            CombatMGMk2 = 3686625920,
            Gusenberg = 1627465347,
            MiniSMG = 3173288789,
            RayMinigun = 3056410471,
        }

        public enum Assault_Rifles : uint
        {
            AssaultRifle = 3220176749,
            AssaultRifleMk2 = 961495388,
            CarbineRifle = 2210333304,
            CarbineRifleMk2 = 4208062921,
            SpecialCarbineMk2 = 2526821735,
            RayCarbine = 1198256469,
            AdvancedRifle = 2937143193,
            SpecialCarbine = 3231910285,
            BullpupRifle = 2132975508,
            CompactRifle = 1649403952,
            MarksmanRifleMk2 = 1785463520,
            BullpupRifleMk2 = 2228681469,
        }

        public enum Sniper_Rifles : uint
        {
            SniperRifle = 100416529,
            HeavySniper = 205991906,
            HeavySniperMk2 = 177293209,
            MarksmanRifle = 3342088282,
        }

        public enum Shotguns : uint
        {
            PumpShotgun = 487013001,
            PumpShotgunMk2 = 1432025498,
            SawnOffShotgun = 2017895192,
            BullpupShotgun = 2640438543,
            AssaultShotgun = 3800352039,
            Musket = 2828843422,
            HeavyShotgun = 984333226,
            DoubleBarrelShotgun = 4019527611,
            SweeperShotgun = 317205821,
        }

        public enum Heavy_Weapons : uint
        {
            GrenadeLauncher = 2726580491,
            CompactGrenadeLauncher = 125959754,
            RPG = 2982836145,
            Minigun = 1119849093,
            Firework = 2138347493,
            Railgun = 1834241177,
            HomingLauncher = 1672152130,
            GrenadeLauncherSmoke = 1305664598,
        }

        public enum Thrown_Weapons : uint
        {
            Grenade = 2481070269,
            StickyBomb = 741814745,
            ProximityMine = 2874559379,
            BZGas = 2694266206,
            Molotov = 615608432,
            FireExtinguisher = 101631238,
            PetrolCan = 883325847,
            Flare = 1233104067,
            Ball = 600439132,
            Snowball = 126349499,
            SmokeGrenade = 4256991824,
            PipeBomb = 3125143736,
        }

        public enum Equipement : uint
        {
            NightVision = 2803906140,
            DoubleAction = 2548703416,
        }

        public enum Parachute : uint
        {
            Parachute = 4222310262,
        }
    }
}
