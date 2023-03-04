// Copyright (c) Microsoft Corporation and Contributors.
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
using WinUICommunity.Shared.DataModel;
using Microsoft.UI.Xaml.Media.Animation;
using System.Diagnostics;
using System.Collections.ObjectModel;

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
                        dispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal, () =>
                        {
                            NetworkInfo.Message = "校园网余额：" + fee + "\n本机校园网已用流量：\n" + flowused + "\nIPV4地址：" + V4IP + "\n网卡MAC：" + drcomStatus.olmac;
                        });
                    }
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
                re = temp.ToString() + "GB";
            }
            else if (temp > 1024)
            {
                temp /= (double)(1024);
                re = temp.ToString() + "MB";
            }
            else
            {
                re = temp + "KB";
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

        }

        private void SelfService_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CleanDNS_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LSPFix_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ManualConnect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ManualDisconnect_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NetworkEnhance_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshNetworkStatus_Click(object sender, RoutedEventArgs e)
        {
            NetworkInfo.Message = "正在加载信息。。。";
            LoadNetInfo();
        }
    }
}
