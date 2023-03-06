// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Graphics;
using WinUICommunity.Common.Helpers;
using Microsoft.UI.Windowing;
using DLUTToolBoxV3.Entities;
using System.Diagnostics;
using DLUTToolBoxV3.Helpers;
using DLUTToolBoxV3.Configurations;
using DLUTToolBoxV3.Pages;
using NLog;
using Windows.ApplicationModel.DataTransfer;
using Microsoft.Web.WebView2.Core;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System.Threading.Tasks;
using Castle.Core.Internal;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BrowserWindow : Window
    {
        private AppWindow m_AppWindow;
        public NLog.Logger logger;
        bool LoginTried = false;
        bool APILoginTried = false;
        private AppDataItem app;
        public BrowserWindow(AppDataItem appDataItem)
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("打开浏览器页面");
            this.InitializeComponent();
            this.Closed += (o, e) =>
            {
                WebView.Close();
            };
            m_AppWindow = WindowHelper.GetAppWindowForCurrentWindow(this);
            m_AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            m_AppWindow.SetIcon("Assets/logo.ico");
            this.Title = "内容子界面";
            app = appDataItem;
            if (appDataItem != null)
            {
                WebView.Source = appDataItem.Uri;
                WindowTitle.Text= appDataItem.Title;
            }
        }

        private void WebView_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            WebView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = true;
            WebView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;
            WebView.CoreWebView2.Settings.IsZoomControlEnabled = true;
            WebView.CoreWebView2.Settings.IsGeneralAutofillEnabled = true;
            WebView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = true;
            WebView.CoreWebView2.NewWindowRequested += (sender, args) =>
            {
                WebView.CoreWebView2.ExecuteScriptAsync("window.location.href='" + args.Uri.ToString() + "'");
                args.Handled = true;
            };
            WebView.CoreWebView2.ContentLoading += (sender, args) =>
            {
                RefreshOrStopIcon.Glyph = "\xe711";
            };
            WebView.CoreWebView2.DOMContentLoaded += (sender, args) =>
            {
                RefreshOrStopIcon.Glyph = "\xe72c";
                this.Title = WebView.CoreWebView2.DocumentTitle;
                WindowTitle.Text = WebView.CoreWebView2.DocumentTitle;
            };
            WebView.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
        }

        private void CoreWebView2_NavigationCompleted(CoreWebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
        {
            if (WebView.CoreWebView2.DocumentTitle.IndexOf("过期") != -1)
            {
                WebView.ExecuteScriptAsync("document.getElementsByClassName('layui-layer-btn0')[0].click()");
                WebView.ExecuteScriptAsync("document.getElementsByClassName('layui-layer-btn0')[0].click()");
                WebView.ExecuteScriptAsync("document.getElementsByClassName('layui-layer-btn0')[0].click()");
                var builder = new AppNotificationBuilder()
                    .AddText("\"⚠来自网页的安全提示⚠\\n根据信息安全等级保护要求，用户密码需定期更换。请尽快到【校园门户】-【我的信息】-【安全设置】中修改！\"");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
                return;
            }
            if (WebView.Source.AbsoluteUri.StartsWith("https://webvpn.dlut.edu.cn/https/77726476706e69737468656265737421e3e44ed2233c7d44300d8db9d6562d/cas/login?service=") || WebView.Source.AbsoluteUri.StartsWith("https://sso.dlut.edu.cn/cas/login?service=") || WebView.Source.AbsoluteUri.StartsWith("http://sso.dlut.edu.cn/cas/login?service=") || WebView.Source.AbsoluteUri.StartsWith("https://webvpn.dlut.edu.cn/https/77726476706e69737468656265737421e3e44ed2233c7d44300d8db9d6562d/cas/login;JSESSIONIDCAS="))
            {
                if (LoginTried)
                {
                    Notify();
                }
                else
                {
                    LoginTried = true;
                    login();
                    return;
                }
            }
            else
            {
                LoginTried = false;
            }
            if (WebView.Source.AbsoluteUri.StartsWith("https://api.m.dlut.edu.cn/login"))
            {
                if (APILoginTried)
                {
                    Notify();
                }
                else
                {
                    APILoginTried = true;
                    apilogin();
                    return;
                }
            }
            else
            {
                APILoginTried = false;
            }
            switch(app.HandleId)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        logger.Info("开发区校区校园网自服务特殊处理");
                        break;
                    }
            }
        }

        async Task Notify()
        {
            var builder = new AppNotificationBuilder()
                .AddText("⚠自动认证失败⚠")
                .AddText("请前往设置界面检查账号密码是否正确");
            var notificationManager = AppNotificationManager.Default;
            notificationManager.Show(builder.BuildNotification());
        }

        async Task apilogin()
        {
            if (ApplicationConfig.GetSettings("Uid").IsNullOrEmpty() || ApplicationConfig.GetSettings("Password").IsNullOrEmpty())
            {
                var builder = new AppNotificationBuilder()
                    .AddText("⚠请先配置账号密码⚠")
                    .AddText("配置完成后刷新原始页面即可自动登录");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
            }
            else
            {
                logger.Info("执行api登录注入");
                string jscode = "username.value='" + ApplicationConfig.GetSettings("Uid") + "'";
                string jscode1 = "password.value='" + ApplicationConfig.GetSettings("Password") + "'";
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode);
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode1);
                string jscode2 = "$(\"#formpc\").submit()";
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode2);
            }
        }

        async Task login()
        {
            if (ApplicationConfig.GetSettings("Uid").IsNullOrEmpty() || ApplicationConfig.GetSettings("Password").IsNullOrEmpty())
            {
                var builder = new AppNotificationBuilder()
                    .AddText("⚠请先配置账号密码⚠")
                    .AddText("配置完成后刷新原始页面即可自动登录");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
            }
            else
            {
                logger.Info("执行sso登录注入");
                string jscode = "un.value='" + ApplicationConfig.GetSettings("Uid") + "'";
                string jscode1 = "pd.value='" + ApplicationConfig.GetSettings("Password") + "'";
                string rm = "rememberName.checked='checked'";
                await WebView.CoreWebView2.ExecuteScriptAsync(rm);
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode);
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode1);
                string jscode2 = "login()";
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode2);
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            WebView.GoBack();
        }

        private void GoForward_Click(object sender, RoutedEventArgs e)
        {
            WebView.GoForward();
        }

        private void RefreshOrStop_Click(object sender, RoutedEventArgs e)
        {
            if (WebView.IsLoaded)
            {
                WebView.CoreWebView2.Reload();
                LoginTried = false;
            }
            else
            {
                WebView.CoreWebView2.Stop();
            }
        }

        private void AddressBox_KeyUp(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (!AddressBox.Text.StartsWith("http"))
                {
                    AddressBox.Text = "http://" + AddressBox.Text;
                }
                WebView.CoreWebView2.ExecuteScriptAsync("window.location.href='" + AddressBox.Text + "'");
            }
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("0", "下载", "", "", "edge://downloads/all", 0));
        }

        private void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("0", "历史记录", "", "", "edge://history/all", 0));
        }

        private void MenuFlyoutItem_Click_3(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText("【" + WebView.CoreWebView2.DocumentTitle + "】" + WebView.Source.AbsoluteUri);
            Clipboard.SetContent(dataPackage);
            var builder = new AppNotificationBuilder()
                .AddText("页面链接已复制到剪贴板");
            var notificationManager = AppNotificationManager.Default;
            notificationManager.Show(builder.BuildNotification());
        }
    }
}
