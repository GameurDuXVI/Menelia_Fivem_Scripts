using CitizenFX.Core.UI;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace Menelia.Client.AdminMenu
{
    public class MainMenu : BaseScript
    {
        public static MenuPool MenuPool;
        public MainMenu()
        {
            MenuPool = new MenuPool();
            var mainMenu = new UIMenu("Admin Panel", "DESC");
            MenuPool.Add(mainMenu);
            new VehiclesMenu(mainMenu);
            new ServerMenu(mainMenu);
            new CharacterMenu(mainMenu);

            MenuPool.MouseEdgeEnabled = false;
            MenuPool.ControlDisablingEnabled = false;
            MenuPool.RefreshIndex();

            Tick += async () =>
            {
                MenuPool.ProcessMenus();
                if (API.IsControlJustPressed(0, 166) && !MenuPool.IsAnyMenuOpen())
                {
                    if (!await ClientUtils.hasPermission("menu.admin"))
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
