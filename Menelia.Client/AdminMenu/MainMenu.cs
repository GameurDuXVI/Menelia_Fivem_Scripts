using CitizenFX.Core.UI;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using NativeUI;

namespace Menelia.Client.AdminMenu
{
    public class MainMenu : BaseScript
    {
        public static MenuPool menuPool;
        private static UIMenu _mainMenu;
        public MainMenu()
        {
            menuPool = new MenuPool();
            _mainMenu = new UIMenu("Admin Panel", "DESC");
            menuPool.Add(_mainMenu);
            new VehiclesMenu(_mainMenu);
            new ServerMenu(_mainMenu);
            new CharacterMenu(_mainMenu);

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
                    _mainMenu.Visible = !_mainMenu.Visible;
                }
            };
        }
    }
}
