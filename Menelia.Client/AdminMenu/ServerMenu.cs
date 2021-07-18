using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using NativeUI;

namespace Menelia.Client.AdminMenu
{
    public class ServerMenu : BaseScript
    {
        public ServerMenu(UIMenu mainMenu)
        {
            var menu = MainMenu.MenuPool.AddSubMenu(mainMenu, "Serveur");

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;
            
            /*EventHandlers["AdminPanel:Weather"] += new Action<int, List<object>>((serverId, objects) =>
            {
                if (GetPlayerServerId(PlayerId()) == serverId)
                {
                    ClientUtils.teleport((float)objects[0], (float)objects[1], (float)objects[2], (float)objects[3]);
                }
            });*/

            var now = Weather.XMAS;
            var weathers = new List<dynamic>();
            foreach(Weather weather in Enum.GetValues(typeof(Weather)))
            {
                if (GetNextWeatherType() == (int) weather) now = weather;
                weathers.Add(weather);
            }

            var weatherItem = new UIMenuListItem("Meteo", weathers, weathers.IndexOf(now));
            menu.AddItem(weatherItem);
            menu.OnListChange += (sender, item, index) =>
            {
                if (item == weatherItem)
                {
                    SetOverrideWeather(Enum.GetName(typeof(Weather), weathers[index]));
                }
            };

            timeMenu(menu);
        }

        private static void timeMenu(UIMenu supMenu)
        {
            var menu = MainMenu.MenuPool.AddSubMenu(supMenu, "Temps", "Permet de changer le temps du serveur");
            for (int i = 0; i < 1; i++) ;

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            var hours = new List<dynamic>();
            for (var i = 0; i < 24; i++) hours.Add(i);

            var currentHour = 0;
            try
            {
                currentHour = hours.IndexOf(World.CurrentDate.Hour);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }

            var hourItem = new UIMenuListItem("Heure", hours, currentHour);
            menu.AddItem(hourItem);
            menu.OnListChange += (sender, item, index) =>
            {
                if (item == hourItem)
                {
                    NetworkOverrideClockTime(hours[index], GetClockMinutes(), 00);
                    //World.CurrentDayTime = new TimeSpan(hours[index], World.CurrentDate.Minute, World.CurrentDate.Second);
                    //World.CurrentDate = new DateTime(World.CurrentDate.Year, World.CurrentDate.Month, World.CurrentDate.Day, hours[index], World.CurrentDate.Minute, World.CurrentDate.Second);
                }
            };

            var minutes = new List<dynamic>();
            for (var i = 0; i < 60; i++) minutes.Add(i);

            var currentMinute = 0;

            try
            {
                currentMinute = minutes.IndexOf(World.CurrentDate.Minute);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);
            }

            var minutesItem = new UIMenuListItem("Minutes", minutes, currentMinute);
            menu.AddItem(minutesItem);
            menu.OnListChange += (sender, item, index) =>
            {
                if (item == minutesItem)
                {
                    NetworkOverrideClockTime(GetClockHours(), minutes[index], 00);
                    //World.CurrentDayTime = new TimeSpan(World.CurrentDate.Hour, minutes[index], World.CurrentDate.Second);
                    //World.CurrentDate = new DateTime(World.CurrentDate.Year, World.CurrentDate.Month, World.CurrentDate.Day, World.CurrentDate.Hour, minutes[index], World.CurrentDate.Second);
                }
            };
        }
    }
    
    enum Weather
    {
        EXTRASUNNY = -1750463879,
        Neutral = -1530260698,
        XMAS = -1429616491,
        FOGGY = -1368164796,
        THUNDER = -1233681761,
        OVERCAST = -1148613331,
        Halloween = -921030142,
        //Snowing = -273223690,
        //Unknown = -1,
        SMOG = 282916021,
        SNOWLIGHT = 603685163,
        BLIZZARD = 669657108,
        CLOUDS = 821931868,
        CLEAR = 916995460,
        RAIN = 1420204096,
        CLEARING = 1840358669
    }
}
