using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Menelia.Client.SpawnManager;
using static CitizenFX.Core.Native.API;

using NativeUI;

namespace Menelia.Client.AdminMenu
{
    public class CharacterMenu
    {
        public CharacterMenu(UIMenu mainMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(mainMenu, "Personnage");

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            policeMenu(menu);
            weaponsMenu(menu);
            teleportMenu(menu);
        }

        private void weaponsMenu(UIMenu supMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(supMenu, "Armes", "Accède aux armes");
            for (int i = 0; i < 1; i++) ;

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            var weaponsItem = new UIMenuItem($"Toutes les armes", $"Give toutes les armes");
            menu.AddItem(weaponsItem);
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == weaponsItem)
                {
                    var current = Game.PlayerPed.Weapons.Current;
                    foreach (WeaponHash wh in Enum.GetValues(typeof(WeaponHash)))
                    {
                        if (!Game.PlayerPed.Weapons.HasWeapon(wh))
                        {
                            var wp = Game.PlayerPed.Weapons.Give(wh, 250, false, true);
                            wp.Ammo = wp.MaxAmmo;
                        }
                    }
                    if (current != null)
                        Game.PlayerPed.Weapons.Select(current);
                }
            };

            var munitionsItem = new UIMenuItem("Munitions max", $"Give les munitions des armes");
            menu.AddItem(munitionsItem);
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == munitionsItem)
                {
                    var current = Game.PlayerPed.Weapons.Current;
                    foreach (WeaponHash wh in Enum.GetValues(typeof(WeaponHash)))
                    {
                        if (Game.PlayerPed.Weapons.HasWeapon(wh))
                        {
                            var wp = Game.PlayerPed.Weapons[wh];
                            wp.Ammo = wp.MaxAmmo;
                        }
                    }
                    if (current != null)
                        Game.PlayerPed.Weapons.Select(current);
                }
            };

            menu.AddItem(new UIMenuSeparatorItem());

            addWeaponCategoryItem(menu, "Melee", typeof(Weapons.Melee));
            addWeaponCategoryItem(menu, "HandGuns", typeof(Weapons.HandGuns));
            addWeaponCategoryItem(menu, "Machine Guns", typeof(Weapons.Machine_Guns));
            addWeaponCategoryItem(menu, "Assault_Rifles", typeof(Weapons.Assault_Rifles));
            addWeaponCategoryItem(menu, "Equipement", typeof(Weapons.Equipement));
            addWeaponCategoryItem(menu, "Heavy Weapons", typeof(Weapons.Heavy_Weapons));
            addWeaponCategoryItem(menu, "Parachute", typeof(Weapons.Parachute));
            addWeaponCategoryItem(menu, "Shotguns", typeof(Weapons.Shotguns));
            addWeaponCategoryItem(menu, "Sniper Rifles", typeof(Weapons.Sniper_Rifles));
            addWeaponCategoryItem(menu, "Thrown Weapons", typeof(Weapons.Thrown_Weapons));
        }

        private void addWeaponCategoryItem(UIMenu supMenu, string optionName, Type classType)
        {
            var menu = MainMenu.menuPool.AddSubMenu(supMenu, optionName);

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            List<Object> list = new List<object>();
            foreach (Object obj in Enum.GetValues(classType))
            {
                list.Add(obj);
            }

            var sortedList = list.OrderBy(l => l.ToString());

            var weaponsItem = new UIMenuItem($"Toutes les armes: {optionName}", $"Give les munitions des armes de cette categorie");
            menu.AddItem(weaponsItem);
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == weaponsItem)
                {
                    var current = Game.PlayerPed.Weapons.Current;
                    foreach (Object obj in sortedList)
                    {
                        WeaponHash wh;
                        Enum.TryParse<WeaponHash>(Enum.GetName(classType, obj), out wh);
                        if (!Game.PlayerPed.Weapons.HasWeapon(wh))
                        {
                            var wp = Game.PlayerPed.Weapons.Give(wh, 250, false, true);
                            wp.Ammo = wp.MaxAmmo;
                        }    
                    }
                    if(current != null)
                        Game.PlayerPed.Weapons.Select(current);
                }
            };

            var munitionsItem = new UIMenuItem("Munitions max", $"Give les munitions des armes de cette categorie");
            menu.AddItem(munitionsItem);
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == munitionsItem)
                {
                    var current = Game.PlayerPed.Weapons.Current;
                    foreach (var obj in sortedList)
                    {
                        WeaponHash wh;//= (WeaponHash) Enum.TryParse(typeof(WeaponHash), vehicleName);
                        Enum.TryParse<WeaponHash>(Enum.GetName(classType, obj), out wh);
                        if (Game.PlayerPed.Weapons.HasWeapon(wh))
                        {
                            var wp = Game.PlayerPed.Weapons[wh];
                            wp.Ammo = wp.MaxAmmo;
                        }
                    }
                    if (current != null)
                        Game.PlayerPed.Weapons.Select(current);
                }
            };

            menu.AddItem(new UIMenuSeparatorItem());

            foreach (var obj in sortedList)
            {
                var weaponName = Enum.GetName(classType, obj);
                var weaponItem = new UIMenuItem(weaponName, $"Give l'arme: {weaponName}");
                menu.AddItem(weaponItem);
                menu.OnItemSelect += (sender, item, index) =>
                {
                    if (item == weaponItem)
                    {
                        Enum.TryParse<WeaponHash>(weaponName, out var wh);
                        if (!Game.PlayerPed.Weapons.HasWeapon(wh))
                            Game.PlayerPed.Weapons.Give(wh, 250, false, true);
                    }
                };
            }
        }

        private void policeMenu(UIMenu supMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(supMenu, "Police", "");
            
            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            bool ignoreByPolice = false;

            var ignoreItem = new UIMenuCheckboxItem("Ignoré par la police", UIMenuCheckboxStyle.Cross, ignoreByPolice, "");
            menu.AddItem(ignoreItem);
            menu.OnCheckboxChange += (sender, item, checked_) =>
            {
                if (item == ignoreItem)
                {
                    Game.Player.IgnoredByPolice = checked_;
                    ignoreByPolice = checked_;
                }
            };

            var policeLevels = new List<dynamic>();
            for (var i = 0; i < 6; i++) policeLevels.Add(i);

            var weaponsItem = new UIMenuListItem("Niveau de police", policeLevels, Game.Player.WantedLevel);
            menu.AddItem(weaponsItem);
            menu.OnListChange += (sender, item, index) =>
            {
                if (item == weaponsItem)
                {
                    Game.Player.WantedLevel = policeLevels[index];
                }
            };
        }

        private void teleportMenu(UIMenu supMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(supMenu, "Teleportation", "");

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            foreach (var wp in Waypoints.waypoints)
            {
                var teleportItem = new UIMenuItem(wp.Name, "");
                menu.AddItem(teleportItem);
                menu.OnItemSelect += async (sender, item, index) =>
                {
                    if (item == teleportItem)
                    {
                        DoScreenFadeOut(500);

                        while (IsScreenFadingOut())
                        {
                            await BaseScript.Delay(1);
                        }

                        SManager.freezePlayer(PlayerId(), true);
                        SetPedDefaultComponentVariation(GetPlayerPed(-1));
                        RequestCollisionAtCoord(wp.X, wp.Y, wp.Z);

                        var ped = GetPlayerPed(-1);

                        SetEntityCoordsNoOffset(ped, wp.X, wp.Y, wp.Z, false, false, false);
                        NetworkResurrectLocalPlayer(wp.X, wp.Y, wp.Z, wp.Heading, true, true);
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
                };
            }
        }
    }
}
