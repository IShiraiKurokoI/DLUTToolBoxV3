using DLUTToolBoxV3.Configurations;
using DLUTToolBoxV3.Entities;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUICommunity;

namespace DLUTToolBoxV3.Helpers
{
    public static class WebHelper
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static IThemeService themeService { get; private set; }
        public static void AddOrCreateNewPage(AppDataItem appDataItem)
        {
            ElementTheme SettingsTheme = ElementTheme.Default;
            if (ApplicationConfig.GetSettings("Theme") != null)
            {
                if (ApplicationConfig.GetSettings("Theme") == "Light")
                {
                    SettingsTheme = ElementTheme.Light;
                }
                if (ApplicationConfig.GetSettings("Theme") == "Dark")
                {
                    SettingsTheme = ElementTheme.Dark;
                }
            }
            else
            {
                ApplicationConfig.SaveSettings("Theme", "Default");
            }
            logger.Debug("新建浏览器页面");
            BrowserWindow browserWindow = new BrowserWindow(appDataItem);
            try
            {
                themeService = new ThemeService();
                themeService.Initialize(browserWindow);
                themeService.ConfigBackdrop(BackdropType.AcrylicThin);
                themeService.ConfigElementTheme(SettingsTheme);
                themeService.ConfigTitleBar(new TitleBarCustomization
                {
                    TitleBarWindowType = TitleBarWindowType.AppWindow,
                    LightTitleBarButtons = new TitleBarButtons
                    {
                        ButtonBackgroundColor = Colors.Transparent
                    },
                    DarkTitleBarButtons = new TitleBarButtons
                    {
                        ButtonBackgroundColor = Colors.Transparent
                    }
                });
            }
            catch (Exception)
            {

            }
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(browserWindow);
            Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
            if (appWindow is not null)
            {
                Microsoft.UI.Windowing.DisplayArea displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(windowId, Microsoft.UI.Windowing.DisplayAreaFallback.Nearest);
                if (displayArea is not null)
                {
                    var CenteredPosition = appWindow.Position;
                    CenteredPosition.X = ((displayArea.WorkArea.Width - appWindow.Size.Width) / 2);
                    CenteredPosition.Y = ((displayArea.WorkArea.Height - appWindow.Size.Height) / 2);
                    appWindow.Move(CenteredPosition);
                }
            }
            browserWindow.Activate();
        }
    }
}
