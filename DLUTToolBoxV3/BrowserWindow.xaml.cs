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
using WinUICommunity;
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
using System.Text.RegularExpressions;
using System.Runtime.ConstrainedExecution;
using QRCoder;
using static QRCoder.PayloadGenerator;
using DLUTToolBoxV3.Dialogs;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;

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

        [DllImport("user32.dll")]
        static extern int GetDpiForWindow(IntPtr hwnd);

        public BrowserWindow(AppDataItem appDataItem)
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("打开浏览器页面");
            this.InitializeComponent();
            this.Closed += (o, e) =>
            {
                WebView.CoreWebView2.Stop();
                WebView.Close();
            };
            int dpi = GetDpiForWindow(WinRT.Interop.WindowNative.GetWindowHandle(this));
            m_AppWindow = this.AppWindow;
            m_AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            m_AppWindow.SetIcon("Assets/logo.ico");
            this.Title = "内容子界面";
            app = appDataItem;
            if (appDataItem != null)
            {
                WebView.Source = appDataItem.Uri;
                WindowTitle.Text= appDataItem.Title;
                if(appDataItem.HandleId!=-1&&appDataItem.HandleId!=2)
                {
                    m_AppWindow.Resize(new SizeInt32((int)(1570 * (double)((double)dpi / (double)120)), (int)(800 * (double)((double)dpi / (double)120))));
                }
                else
                {
                    m_AppWindow.Resize(new SizeInt32((int)(530 * (double)((double)dpi / (double)120)), (int)(900 * (double)((double)dpi / (double)120))));
                }
            }
        }

        private void WebView_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            WebView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;
            WebView.CoreWebView2.Settings.IsZoomControlEnabled = true;
            WebView.CoreWebView2.Settings.IsGeneralAutofillEnabled = true;
            WebView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = true;
            WebView.CoreWebView2.Settings.IsSwipeNavigationEnabled = true;
            WebView.CoreWebView2.Settings.IsBuiltInErrorPageEnabled = true;
            WebView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;
            WebView.CoreWebView2.Settings.IsScriptEnabled = true;
            WebView.CoreWebView2.Settings.IsWebMessageEnabled = true;
            WebView.CoreWebView2.Settings.IsStatusBarEnabled = true;
            WebView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;
            WebView.CoreWebView2.NewWindowRequested += (sender, args) =>
            {
                if(app.HandleId!=3&&app.HandleId!=-2)
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
                this.Title = WebView.CoreWebView2.DocumentTitle;
                WindowTitle.Text = WebView.CoreWebView2.DocumentTitle;
            };
            WebView.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
            WebView.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
            WebView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
        }

        private void CoreWebView2_WebMessageReceived(CoreWebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            //if(args.WebMessageAsJson.IndexOf("changegroup")!=-1)
            //{
            //    Show();
            //}
            //else
            //{
                var builder = new AppNotificationBuilder()
                    .AddText(args.WebMessageAsJson);
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
            //}
        }

        //private async Task Show()
        //{
        //    ContentDialog dialog = new ContentDialog();
        //    dialog.XamlRoot = this.Content.XamlRoot;
        //    dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        //    dialog.Title = "确认执行结转吗？";
        //    dialog.PrimaryButtonText = "确定";
        //    dialog.CloseButtonText = "取消";
        //    dialog.DefaultButton = ContentDialogButton.Primary;
        //    dialog.Content = "⚠请先注销所有在线设备！⚠\n否则后果自负！";
        //    var result = await dialog.ShowAsync();
        //    if (result == ContentDialogResult.Primary)
        //    {
        //        DrcomHelper.ChangeGroup();
        //    }
        //}

        private void CoreWebView2_NavigationStarting(CoreWebView2 sender, CoreWebView2NavigationStartingEventArgs e)
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
                    e.Cancel= true;
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

        int count = 0;
        private async void CoreWebView2_NavigationCompleted(CoreWebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
        {
            if (WebView.CoreWebView2.DocumentTitle.IndexOf("过期") != -1)
            {
                await WebView.ExecuteScriptAsync("document.getElementsByClassName('layui-layer-btn0')[0].click()");
                await WebView.ExecuteScriptAsync("document.getElementsByClassName('layui-layer-btn0')[0].click()");
                await WebView.ExecuteScriptAsync("document.getElementsByClassName('layui-layer-btn0')[0].click()");
                logger.Info("密码即将过期");
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
                logger.Info("密码已经过期");
                string jscode = "new_pwd.value='" + ApplicationConfig.GetSettings("Password") + "'";
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode);
                jscode = "confirm_pwd.value='" + ApplicationConfig.GetSettings("Password") + "'";
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode);
                jscode = "sub_btn.click()";
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode);
                jscode = "btn_sub.click()";
                await WebView.CoreWebView2.ExecuteScriptAsync(jscode);
                var builder = new AppNotificationBuilder()
                    .AddText("⚠密码已经过期⚠\n工具箱将尝试自动续期密码");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
                return;
            }
            if (WebView.Source.AbsoluteUri.StartsWith("https://webvpn.dlut.edu.cn/https/57787a7876706e323032336b657940246b0b0d56f80f865ae449fe2ddfb88b/cas/login?service=") || WebView.Source.AbsoluteUri.StartsWith("https://sso.dlut.edu.cn/cas/login?service=") || WebView.Source.AbsoluteUri.StartsWith("http://sso.dlut.edu.cn/cas/login?service=") || WebView.Source.AbsoluteUri.StartsWith("https://webvpn.dlut.edu.cn/https/57787a7876706e323032336b657940246b0b0d56f80f865ae449fe2ddfb88b/cas/login;JSESSIONIDCAS="))
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
                case -2:
                case -1:
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        logger.Info("开发区校区校园网自服务特殊处理");
                        break;
                    }
                case 2:
                    {
                        logger.Info("玉兰卡特殊处理");
                        if (WebView.Source.AbsoluteUri.IndexOf("https://card.m.dlut.edu.cn/virtualcard/openVirtualcard?") != -1)
                        {
                            WebView.CoreWebView2.ExecuteScriptAsync("document.getElementsByClassName('code')[0].className=''");
                        }
                        switch (count)
                        {
                            case 0:
                                {
                                    WebView.CoreWebView2.Settings.UserAgent = "Mozilla/5.0 (Linux; Android 10; EBG-AN00 Build/HUAWEIEBG-AN00; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/83.0.4103.106 Mobile Safari/537.36 weishao(3.2.2.74616)";
                                    WebView.Source = new Uri("https://api.m.dlut.edu.cn/oauth/authorize?client_id=19b32196decf419a&redirect_uri=https%3A%2F%2Fcard.m.dlut.edu.cn%2Fhomerj%2FopenRjOAuthPage&response_type=code&scope=base_api&state=weishao");
                                    break;
                                }
                            default:
                                break;
                        }
                        break;
                    }
                    case 3:
                    {
                        logger.Info("选课系统特殊处理");
                        if (WebView.Source.AbsoluteUri.IndexOf("/student/home") != -1)
                        {
                            WebView.ExecuteScriptAsync("window.location.href='/student/for-std/course-select'");
                        }
                        if (WebView.Source.AbsoluteUri.IndexOf("for-std/course-select") != -1)
                        {
                            newtab();
                        }
                        break;
                    }
                    case 4:
                    {
                        logger.Info("评教系统特殊处理");
                        if (WebView.Source.AbsoluteUri.IndexOf("/student/home") != -1)
                        {
                            WebView.ExecuteScriptAsync("window.location.href='/student/for-std/evaluation/summative'");
                        }
                        break;
                    }
                    case 5:
                    {
                        logger.Info("考试安排特殊处理");
                        if (WebView.Source.AbsoluteUri.IndexOf("/student/home") != -1)
                        {
                            WebView.ExecuteScriptAsync("window.location.href='/student/for-std/exam-arrange'");
                        }
                        break;
                    }
                    case 6:
                    {
                        logger.Info("缓考系统特殊处理");
                        if (WebView.Source.AbsoluteUri.IndexOf("/student/home") != -1)
                        {
                            WebView.ExecuteScriptAsync("window.location.href='/student/for-std/exam-delay-apply'");
                        }
                        break;
                    }
                    case 7:
                    {
                        logger.Info("成绩信息特殊处理");
                        if (WebView.Source.AbsoluteUri.IndexOf("/student/home") != -1)
                        {
                            WebView.ExecuteScriptAsync("window.location.href='/student/for-std/grade/sheet'");
                        }
                        break;
                    }
                    case 8:
                    {
                        logger.Info("冲突选课特殊处理");
                        if (WebView.Source.AbsoluteUri.IndexOf("/student/home") != -1)
                        {
                            WebView.ExecuteScriptAsync("window.location.href='/student/for-std/course-select-apply'");
                        }
                        break;
                    }
                    case 9:
                    {
                        logger.Info("我的课表特殊处理");
                        if (WebView.Source.AbsoluteUri.IndexOf("/student/home") != -1)
                        {
                            WebView.ExecuteScriptAsync("window.location.href='/student/for-std/course-table'");
                        }
                        break;
                    }
                    case 10:
                    {
                        logger.Info("班级课表特殊处理");
                        if (WebView.Source.AbsoluteUri.IndexOf("/student/home") != -1)
                        {
                            WebView.ExecuteScriptAsync("window.location.href='/student/for-std/adminclass-course-table'");
                        }
                        break;
                    }
                    case 11:
                    {
                        logger.Info("培养方案特殊处理");
                        if (WebView.Source.AbsoluteUri.IndexOf("/student/home") != -1)
                        {
                            WebView.ExecuteScriptAsync("window.location.href='/student/for-std/program-completion-preview'");
                        }
                        break;
                    }
                    case 12:
                    {
                        logger.Info("开课查询特殊处理");
                        if (WebView.Source.AbsoluteUri.IndexOf("/student/home") != -1)
                        {
                            WebView.ExecuteScriptAsync("window.location.href='/student/for-std/lesson-search'");
                        }
                        break;
                    }
                    case 13:
                    {
                        logger.Info("座位预约特殊处理");
                        if (WebView.Source.AbsoluteUri.ToString().IndexOf("http://seat.lib.dlut.edu.cn/yanxiujian/client/orderSeat.php") != -1)
                        {
                            resize();
                        }
                        else
                        {
                            resize_back();
                        }
                        break;
                    }
                    case 14:
                    {
                        logger.Info("大工邮箱特殊处理");
                        if(WebView.Source.AbsoluteUri.ToString()== "https://mail.dlut.edu.cn/")
                        {
                            if(String.IsNullOrEmpty(ApplicationConfig.GetSettings("MailAddress")) || String.IsNullOrEmpty(ApplicationConfig.GetSettings("MailPassword")))
                            {
                                string rm = "domain.value='mail.dlut.edu.cn';document.getElementsByClassName('domainTxt')[0].textContent = 'mail.dlut.edu.cn'";
                                WebView.CoreWebView2.ExecuteScriptAsync(rm);
                            }
                            else
                            {
                                string jscode = "uid.value='" + ApplicationConfig.GetSettings("MailAddress") + "'";
                                WebView.CoreWebView2.ExecuteScriptAsync(jscode);
                                string jscode1 = "password.value='" + ApplicationConfig.GetSettings("MailPassword") + "'";
                                WebView.CoreWebView2.ExecuteScriptAsync(jscode1);
                                string rm = "domain.value='mail.dlut.edu.cn';document.getElementsByClassName('domainTxt')[0].textContent = 'mail.dlut.edu.cn'";
                                WebView.CoreWebView2.ExecuteScriptAsync(rm);
                                WebView.CoreWebView2.ExecuteScriptAsync(jscode);
                                string jscode2 = "document.getElementsByClassName('u-btn u-btn-primary submit j-submit')[0].click()";
                                WebView.CoreWebView2.ExecuteScriptAsync(jscode2);
                            }
                        }
                        break;
                    }
                    case 500:
                    {
                        logger.Info("就业网特殊处理");
                        if (WebView.Source.AbsoluteUri.ToString().IndexOf("/autoLogin") != -1)
                        {
                            WebView.CoreWebView2.ExecuteScriptAsync("window.location.href='https://job.dlut.edu.cn/login.html'");
                        }
                        break;
                    }
            }
            count++;
        }


        void resize()
        {
            WebView.CoreWebView2.ExecuteScriptAsync("document.body.style.zoom = \"50%\";");
            string rs = "document.getElementsByClassName('main-container')[0].style='margin:0px;padding:0px;width:100%'";
            WebView.CoreWebView2.ExecuteScriptAsync(rs);
            rs = "document.getElementsByClassName('container')[2].style='margin:0px;padding:0px;width:100%;'";
            WebView.CoreWebView2.ExecuteScriptAsync(rs);
            rs = "document.getElementsByClassName('row')[2].style='margin:0px;padding:0px;width:100%'";
            WebView.CoreWebView2.ExecuteScriptAsync(rs);
        }

        void resize_back()
        {
            WebView.CoreWebView2.ExecuteScriptAsync("document.body.style.zoom = \"100%\";");
            string rs = "document.getElementsByClassName('main-container')[0].style=''";
            WebView.CoreWebView2.ExecuteScriptAsync(rs);
            rs = "document.getElementsByClassName('container')[2].style=''";
            WebView.CoreWebView2.ExecuteScriptAsync(rs);
            rs = "document.getElementsByClassName('row')[2].style=''";
            WebView.CoreWebView2.ExecuteScriptAsync(rs);
        }

        async Task newtab()
        {
            try
            {
                if(WebView.CoreWebView2 ==null)
                {
                    return;
                }
                WebView.CoreWebView2.ExecuteScriptAsync("var num=document.getElementsByClassName('get-into').length;for(i = 0; i < num; i++){document.getElementsByClassName('get-into')[i].target = '_blank'}");
                await Task.Delay(3000);
                if (WebView.CoreWebView2 == null)
                {
                    return;
                }
                WebView.CoreWebView2.ExecuteScriptAsync("var num=document.getElementsByClassName('get-into').length;for(i = 0; i < num; i++){document.getElementsByClassName('get-into')[i].target = '_blank'}");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
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
                    AddressBox.Text = "https://" + AddressBox.Text;
                }
                WebView.CoreWebView2.ExecuteScriptAsync("window.location.href='" + AddressBox.Text + "'");
            }
        }
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (WebView.CoreWebView2.IsDefaultDownloadDialogOpen)
            {
                WebView.CoreWebView2.CloseDefaultDownloadDialog();
            }
            else
            {
                WebView.CoreWebView2.OpenDefaultDownloadDialog();
            }
        }

        private void Download_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("0", "下载", "", "", "edge://downloads/all", 0));
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("0", "历史记录", "", "", "edge://history/all", 0));
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
        private void DevTools_Click(object sender, RoutedEventArgs e)
        {
            WebView.CoreWebView2.OpenDevToolsWindow();
        }

    }
}
