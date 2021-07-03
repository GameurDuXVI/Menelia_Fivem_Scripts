using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using NativeUI;

namespace AdminMenus
{
    class CharacterMenu
    {
        public CharacterMenu(UIMenu mainMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(mainMenu, "Personnage");
            for (int i = 0; i < 1; i++) ;

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            policeMenu(menu);
            weaponsMenu(menu);
        }

        public void weaponsMenu(UIMenu supMenu)
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

            give(menu, "Melee", typeof(Weapons.Melee));
            give(menu, "HandGuns", typeof(Weapons.HandGuns));
            give(menu, "Machine Guns", typeof(Weapons.Machine_Guns));
            give(menu, "Assault_Rifles", typeof(Weapons.Assault_Rifles));
            give(menu, "Equipement", typeof(Weapons.Equipement));
            give(menu, "Heavy Weapons", typeof(Weapons.Heavy_Weapons));
            give(menu, "Parachute", typeof(Weapons.Parachute));
            give(menu, "Shotguns", typeof(Weapons.Shotguns));
            give(menu, "Sniper Rifles", typeof(Weapons.Sniper_Rifles));
            give(menu, "Thrown Weapons", typeof(Weapons.Thrown_Weapons));
        }

        public void give(UIMenu supMenu, String optionName, Type classType)
        {
            var menu = MainMenu.menuPool.AddSubMenu(supMenu, optionName);
            for (int i = 0; i < 1; i++) ;

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
                    foreach (Object obj in sortedList)
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

            foreach (Object obj in sortedList)
            {
                String weaponName = Enum.GetName(classType, obj);
                var weaponItem = new UIMenuItem(weaponName, $"Give l'arme: {weaponName}");
                menu.AddItem(weaponItem);
                menu.OnItemSelect += (sender, item, index) =>
                {
                    if (item == weaponItem)
                    {
                        WeaponHash wh;
                        Enum.TryParse<WeaponHash>(weaponName, out wh);
                        if (!Game.PlayerPed.Weapons.HasWeapon(wh))
                            Game.PlayerPed.Weapons.Give(wh, 250, false, true);
                    }
                };
            }
        }

        public void policeMenu(UIMenu supMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(supMenu, "Police", "");
            for (int i = 0; i < 1; i++) ;

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

            List<dynamic> policeLevels = new List<dynamic>();
            for (int i = 0; i < 6; i++) policeLevels.Add(i);

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
    }
}
