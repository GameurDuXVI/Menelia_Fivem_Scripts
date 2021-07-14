using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CitizenFX.Core.Native.API;
using CitizenFX.Core.UI;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;
using MeneliaAPI.Client;

namespace Menelia.Client.AdminMenu
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
                {
                    if (!await ClientUtils.HasPermission("menu.admin"))
                    {
                        Screen.ShowNotification("Vous n'avez pas la permission d'accéder à cette fonctionalité !");
                        return;
                    }
                    mainMenu.Visible = !mainMenu.Visible;
                }
            };
        }

        
    }
}
