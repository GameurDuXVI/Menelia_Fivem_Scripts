using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using MeneliaAPI.Client;
using CitizenFX.Core.UI;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace AdminMenus
{
    public class MainMenu : BaseScript
    {

        public static MenuPool menuPool;
        public static UIMenu mainMenu;
        public MainMenu()
        {
            menuPool = new MenuPool();
            mainMenu = new UIMenu("Admin Panel", "DESC");
            menuPool.Add(mainMenu);
            new VehiclesMenu(mainMenu);
            new ServerMenu(mainMenu);
            new CharacterMenu(mainMenu);

            menuPool.MouseEdgeEnabled = false;
            menuPool.ControlDisablingEnabled = false;
            menuPool.RefreshIndex();

            Tick += async () =>
            {
                menuPool.ProcessMenus();
                if (API.IsControlJustPressed(0, 166) && !menuPool.IsAnyMenuOpen())
                    mainMenu.Visible = !mainMenu.Visible;
            };
        }

        
    }
}
