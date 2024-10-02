// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;
using System;
using DLUTToolBoxV3.Pages;
using DLUTToolBoxV3.Configurations;
using Microsoft.Windows.AppNotifications.Builder;
using Microsoft.Windows.AppNotifications;
using WinUICommunity;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DLUTToolBoxV3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellPage : Page
    {
        public static ShellPage Instance { get; private set; }
        public ShellPageService shellPageService { get; set; }
        public ShellPage()
        {
            this.InitializeComponent();
            shellPageService = new ShellPageService();
            shellPageService.SetDefaultPage(typeof(GeneralPage));
            INavigationViewServiceEx navigationViewService;
            INavigationServiceEx navigationService;
            navigationService = new NavigationServiceEx(shellPageService);
            navigationService.Frame = shellFrame;
            navigationViewService = new NavigationViewServiceEx(navigationService, shellPageService);
            navigationViewService.Initialize(navigationView);
            navigationViewService.ConfigAutoSuggestBox(autoSuggestBox,"找不到匹配的结果😥");


            if (String.IsNullOrEmpty(ApplicationConfig.GetSettings("Uid")) || String.IsNullOrEmpty(ApplicationConfig.GetSettings("Password")))
            {
                var builder = new AppNotificationBuilder()
                    .AddText("请先在参数配置界面设置学工号和统一认证密码!\n设置完成后⚠重启本程序⚠方可正常使用所有功能！");
                var notificationManager = AppNotificationManager.Default;
                notificationManager.Show(builder.BuildNotification());
                shellFrame.Navigate(typeof(SettingsPage));
            }
        }
    }
}
