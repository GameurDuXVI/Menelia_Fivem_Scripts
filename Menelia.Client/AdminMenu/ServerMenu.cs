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
    class ServerMenu
    {
        public ServerMenu(UIMenu mainMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(mainMenu, "Server");
            for (int i = 0; i < 1; i++) ;

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            Server.Weather now = Server.Weather.XMAS;
            List<dynamic> weathers = new List<dynamic>();
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

        public void timeMenu(UIMenu supMenu)
        {
            var menu = MainMenu.menuPool.AddSubMenu(supMenu, "Temps", "Permet de changer le temps du serveur");
            for (int i = 0; i < 1; i++) ;

            menu.MouseEdgeEnabled = false;
            menu.ControlDisablingEnabled = false;

            List<dynamic> hours = new List<dynamic>();
            for (int i = 0; i < 24; i++) hours.Add(i);

            var hourItem = new UIMenuListItem("Heure", hours, hours.IndexOf(World.CurrentDate.Hour));
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

            List<dynamic> minutes = new List<dynamic>();
            for (int i = 0; i < 24; i++) minutes.Add(i);

            var minutesItem = new UIMenuListItem("Minutes", hours, hours.IndexOf(World.CurrentDate.Minute));
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
