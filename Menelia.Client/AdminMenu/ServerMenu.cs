using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using NativeUI;

namespace Menelia.Client.AdminMenu
{
    class ServerMenu
    {
        public ServerMenu(UIMenu mainMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(mainMenu, "Server");

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            var now = Server.Weather.XMAS;
            var weathers = new List<dynamic>();
            foreach(Server.Weather weather in Enum.GetValues(typeof(Server.Weather)))
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
                    SetOverrideWeather(Enum.GetName(typeof(Server.Weather), weathers[index]));
                }
            };

            timeMenu(menu);
        }

        private void timeMenu(UIMenu supMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(supMenu, "Temps", "Permet de changer le temps du serveur");
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
}
