// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using DLUTToolBoxV3.Configurations;
using DLUTToolBoxV3.Entities;
using DLUTToolBoxV3.Ultilities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Diagnostics;
using System.IO.Pipes;
using System.Security.Principal;
using System.Text.RegularExpressions;
using DLUTToolBoxV3.Helpers;
using WinUICommunity;
using Windows.UI;
using System.Drawing;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        public NLog.Logger logger;
        public static ThemeManager themeManager { get; private set; }
        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs e)
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("--------程序启动--------");
            logger.Info("日志记录初始化成功");
            Environment.SetEnvironmentVariable("WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS", "--proxy-server=\"direct://\"");
            logger.Info("WebView参数初始化成功");
            //Task线程内未捕获异常处理事件
            System.Threading.Tasks.TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            App.Current.UnhandledException += CurrentDomain_UnhandledException;

            if (ApplicationConfig.GetSettings("AutoLogin") != "None")
            {
                ActionHelper.CheckAccountData();
            }

            ElementTheme SettingsTheme = ElementTheme.Default;
            if (ApplicationConfig.GetSettings("Theme") != null)
            {
                if(ApplicationConfig.GetSettings("Theme") == "Light")
                {
                    SettingsTheme = ElementTheme.Light;
                }
                if(ApplicationConfig.GetSettings("Theme") == "Dark")
                {
                    SettingsTheme = ElementTheme.Dark;
                }
            }
            else
            {
                ApplicationConfig.SaveSettings("Theme", "Default");
            }

            m_window = new MainWindow();
            themeManager = ThemeManager.Initialize(m_window, new ThemeOptions
            {
                BackdropType = BackdropType.DesktopAcrylic,
                ElementTheme = SettingsTheme,
                TitleBarCustomization = new TitleBarCustomization
                {
                    TitleBarType = TitleBarType.AppWindow
                }
            }); 
            if (ApplicationConfig.GetSettings("AutoLogin") == null)
            {
                ApplicationConfig.SaveSettings("AutoLogin", "None");
            }
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(m_window);
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
            m_window.Activate();
        }

        private Window m_window;

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // 处理未处理的异常
            HandleException(e.Exception);
            // 将事件标记为已处理，以防止应用程序崩溃
            e.Handled = true;
        }


        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                if (e.Exception is Exception exception)
                {
                    HandleException(exception);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                e.SetObserved();
            }
        }

        //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
        private void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.ExceptionObject is Exception exception)
                {
                    HandleException(exception);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        //日志记录
        private void HandleException(Exception ex)
        {
            var builder = new AppNotificationBuilder()
                .AddText(ex.Message+ex.StackTrace);
            var notificationManager = AppNotificationManager.Default;
            notificationManager.Show(builder.BuildNotification());
            //记录日志
            logger.Error(ex.ToString());
        }

    }
}
