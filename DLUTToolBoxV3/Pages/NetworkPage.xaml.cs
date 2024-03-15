﻿// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using DLUTToolBoxV3.Entities;
using DLUTToolBoxV3.Ultilities;
using Microsoft.UI.Xaml.Media.Animation;
using System.Diagnostics;
using System.Collections.ObjectModel;
using DLUTToolBoxV3.Helpers;
using WinUICommunity;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using System.Text.RegularExpressions;
using System.Text;
using DLUTToolBoxV3.Configurations;
using System.Net;
using static QRCoder.PayloadGenerator;
using System.Net.Http;
using System.Threading;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NetworkPage : Page
    {
        public NLog.Logger logger;
        public NetworkPage()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("打开网络工具页面");
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadNetInfo();
        }

        public void LoadNetInfo()
        {
            logger.Info("加载网络信息");
            var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
            Task.Run(() =>
            {
                DrcomStatus drcomStatus = InfoUltilities.GetEDANetworkOnlineInfo();
                if (drcomStatus != null)
                {
                    if (drcomStatus.result == 1)
                    {
                        double fee = drcomStatus.fee;
                        fee /= 10000;
                        string V4IP = drcomStatus.v4ip;
                        string flowused = FormatFlow(drcomStatus.flow);
                        string flowLeft = FormatFlow(drcomStatus.olflow);
                        dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                        {
                            NetworkInfo.Message = "校园网余额：" + fee + "\n校园网已用流量：\n" + flowused + "\n校园网剩余流量：\n" + flowLeft + "\nIPV4地址：" + V4IP + "\n网卡MAC：" + drcomStatus.olmac;
                        });
                    }
                    else
                    {
                        dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                        {
                            NetworkInfo.Message = "校园网已连接但尚未认证";
                        });
                    }
                }
                else
                {
                    dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                    {
                        NetworkInfo.Message = "未连接校园网";
                    });
                }
            });
        }

        string FormatFlow(long num)
        {
            double temp = num;
            string re = "";
            if (temp > 1048576)
            {
                temp /= (double)(1024 * 1024);
                re = temp.ToString("f2") + "GB";
            }
            else if (temp > 1024)
            {
                temp /= (double)(1024);
                re = temp.ToString("f2") + "MB";
            }
            else
            {
                re = temp.ToString("f2") + "KB";
            }
            return re;
        }

        public void OnItemGridViewItemClick(object sender, ItemClickEventArgs e)
        {
            var item = (AppDataItem)e.ClickedItem;
            Debug.WriteLine(item.Title);
        }

        private void NetworkMonitor_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "校园网状态", "ms-appx:///Assets/AppIcons/Network/NetworkMonitor.png", "", PathHelper.GetFullPathToExe() + @"\Assets\Web\Monitor.html", 0));
        }

        private void SelfService_Click(object sender, RoutedEventArgs e)
        {
            WebHelper.AddOrCreateNewPage(new AppDataItem("1", "开发区校园网自服务", "ms-appx:///Assets/AppIcons/Network/SelfService.png", "", "https://sso.dlut.edu.cn/cas/login?service=http%3A%2F%2F172.20.30.2%3A8080%2FSelf%2Fsso_login%3Flogin_method%3D1", 1));
        }

        private void CleanDNS_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.StandardInput.WriteLine("ipconfig /flushdns");
                p.StandardInput.WriteLine("exit");
                p.StandardInput.AutoFlush = true;
                string result = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                if (result.IndexOf("成功") != -1)
                {
                    var builder = new AppNotificationBuilder()
                        .AddText("清理成功!");
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                }
                else
                {
                    var builder = new AppNotificationBuilder()
                        .AddText("清理失败!");
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                }
            });
        }

        private void LSPFix_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                Process p = new Process();
                p.StartInfo.FileName = "netsh.exe";
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.Verb = "runas";
                p.StartInfo.Arguments = "winsock reset";
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                p.WaitForExit();
                var builder = new AppNotificationBuilder()
                    .AddText("修复成功!");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
            });
        }

        //private void ManualConnect_Click(object sender, RoutedEventArgs e)
        //{
        //    var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
        //    logger.Info("登陆校园网");
        //    NetworkInfo.Message = "正在尝试登录。。。";
        //    Task.Run(() =>
        //    {
        //        DrcomStatus drcomStatus = InfoUltilities.GetEDANetworkOnlineInfo();
        //        if (drcomStatus != null)
        //        {
        //            if (drcomStatus.result != 1)
        //            {
        //                dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
        //                {
        //                    string LoginURL = "https://sso.dlut.edu.cn/cas/login?service=http%3A%2F%2F172.20.30.2%3A8080%2FSelf%2Fsso_login%3Fwlan_user_ip%3D" + drcomStatus.v46ip + "%26authex_enable%3D%26type%3D1";
        //                    //string Response = InfoUltilities.CommonGetWebRequest(LoginURL, Encoding.UTF8);
        //                    //string LT = Response.Split("<input type=\"hidden\" id=\"lt\" name=\"lt\" value=\"", StringSplitOptions.None)[1].Split("\">")[0];
        //                    //string JSESSIONIDCAS = Response.Split("action=\"/cas/login;JSESSIONIDCAS=", StringSplitOptions.None)[1].Split("?service=")[0];
        //                    //string rsa = DES.GetRSA(ApplicationConfig.GetSettings("Uid"), ApplicationConfig.GetSettings("Password"), LT);
        //                    WebView2 loginweb = new WebView2();
        //                    loginweb.CoreWebView2Initialized += (sender, args) =>
        //                    {
        //                        loginweb.NavigationCompleted += (sender1, args1) =>
        //                        {
        //                            if (loginweb.Source.AbsoluteUri.IndexOf("https://sso.dlut.edu.cn/cas/login?service=http%3A%2F%2F172.20.30.2%3A8080%2FSelf%2Fsso_login") != -1)
        //                            {
        //                                logger.Info("执行sso登录注入");
        //                                string jscode = "un.value='" + ApplicationConfig.GetSettings("Uid") + "'";
        //                                string jscode1 = "pd.value='" + ApplicationConfig.GetSettings("Password") + "'";
        //                                string rm = "rememberName.checked='checked'";
        //                                loginweb.CoreWebView2.ExecuteScriptAsync(rm);
        //                                loginweb.CoreWebView2.ExecuteScriptAsync(jscode);
        //                                loginweb.CoreWebView2.ExecuteScriptAsync(jscode1);
        //                                string jscode2 = "login()";
        //                                loginweb.CoreWebView2.ExecuteScriptAsync(jscode2);
        //                            }
        //                            else if (loginweb.Source.AbsoluteUri.IndexOf("http://172.20.30.2:8080/Self/dashboard;jsessionid=") != -1)
        //                            {
        //                                var builder = new AppNotificationBuilder()
        //                                    .AddText("登陆成功!");
        //                                var notificationManager = AppNotificationManager.Default;
        //                                notificationManager.Show(builder.BuildNotification());
        //                                logger.Info("登陆成功");
        //                                loginweb.CoreWebView2.CookieManager.DeleteCookiesWithDomainAndPath("JSESSIONID", "172.20.30.2", "/Self");
        //                                loginweb.CoreWebView2.CookieManager.DeleteCookiesWithDomainAndPath("JSESSIONID", "172.20.30.2", "/");
        //                                LoadNetInfo();
        //                            }
        //                        };
        //                        loginweb.Source = new Uri(LoginURL);
        //                    };
        //                    loginweb.EnsureCoreWebView2Async();
        //                });
        //            }
        //            else
        //            {
        //                var builder = new AppNotificationBuilder()
        //                    .AddText("无需登录!");
        //                var notificationManager = AppNotificationManager.Default;
        //                notificationManager.Show(builder.BuildNotification());
        //                logger.Info("无需登录");
        //                dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () => {
        //                    LoadNetInfo();
        //                });
        //            }
        //        }
        //    });
        //}
        
        private void ManualConnect_Click(object sender, RoutedEventArgs e)
        {
            var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
            logger.Info("登陆校园网");
            NetworkInfo.Message = "正在尝试登录。。。";
            Task.Run(() =>
            {
                DrcomStatus drcomStatus = InfoUltilities.GetEDANetworkOnlineInfo();
                if (drcomStatus != null)
                {
                    if (drcomStatus.result != 1)
                    {
                        dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                        {
                            string LoginURL = "https://sso.dlut.edu.cn/cas/login?service=http%3A%2F%2F172.20.30.2%3A8080%2FSelf%2Fsso_login%3Fwlan_user_ip%3D" + drcomStatus.v46ip + "%26authex_enable%3D%26type%3D1";
                            var cookieContainer = new CookieContainer();
                            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer,AllowAutoRedirect = true, UseProxy = false })
                            using (HttpClient client = new HttpClient(handler))
                            {
                                try
                                {
                                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36 Edg/111.0.1661.44");
                                    client.Timeout = new TimeSpan(0, 0, 10); // 10是秒数，用于设置超时时长
                                    Task<HttpResponseMessage> res = client.GetAsync(LoginURL);
                                    string Response = res.Result.Content.ReadAsStringAsync().Result;
                                    string JSESSIONIDCAS = res.Result.Headers.ToString().Split("JSESSIONIDCAS=")[1].Split("; path=")[0];
                                    logger.Info(JSESSIONIDCAS);
                                    string LT = Response.Split("<input type=\"hidden\" id=\"lt\" name=\"lt\" value=\"", StringSplitOptions.None)[1].Split("\"")[0];
                                    logger.Info(LT);
                                    string execution = Response.Split("<input type=\"hidden\" name=\"execution\" value=\"", StringSplitOptions.None)[1].Split("\"")[0];
                                    logger.Info(execution);
                                    string RSA = DES.GetRSA(ApplicationConfig.GetSettings("Uid"), ApplicationConfig.GetSettings("Password"), LT);
                                    logger.Info(RSA);
                                    HttpContent content = new StringContent("none=on&rsa="+RSA+"&ul="+ ApplicationConfig.GetSettings("Uid").Length+ "&pl="+ ApplicationConfig.GetSettings("Password").Length + "&sl=0&lt="+LT+"&execution="+execution+"&_eventId=submit");
                                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
                                    cookieContainer.Add(new Uri("https://sso.dlut.edu.cn"), new Cookie("cas_hash", ""));
                                    cookieContainer.Add(new Uri("https://sso.dlut.edu.cn"), new Cookie("dlut_cas_un", ApplicationConfig.GetSettings("Uid")));
                                    cookieContainer.Add(new Uri("https://sso.dlut.edu.cn"), new Cookie("Language", "zh_CN"));
                                    cookieContainer.Add(new Uri("https://sso.dlut.edu.cn"), new Cookie("JSESSIONIDCAS", JSESSIONIDCAS));
                                    Task<HttpResponseMessage> res1 = client.PostAsync(LoginURL, content);
                                    HttpResponseMessage Response1 = res1.Result;
                                    logger.Info(Response1.Headers.Location);
                                    Task<HttpResponseMessage> res2 = client.GetAsync(Response1.Headers.Location);
                                    HttpResponseMessage Response2 = res2.Result;
                                    String FinalResponse = Response2.Content.ReadAsStringAsync().Result;
                                    if (FinalResponse.Contains("您好！"))
                                    {
                                        logger.Info("登陆成功");
                                        var builder = new AppNotificationBuilder()
                                            .AddText("登陆成功!");
                                        var notificationManager = AppNotificationManager.Default;
                                        notificationManager.Show(builder.BuildNotification());
                                        Thread.Sleep(500);
                                        LoadNetInfo();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex);
                                }
                                finally
                                {
                                    client.Dispose();
                                }
                            }
                        });
                    }
                    else
                    {
                        var builder = new AppNotificationBuilder()
                            .AddText("无需登录!");
                        var notificationManager = AppNotificationManager.Default;
                        notificationManager.Show(builder.BuildNotification());
                        logger.Info("无需登录");
                        dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () => {
                            LoadNetInfo();
                        });
                    }
                }
            });
        }

        private void ManualDisconnect_Click(object sender, RoutedEventArgs e)
        {
            var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
            logger.Info("注销校园网");
            Task.Run(() =>
            {
                try
                {
                    DrcomStatus drcomStatus = InfoUltilities.GetEDANetworkOnlineInfo();
                    if (drcomStatus != null && drcomStatus.result == 1)
                    {
                        string Response =  InfoUltilities.GetWebRequest("http://172.20.30.1:801/eportal/portal/logout?callback=&wlan_user_ip=" + drcomStatus.v4ip, Encoding.UTF8);
                        if(Response.Contains("成功"))
                        {
                            var builder = new AppNotificationBuilder()
                                .AddText("注销成功!");
                            var notificationManager = AppNotificationManager.Default;
                            notificationManager.Show(builder.BuildNotification());
                            logger.Info("注销成功");
                            dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                            {
                                LoadNetInfo();
                            });
                        }
                        else
                        {
                            var builder = new AppNotificationBuilder()
                                .AddText("注销失败!");
                            var notificationManager = AppNotificationManager.Default;
                            notificationManager.Show(builder.BuildNotification());
                            logger.Info("注销失败");
                        }
                    }
                    else
                    {
                        var builder = new AppNotificationBuilder()
                            .AddText("无需注销!");
                        var notificationManager = AppNotificationManager.Default;
                        notificationManager.Show(builder.BuildNotification());
                        logger.Info("无需注销");
                    }
                }
                catch (Exception ex)
                {
                    var builder = new AppNotificationBuilder()
                        .AddText("注销失败!\n"+ex.Message);
                    var notificationManager = AppNotificationManager.Default;
                    notificationManager.Show(builder.BuildNotification());
                    logger.Error(ex);
                }
            });
        }

        private void NetworkEnhance_Click(object sender, RoutedEventArgs e)
        {
            ActionHelper.SendMessageToUserCore("2", (o, e) => { });
        }

        private void RefreshNetworkStatus_Click(object sender, RoutedEventArgs e)
        {
            NetworkInfo.Message = "正在加载信息。。。";
            LoadNetInfo();
        }
    }
}
