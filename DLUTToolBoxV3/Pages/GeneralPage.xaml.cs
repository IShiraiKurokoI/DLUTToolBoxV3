// Copyright (c) Microsoft Corporation and Contributors.
// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Castle.Core.Internal;
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GeneralPage : Page
    {
        public NLog.Logger logger;
        public GeneralPage()
        {
            logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("打开信息总览页面");
            this.InitializeComponent();
            ClassTable.DefaultBackgroundColor = Windows.UI.Color.FromArgb(0, 0, 0, 0);
            Weather.DefaultBackgroundColor = Windows.UI.Color.FromArgb(0, 0, 0, 0);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            logger.Info("加载电费网络信息");
            var dispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread();
            Task.Run(() =>
            {
                DrcomStatus drcomStatus = InfoUltilities.GetEDANetworkOnlineInfo();
                if (drcomStatus != null)
                {
                    if(drcomStatus.result== 1)
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
            Task.Run(() =>
            {
                InfoUltilities.GetElectricityInfo(ElectrcityInfo, dispatcherQueue);
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

        private void ClassTable_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            ClassTable.CoreWebView2.Settings.AreHostObjectsAllowed = false;
            ClassTable.CoreWebView2.Settings.AreDevToolsEnabled = false;
            ClassTable.CoreWebView2.Settings.IsStatusBarEnabled = false;
            ClassTable.CoreWebView2.Settings.IsSwipeNavigationEnabled = false;
            ClassTable.CoreWebView2.Settings.IsZoomControlEnabled = false;
            ClassTable.CoreWebView2.NavigationStarting += (sender, e) => {
                if(e.Uri.Contains("card.m.dlut.edu.cn"))
                {
                    e.Cancel= true;
                    ClassTable.Source = ClassTable.BaseUri;
                }
            };
        }

        private void ClassTable_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            string un = ApplicationConfig.GetSettings("Uid");
            string upd = ApplicationConfig.GetSettings("Password");
            if (un.IsNullOrEmpty()||upd.IsNullOrEmpty())
            {
                ClassTable.Reload();
                return;
            }
            if (ClassTable.Source.AbsoluteUri.IndexOf("api") != -1)
            {
                apiloginforClassTable();
            }
            if (ClassTable.Source.AbsoluteUri == "https://lightapp.m.dlut.edu.cn/courseschedule")
            {
                resize();
            }
        }

        async Task apiloginforClassTable()
        {
            string jscode = "username.value='" + ApplicationConfig.GetSettings("Uid") + "'";
            string jscode1 = "password.value='" + ApplicationConfig.GetSettings("Password") + "'";
            await ClassTable.CoreWebView2.ExecuteScriptAsync(jscode);
            await ClassTable.CoreWebView2.ExecuteScriptAsync(jscode1);
            string jsenable = "btnpc.disabled=''";
            await ClassTable.CoreWebView2.ExecuteScriptAsync(jsenable);
            string jscode2 = "btnpc.click()";
            await ClassTable.CoreWebView2.ExecuteScriptAsync(jscode2);
        }

        async Task resize()
        {
            await Task.Delay(500);
            await ClassTable.CoreWebView2.ExecuteScriptAsync("document.getElementsByTagName('div')[4].style=\"overflow:auto;\"");
            await ClassTable.CoreWebView2.ExecuteScriptAsync("document.body.style=\"background-color:transparent\"");
            await ClassTable.CoreWebView2.ExecuteScriptAsync("document.getElementsByClassName('header')[0].outerHTML=\"\"");
            await ClassTable.CoreWebView2.ExecuteScriptAsync("document.getElementsByClassName('bottom')[0].style=\"height: 650px; \"");
            await ClassTable.CoreWebView2.ExecuteScriptAsync("document.getElementsByClassName('wrap')[0].style=\"background-color:transparent\"");
            ClassTable.Visibility = Visibility.Visible;
            ClassTableProgress.Visibility = Visibility.Collapsed;
        }
        private void Weather_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            Weather.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            Weather.CoreWebView2.Settings.AreDevToolsEnabled = false;
            Weather.CoreWebView2.Settings.AreHostObjectsAllowed = false;
            Weather.CoreWebView2.Settings.IsStatusBarEnabled = false;
            Weather.CoreWebView2.Settings.IsSwipeNavigationEnabled = false;
            Weather.CoreWebView2.Settings.IsZoomControlEnabled = false;
        }

        private void Weather_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            if (Weather.Source.AbsoluteUri.IndexOf("https://sso.dlut.edu.cn/cas/login?service=http%3A%2F%2F172.20.30.2%3A8080%2FSelf%2Fsso_login") != -1)
            {
                string jscode = "un.value='" + ApplicationConfig.GetSettings("Uid") + "'";
                string jscode1 = "pd.value='" + ApplicationConfig.GetSettings("Password") + "'";
                string rm = "rememberName.checked='checked'";
                Weather.CoreWebView2.ExecuteScriptAsync(rm);
                Weather.CoreWebView2.ExecuteScriptAsync(jscode);
                Weather.CoreWebView2.ExecuteScriptAsync(jscode1);
                string jscode2 = "login()";
                Weather.CoreWebView2.ExecuteScriptAsync(jscode2);
            }
            else if (Weather.Source.AbsoluteUri.IndexOf("http://www.weather.com.cn/") != -1)
            {
                WeatherHandle();
            }
            else if (Weather.Source.AbsoluteUri.IndexOf("http://172.20.30.2:8080/Self/dashboard;jsessionid=") != -1)
            {
                ClassTable.Source = new Uri("https://api.m.dlut.edu.cn/login?client_id=9qXqHnRQuhhViycC&redirect_uri=https%3a%2f%2flightapp.m.dlut.edu.cn%2fcheck%2fcourseschedule&response_type=code");
                Weather.Source = Weather.BaseUri;
            }
        }

        async Task WeatherHandle()
        {
            await Task.Delay(500);
            await Weather.CoreWebView2.ExecuteScriptAsync("document.body.innerHTML=document.getElementsByClassName('myWeather')[0].outerHTML");
            await Weather.CoreWebView2.ExecuteScriptAsync("document.getElementsByClassName('myWeatherTop')[0].outerHTML=''");
            await Weather.CoreWebView2.ExecuteScriptAsync("document.getElementsByTagName('a')[0].outerHTML=\"\"");
            await Weather.CoreWebView2.ExecuteScriptAsync("document.body.style=\"overflow:hidden;background-color:transparent\"");
            await Weather.CoreWebView2.ExecuteScriptAsync("document.getElementsByTagName('div')[0].style='background-color:transparent'");
            Weather.Visibility = Visibility.Visible;
            WeatherProgress.Visibility = Visibility.Collapsed;
        }
    }
}
