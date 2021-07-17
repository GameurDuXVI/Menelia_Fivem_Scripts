﻿using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using CitizenFX.Core.UI;
using NativeUI;
using static CitizenFX.Core.Native.API;

namespace Menelia.Client.AdminMenu
{
    public class VehiclesMenu
    {
        public static List<CustomVehicle> CustomVehicles = new List<CustomVehicle>();

        public VehiclesMenu(UIMenu mainMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(mainMenu, "Vehicle");
            for (int i = 0; i < 1; i++) ;

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            spawnMenu(menu);
            modificationMenu(menu);
            styleMenu(menu);

            var vehicleItem = new UIMenuItem("Réparer", "Répare le vehicule");
            menu.AddItem(vehicleItem);
            menu.OnItemSelect += (sender, item, index) =>
            {
                if (item == vehicleItem)
                {
                    var veh = Game.PlayerPed.CurrentVehicle;
                    if (veh == null)
                    {
                        Screen.ShowNotification("Vous n'êtes pas dans un vehicule !");
                        return;
                    }

                    veh.Repair();
                }
            };

            godMenu(menu);
        }

        private void spawnMenu(UIMenu supMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(supMenu, "Spawn", "Permet de spawn un nouveau vehicule");

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            CustomVehicles.Add(new CustomVehicle("polraiden", typeof(Vehicles.Emergency)));
            CustomVehicles.Add(new CustomVehicle("menswat", typeof(Vehicles.Emergency)));
            CustomVehicles.Add(new CustomVehicle("policegt350r", typeof(Vehicles.Emergency)));

            spawn(menu, "Batteaux", typeof(Vehicles.Boats), 10f);
            spawn(menu, "Camions", typeof(Vehicles.Commercials), 10f);
            spawn(menu, "Minis", typeof(Vehicles.Compacts), 5f);
            spawn(menu, "Coupes", typeof(Vehicles.Coupes), 5f);
            spawn(menu, "Vélos", typeof(Vehicles.Cycles), 2f);
            spawn(menu, "Urgences", typeof(Vehicles.Emergency), 5f);
            spawn(menu, "Hélicoptères", typeof(Vehicles.Helicopters), 10f);
            spawn(menu, "Industriels", typeof(Vehicles.Industrial), 10f);
            spawn(menu, "Militaires", typeof(Vehicles.Military), 5f);
            spawn(menu, "Motos", typeof(Vehicles.Motorcycles), 5f);
            spawn(menu, "Muscles", typeof(Vehicles.Muscle), 5f);
            spawn(menu, "Hors piste", typeof(Vehicles.Off_Road), 5f);
            spawn(menu, "Voitures de courses", typeof(Vehicles.Open_Wheel), 5f);
            spawn(menu, "Avions", typeof(Vehicles.Planes), 20f);
            spawn(menu, "SUV", typeof(Vehicles.SUV), 5f);
            spawn(menu, "Sedans", typeof(Vehicles.Sedans), 5f);
            spawn(menu, "Vehicules de service", typeof(Vehicles.Service), 10f);
            spawn(menu, "Sports", typeof(Vehicles.Sports), 5f);
            spawn(menu, "Super", typeof(Vehicles.Super), 5f);
            spawn(menu, "Trailer", typeof(Vehicles.Trailer), 15f);
            spawn(menu, "Trains", typeof(Vehicles.Trains), 15f);
            spawn(menu, "Utility", typeof(Vehicles.Utility), 10f);
            spawn(menu, "Vans", typeof(Vehicles.Vans), 5f);
        }

        private void spawn(UIMenu supMenu, string optionName, Type classType, float distance)
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

            foreach (Object obj in sortedList)
            {
                String vehicleName = Enum.GetName(classType, obj);
                var vehicleItem = new UIMenuItem(vehicleName, $"Spawn le véhicule: {vehicleName}");
                menu.AddItem(vehicleItem);
                menu.OnItemSelect += async (sender, item, index) =>
                {
                    if (item == vehicleItem)
                    {

                        uint enumObj = Convert.ToUInt32(obj);
                        var veh = await World.CreateVehicle(new Model((int)enumObj), Game.PlayerPed.GetOffsetPosition(new Vector3(0f, distance, 0f)), Game.PlayerPed.Heading);
                        veh.NeedsToBeHotwired = false;
                        veh.Mods.InstallModKit();
                        foreach (VehicleModType type in Enum.GetValues(typeof(VehicleModType)))
                        {
                            veh.Mods[type].Index = 0;
                        }
                    }
                };
            }

            foreach(var cv in CustomVehicles)
            {
                if (cv.category.Equals(classType))
                {
                    String vehicleName = cv.name;
                    var vehicleItem = new UIMenuItem(vehicleName, $"Spawn le véhicule: {vehicleName}");
                    menu.AddItem(vehicleItem);
                    menu.OnItemSelect += async (sender, item, index) =>
                    {
                        if (item == vehicleItem)
                        {
                            var veh = await World.CreateVehicle(vehicleName, Game.PlayerPed.GetOffsetPosition(new Vector3(0f, distance, 0f)), Game.PlayerPed.Heading);
                            veh.NeedsToBeHotwired = false;
                            veh.Mods.InstallModKit();
                            foreach (VehicleModType type in Enum.GetValues(typeof(VehicleModType)))
                            {
                                veh.Mods[type].Index = 0;
                            }
                        }
                    };
                }
            }
        }

        private void modificationMenu(UIMenu supMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(supMenu, "Modification", "Permet la modification du vehicule actuel");
            
            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            menu.OnMenuStateChanged += (oldMenu, newMenu, state) =>
            {
                if(state == MenuState.Opened || state == MenuState.ChangeForward)
                {
                    newMenu.Clear();

                    var veh = Game.PlayerPed.CurrentVehicle;

                    List<VehicleModType> list = new List<VehicleModType>();
                    foreach (VehicleModType obj in Enum.GetValues(typeof(VehicleModType)))
                    {
                        list.Add(obj);
                    }

                    var sortedList = list.OrderBy(l => l.ToString());

                    if(veh == null)
                    {
                        Screen.ShowNotification("Vous n'êtes pas dans un vehicule !");
                        return;
                    }

                    veh.Mods.InstallModKit();

                    foreach (VehicleModType type in sortedList)
                    {
                        List<dynamic> options = new List<dynamic>();

                        if(veh.Mods[type].ModCount == 0)
                        {
                            continue;
                        }

                        for (int i = 0; i < veh.Mods[type].ModCount; i++)
                        {
                            options.Add(i);
                        }

                        String modName = Enum.GetName(typeof(VehicleModType), type);
                        var modItem = new UIMenuListItem(modName, options, veh.Mods[type].Index);
                        newMenu.AddItem(modItem);
                        newMenu.OnListChange += (sender, item, index) =>
                        {
                            if (item == modItem)
                            {
                                veh.Mods[type].Index = options[index];
                            }
                        };

                    }
                }
            };
        }

        private void styleMenu(UIMenu supMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(supMenu, "Style", "Permet de changer le style du vehicule");
            
            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            menu.OnMenuStateChanged += (oldMenu, newMenu, state) =>
            {
                if (state == MenuState.Opened || state == MenuState.ChangeForward)
                {
                    newMenu.Clear();

                    var veh = Game.PlayerPed.CurrentVehicle;

                    if (veh == null)
                    {
                        newMenu.Clear();
                        Screen.ShowNotification("Vous n'êtes pas dans un vehicule !");
                        return;
                    }

                    List<dynamic> colors = new List<dynamic>();
                    foreach (VehicleColor obj in Enum.GetValues(typeof(VehicleColor)))
                        colors.Add(obj);

                    var primaryItem = new UIMenuListItem("Couleur primaire", colors, colors.IndexOf(veh.Mods.PrimaryColor));
                    newMenu.AddItem(primaryItem);
                    newMenu.OnListChange += (sender, item, index) =>
                    {
                        if (item == primaryItem)
                        {
                            veh.Mods.PrimaryColor = colors[index];
                        }
                    };

                    var secondaryItem = new UIMenuListItem("Couleur secondaire", colors, colors.IndexOf(veh.Mods.SecondaryColor));
                    newMenu.AddItem(secondaryItem);
                    newMenu.OnListChange += (sender, item, index) =>
                    {
                        if (item == secondaryItem)
                        {
                            veh.Mods.SecondaryColor = colors[index];
                        }
                    };

                    var pearlescentColorItem = new UIMenuListItem("PearlescentColor (EN)", colors, colors.IndexOf(veh.Mods.PearlescentColor));
                    newMenu.AddItem(pearlescentColorItem);
                    newMenu.OnListChange += (sender, item, index) =>
                    {
                        if (item == pearlescentColorItem)
                        {
                            veh.Mods.PearlescentColor = colors[index];
                        }
                    };

                    var rimColorItem = new UIMenuListItem("RimColor (EN)", colors, colors.IndexOf(veh.Mods.RimColor));
                    newMenu.AddItem(rimColorItem);
                    newMenu.OnListChange += (sender, item, index) =>
                    {
                        if (item == rimColorItem)
                        {
                            veh.Mods.RimColor = colors[index];
                        }
                    };

                    var trimColorItem = new UIMenuListItem("TrimColor (EN)", colors, colors.IndexOf(veh.Mods.TrimColor));
                    newMenu.AddItem(trimColorItem);
                    newMenu.OnListChange += (sender, item, index) =>
                    {
                        if (item == trimColorItem)
                        {
                            veh.Mods.TrimColor = colors[index];
                        }
                    };


                    var liscensePlatesStyles = new List<dynamic>();
                    foreach (var obj in Enum.GetValues(typeof(LicensePlateStyle)))
                        liscensePlatesStyles.Add(obj);

                    var liscensePlatesStyleItem = new UIMenuListItem("Style de plaque", liscensePlatesStyles, liscensePlatesStyles.IndexOf(veh.Mods.LicensePlateStyle));
                    newMenu.AddItem(liscensePlatesStyleItem);
                    newMenu.OnListChange += (sender, item, index) =>
                    {
                        if (item == liscensePlatesStyleItem)
                        {
                            veh.Mods.LicensePlateStyle = liscensePlatesStyles[index];
                        }
                    };


                    var wheelTypes = new List<dynamic>();
                    foreach (var obj in Enum.GetValues(typeof(VehicleWheelType)))
                        wheelTypes.Add(obj);

                    var wheelTypesItem = new UIMenuListItem("Types de roues", wheelTypes, wheelTypes.IndexOf(veh.Mods.WheelType));
                    newMenu.AddItem(wheelTypesItem);
                    newMenu.OnListChange += (sender, item, index) =>
                    {
                        if (item == wheelTypesItem)
                        {
                            veh.Mods.WheelType = wheelTypes[index];
                        }
                    };


                    List<dynamic> windowTints = new List<dynamic>();
                    foreach (VehicleWindowTint obj in Enum.GetValues(typeof(VehicleWindowTint)))
                        windowTints.Add(obj);

                    var windowTintsItem = new UIMenuListItem("Teints de fenêtres", windowTints, windowTints.IndexOf(veh.Mods.WindowTint));
                    newMenu.AddItem(windowTintsItem);
                    newMenu.OnListChange += (sender, item, index) =>
                    {
                        if (item == windowTintsItem)
                        {
                            veh.Mods.WindowTint = windowTints[index];
                        }
                    };
                }
            };
        }

        private void godMenu(UIMenu supMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(supMenu, "God", "Permet de changer l'invincibilité du véhicule");
            
            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            menu.OnMenuStateChanged += (oldMenu, newMenu, state) =>
            {
                if (state == MenuState.Opened || state == MenuState.ChangeForward)
                {
                    newMenu.Clear();

                    var veh = Game.PlayerPed.CurrentVehicle;

                    if (veh == null)
                    {
                        newMenu.Clear();
                        Screen.ShowNotification("Vous n'êtes pas dans un vehicule !");
                        return;
                    }


                    var invincibleItem = new UIMenuCheckboxItem("Crash Test", UIMenuCheckboxStyle.Cross, veh.IsInvincible, "Change l'invicibilité du vehicule lors d'un crash (ne veut pas dire qu'elle ne serra pas déformé)"); ;
                    menu.AddItem(invincibleItem);
                    menu.OnCheckboxChange += (sender, item, isChecked) =>
                    {
                        if (item == invincibleItem)
                        {
                            if (!veh.IsInvincible)
                            {
                                veh.Repair();
                            }
                            veh.IsInvincible = isChecked;
                        }
                    };

                    var bulletItem = new UIMenuCheckboxItem("Par balles", UIMenuCheckboxStyle.Cross, veh.IsBulletProof, "Change la résistance par balle du vehicule"); ;
                    menu.AddItem(bulletItem);
                    menu.OnCheckboxChange += (sender, item, isChecked) =>
                    {
                        if (item == bulletItem)
                        {
                            veh.IsBulletProof = isChecked;
                        }
                    };

                    var collisionItem = new UIMenuCheckboxItem("Collisions", UIMenuCheckboxStyle.Cross, veh.IsCollisionEnabled, "Change la "); ;
                    menu.AddItem(collisionItem);
                    menu.OnCheckboxChange += (sender, item, isChecked) =>
                    {
                        if (item == collisionItem)
                        {
                            veh.IsCollisionEnabled = isChecked;
                        }
                    };

                    var exlosionsItem = new UIMenuCheckboxItem("Explosions", UIMenuCheckboxStyle.Cross, veh.IsExplosionProof, "Change la résistance aux explosions"); ;
                    menu.AddItem(exlosionsItem);
                    menu.OnCheckboxChange += (sender, item, isChecked) =>
                    {
                        if (item == exlosionsItem)
                        {
                            veh.IsExplosionProof = isChecked;
                        }
                    };

                    var fireItem = new UIMenuCheckboxItem("Feu", UIMenuCheckboxStyle.Cross, veh.IsFireProof, "Change la résistance au feu"); ;
                    menu.AddItem(fireItem);
                    menu.OnCheckboxChange += (sender, item, isChecked) =>
                    {
                        if (item == fireItem)
                        {
                            veh.IsFireProof = isChecked;
                        }
                    };

                    var meleeItem = new UIMenuCheckboxItem("Melee", UIMenuCheckboxStyle.Cross, veh.IsMeleeProof, "Change la résistance aux dégats de melee"); ;
                    menu.AddItem(meleeItem);
                    menu.OnCheckboxChange += (sender, item, isChecked) =>
                    {
                        if (item == meleeItem)
                        {
                            veh.IsMeleeProof = isChecked;
                        }
                    };


                    var onlyDamagedByPlayerItem = new UIMenuCheckboxItem("Dégats des non-joueurs", UIMenuCheckboxStyle.Cross, veh.IsOnlyDamagedByPlayer, "Change si les non-joueurs peuvent faire des dégats"); ;
                    menu.AddItem(onlyDamagedByPlayerItem);
                    menu.OnCheckboxChange += (sender, item, isChecked) =>
                    {
                        if (item == onlyDamagedByPlayerItem)
                        {
                            veh.IsOnlyDamagedByPlayer = isChecked;
                        }
                    };
                }
            };
        }
    }
}
