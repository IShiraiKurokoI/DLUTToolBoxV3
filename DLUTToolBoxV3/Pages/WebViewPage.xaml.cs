// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using DLUTToolBoxV3.Configurations;
using DLUTToolBoxV3.Entities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Web.WebView2.Core;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation.Metadata;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebViewPage : Page
    {
        TabApp app;
        bool LoginTried = false;
        public WebViewPage()
        {
            this.InitializeComponent();
            WebView.Height = this.Height - 48;
        }
        public void PrepareConnectedAnimation(ConnectedAnimationConfiguration config)
        {
            var anim = ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("ForwardConnectedAnimation", SourceElement);

            if (config != null && ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7))
            {
                anim.Configuration = config;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter!=null)
            {
                app = ((TabApp)e.Parameter);
                if (app.appDataItem != null)
                {
                    WebView.Source = app.appDataItem.Uri;
                }
            }
            base.OnNavigatedTo(e);

            var anim = ConnectedAnimationService.GetForCurrentView().GetAnimation("BackwardConnectedAnimation");
            if (anim != null)
            {
                anim.TryStart(SourceElement);
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
                app.tabViewItem.Header = WebView.CoreWebView2.DocumentTitle;
            };
            WebView.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
        }

        private async void CoreWebView2_NavigationCompleted(CoreWebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
        {
            if (WebView.CoreWebView2.DocumentTitle.IndexOf("过期") != -1)
            {
                await WebView.ExecuteScriptAsync("document.getElementsByClassName('layui-layer-btn0')[0].click()");
                await WebView.ExecuteScriptAsync("document.getElementsByClassName('layui-layer-btn0')[0].click()");
                await WebView.ExecuteScriptAsync("document.getElementsByClassName('layui-layer-btn0')[0].click()");
                var builder = new AppNotificationBuilder()
                    .AddText("⚠来自网页的安全提示⚠\n根据信息安全等级保护要求，用户密码需定期更换。请尽快到【校园门户】-【我的信息】-【安全设置】中修改！");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
                return;
            }
            if (WebView.CoreWebView2.DocumentTitle.IndexOf("密码重置") != -1)
            {
                await WebView.ExecuteScriptAsync("document.getElementsByClassName('layui-layer-btn0')[0].click()");
                await WebView.ExecuteScriptAsync("document.getElementsByClassName('layui-layer-btn0')[0].click()");
                await WebView.ExecuteScriptAsync("document.getElementsByClassName('layui-layer-btn0')[0].click()");
                string jscode = "new_pwd.value='" + ApplicationConfig.GetSettings("Password") + "'";
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode);
                jscode = "confirm_pwd.value='" + ApplicationConfig.GetSettings("Password") + "'";
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode);
                jscode = "sub_btn.click()";
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode);
                var builder = new AppNotificationBuilder()
                    .AddText("⚠密码已经过期⚠\n工具箱将尝试自动续期密码");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
                return;
            }
            if (WebView.Source.AbsoluteUri.StartsWith("https://webvpn.dlut.edu.cn/https/57787a7876706e323032336b657940246b0b0d56f80f865ae449fe2ddfb88b/cas/login?") || WebView.Source.AbsoluteUri.StartsWith("https://sso.dlut.edu.cn/cas/login?") || WebView.Source.AbsoluteUri.StartsWith("http://sso.dlut.edu.cn/cas/login?") || WebView.Source.AbsoluteUri.StartsWith("https://webvpn.dlut.edu.cn/https/57787a7876706e323032336b657940246b0b0d56f80f865ae449fe2ddfb88b/cas/login;JSESSIONIDCAS="))
            {
                if (LoginTried)
                {
                    if (WebView.CoreWebView2.DocumentTitle.IndexOf("密码重置") == -1)
                    {
                        await Notify();
                    }
                    else
                    {
                        LoginTried = false;
                    }
                }
                else
                {
                    LoginTried = true;
                    await login();
                }
            }
            else
            {
                LoginTried = false;
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

        async Task login()
        {
            if (String.IsNullOrEmpty(ApplicationConfig.GetSettings("Uid")) || String.IsNullOrEmpty(ApplicationConfig.GetSettings("Password")))
            {
                var builder = new AppNotificationBuilder()
                    .AddText("⚠请先配置账号密码⚠")
                    .AddText("配置完成后刷新原始页面即可自动登录");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
            }
            else
            {
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
                WebView.CoreWebView2.ExecuteScriptAsync("window.location.href='" + AddressBox.Text + "'");
            }
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            //WebHelper.browserWindow.GetTabView().TabItems.Add(WebHelper.browserWindow.CreateNewTab(null));
            //WebHelper.browserWindow.GetTabView().SelectedIndex = WebHelper.browserWindow.GetTabView().TabItems.Count - 1;
        }

        private void MenuFlyoutItem_Click_1(object sender, RoutedEventArgs e)
        {
            //WebHelper.browserWindow.AddPage(new AppDataItem("0","下载","","", "edge://downloads/all",0));
        }

        private void MenuFlyoutItem_Click_2(object sender, RoutedEventArgs e)
        {
            //WebHelper.browserWindow.AddPage(new AppDataItem("0", "历史记录", "", "", "edge://history/all", 0));
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
