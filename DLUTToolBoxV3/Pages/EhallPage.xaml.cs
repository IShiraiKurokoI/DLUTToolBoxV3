// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using DLUTToolBoxV3.Configurations;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Web.WebView2.Core;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Castle.Core.Internal;
using Windows.ApplicationModel.DataTransfer;
using DLUTToolBoxV3.Helpers;
using DLUTToolBoxV3.Dialogs;
using NLog;
using QRCoder;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EhallPage : Page
    {
        bool LoginTried = false;
        public NLog.Logger logger;
        public EhallPage()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("打开办事大厅页面");
            this.InitializeComponent();
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
                if(!args.Uri.StartsWith("edge://"))
                {
                    WebView.CoreWebView2.ExecuteScriptAsync("window.location.href='" + args.Uri.ToString() + "'");
                    args.Handled = true;
                }
            };
            WebView.CoreWebView2.ContentLoading += (sender, args) =>
            {
                RefreshOrStopIcon.Glyph = "\xe711";
            };
            WebView.CoreWebView2.DOMContentLoaded += (sender, args) =>
            {
                RefreshOrStopIcon.Glyph = "\xe72c";
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
            }
            if (WebView.Source.AbsoluteUri.Contains("/cas/login?service="))
            {
                if (LoginTried)
                {
                    Notify();
                }
                else
                {
                    LoginTried = true;
                    login();
                }
            }
            else
            {
                LoginTried = false;
            }
            if (WebView.Source.AbsoluteUri.Contains("https://webvpn.dlut.edu.cn/login"))
            {
                WebView.ExecuteScriptAsync("window.location.href = \"https://webvpn.dlut.edu.cn/login?cas_login=true\"");
            }
        }


        private void WebView_NavigationStarting(WebView2 sender, CoreWebView2NavigationStartingEventArgs e)
        {
            logger.Info("页面|" + WebView.CoreWebView2.DocumentTitle + "|尝试打开URL:" + e.Uri.ToString());
            if (e.Uri.StartsWith("https://ibsbjstar.ccb.com.cn/CCBIS/B2CMainPlat"))
            {
                if (e.Uri.IndexOf("?CLIENTIP=&BRANCHID=") != -1)
                {
                    var builder = new AppNotificationBuilder()
                        .AddText("正在打开建行Web支付页面，请自行支付!");
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                    Windows.System.Launcher.LaunchUriAsync(new Uri(e.Uri));
                    e.Cancel = true;
                }
            }
            if (e.Uri.StartsWith("alipays://"))
            {
                ShowQRCode(e.Uri);
            }
            if (e.Uri.StartsWith("https://mclient.alipay.com/cashier/mobilepay.htm?"))
            {
                var builder = new AppNotificationBuilder()
                    .AddText("链接获取成功，请点击打开支付宝APP后使用手机支付宝扫码付款！");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
                ShowQRCode(e.Uri);
            }
            if (e.Uri.StartsWith("weixin://"))
            {
                var builder = new AppNotificationBuilder()
                    .AddText("暂不支持微信支付功能,敬请期待（TNND微信接口太难用了）");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
                e.Cancel = true;
            }
            if (e.Uri.IndexOf("mobile/api/unifiedOrderIndex.action?") != -1)
            {
                var builder = new AppNotificationBuilder()
                    .AddText("链接获取成功，请使用云闪付手机APP扫码付款！");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
                ShowQRCode(e.Uri);
            }
        }

        public void ShowQRCode(string Uri)
        {
            try
            {
                QRCodeGenerator.ECCLevel eccLevel = (QRCodeGenerator.ECCLevel)(1);
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                {
                    using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(Uri, eccLevel))
                    {
                        using (QRCode qrCode = new QRCode(qrCodeData))
                        {
                            System.Drawing.Bitmap bmp = qrCode.GetGraphic(20, System.Drawing.Color.Black, System.Drawing.Color.White, false);
                            QRCodeDialog qRCodeDialog = new QRCodeDialog(bmp);
                            qRCodeDialog.XamlRoot = this.Content.XamlRoot;
                            qRCodeDialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                            qRCodeDialog.ShowAsync();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
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

        private void Share_Click(object sender, RoutedEventArgs e)
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

        private void History_Click(object sender, RoutedEventArgs e)
        {
            WebView.Source = new Uri("edge://history/all");
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            WebView.Source = new Uri("edge://downloads/all");
        }
    }
}
