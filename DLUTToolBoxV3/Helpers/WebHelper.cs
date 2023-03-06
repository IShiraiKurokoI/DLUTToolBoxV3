using DLUTToolBoxV3.Configurations;
using DLUTToolBoxV3.Entities;
using Microsoft.UI.Xaml;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUICommunity.Common.Helpers;

namespace DLUTToolBoxV3.Helpers
{
    public static class WebHelper
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static BrowserWindow browserWindow;
        public static void AddOrCreateNewPage(AppDataItem appDataItem)
        {
            logger.Debug("新建浏览器页面");
            BrowserWindow browserWindow = new BrowserWindow(appDataItem);
            ThemeHelper.Initialize(browserWindow, BackdropType.DesktopAcrylic);
            browserWindow.Activate();
        }
    }
}
