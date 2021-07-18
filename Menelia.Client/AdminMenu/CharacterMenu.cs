using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

using NativeUI;

namespace Menelia.Client.AdminMenu
{
    public class CharacterMenu : BaseScript
    {
        public CharacterMenu(UIMenu mainMenu)
        {
            var menu = MainMenu.MenuPool.AddSubMenu(mainMenu, "Personnage");

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;
            
            EventHandlers["AdminPanel:Teleport"] += new Action<int, List<object>>((serverId, objects) =>
            {
                if (GetPlayerServerId(PlayerId()) == serverId)
                {
                    ClientUtils.teleport((float)objects[0], (float)objects[1], (float)objects[2], (float)objects[3]);
                }
            });

            //policeMenu(menu);
            weaponsMenu(menu);
            //teleportMenu(menu);
            playersMenu(menu);
        }

        private static void weaponsMenu(UIMenu supMenu)
        {
            var menu = MainMenu.MenuPool.AddSubMenu(supMenu, "Armes", "Accède aux armes");

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
            addWeaponCategoryItem(menu, "Assault Rifles", typeof(Weapons.Assault_Rifles));
            addWeaponCategoryItem(menu, "Equipement", typeof(Weapons.Equipement));
            addWeaponCategoryItem(menu, "Heavy Weapons", typeof(Weapons.Heavy_Weapons));
            addWeaponCategoryItem(menu, "Parachute", typeof(Weapons.Parachute));
            addWeaponCategoryItem(menu, "Shotguns", typeof(Weapons.Shotguns));
            addWeaponCategoryItem(menu, "Sniper Rifles", typeof(Weapons.Sniper_Rifles));
            addWeaponCategoryItem(menu, "Thrown Weapons", typeof(Weapons.Thrown_Weapons));
        }

        private static void addWeaponCategoryItem(UIMenu supMenu, string optionName, Type classType)
        {
            var menu = MainMenu.MenuPool.AddSubMenu(supMenu, optionName);

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            var list = new List<object>();
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
                        Enum.TryParse(Enum.GetName(classType, obj), out WeaponHash wh);
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
                        Enum.TryParse(Enum.GetName(classType, obj), out WeaponHash wh);
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

        private static void policeMenu(UIMenu supMenu, int playerId)
        {
            var menu = MainMenu.MenuPool.AddSubMenu(supMenu, "Police", "");
            
            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            /*var ignoreItem = new UIMenuCheckboxItem("Ignoré par la police (non fonctionnel)", UIMenuCheckboxStyle.Cross, false, "");
            menu.AddItem(ignoreItem);
            menu.OnCheckboxChange += (sender, item, isChecked) =>
            {
                if (item == ignoreItem)
                {
                    Game.Player.IgnoredByPolice = isChecked;
                }
            };*/

            menu.OnMenuStateChanged += (oldMenu, newMenu, state) =>
            {
                if (state == MenuState.Opened || state == MenuState.ChangeForward)
                {
                    newMenu.Clear();
                    
                    var policeLevels = new List<dynamic>();
                    for (var i = 0; i < 6; i++) policeLevels.Add(i);

                    var weaponsItem = new UIMenuListItem("Niveau de police", policeLevels, GetPlayerWantedLevel(playerId));
                    menu.AddItem(weaponsItem);
                    menu.OnListChange += (sender, item, index) =>
                    {
                        if (item == weaponsItem)
                        {
                            SetPlayerWantedLevel(playerId, policeLevels[index], false);
                        }
                    };
                }
            };
        }

        private static void playersMenu(UIMenu supMenu)
        {
            var menu = MainMenu.MenuPool.AddSubMenu(supMenu, "Joueurs", "");

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            menu.OnMenuStateChanged += (oldMenu, newMenu, state) =>
            {
                if (state == MenuState.Opened || state == MenuState.ChangeForward)
                {
                    newMenu.Clear();
                    
                    playerMenu(menu, PlayerId());
                    
                    int[] playersId = GetActivePlayers();
                    foreach (var playerId in playersId)
                    {
                        if(playerId != PlayerId())
                            playerMenu(menu, playerId);
                    }
                }
            };
        }

        private static void playerMenu(UIMenu supMenu, int playerId)
        {
            var menu = MainMenu.MenuPool.AddSubMenu(supMenu, GetPlayerName(playerId), "");

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;
            
            policeMenu(menu, playerId);
            
            var teleportToItem = new UIMenuItem("Se teleporter vers " + GetPlayerName(playerId), "");
            menu.AddItem(teleportToItem);
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == teleportToItem)
                {
                    var loc = GetEntityCoords(GetPlayerPed(playerId), true);
                    ClientUtils.teleport(loc.X, loc.Y, loc.Z, 0);
                }
            };
            
            teleportToLocMenu(menu, playerId);
        }
        
        private static void teleportToLocMenu(UIMenu supMenu, int playerId)
        {
            var menu = MainMenu.MenuPool.AddSubMenu(supMenu, "Teleportation", "");

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            foreach (var wp in Waypoint.Waypoints)
            {
                var teleportToLocItem = new UIMenuItem(wp.Name , "");
                menu.AddItem(teleportToLocItem);
                menu.OnItemSelect += (sender, item, index) =>
                {
                    if (item == teleportToLocItem)
                    {
                        ClientUtils.sendToSecificPlayer("AdminPanel:Teleport", GetPlayerServerId(playerId), wp.X, wp.Y, wp.Z, wp.Heading);
                    }
                };
            }
        }

        /*private static void teleportMenu(UIMenu supMenu)
        {
            var menu = MainMenu.MenuPool.AddSubMenu(supMenu, "Teleportation", "");

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            foreach (var wp in Waypoint.Waypoints)
            {
                var teleportItem = new UIMenuItem(wp.Name, "");
                menu.AddItem(teleportItem);
                menu.OnItemSelect += async (sender, item, index) =>
                {
                    if (item == teleportItem)
                    {
                        ClientUtils.teleport(wp.X, wp.Y, wp.Z, wp.Heading);
                    }
                };
            }
        }*/
    }
}
